using Godot;

[GlobalClass]
public partial class GameScene : Node
{
    public override void _Ready()
    {
        SceneManager.SetCurrentScene(this);
        Debug.Log($"Current Locale {TranslationServer.GetLocale()}");
    }
}
