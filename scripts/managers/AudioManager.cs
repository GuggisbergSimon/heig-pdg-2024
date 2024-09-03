using Godot;
using heigpdg2024.scripts.notes;
using heigpdg2024.scripts.resources;

namespace heigpdg2024.scripts.managers;

public partial class AudioManager : Node {
    private static Node _piano;
    private static Node _guitar;

    public override void _Ready() {
        _piano = GetNode("Sampler/Piano");
        _guitar = GetNode("Sampler/Guitar");
    }

    public void PlayNote(Note note) {
        var duration = note.Duration;
        var pitches = note.Pitches;

        foreach (var pitch in pitches) {
            switch (note.Instrument) {
                case InstrumentType.Piano:
                    _piano.Call("play_note", pitch.ToString(), 4, (int)duration.Notation);
                    break;
                case InstrumentType.Guitar:
                    _guitar.Call("play_note", pitch.ToString(), 4, (int)duration.Notation);
                    break;
                case InstrumentType.None:
                    // Do nothing
                    break;
                default:
                    GD.PushWarning("Instrument not found");
                    break;
            }
        }
    }
}