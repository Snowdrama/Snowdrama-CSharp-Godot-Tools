using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class TwitchHistory : Node
{
	[Export] TwitchAlertQueue queue;
	[Export] VBoxContainer targetContainer;


	CompletedAlertsChangedMessage alertsCompleted;
    int _lastAlert = -1;
	List<AlertData> alerts = new List<AlertData>();

	List<TwitchAlertButton> buttonList = new List<TwitchAlertButton>();

    public override void _EnterTree()
    {
		alertsCompleted = Messages.Get<CompletedAlertsChangedMessage>();
        alertsCompleted.AddListener(AlertsCompleted);
    }

    public override void _ExitTree()
    {
		alertsCompleted.RemoveListener(AlertsCompleted);
		Messages.Return<CompletedAlertsChangedMessage>();
    }

	public void AlertsCompleted()
    {
        GD.Print("Getting Updated Queue");
        _lastAlert = queue.GetLastAlertId();
        alerts = queue.GetCompletedAlerts().ToList();
        UpdateAlertList();
    }

	public void UpdateAlertList()
	{
		GD.Print("Clearing Buttons");
		foreach (var button in buttonList)
		{
			button.QueueFree();
		}
        buttonList.Clear();

        GD.Print("Adding New Buttons");
        for (int i = 0; i < alerts.Count; i++)
        {
            GD.Print($"Button For{alerts[0].UserName}");
            var newButton = new TwitchAlertButton()
			{
				Text = $"{alerts[0].UserName} : {alerts[0].UserMessage}",
            };
            newButton.alertData = alerts[i];
			buttonList.Add(newButton);
            targetContainer.AddChild(newButton);
        }
    }
}
