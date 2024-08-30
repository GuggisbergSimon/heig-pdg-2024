using System.Collections.Generic;
using Godot;
using heigpdg2024.scripts.cells;

public partial class GameManager : Node {
    public readonly List<Merger> _mergers = new();
    private readonly List<Source> _sources = new();
    [Export] private PackedScene _noteScene;

    public static GameManager Instance { get; private set; }
    public Node CurrentScene { get; private set; }
    public ProgressionManager ProgressionManager { get; private set; }
    public MusicTilemap Tilemap { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public int Tempo { get; private set; } = 120;
    public float PercentToStartAnims { get; private set; } = 0.1f;
    public Timer TimerTempo { get; private set; }

    public override void _Ready() {
        Instance = this;

        Input.SetMouseMode(Input.MouseModeEnum.Confined);

        //Timer setup
        TimerTempo = GetNode<Timer>("Timer");
        TimerTempo.SetWaitTime(60f / Tempo);
        TimerTempo.Autostart = true;
        TimerTempo.Timeout += OnTempo;
        TimerTempo.Start();

        // Scene Manager behaviour
        Viewport root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
        ProgressionManager =
            GetNode<ProgressionManager>("ProgressionManager");
        AudioManager = GetNode<AudioManager>("AudioManager");
    }

    private void OnTempo() {
        foreach (var source in _sources) {
            var output = Tilemap.GetInput(source.Position, source.Output);
            if (output == null || output.IsBusy) continue;

            if (output.IsCompatible(source.Output)) {
                var note = _noteScene.Instantiate<Note>();
                note.Position = source.Position;
                GetTree().Root.AddChild(note);
                output.Process(note);
            }
        }

        foreach (var merger in _mergers) {
            GD.Print("clear");
            merger.ClearMemory();
        }
    }

    public void RegisterTilemap(MusicTilemap tilemap) {
        Tilemap = tilemap;
    }

    public void RegisterSource(Source source) {
        _sources.Add(source);
    }

    public void RegisterMerger(Merger merger) {
        _mergers.Add(merger);
    }

    public void UnregisterSource(Source source) {
        _sources.Remove(source);
    }

    public void UnregisterMerger(Merger merger) {
        _mergers.Remove(merger);
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