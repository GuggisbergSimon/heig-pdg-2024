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
    [Export] private PackedScene _confettiScene;
    private Confetti _confetti;

    private static readonly BlockType[] _defaultTools = {
        BlockType.Belt,
        BlockType.Source,
        BlockType.Speaker
    };

    private static Dictionary<BlockType, bool> _toolsUnlocked =
        new Dictionary<BlockType, bool> {
            { BlockType.Belt, false },
            { BlockType.Source, false },
            { BlockType.Speaker, false },
            { BlockType.Instrument1, false },
            { BlockType.ShiftUp, false },
            { BlockType.ShiftDown, false },
            { BlockType.Instrument2, false },
            { BlockType.SpeedUp, false },
            { BlockType.SpeedDown, false },
            { BlockType.Merger, false }
        };

    private List<Requirement> _todos = new();

    /// <summary>
    /// Signal called on level change
    /// </summary>
    [Signal]
    public delegate void LevelChangeEventHandler();

    /// <summary>
    /// Signal called when a requirement is completed
    /// </summary>
    [Signal]
    public delegate void RequirementCompletedEventHandler(string requirement);

    public int CurrentTier { get; private set; }

    /// <summary>
    /// Initializes the progression manager by setting up the initial level.
    /// </summary>
    public void Initialize() {
        base._Ready();
        ApplyLevel(CurrentTier);
        _confetti = _confettiScene.Instantiate<Confetti>();
        AddChild(_confetti);
    }

    /// <summary>
    /// Try to match a note to current requirements
    /// </summary>
    /// <param name="note">The note to try to match</param>
    /// <param name="position">The position the note is validated</param>
    public void TryRequirement(Note note, Vector2 position) {
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
                continue;
            }

            _todos.Remove(requirement);
            EmitSignal(nameof(RequirementCompleted), requirement.ToString());
            break;
        }

        if (_todos.Count == 0) {
            _confetti.Position = position;
            _confetti.Emit();
            LevelUp();
        }
    }

    /// <summary>
    /// Level up the current tier
    /// </summary>
    public void LevelUp() {
        GameManager.Instance.AudioManager.PlayLevelUpSound();
        ChangeLevel(CurrentTier + 1);
    }

    /// <summary>
    /// Level down the current tier
    /// </summary>
    public void LevelDown() {
        if (CurrentTier > 0) {
            UpdateTools(_levelsResources.FirstOrDefault(lvl => lvl.Tier == CurrentTier), false);
            ChangeLevel(CurrentTier - 1);
        }
    }

    /// <summary>
    /// Get the tools available for current tier
    /// </summary>
    /// <returns>A dictionary of tools and whether they're enabled, or not</returns>
    public Dictionary<BlockType, bool> GetTools() {
        return _toolsUnlocked;
    }

    /// <summary>
    /// Update the tools based on the level
    /// </summary>
    /// <param name="level">The level containing the tools to update</param>
    /// <param name="unlock">Whether to unlock or lock the tools</param>
    private void UpdateTools(Level level, bool unlock = true) {
        if (level != null) {
            foreach (BlockType tool in level.Tools) {
                _toolsUnlocked[tool] = unlock;
            }
        }
    }

    /// <summary>
    /// Update the todos list based on the level
    /// </summary>
    /// <param name="level">The level containing the requirements</param>
    private void UpdateTodos(Level level) {
        _todos = level?.Requirements?.ToList() ?? new List<Requirement>();
    }

    /// <summary>
    /// Apply the properties of a level based on its tier
    /// </summary>
    /// <param name="tier">The tier of the level to apply</param>
    private void ApplyLevel(int tier) {
        Level level = _levelsResources.FirstOrDefault(lvl => lvl.Tier == tier);

        if (level != null) {
            GameManager.Instance.Tempo = level.Tempo;
            UpdateTools(level);
            UpdateTodos(level);
            EmitSignal(SignalName.LevelChange);
        }
    }

    /// <summary>
    /// Change the level and apply its properties
    /// </summary>
    /// <param name="newTier">The new tier to change to</param>
    private void ChangeLevel(int newTier) {
        int oldTier = CurrentTier;
        CurrentTier = newTier;

        if (_levelsResources.Any(lvl => lvl.Tier == CurrentTier)) {
            ApplyLevel(CurrentTier);
        }
        else {
            CurrentTier = oldTier;
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