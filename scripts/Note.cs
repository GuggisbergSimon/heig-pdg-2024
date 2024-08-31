using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public enum PitchNotation {
    C, //Do
    D, //Ré
    E, //Mi
    F, //Fa
    G, //Sol
    A, //La
    B //Si
}

public enum InstrumentType {
    Piano,
    Guitar
}

public partial class Note : Node2D {
    private static readonly DurationNotation MAX_DURATION = DurationNotation.Minim;
    private static readonly DurationNotation MIN_DURATION = DurationNotation.Crotchet;
    [Export] private Duration[] _durationsResources;
    [Export] private PackedScene _lineStaffScene;
    private Dictionary<PitchNotation, Node2D> _positions = new();
    private Dictionary<DurationNotation, Duration> _durations = new();
    private List<List<Sprite2D>> _notesSprites = new();
    public Duration Duration { get; private set; }
    public InstrumentType Instrument { get; set; }
    public List<PitchNotation> Pitches { get; } = new();

    private static DurationNotation MAX_DURATION = DurationNotation.Minim;
    private static DurationNotation MIN_DURATION = DurationNotation.Crotchet;

    public override void _Ready() {
        base._Ready();

        //Setups dictionary
        foreach (var durationsResource in _durationsResources)
            _durations.Add(durationsResource.Notation, durationsResource);
        }

        Pitches.Add(PitchNotation.C);
        Duration = _durations[DurationNotation.Minim];

        //Positions of each note 
        foreach (var pitch in Enum.GetValues<PitchNotation>())
            _positions.Add(pitch, GetNode<Node2D>(pitch.ToString()));

        //Staff is only drawn on some lines
        string[] lines = { "E", "G", "B", "Dva", "Fva" };
        foreach (var line in lines)
            GetNode<Node2D>(line)
                .AddChild(_lineStaffScene.Instantiate<Sprite2D>());

        //TODO add special symbol under the staff if C or D
        foreach (var pitch in Pitches) {
            CreateNoteSprite(pitch);
        }

        GameManager.Instance.TimerTempo.Timeout += Process;
    }

    public override void _Notification(int what) {
        base._Notification(what);
        if (what == NotificationPredelete)
            GameManager.Instance.TimerTempo.Timeout -= Process;
    }

    private void Process() {
        var input = GameManager.Instance.Tilemap.GetProcessor(Position);
        input?.Process(this);
    }

    public override string ToString() {
        return Instrument + " : " + Duration + " : " + Pitches;
    }

    public void MoveByTempo(Vector2 to) {
        var tween = GetTree().CreateTween();
        var duration = 60 * GameManager.Instance.PercentToStartAnims /
                       GameManager.Instance.Tempo;
        tween.TweenProperty(this, "position", to, duration);
    }

    public void DurationChange(int value) {
        var notation = Duration.Notation;
        switch (value) {
            case > 0 when notation == MAX_DURATION:
            case < 0 when notation == MIN_DURATION:
                return;
            default:
                Duration.Notation =
                    (DurationNotation)((int)notation + value);
                //TODO sprite update
                break;
        }
    }

    public void PitchChange(int value) {
        switch (value) {
            case > 0 when Pitches.Any(pitch => pitch == PitchNotation.B):
            case < 0 when Pitches.Any(pitch => pitch == PitchNotation.C):
                return;
        }

        for (var i = 0; i < Pitches.Count; i++)
            Pitches[i] = (PitchNotation)((int)Pitches[i] + 1);

        //sprite update
        UpdateNotePitch();
    }

    public bool AddNote(Note note) {
        if (note.Instrument != Instrument || note.Duration != Duration)
            return false;

        foreach (var pitch in note.Pitches.Where(pitch =>
                     !Pitches.Contains(pitch))) {
            Pitches.Add(pitch);
            CreateNoteSprite(pitch);
        }

        //Deletes the merged note 
        note.QueueFree();
        return true;
    }

    public void InstrumentChange(InstrumentType instrument) {
        Instrument = instrument;
        foreach (var notesSprite in _notesSprites) {
            notesSprite[0].Modulate = instrument == InstrumentType.Guitar ? Colors.Red : Colors.Black;
        }
        return true;
    }

    private void CreateNoteSprite(PitchNotation pitch) {
        var spriteList = new List<Sprite2D>();
        var spriteInstance = _singleNoteScene.Instantiate<Sprite2D>();
        //Duration and pitch setup
        spriteInstance.Texture = Duration.Sprite;
        spriteInstance.Position = _positions[pitch].Position;
        spriteInstance.Modulate = Instrument == InstrumentType.Guitar ? Colors.Red : Colors.Black;
        spriteInstance.ZIndex = 0;

        // Add a white spot to the note if it's a minim or a semibreve
        if (Duration.SpriteWhiteDot != null) {
            var topSprite = new Sprite2D();
            topSprite.Texture = Duration.SpriteWhiteDot;
            topSprite.Position = _positions[pitch].Position;
            topSprite.ZIndex = 1;
            AddChild(topSprite);
            spriteList.Add(topSprite);
        }

        AddChild(spriteInstance);
        // Add the note to the start of the list
        spriteList.Insert(0, spriteInstance);

        _notesSprites.Add(spriteList);
    }

    private void UpdateNotePitch() {
        for (var i = 0; i < _notesSprites.Count; i++) {
            foreach (var sprite in _notesSprites[i]) {
                sprite.Position = _positions[Pitches[i]].Position;
            }
        }
    }
}