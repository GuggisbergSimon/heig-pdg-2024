using System;
using Godot;
using Godot.Collections;

public partial class NoteVisualizer : Node {
    [Export] private Texture2D[] _sprites;
    [Export] private PackedScene _sprite;
    [Export] private PackedScene _line;

    private Dictionary<PitchNotation, Node2D> _positions = new();

    public override void _Ready() {
        base._Ready();

        //Positions of each note 
        foreach (var pitch in Enum.GetValues<PitchNotation>()) {
            _positions.Add(pitch, GetNode<Node2D>(pitch.ToString()));
        }

        //Staff is only drawn on some lines
        string[] lines = { "E", "G", "B", "Dva", "Fva" };
        foreach (var line in lines) {
            GetNode<Node2D>(line).AddChild(_line.Instantiate<Sprite2D>());
        }

        //Test purposes
        Note note = new Note(InstrumentType.Piano, PitchNotation.G, DurationNotation.Quaver);
        note.AddNote(new Note(InstrumentType.Piano, PitchNotation.C, DurationNotation.Quaver));

        //TODO add special symbol under the staff if C or D
        foreach (var pitch in note.Pitches) {
            var spriteInstance = _sprite.Instantiate<Sprite2D>();
            AddChild(spriteInstance);
            //Duration and pitch setup
            spriteInstance.Texture = _sprites[(int)note.Duration];
            spriteInstance.Position = _positions[pitch].Position;
        }
    }
}