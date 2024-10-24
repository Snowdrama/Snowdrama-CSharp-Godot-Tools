using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class LocalizedTextureRect : TextureRect
{
	[Export]
	Array<LocalizedTextureRectData> localizedTextureList = new Array<LocalizedTextureRectData>()
	{
		new LocalizedTextureRectData{code = "en" , texture = null},
		new LocalizedTextureRectData{code = "es" , texture = null},
		new LocalizedTextureRectData{code = "fr" , texture = null},
		new LocalizedTextureRectData{code = "de" , texture = null},
		new LocalizedTextureRectData{code = "zh" , texture = null},
		new LocalizedTextureRectData{code = "ja" , texture = null},
		new LocalizedTextureRectData{code = "ru" , texture = null},
		new LocalizedTextureRectData{code = "ar" , texture = null},
		new LocalizedTextureRectData{code = "pt" , texture = null},
		new LocalizedTextureRectData{code = "it" , texture = null},
	};

	Dictionary<string, Texture2D> localizedTextures = new Dictionary<string, Texture2D>();

	Texture2D defaultTexture;
    string currentLocale = "en";
	public override void _Ready()
	{
		foreach (var item in localizedTextureList)
		{
			//Debug.Log($"Adding {item.code} texture {item.texture} is null? {item.texture == null}");
			localizedTextures.Add(item.code, item.texture);
		}
		LoadOptionFromSave();
		currentLocale = TranslationServer.GetLocale();
		defaultTexture = this.Texture;
        UpdateTexture();
    }

	public override void _Process(double delta)
	{
		UpdateTexture();
    }

	private void UpdateTexture()
	{
		if(currentLocale !=  TranslationServer.GetLocale())
		{
			//Debug.Log($"Updating Locale to {TranslationServer.GetLocale()}");
			currentLocale = TranslationServer.GetLocale();
			LoadFromLocale(currentLocale);
		}
    }

	private void LoadFromLocale(string locale)
    {
        if (localizedTextures.ContainsKey(locale))
        {
            //Debug.Log($"localizedTextures contains key: {TranslationServer.GetLocale()}");
            if (localizedTextures[locale] != null)
            {
                //Debug.Log($"localizedTextures texture not null: {TranslationServer.GetLocale()}");
                this.Texture = localizedTextures[locale];
            }
            else
            {
                //Debug.Log($"localizedTextures is null apparently: {TranslationServer.GetLocale()}");
                this.Texture = defaultTexture;
            }
        }
    }
    private void LoadOptionFromSave()
    {
        var langCode = Options.GetString(Options.LANUGAGE_OPTION_KEY, "en");
        TranslationServer.SetLocale(langCode);
		LoadFromLocale(langCode);
    }
}
