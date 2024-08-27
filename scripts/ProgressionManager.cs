using Godot;
using System.Collections.Generic;

public partial class ProgressionManager : Node {
    private int _currentTier;

    private static Dictionary<string, int> _toolTiers = new Dictionary<string, int> {
        { "TopLeft", 0 },
        { "TopRight", 1 },
        { "BottomLeft", 1 },
        { "BottomRight", 2 },
    };
    
    [Signal]
    public delegate void LevelChangeEventHandler();

    public override void _Ready() {
        _currentTier = 0;
    }

    public void levelUp() {
        _currentTier++;
        EmitSignal(SignalName.LevelChange);
    }

    public void levelDown() {
        _currentTier--;
        EmitSignal(SignalName.LevelChange);
    }

    public string[] getTools() {
       List<string> tools = new List<string>();
        foreach (KeyValuePair<string, int> entry in _toolTiers) {
            if (entry.Value <= _currentTier) {
                tools.Add(entry.Key);
            }
        }

        return tools.ToArray();
    }
    
    public int CurrentTier {
        get => _currentTier;
    }
}