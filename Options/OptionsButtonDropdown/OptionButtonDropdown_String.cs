using Godot;
using System;

[GlobalClass]
public partial class OptionButtonDropdown_String : OptionButton
{
    [Export(hintString: "The key of the item in the option object")] string optionKey;
    [Export] string optionDefaultValue;
    [Export] OptionButtonDropdown_String_Resource options;
    public override void _Ready()
    {
        LoadDataIntoOptionButton();

        this.ItemSelected += OnItemSelected;
        this.VisibilityChanged += OnVisibilityChanged;
        LoadValueFromOptions();
    }

    private void OnItemSelected(long index)
    {
        Options.SetString(optionKey, options.optionsData[(int)index].Data);
    }
    private void OnVisibilityChanged()
    {
        LoadDataIntoOptionButton();
        LoadValueFromOptions();
    }

    private void LoadValueFromOptions()
    {
		var loadedValue = Options.GetString(optionKey, optionDefaultValue);

		if (loadedValue != null)
		{
            for (int i = 0; i < options.optionsData.Count; i++)
            {
                if (options.optionsData[i].Data == loadedValue)
                {
                    this.Select(i);
                }
            }
        }
    }


    private void LoadDataIntoOptionButton()
    {
        for(int i = 0;i < options.optionsData.Count;i++)
        {
            if (options.optionsData[i].Separator)
            {
                this.AddSeparator(options.optionsData[i].Text);
            }
            else
            {
                if (options.optionsData[i].Icon == null)
                {
                    this.AddItem(options.optionsData[i].Text, options.optionsData[i].Id);
                }
                else
                {
                    this.AddIconItem(options.optionsData[i].Icon, options.optionsData[i].Text, options.optionsData[i].Id);
                }
            }
        }
    }
}
