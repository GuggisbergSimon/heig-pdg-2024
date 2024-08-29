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
    [Export] private Duration[] _durationsResources;
    [Export] private PackedScene _singleNoteScene;
    [Export] private PackedScene _lineStaffScene;
    private Dictionary<PitchNotation, Node2D> _positions = new();
    private Dictionary<DurationNotation, Duration> _durations = new ();
    public Duration Duration { get; private set; }
    public InstrumentType Instrument { get; set; }
    public List<PitchNotation> Pitches { get; } = new();

    private static DurationNotation MAX_DURATION = DurationNotation.Minim;
    private static DurationNotation MIN_DURATION = DurationNotation.Crotchet;
    public override void _Ready() {
        base._Ready();

        //Setups dictionary
        foreach (var durationsResource in _durationsResources) {
            _durations.Add(durationsResource.Notation, durationsResource);
        }
        
        Pitches.Add(PitchNotation.C);
        Duration = _durations[DurationNotation.Minim];

        //Positions of each note 
        foreach (var pitch in Enum.GetValues<PitchNotation>()) {
            _positions.Add(pitch, GetNode<Node2D>(pitch.ToString()));
        }

        //Staff is only drawn on some lines
        string[] lines = { "E", "G", "B", "Dva", "Fva" };
        foreach (var line in lines) {
            GetNode<Node2D>(line).AddChild(_lineStaffScene.Instantiate<Sprite2D>());
        }

        //TODO add special symbol under the staff if C or D
        foreach (var pitch in Pitches) {
            CreateNoteSprite(pitch);
        }
    }

    public void Process() {
        var input = GameManager.Instance.Tilemap.GetInput(Position);
        input?.Process(this);
    }

    public override string ToString() {
        return Instrument + " : " + Duration + " : " + Pitches;
    }

    public void MoveByTempo(Vector2 to) {
        Tween tween = GetTree().CreateTween();
        float duration = 60 * GameManager.Instance.PercentToStartAnims / GameManager.Instance.Tempo;
        tween.TweenProperty(this, "position", to, duration);
    }

    //TODO refactor as those 4 functions (duration/pitches/up/down) have a lot in common
    public bool DurationUp() {
        if (Duration == MAX_DURATION) {
            return false;
        }

        Duration.Notation = (DurationNotation)((int)Duration.Notation - 1);
        return true;
    }

    public bool DurationDown() {
        if (Duration == MIN_DURATION) {
            return false;
        }

        Duration.Notation = (DurationNotation)((int)Duration.Notation + 1);
        return true;
    }

    public bool PitchesUp() {
        if (Pitches.Any(pitch => pitch == PitchNotation.B)) {
            return false;
        }
        
        Pitches.ForEach(pitch => pitch = (PitchNotation)((int)pitch + 1));
        return true;
    }

    public bool PitchesDown() {
        if (Pitches.Any(pitch => pitch == PitchNotation.C)) {
            return false;
        }

        Pitches.ForEach(pitch => pitch = (PitchNotation)((int)pitch - 1));
        return true;
    }

    public bool AddNote(Note note) {
        if (note.Instrument != Instrument || note.Duration != Duration) {
            return false;
        }

        foreach (var pitch in note.Pitches.Where(pitch => !Pitches.Contains(pitch))) {
            Pitches.Add(pitch);
            CreateNoteSprite(pitch);
        }

        //Deletes the merged note 
        note.QueueFree();
        return true;
    }
    
    public bool ChangeInstrument(InstrumentType instrument) {
        Instrument = instrument;
        return true;
    }

    private void CreateNoteSprite(PitchNotation pitch) {
        var spriteInstance = _singleNoteScene.Instantiate<Sprite2D>();
        //Duration and pitch setup
        spriteInstance.Texture = Duration.Sprite;
        spriteInstance.Position = _positions[pitch].Position;
        AddChild(spriteInstance);
    }
}