using System.Collections.Generic;
using System.Linq;
using Godot;

public enum DurationNotation {
    Semibreve, //Ronde
    Minim, //Blanche
    Crotchet, //Noire
    Quaver //Croche
}

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
    Keyboard,
    Guitar
}

public partial class Note : Node {
    public DurationNotation Duration { get; set; } = DurationNotation.Minim;
    public InstrumentType Instrument { get; set; }

    public List<PitchNotation> Pitches { get; } = new List<PitchNotation>();

    public bool AddNote(Note note) {
        if (note.Instrument != Instrument || note.Duration != Duration) {
            return false;
        }

        foreach (var pitch in note.Pitches.Where(pitch => !Pitches.Contains(pitch))) {
            Pitches.Add(pitch);
        }

        return true;
    }
}