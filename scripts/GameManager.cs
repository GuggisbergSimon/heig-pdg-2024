using Godot;

public partial class GameManager : Node {
    public static GameManager Instance { get; private set; }
    public Node CurrentScene { get; set; }

public override void _Ready() {
    Instance = this;

    // Scene Manager behaviour
    Viewport root = GetTree().Root;
    CurrentScene = root.GetChild(root.GetChildCount() - 1);
}

#region SceneManager

/// <summary>
/// Unloads current scene and loads the one given in argument
/// </summary>
/// <param name="path">"res://scenes/Game.tscn" for example</param>
public void GotoScene(string path) {
    CallDeferred(MethodName.DeferredGotoScene, path);
}

private void DeferredGotoScene(string path) {
    CurrentScene.Free();
    var nextScene = GD.Load<PackedScene>(path);
    CurrentScene = nextScene.Instantiate();
    GetTree().Root.AddChild(CurrentScene);
    GetTree().CurrentScene = CurrentScene;
}

#endregion

}