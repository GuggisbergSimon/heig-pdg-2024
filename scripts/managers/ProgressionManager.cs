using Godot;
using System.Collections.Generic;
using System.Linq;
using heigpdg2024.scripts.notes;
using heigpdg2024.scripts.resources;

namespace heigpdg2024.scripts.managers;

/// <summary>
/// Class representing a progression manager, handling the progress of the player
/// </summary>
public partial class ProgressionManager : Node {
    [Export] private Level[] _levelsResources;

    private static readonly BlockType[] _defaultTools = {
        BlockType.Belt,
        BlockType.Source,
        BlockType.Speaker
    };

    private static Dictionary<BlockType, bool> _toolsUnlocked =
        new Dictionary<BlockType, bool> {
            { BlockType.Belt, true },
            { BlockType.Source, true },
            { BlockType.Speaker, true },
            { BlockType.Instrument1, false },
            { BlockType.ShiftUp, false },
            { BlockType.ShiftDown, false },
            { BlockType.SpeedUp, false },
            { BlockType.SpeedDown, false },
            { BlockType.Merger, false },
            { BlockType.Instrument2, false },
        };

    private List<Requirement> _todos = new();

    /// <summary>
    /// Signal called on level change
    /// </summary>
    [Signal]
    public delegate void LevelChangeEventHandler();

    public int CurrentTier { get; private set; }

    public void Initialize() {
        base._Ready();
        LevelChangement(0);
    }

    /// <summary>
    /// Try to match a note to current requirements
    /// </summary>
    /// <param name="note">The note to try to match</param>
    public void TryRequirement(Note note) {
        foreach (var requirement in _todos) {
            if (requirement.Duration != note.Duration.Notation ||
                requirement.Instrument != note.Instrument ||
                requirement.Pitches.Length != note.Pitches.Count) {
                continue;
            }

            List<PitchNotation> requiredPitches = requirement.Pitches
                .Select(pitch => (PitchNotation)pitch).ToList();
            requiredPitches.Sort();
            note.Pitches.Sort();
            if (requiredPitches.Where((t, i) => t != note.Pitches[i])
                .Any()) {
                return;
            }

            _todos.Remove(requirement);
            break;
        }

        if (_todos.Count == 0) {
            LevelUp();
        }
    }

    /// <summary>
    /// Level up the current tier
    /// </summary>
    public void LevelUp() {
        LevelChangement(CurrentTier++);
        GameManager.Instance.AudioManager.PlayLevelUpSound();
    }

    /// <summary>
    /// Level down the current tier
    /// </summary>
    public void LevelDown() {
        LevelChangement(CurrentTier--);
    }

    private void LevelChangement(int oldTier) {
        Level level =
            _levelsResources.FirstOrDefault(lvl =>
                lvl.Tier == CurrentTier);
        if (level == null) {
            CurrentTier = oldTier;
            return;
        }

        GameManager.Instance.Tempo = level.Tempo;
        UpdateTools(level);
        UpdateTodos(level);
        EmitSignal(SignalName.LevelChange);
    }

    /// <summary>
    /// Get the tools available for current tier
    /// </summary>
    /// <returns>A dictionary of tools and whether they're enabled, or not</returns>
    public Dictionary<BlockType, bool> GetTools() {
        return _toolsUnlocked;
    }

    private void UpdateTools(Level level) {
        foreach (var tool in _toolsUnlocked.Keys.ToList()) {
            _toolsUnlocked[tool] = false;
        }

        foreach (var tool in _defaultTools) {
            _toolsUnlocked[tool] = true;
        }

        if (level != null) {
            foreach (BlockType tool in level.Tools) {
                _toolsUnlocked[tool] = true;
            }
        }
    }

    private void UpdateTodos(Level level) {
        if (level != null) {
            _todos = level.Requirements != null
                ? level.Requirements.ToList()
                : new List<Requirement>();
        }
        else {
            _todos.Clear();
        }
    }

    /// <summary>
    /// Get the requirements for current tier
    /// </summary>
    /// <returns>An array of the requirements, as string</returns>
    public string[] GetLevelRequirements() {
        Level level =
            _levelsResources.FirstOrDefault(lvl =>
                lvl.Tier == CurrentTier);

        if (level != null && level.Requirements != null) {
            return level.Requirements.Select(req => req.ToString())
                .ToArray();
        }

        return new string[0];
    }
}