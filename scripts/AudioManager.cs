using Godot;

public partial class AudioManager : Node {

    private static Node _piano;
    private static Node _guitar;
	public override void _Ready() {
        _piano = GetNode("Sampler/Piano");
        _guitar = GetNode("Sampler/Guitar");
    }

    public static void PlayNote(Note note) {
        var duration = note.Duration;
        var pitches = note.Pitches;

        foreach (var pitch in pitches) {
            if (note.Instrument == InstrumentType.Piano) {
                _piano.Call("play_note", pitch.ToString(), 4, (int)duration);
            }
            else if (note.Instrument == InstrumentType.Guitar) {
                _guitar.Call("play_note", pitch.ToString(), 4, (int)duration);
            }
        }
    }
}
