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
    Piano,
    Guitar
}

public partial class Note : Node {
    public DurationNotation Duration { get; private set; }

    public InstrumentType Instrument { get; set; }

    public List<PitchNotation> Pitches { get; } = new List<PitchNotation>();
    
    private static DurationNotation MAX_DURATION = DurationNotation.Minim;
    private static DurationNotation MIN_DURATION = DurationNotation.Crotchet;

    //TODO refactor as those 4 functions (duration/pitches/up/down) have a lot in common
    public bool DurationUp() {
        if (Duration == MAX_DURATION) {
            return false;
        }

        Duration = (DurationNotation)((int) Duration - 1);
        return true;
    }

    public bool DurationDown() {
        if (Duration == MIN_DURATION) {
            return false;
        }

        Duration = (DurationNotation)((int)Duration + 1);
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
        }

        return true;
    }
    
    public bool ChangeInstrument(InstrumentType instrument) {
        Instrument = instrument;
        return true;
    }

    public Note(InstrumentType instrument = InstrumentType.Piano, PitchNotation pitch = PitchNotation.C, DurationNotation duration = DurationNotation.Minim) {
        Instrument = instrument;
        Pitches.Add(pitch);
        Duration = duration;
    }
}