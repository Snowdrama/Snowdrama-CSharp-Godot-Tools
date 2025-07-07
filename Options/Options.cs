
using Godot;


//Make this an auto load in the settings! 
[GlobalClass]
public partial class Options : Node
{
    public static Options instance;
    public const string MASTER_VOLUME_OPTION_KEY = "Master";
    public const string MUSIC_VOLUME_OPTION_KEY = "Music";
    public const string SOUND_VOLUME_OPTION_KEY = "Sounds";
    public const string VOICE_VOLUME_OPTION_KEY = "Voices";
    public const string DISPLAY_MODE_OPTION_KEY = "WindowMode";

    public const string WINDOW_MODE_OPTION_KEY = "WindowMode";
    public const string WINDOWED_RESOLUTION_OPTION_KEY = "WindowResolution";
    public const string VSYNC_OPTION_KEY = "VSync";
    public const string LANUGAGE_OPTION_KEY = "Language";
    public const string LANUGAGE_OPTION_STRING_KEY = "LanguageString";


    public static readonly Vector2I DEFAULT_WINDOW_RESOLUTION = new Vector2I(1280, 720);

    public static string defaultConfigLocation = "res://Tools/Options/DefaultConfig.cfg";
    public static string userConfigLocation = "user://Config.cfg";

    public static ConfigFile config;


    //loads the config ONLY if the config hasn't ben loaded already
    //used to validate that the config has been loaded
    private static void ValidateLoadConfig()
    {
        if (config == null)
        {
            LoadConfig();
        }
    }


    //forces a reload of the config, it does not check if the config has been loaded
    private static void LoadConfig()
    {
        config = new ConfigFile();
        Error configErr = config.Load(userConfigLocation);
        Debug.Log($"Loading Config From {userConfigLocation}");
        if (configErr != Error.Ok)
        {
            Debug.Log($"Loading Failed, Loading Default From {defaultConfigLocation}");
            //load from the default cfg
            var defaultConfig = new ConfigFile();
            Error defaultConfigErr = defaultConfig.Load(defaultConfigLocation);


            if (defaultConfigErr == Error.Ok)
            {
                Debug.Log("Default Loading Success!");
                config = defaultConfig;
                Debug.Log($"Saving To {userConfigLocation}");
                config.Save(userConfigLocation);
            }
            else
            {
                Debug.LogError($"Loading Default Failed Somehow...");
            }
        }
        else
        {
            Debug.Log("Options Loaded from config!");

            foreach (var section in config.GetSections())
            {
                // Fetch the data for each section.
                Debug.Log($"[[[{section}]]]");
                foreach (var key in config.GetSectionKeys(section))
                {
                    Debug.Log($"{key}: {config.GetValue(section, key)}");
                }
            }
        }
    }

    private static void SaveConfig()
    {
        if (config != null)
        {
            config.Save(userConfigLocation);
        }
        else
        {
            Debug.LogError("Couldn't save config because config is null");
        }
    }
    public static Vector2I GetVector2I(string key, Vector2I defaultValue = new Vector2I())
    {
        ValidateLoadConfig();
        var value = config.GetValue("Options", key, defaultValue);
        var hasSection = config.HasSection("Options");
        var hasSectionKey = config.HasSectionKey("Options", key);
        if (hasSection && hasSectionKey)
        {
            if (value.VariantType == Variant.Type.Vector2I)
            {
                return (Vector2I)value.AsVector2I();
            }
        }
        return defaultValue;
    }

    public static void SetVector2I(string key, Vector2I value)
    {
        ValidateLoadConfig();
        config.SetValue("Options", key, value);
        var err = config.Save(userConfigLocation);
        SaveConfig();
    }
    public static Vector2 GetVector2(string key, Vector2 defaultValue = new Vector2())
    {
        ValidateLoadConfig();
        var value = config.GetValue("Options", key, defaultValue);
        var hasSection = config.HasSection("Options");
        var hasSectionKey = config.HasSectionKey("Options", key);

        if (value.VariantType == Variant.Type.Vector2)
        {
            return (Vector2)value.AsVector2();
        }
        return defaultValue;
    }

