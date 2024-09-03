using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using heigpdg2024.scripts.managers;
using heigpdg2024.scripts.resources;

namespace heigpdg2024.scripts.notes;

public enum PitchNotation {
    C = 0, //Do
    D = 1, //Ré
    E = 2, //Mi
    F = 3, //Fa
    G = 4, //Sol
    A = 5, //La
    B = 6 //Si
}

/// <summary>
/// Class representing a note, comprised of multiple pitches
/// </summary>
public partial class Note : Node2D {
    [Export] private Duration[] _durationsResources;
    [Export] private Instrument[] _instrumentsResources;
    [Export] private PackedScene _singleNoteScene;
    [Export] private PackedScene _whiteDotScene;
    [Export] private PackedScene _lineStaffScene;
    private static readonly DurationNotation MAX_DURATION = DurationNotation.Quarter;
    private static readonly DurationNotation MIN_DURATION = DurationNotation.Half;
    private readonly Dictionary<PitchNotation, Node2D> _positions = new();
    private readonly Dictionary<DurationNotation, Duration> _durations = new();
    private readonly Dictionary<InstrumentType, Instrument> _instruments = new();
    private readonly List<Sprite2D> _notesSprites = new();
    private readonly List<Sprite2D> _whiteDotSprites = new();
    public Duration Duration { get; private set; }
    public InstrumentType Instrument { get; private set; }
    public List<PitchNotation> Pitches { get; } = new();

    public override void _Ready() {
        base._Ready();

        // Positions of each note 
        foreach (var pitch in Enum.GetValues<PitchNotation>()) {
            _positions.Add(pitch, GetNode<Node2D>(pitch.ToString()));
        }

        // Staff is only drawn on some lines
        string[] lines = { "E", "G", "B", "Dva", "Fva" };
        foreach (var line in lines) {
            GetNode<Node2D>(line).AddChild(_lineStaffScene.Instantiate<Sprite2D>());
        }

        foreach (var pitch in Pitches) {
            CreateNoteSprite(pitch);
        }

        GameManager.Instance.TimerTempo.Timeout += Process;
    }

    public override void _Notification(int what) {
        base._Notification(what);
        if (what == NotificationPredelete) {
            GameManager.Instance.TimerTempo.Timeout -= Process;
        }
    }

    /// <summary>
    /// Process a note at each tempo beat
    /// </summary>
    private void Process() {
        var cell = GameManager.Instance.Tilemap.GetProcessor(Position);
        if (cell == null) {
            //TODO destroy animation
            QueueFree();
        }
        else {
            cell.Process(this);
        }
    }

    /// <summary>
    /// Initialize a note
    /// </summary>
    /// <param name="duration">The duration of the note</param>
    /// <param name="pitch">The pitch of the note</param>
    public void Initialize(DurationNotation duration, PitchNotation pitch) {
        foreach (var durationsResource in _durationsResources) {
            _durations.Add(durationsResource.Notation, durationsResource);
        }

        foreach (var instrumentsResource in _instrumentsResources) {
            _instruments.Add(instrumentsResource.Type, instrumentsResource);
        }

        // A note starts with no instrument
        Instrument = InstrumentType.None;
        Duration = _durations[duration];
        Pitches.Add(pitch);
    }

    /// <summary>
    /// Move a note towards a given position, in the timeframe allowed per beat
    /// </summary>
    /// <param name="to">The world position to which the note will move</param>
    public void MoveByTempo(Vector2 to) {
        MoveByTempo(to, Callable.From(() => { }));
    }

    /// <summary>
    /// Move a note towards a given position, in the timeframe allowed per beat
    /// </summary>
    /// <param name="to">The world position to which the note will move</param>
    /// <param name="callback">A callback function to call after having moved</param>
    public void MoveByTempo(Vector2 to, Callable callback) {
        var tween = GetTree().CreateTween();
        var duration = 60 * GameManager.Instance.PercentToStartAnims /
                       GameManager.Instance.Tempo;
        tween.TweenProperty(this, "position", to, duration);
        tween.TweenCallback(callback);
    }

