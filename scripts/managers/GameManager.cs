using Godot;
using heigpdg2024.scripts.tiles;

namespace heigpdg2024.scripts.managers;

/// <summary>
/// Class representing a game manager as a static singleton
/// </summary>
public partial class GameManager : Node {
    [Export] private float _percentToStartAnims = 0.4f;
    public static GameManager Instance { get; private set; }
    public Node CurrentScene { get; private set; }
    public ProgressionManager ProgressionManager { get; private set; }
    public MusicTilemap Tilemap { get; private set; }
    public AudioManager AudioManager { get; private set; }
    private int _tempo;

    public int Tempo {
        get => _tempo;
        set {
            _tempo = value;
            float waitTime = 60f / _tempo;
            TimerTempo?.SetWaitTime(waitTime);
            TimerTempoAnimation = _percentToStartAnims * waitTime;
        }
    }

    public float TimerTempoAnimation { get; private set; }

    public Timer TimerTempo { get; private set; }

    public override void _Ready() {
        Instance = this;

        ProgressionManager = GetNode<ProgressionManager>("ProgressionManager");
        ProgressionManager.Initialize();

        Input.SetMouseMode(Input.MouseModeEnum.Confined);

        // Timer setup
        TimerTempo = GetNode<Timer>("Timer");
        TimerTempo.SetWaitTime(60f / _tempo);
        TimerTempo.Autostart = true;
        TimerTempo.Start();

        // Scene Manager setup
        Viewport root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
        AudioManager = GetNode<AudioManager>("AudioManager");
    }

    /// <summary>
    /// Registers a tilemap to be referenced for other scripts
    /// </summary>
    /// <param name="tilemap"></param>
    public void RegisterTilemap(MusicTilemap tilemap) {
        Tilemap = tilemap;
    }

    public void Pause() {
        TimerTempo.Stop();
    }

    public void Play() {
        TimerTempo.Start();
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