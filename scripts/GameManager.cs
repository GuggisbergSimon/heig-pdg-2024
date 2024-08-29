using System.Collections.Generic;
using Godot;
using heigpdg2024.scripts.cells;

public partial class GameManager : Node {
    [Export] private PackedScene _noteScene;
    private readonly List<Note> _notes = new();
    private readonly List<Source> _sources = new();

    private double _timeAccumulator;
    private Timer _timer;
    public static GameManager Instance { get; private set; }
    public Node CurrentScene { get; private set; }
    public ProgressionManager ProgressionManager { get; private set; }
    public MusicTilemap Tilemap { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public int Tempo { get; private set; } = 120;
    public float PercentToStartAnims { get; private set; } = 0.1f;

    public override void _Ready() {
        Instance = this;

        //Timer setup
        _timer = GetNode<Timer>("Timer");
        _timer.SetWaitTime(60f / Tempo);
        _timer.Autostart = true;
        _timer.Timeout += OnTempo;
        _timer.Start();

        // Scene Manager behaviour
        Viewport root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
        ProgressionManager = GetNode<ProgressionManager>("ProgressionManager");
        AudioManager = GetNode<AudioManager>("AudioManager");
    }

    private void OnTempo() {
        foreach (var note in _notes) {
            note.Process();
        }

        foreach (var source in _sources) {
            Processor output = Tilemap.GetInput(source.Position, source.Output);
            if (output == null || output.IsBusy) {
                return;
            }

            var note = _noteScene.Instantiate<Note>();
            note.Position = source.Position;
            GetTree().Root.AddChild(note);
            _notes.Add(note);
            output.Process(note);
        }
    }

    public void RegisterTilemap(MusicTilemap tilemap) {
        Tilemap = tilemap;
    }
    
    public void RegisterSource(Source source) {
        _sources.Add(source);
    }

    #region SceneManager

    /// <summary>
    ///     Unloads current scene and loads the one given in argument
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