    /// <summary>
    /// Change the duration of the note, comprised between MAX_DURATION and MIN_DURATION
    /// </summary>
    /// <param name="value">the value to increment, -1 and 1 are supported</param>
    public void DurationChange(int value) {
        var notation = Duration.Notation;
        switch (value) {
            case > 0 when notation == MAX_DURATION:
            case < 0 when notation == MIN_DURATION:
                return;
            default:
                Duration = _durations[(DurationNotation)((int)notation + value)];
                UpdateSpriteDuration();
                break;
        }
    }

    /// <summary>
    /// Change the pitches by an increment
    /// </summary>
    /// <param name="value">The value to increment, -1 and 1 are supported</param>
    public void PitchChange(int value) {
        switch (value) {
            case > 0 when Pitches.Any(pitch => pitch == PitchNotation.B):
            case < 0 when Pitches.Any(pitch => pitch == PitchNotation.C):
                return;
        }

        for (var i = 0; i < Pitches.Count; i++) {
            Pitches[i] = (PitchNotation)((int)Pitches[i] + value);
        }

        UpdateSpritePitch();
    }

    /// <summary>
    /// Add another note to the current one, then delete the other
    /// Notes can be added only if they share the same instrument and the same duration
    /// </summary>
    /// <param name="note">The other note to add</param>
    /// <returns>true if the operation succeeded, false otherwise</returns>
    public bool AddNote(Note note) {
        if (note.Instrument != Instrument || note.Duration != Duration) {
            return false;
        }

        foreach (var pitch in note.Pitches.Where(pitch => !Pitches.Contains(pitch))) {
            Pitches.Add(pitch);
            CreateNoteSprite(pitch);
        }

        // Delete the merged note 
        note.QueueFree();
        return true;
    }

    /// <summary>
    /// Change the instrument of the note
    /// </summary>
    /// <param name="instrument">The instrument to change to</param>
    public void InstrumentChange(InstrumentType instrument) {
        Instrument = instrument;
        foreach (var sprite in _notesSprites) {
            sprite.Modulate = _instruments[instrument].Color;
        }
    }

    /// <summary>
    /// Create the sprite for a note based on a single pitch
    /// </summary>
    /// <param name="pitch">The pitch to create a sprite from</param>
    private void CreateNoteSprite(PitchNotation pitch) {
        // Duration and pitch setup
        var sprite = _singleNoteScene.Instantiate<Sprite2D>();
        sprite.Texture = Duration.Sprite;
        sprite.Position = _positions[pitch].Position;
        sprite.Modulate = _instruments[Instrument].Color;
        AddChild(sprite);

        var whiteDotSprite = _whiteDotScene.Instantiate<Sprite2D>();
        whiteDotSprite.Texture = Duration.SpriteWhiteDot;
        whiteDotSprite.Position = _positions[pitch].Position;
        whiteDotSprite.Visible = Duration.SpriteWhiteDot != null;
        AddChild(whiteDotSprite);

        _notesSprites.Add(sprite);
        _whiteDotSprites.Add(whiteDotSprite);
    }

    /// <summary>
    /// Update the sprite of a note when the duration has changed
    /// </summary>
    private void UpdateSpriteDuration() {
        foreach (var sprite in _notesSprites) {
            sprite.Texture = Duration.Sprite;
        }

        foreach (var whiteDot in _whiteDotSprites) {
            whiteDot.Visible = Duration.SpriteWhiteDot != null;
        }
    }

    /// <summary>
    /// Update the sprite of a note when the pitches have changed
    /// </summary>
    private void UpdateSpritePitch() {
        for (var i = 0; i < _notesSprites.Count; i++) {
            _notesSprites[i].Position = _positions[Pitches[i]].Position;
        }

        for (var i = 0; i < _whiteDotSprites.Count; i++) {
            _whiteDotSprites[i].Position = _positions[Pitches[i]].Position;
        }
    }
}