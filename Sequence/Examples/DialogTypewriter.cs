using Godot;
using System;

public partial class DialogTypewriter : SequenceNode
{
    [Export] string character;
    [Export(PropertyHint.MultilineText)] string description;
    [Export] TextureRect portraitImage;
    [Export] Label nameLabel;
    [Export] RichTextLabel descriptionLabel;
    [Export] Panel dialogPanel;    

    int currentCharacterCount = 0;
    float characterTime = 0;
    float characterTime_Max;

    float waitTimeAfterEnd_Max = 0.5f;
    float waitTimeAfterEnd = 0.0f;

    [Export] TextureRect nextArrow;
    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
        if(State == SequenceState.Playing)
        {
            if(currentCharacterCount < descriptionLabel.Text.Length)
            {
                characterTime += (float)delta;
                if (characterTime >= characterTime_Max)
                {
                    characterTime = 0;
                    currentCharacterCount++;
                }

                descriptionLabel.VisibleCharacters = currentCharacterCount;
            }
            else
            {
                waitTimeAfterEnd += (float)delta;
                if(waitTimeAfterEnd >= waitTimeAfterEnd_Max)
                {
                    GD.Print("NOW I'm completed...");
                    State = SequenceState.Completed;
                    nextArrow.Show();
                }
            }
        }
    }

    public override void PlaySequence()
    {
        GD.Print($"Playing Sequence: {this.Name}");
        nextArrow.Hide();

        nameLabel.Text = character;
        currentCharacterCount = 0;
        descriptionLabel.VisibleCharacters = currentCharacterCount;
        descriptionLabel.Text = description;
        this.State = SequenceState.Playing;

    }

    public override void ForceComplete()
    {
        //force it to be finished
        State = SequenceState.Completed;
        descriptionLabel.Text = description;
        descriptionLabel.VisibleCharacters = description.Length;
        nextArrow.Show();
    }

    public override void LoadSequence()
    {
        dialogPanel.Show();
    }
    public override void UnloadSequence()
    {
        dialogPanel.Hide();
    }
}
