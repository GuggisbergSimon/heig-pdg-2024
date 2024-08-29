using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class ProgressionManager : Node {
    private int _currentTier;

    private static Dictionary<string, int> _toolTiers = new Dictionary<string, int> {
        {"Source", 0},
        {"Merger", 1},
        {"ShiftUp", 1},
        {"ShiftDown", 2},
        {"SpeedUp", 3},
        {"SpeedDown", 3},
    };

    private static Godot.Collections.Dictionary<string, int> _levelRequirements = new Godot.Collections.Dictionary<string, int> {
        { "Requirement 1", 0 },
        { "Requirement 2", 1 },
        { "Requirement 3", 1 },
        { "Requirement 4", 2 },
    };
    
    [Signal]
    public delegate void LevelChangeEventHandler();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        _currentTier = 0;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
    }

    public void levelUp() {
        _currentTier++;
        EmitSignal(SignalName.LevelChange);
    }

    public void levelDown() {
        _currentTier--;
        EmitSignal(SignalName.LevelChange);
    }

    public Dictionary<string, bool> getTools() {
        Dictionary<string, bool> tools = new Dictionary<string, bool>();
        foreach (KeyValuePair<string, int> entry in _toolTiers) {

                tools.Add(entry.Key, entry.Value <= _currentTier);
        }

        return tools;
    }
    
    public int CurrentTier {
        get => _currentTier;
    }

    public string[] getLevelRequirements() {
        List<string> requirements = new List<string>();
        foreach (KeyValuePair<string, int> entry in _levelRequirements) {
            if (entry.Value == _currentTier) {
                requirements.Add(entry.Key);
            }
        }

        return requirements.ToArray();
    }
}