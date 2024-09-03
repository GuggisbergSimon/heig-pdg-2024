using Godot;
using heigpdg2024.scripts.tiles;

namespace heigpdg2024.scripts.managers;

public partial class GameManager : Node {
	public static GameManager Instance { get; private set; }
	public Node CurrentScene { get; private set; }
	public ProgressionManager ProgressionManager { get; private set; }
	public MusicTilemap Tilemap { get; private set; }
	public AudioManager AudioManager { get; private set; }
	private int _tempo = 120;
	public int Tempo {
		get => _tempo;
		set {
			_tempo = value;
			TimerTempo?.SetWaitTime(60f / _tempo);
		}
	}
	public float PercentToStartAnims { get; private set; } = 0.4f;
	public Timer TimerTempo { get; private set; }

	public override void _Ready() {
		Instance = this;

		Input.SetMouseMode(Input.MouseModeEnum.Confined);

		//Timer setup
		TimerTempo = GetNode<Timer>("Timer");
		TimerTempo.SetWaitTime(60f / _tempo);
		TimerTempo.Autostart = true;
		TimerTempo.Start();

		// Scene Manager setup
		Viewport root = GetTree().Root;
		CurrentScene = root.GetChild(root.GetChildCount() - 1);
		ProgressionManager = GetNode<ProgressionManager>("ProgressionManager");
		AudioManager = GetNode<AudioManager>("AudioManager");
	}

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
