using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

[System.Serializable]
public partial class SaveDataStruct
{
    public int currentSaveIndex = 0;
    public int currentAutoSaveIndex = 0;
    public Dictionary<int, SaveGameInfo> saveLocations = new Dictionary<int, SaveGameInfo>();
    public Dictionary<int, SaveGameInfo> autoSaveLocations = new Dictionary<int, SaveGameInfo>();
}

[System.Serializable]
public partial class SaveGameInfo
{
    public string name;
    public string version;
    public string dateModified;
    public string filePath;

    public override string ToString()
    {
        return $"SaveGameInfo: {name}[v{version}]: {filePath} -> {dateModified}";
    }
}

public partial class SaveGameListChanged : AMessage { }

public partial class SaveManager : Node
{
    private static GameDataStruct loadedSave = new GameDataStruct();
    private static SaveDataStruct saveDataInfo = new SaveDataStruct();

    private static readonly JsonSerializerSettings settings = new JsonSerializerSettings()
    {
        Formatting = Formatting.Indented,
    };
    private void Awake()
    {
        ValidateDirectories();
        var saveDataInfoPath = $"user://save_data_info.json";
        if (FileAccess.FileExists(saveDataInfoPath))
        {
            Debug.Log($"Loading SaveDataInfo");
            var saveDataInfoJson = FileAccess.GetFileAsString(saveDataInfoPath);

            Debug.Log(saveDataInfoJson);
            saveDataInfo = JsonConvert.DeserializeObject<SaveDataStruct>(saveDataInfoJson, settings);
        }
        else
        {
            Debug.Log($"Creating New SaveDataInfo");
            saveDataInfo = new SaveDataStruct();
        }

        foreach (SaveGameInfo file in saveDataInfo.saveLocations.Values)
        {
            Debug.Log($"Found Load: {file.name} {file.filePath}");
        }

        foreach (SaveGameInfo file in saveDataInfo.autoSaveLocations.Values)
        {
            Debug.Log($"Found Load: {file.name} {file.filePath}");
        }

        Debug.Log("Loading Load 0");
        //load save 0 by default in case we're testing
        LoadSave(saveDataInfo.currentSaveIndex);
    }

    public static SaveDataStruct GetSaveList()
    {
        return saveDataInfo;
    }

    public static bool LoadSave(int saveSlot)
    {
        ValidateDirectories();
        if (saveDataInfo.saveLocations == null)
        {
            Debug.LogError("Somehow saveLocations is null");
            return false;
        }
        if (saveDataInfo.saveLocations.Values.Count == 0)
        {
            Debug.LogError("No saves found");
            return false;
        }

        if (!saveDataInfo.saveLocations.ContainsKey(saveSlot))
        {
            Debug.LogError("Tried to load a save slot that doesn't exist");
            return false;
        }

        //can we find the save?
        Debug.Log($"Loading From: {saveDataInfo.saveLocations[saveSlot].filePath}");
        if (FileAccess.FileExists(saveDataInfo.saveLocations[saveSlot].filePath))
        {
            var saveToLoad = saveDataInfo.saveLocations[saveSlot].filePath;
            var fileContents = FileAccess.GetFileAsString(saveToLoad);

            Debug.Log($"Loading file from {saveToLoad}");
            Debug.Log(fileContents);
            loadedSave = JsonConvert.DeserializeObject<GameDataStruct>(fileContents, settings);

            saveDataInfo.currentSaveIndex = saveSlot;
            SaveInfoFile();
            Messages.GetOnce<SaveGameListChanged>().Dispatch();
            return true;
        }

        return false;
    }

    public static bool CanLoadSave(int saveSlot)
    {
        if (saveDataInfo.saveLocations == null)
        {
            Debug.LogError("Somehow saveLocations is null");
            return false;
        }
        if (saveDataInfo.saveLocations.Values.Count == 0)
        {
            Debug.LogError("No saves found");
            return false;
        }

        if (!saveDataInfo.saveLocations.ContainsKey(saveSlot))
        {
            Debug.LogError("Tried to load a save slot that doesn't exist");
            return false;
        }

        if (FileAccess.FileExists(saveDataInfo.saveLocations[saveSlot].filePath))
        {
            return true;
        }
        return false;
    }

