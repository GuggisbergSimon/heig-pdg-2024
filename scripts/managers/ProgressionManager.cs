using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class ProgressionManager : Node {
    [Export] private Requirement[] _requirementsResources;

    private static Dictionary<BlockType, int> _toolTiers = new Dictionary<BlockType, int> {
        { BlockType.Belt, 0 },
        { BlockType.Source, 0 },
        { BlockType.Speaker, 0 },
        { BlockType.Instrument1, 0 },
        { BlockType.ShiftUp, 1 },
        { BlockType.ShiftDown, 1 },
        { BlockType.SpeedUp, 2 },
        { BlockType.SpeedDown, 2 },
        { BlockType.Merger, 3 },
        { BlockType.Instrument2, 4 },
    };

    private List<Requirement> _todos = new();

    [Signal]
    public delegate void LevelChangeEventHandler();

    public int CurrentTier { get; private set; }

    public override void _Ready() {
        base._Ready();
        UpdateTodos();
    }

    public void TryRequirement(Note note) {
        foreach (var requirement in _todos) {
            if (requirement.Duration != note.Duration.Notation || requirement.Instrument != note.Instrument ||
                requirement.Pitches.Length != note.Pitches.Count) {
                continue;
            }

            List<PitchNotation> requiredPitches = requirement.Pitches.Select(pitch => (PitchNotation)pitch).ToList();
            requiredPitches.Sort();
            note.Pitches.Sort();
            if (requiredPitches.Where((t, i) => t != note.Pitches[i]).Any()) {
                return;
            }

            _todos.Remove(requirement);
            break;
        }
        
        if (_todos.Count == 0) {
            LevelUp();
        }
    }

    public void LevelUp() {
        CurrentTier++;
        UpdateTodos();
        EmitSignal(SignalName.LevelChange);
    }

    public void LevelDown() {
        CurrentTier--;
        UpdateTodos();
        EmitSignal(SignalName.LevelChange);
    }

    public Dictionary<BlockType, bool> GetTools() {
        return _toolTiers.ToDictionary(entry => entry.Key, entry => entry.Value <= CurrentTier);
    }

    private void UpdateTodos() {
        _todos = _requirementsResources.Where(requirement => requirement.Level == CurrentTier).ToList();
    }

    public string[] GetLevelRequirements() {
        return (from requirement in _requirementsResources
            where requirement.Level == CurrentTier
            select requirement.ToString()).ToArray();
    }
}