    public static void SetVector2(string key, Vector2 value)
    {
        ValidateLoadConfig();
        config.SetValue("Options", key, value);
        var err = config.Save(userConfigLocation);
        SaveConfig();
    }


    public static bool HasInt(string key)
    {
        ValidateLoadConfig();
        if (config.HasSectionKey("Options", key))
        {
            var value = config.GetValue("Options", key);
            if (value.VariantType == Variant.Type.Int)
            {
                return true;
            }
        }
        return false;
    }
    public static int GetInt(string key, int defaultValue = 0)
    {
        ValidateLoadConfig();
        var value = config.GetValue("Options", key, defaultValue);
        var hasSection = config.HasSection("Options");
        var hasSectionKey = config.HasSectionKey("Options", key);

        if (value.VariantType == Variant.Type.Int)
        {
            return (int)value.AsInt32();
        }
        return defaultValue;
    }

    public static void SetInt(string key, int value)
    {
        ValidateLoadConfig();
        config.SetValue("Options", key, value);
        var err = config.Save(userConfigLocation);
        SaveConfig();
    }

    public static bool HasDouble(string key)
    {
        ValidateLoadConfig();
        if (config.HasSectionKey("Options", key))
        {
            var value = config.GetValue("Options", key);
            if (value.VariantType == Variant.Type.Float)
            {
                return true;
            }
        }
        return false;
    }
    public static double GetDouble(string key, double defaultValue = 0.0f)
    {
        ValidateLoadConfig();
        var value = config.GetValue("Options", key, defaultValue);
        var hasSection = config.HasSection("Options");
        var hasSectionKey = config.HasSectionKey("Options", key);

        if (value.VariantType == Variant.Type.Float)
        {
            return (double)value.AsDouble();
        }
        return defaultValue;
    }

    public static void SetDouble(string key, double value)
    {
        ValidateLoadConfig();
        Debug.LogWarn($"Setting Double for key: {key} to {value}");
        config.SetValue("Options", key, value);
        var err = config.Save(userConfigLocation);
        SaveConfig();
    }

    public static bool HasFloat(string key)
    {
        ValidateLoadConfig();
        if (config.HasSectionKey("Options", key))
        {
            var value = config.GetValue("Options", key);
            if (value.VariantType == Variant.Type.Float)
            {
                return true;
            }
        }
        return false;
    }
    public static float GetFloat(string key, float defaultValue = 0.0f)
    {
        ValidateLoadConfig();
        var value = config.GetValue("Options", key, defaultValue);

        if (value.VariantType == Variant.Type.Float)
        {
            return (float)value;
        }
        return defaultValue;
    }

    public static void SetFloat(string key, float value)
    {
        ValidateLoadConfig();
        config.SetValue("Options", key, value);
        config.Save(userConfigLocation);
        SaveConfig();
    }

    public static bool HasBool(string key)
    {
        ValidateLoadConfig();
        if (config.HasSectionKey("Options", key))
        {
            var value = config.GetValue("Options", key);
            if (value.VariantType == Variant.Type.Bool)
            {
                return true;
            }
        }
        return false;
    }

    public static bool GetBool(string key, bool defaultValue = false)
    {
        ValidateLoadConfig();
        var value = config.GetValue("Options", key, defaultValue);

        if (value.VariantType == Variant.Type.Bool)
        {
            return value.AsBool();
        }
        return defaultValue;
    }

    public static void SetBool(string key, bool value)
    {
        ValidateLoadConfig();
        config.SetValue("Options", key, value);
        config.Save(userConfigLocation);
        SaveConfig();
    }


    public static bool HasString(string key)
    {
        ValidateLoadConfig();
        if (config.HasSectionKey("Options", key))
        {
            var value = config.GetValue("Options", key);
            if (value.VariantType == Variant.Type.String)
            {
                return true;
            }
        }
        return false;
    }
    public static string GetString(string key, string defaultValue = "")
    {
        ValidateLoadConfig();
        var value = config.GetValue("Options", key, defaultValue);

        if (value.VariantType == Variant.Type.String)
        {
            return value.AsString();
        }
        return defaultValue;
    }

    public static void SetString(string key, string value)
    {
        ValidateLoadConfig();
        config.SetValue("Options", key, value);
        config.Save(userConfigLocation);
        SaveConfig();
    }
}