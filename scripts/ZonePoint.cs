using Godot;

public partial class ZonePoint : Area2D
{
    [Export] public string NextScene;

    public void OnAreaEntered(Area2D area)
    {
        // GetTree().ChangeSceneToFile(NextScene) is not deferred as of 4.2 
        // so it causes errors within a physics callback (e.g. OnAreaEntered)
        // https://github.com/godotengine/godot/issues/85852
        GetTree().CallDeferred("change_scene_to_file", NextScene);
    }
}
