using Godot;
using Godot.Collections;
using Microsoft.VisualBasic;
using System;

[GlobalClass]
public partial class Options : Resource
{
    [Export(PropertyHint.File)] string defaultConfigLocation = "res://Tools/Options/DefaultConfig.cfg";
    [Export(PropertyHint.File)] string userConfigLocation = "user://Config.cfg";
    ConfigFile defaultConfig;
    ConfigFile config;
    public Options()
    {
        defaultConfig = new ConfigFile();
        Error defaultErr = defaultConfig.Load(defaultConfigLocation);

        if (defaultErr == Error.Ok)
        {
            config = new ConfigFile();
            Error configErr = config.Load(userConfigLocation);
            if(configErr == Error.Ok)
            {
                foreach (var section in defaultConfig.GetSections())
                {
                    foreach (var sectionKey in defaultConfig.GetSectionKeys(section))
                    {
                        //go through defaults if the user config doesn't contain the default add it
                        var defaultConfigValue = defaultConfig.GetValue(section, sectionKey);
                        var userConfigValue = config.GetValue(section, sectionKey, defaultConfigValue);
                        GD.Print($"Section: {section} Key: {sectionKey} DefaultValue: {defaultConfigValue} UserConfigValue: {userConfigValue}");
                        config.SetValue(section, sectionKey, userConfigValue);
                    }
                }
            }
            GD.Print("Saving User Config");
            config.Save(userConfigLocation);
        }
        else
        {
            GD.PrintErr($"Failed to load defaults, default file is REQUIRED, please add a config file to {defaultConfigLocation} even if it's empty");
        }
    }

    public double GetDouble(string key, double defaultValue = 0.0f)
    {
        var value = config.GetValue("Options", key, defaultValue);
        var hasSection = config.HasSection("Options");
        var hasSectionKey = config.HasSectionKey("Options", key);
        
        if (value.VariantType == Variant.Type.Float)
        {
            return (double)value.AsDouble();
        }
        return defaultValue;
    }

    public void SetDouble(string key, double value)
    {
        config.SetValue("Options", key, value);
        var err = config.Save(userConfigLocation);
    }
    public float GetFloat(string key, float defaultValue = 0.0f)
    {
        var value = config.GetValue("Options", key, defaultValue);

        if (value.VariantType == Variant.Type.Float)
        {
            return (float)value;
        }
        return defaultValue;
    }

    public void SetFloat(string key, float value)
    {
        config.SetValue("Options", key, value);
        config.Save(userConfigLocation);
    }


    public bool GetBool(string key, bool defaultValue = false)
    {
        var value = config.GetValue("Options", key, defaultValue);

        if (value.VariantType == Variant.Type.Bool)
        {
            return value.AsBool();
        }
        return defaultValue;
    }

    public void SetBool(string key, bool value)
    {
        config.SetValue("Options", key, value);
        config.Save(userConfigLocation);
    }

    public string GetString(string key, string defaultValue = "")
    {
        var value = config.GetValue("Options", key, defaultValue);

        if (value.VariantType == Variant.Type.String)
        {
            return value.AsString();
        }
        return defaultValue;
    }

    public void SetString(string key, string value)
    {
        config.SetValue("Options", key, value);
        config.Save(userConfigLocation);
    }

    public void ValidateConfig()
    {
    }
}