    public static bool LoadAutoSave(int saveSlot)
    {
        ValidateDirectories();
        if (saveDataInfo.autoSaveLocations == null)
        {
            Debug.LogError("Somehow autoSaveLocations is null");
            return false;
        }
        if (saveDataInfo.autoSaveLocations.Values.Count == 0)
        {
            Debug.LogError("No auto saves found");
            return false;
        }

        if (!saveDataInfo.autoSaveLocations.ContainsKey(saveSlot))
        {
            Debug.LogError("Tried to load an auto save slot that doesn't exist");
            return false;
        }

        //can we find the save?
        Debug.Log($"Loading Auto Save From: {saveDataInfo.autoSaveLocations[saveSlot].filePath}");
        if (FileAccess.FileExists(saveDataInfo.autoSaveLocations[saveSlot].filePath))
        {
            var autoSaveToLoad = saveDataInfo.autoSaveLocations[saveSlot].filePath;
            var fileContents = FileAccess.GetFileAsString(autoSaveToLoad);

            Debug.Log($"Loading file from {autoSaveToLoad}");
            Debug.Log(fileContents);
            loadedSave = JsonConvert.DeserializeObject<GameDataStruct>(fileContents, settings);

            saveDataInfo.currentAutoSaveIndex = saveSlot;
            SaveInfoFile();
            Messages.GetOnce<SaveGameListChanged>().Dispatch();
            return true;
        }

        return false;
    }
    public static bool CanLoadAutoSave(int saveSlot)
    {
        if (saveDataInfo.autoSaveLocations == null)
        {
            Debug.LogError("Somehow autoSaveLocations is null");
            return false;
        }
        if (saveDataInfo.autoSaveLocations.Values.Count == 0)
        {
            Debug.LogError("No saves found");
            return false;
        }

        if (!saveDataInfo.autoSaveLocations.ContainsKey(saveSlot))
        {
            Debug.LogError("Tried to load a save slot that doesn't exist");
            return false;
        }

        if (FileAccess.FileExists(saveDataInfo.autoSaveLocations[saveSlot].filePath))
        {
            return true;
        }
        return false;
    }


    public static int GetUnusedSaveSlot()
    {
        int index = 0;
        //This assumes that there's no way the player
        //has int.MaxValue number of saves...
        //we will EVENTUALLY find one they don't use
        while (saveDataInfo.saveLocations.ContainsKey(index))
        {
            index++;
        }
        return index;
    }

    public static int GetUnusedAutoSaveSlot()
    {
        int index = 0;
        //This assumes that there's no way the player
        //has int.MaxValue number of saves...
        //we will EVENTUALLY find one they don't use
        while (saveDataInfo.autoSaveLocations.ContainsKey(index))
        {
            index++;
        }
        return index;
    }

    public static bool SaveGame(GameDataStruct gameData, bool force = false, string saveName = null, string version = null)
    {
        return SaveGame(saveDataInfo.currentSaveIndex, gameData, force, saveName, version);
    }

    public static bool SaveGameToNewSlot(GameDataStruct gameData, bool force = false, string saveName = null, string version = null)
    {
        saveDataInfo.currentSaveIndex = GetUnusedSaveSlot();
        return SaveGame(saveDataInfo.currentSaveIndex, gameData, force, saveName, version);
    }

    public static bool SaveGame(int saveSlot, GameDataStruct gameData, bool force = false, string saveName = null, string version = null)
    {
        ValidateDirectories();

        var filePath = $"user://Saves/Save{saveSlot}.json";

        //do we have a file there already?
        bool fileExists = FileAccess.FileExists(filePath);

        Debug.Log($"FileAccess.FileExists() = {fileExists} && force == {force}");

        if (fileExists && force == false)
        {
            Debug.LogWarning($"FileAccess.FileExists() = {fileExists} && force == {force}");
            return false;
        }

        //Update save info
        Debug.Log("Creating new save info");
        var newSaveInfo = new SaveGameInfo()
        {
            name = (!string.IsNullOrEmpty(saveName)) ? saveName : $"Save {saveSlot}",
            dateModified = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
            filePath = filePath,
            version = (!string.IsNullOrEmpty(version)) ? version : $"0.0.1",
        };
        Debug.Log(newSaveInfo.ToString());
        if (!saveDataInfo.saveLocations.ContainsKey(saveSlot))
        {
            saveDataInfo.saveLocations.Add(saveSlot, newSaveInfo);
        }
        else
        {
            saveDataInfo.saveLocations[saveSlot] = newSaveInfo;
        }
        Debug.Log($"Serializing game Data, writing to {filePath}");
        var fileContents = JsonConvert.SerializeObject(gameData, settings);


        var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Write);
        file.StoreString(fileContents);
        file.Close();

        SaveInfoFile();
        Messages.GetOnce<SaveGameListChanged>().Dispatch();

