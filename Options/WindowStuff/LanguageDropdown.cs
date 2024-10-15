using Godot;
using Godot.Collections;
using System;
using System.Reflection;
public partial class LanguageDropdown : OptionButton
{
    [Export]
    Array<LanguageCodePair> languages = new Array<LanguageCodePair>
    {
        new LanguageCodePair{code = "en", name = "English"},
        new LanguageCodePair{code = "es", name = "Español"},
        new LanguageCodePair{code = "fr", name = "Français"},
        new LanguageCodePair{code = "de", name = "Deutsch"},
        new LanguageCodePair{code = "zh", name = "中文"},
        new LanguageCodePair{code = "ja", name = "日本語"},
        new LanguageCodePair{code = "ru", name = "Русский"},
        new LanguageCodePair{code = "ar", name = "العربية"},
        new LanguageCodePair{code = "pt", name = "Português"},
        new LanguageCodePair{code = "it", name = "Italiano"},
    };
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.ItemSelected += VSyncToggle_ItemSelected;
        this.VisibilityChanged += OnVisibilityChanged;
        LoadOptions();
        LoadOptionFromSave();
    }


    private void OnVisibilityChanged()
    {
        LoadOptions();
        LoadOptionFromSave();
    }
    private void VSyncToggle_ItemSelected(long index)
    {
        var langCode = languages[(int)index];
        TranslationServer.SetLocale(langCode.code);
        Options.SetString(Options.LANUGAGE_OPTION_KEY, langCode.code);
    }
    private void LoadOptionFromSave()
    {
        var langCode = Options.GetString(Options.LANUGAGE_OPTION_KEY, "en");
        TranslationServer.SetLocale(langCode);

        for (int i = 0; i < languages.Count; i++)
        {
            if (languages[i].code == langCode)
            {
                this.Select(i);
                break;
            }
        }
    }

    private void LoadOptions()
    {
        this.Clear();
        foreach (var langCode in languages)
        {
            this.AddItem($"{langCode.name}");
        }
    }
}
