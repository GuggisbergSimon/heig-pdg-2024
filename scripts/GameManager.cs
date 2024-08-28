using System.Collections.Generic;
using Godot;

public partial class GameManager : Node {
    public static GameManager Instance { get; private set; }
    public Node CurrentScene { get; private set; }
    public ProgressionManager ProgressionManager { get; private set; }
    public MusicTilemap Tilemap { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public int Tempo { get; private set; } = 120;
    public float PercentToStartAnims { get; private set; } = 0.1f;
    private List<Note> _notes = new();

    public override void _Ready() {
        Instance = this;

        // Scene Manager behaviour
        Viewport root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
        ProgressionManager = GetNode<ProgressionManager>("ProgressionManager");
        AudioManager = GetNode<AudioManager>("AudioManager");
    }

    //TODO every x seconds depending on tempo, call for each note their Process method (need to create it as godot default one is happening every frame)
    public override void _Process(double delta) { }

    public void RegisterTilemap(MusicTilemap tilemap) {
        Tilemap = tilemap;
    }

    public void RegisterNote(Note note) {
        //TODO be called by source when instantiating a note
        _notes.Add(note);
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