        return true;
    }

    public static void AutoSave(GameDataStruct gameData, string version = null)
    {
        ValidateDirectories();
        var unusedSlot = GetUnusedAutoSaveSlot();
        //we always want to use an unused slot if we can find one
        //in case the player manually deletes an auto save
        //say the player has 10 saves, and deletes 3
        //but the current index is 7. We don't want to overwrite 8
        //we want to go back and save to 3 first. 

        if (unusedSlot >= 10)
        {
            //we couldn't find a slot.
            var listOfAutoSaves = saveDataInfo.autoSaveLocations.OrderBy(x => x.Value.dateModified).ToList();

            foreach (var item in listOfAutoSaves)
            {
                Debug.LogError($"AutoSave[{item.Key}]: {item.Value.dateModified}");
            }

            //get the oldest key and use that auto save key:
            saveDataInfo.currentAutoSaveIndex = listOfAutoSaves.First().Key;
            //arbitrary loop on auto save 10.
            saveDataInfo.currentAutoSaveIndex = saveDataInfo.currentAutoSaveIndex % 10;

            Debug.LogWarning($"Adding a new Auto Save - Oldest is: {saveDataInfo.currentAutoSaveIndex} -> {listOfAutoSaves.First().Value.dateModified}");

        }
        else
        {
            saveDataInfo.currentAutoSaveIndex = unusedSlot;
        }

        //create the file contents
        var fileContents = JsonConvert.SerializeObject(gameData, settings);
        var filePath = $"user://AutoSaves/AutoSave{saveDataInfo.currentAutoSaveIndex}.json";

        var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Write);
        file.StoreString(fileContents);
        file.Close();

        Debug.Log($"Auto Save[{saveDataInfo.currentAutoSaveIndex}]: {filePath}");
        var autoSaveInfo = new SaveGameInfo()
        {
            name = $"Autosave {saveDataInfo.currentAutoSaveIndex}",
            dateModified = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"),
            filePath = filePath,
            version = (!string.IsNullOrEmpty(version)) ? version : $"0.0.1",
        };

        if (!saveDataInfo.autoSaveLocations.ContainsKey(saveDataInfo.currentAutoSaveIndex))
        {
            saveDataInfo.autoSaveLocations.Add(saveDataInfo.currentAutoSaveIndex, autoSaveInfo);
        }
        else
        {
            saveDataInfo.autoSaveLocations[saveDataInfo.currentAutoSaveIndex] = autoSaveInfo;
        }

        SaveInfoFile();
        Messages.GetOnce<SaveGameListChanged>().Dispatch();
    }

    public static bool DeleteSaveGame(int saveSlot, bool force = false)
    {
        if (saveDataInfo.saveLocations == null)
        {
            Debug.LogError("Somehow saveLocations is null");
            return false;
        }
        if (saveDataInfo.saveLocations.Values.Count == 0)
        {
            Debug.LogError("No saves found");
            return false;
        }

        if (!saveDataInfo.saveLocations.ContainsKey(saveSlot))
        {
            Debug.LogError("Tried to load a save slot that doesn't exist");
            return false;
        }

        if (force == false)
        {
            return false;
        }
        var filePath = $"user://Saves/Save{saveSlot}.json";
        Debug.Log($"Deleting file: {filePath}");
        if (FileAccess.FileExists(filePath))
        {
            DirAccess.RemoveAbsolute(filePath);
        }
        else
        {
            Debug.LogError($"File path {filePath} doesn't exist can't delete!");
        }
        saveDataInfo.saveLocations.Remove(saveSlot);
        SaveInfoFile();
        Messages.GetOnce<SaveGameListChanged>().Dispatch();
        return true;
    }





    private static void SaveInfoFile()
    {
        ValidateDirectories();

        var filePath = $"user://save_data_info.json";
        var fileContents = JsonConvert.SerializeObject(saveDataInfo, settings);

        Debug.Log($"Saving Info File! {filePath}");

        var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Write);
        file.StoreString(fileContents);
        file.Close();
    }

    public static GameDataStruct GetGameData()
    {
        return loadedSave;
    }

    private static void ValidateDirectories()
    {
        if (!DirAccess.DirExistsAbsolute($"user://Saves"))
        {
            DirAccess.MakeDirRecursiveAbsolute($"user://Saves");
        }

        if (!DirAccess.DirExistsAbsolute($"user://AutoSaves"))
        {
            DirAccess.MakeDirRecursiveAbsolute($"user://AutoSaves");
        }
    }
}
