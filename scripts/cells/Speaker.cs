using Godot;

namespace heigpdg2024.scripts.cells;

public class Speaker : Processor {
    public Speaker(Vector2 position, bool isBusy, Vector2I input) : base(position, isBusy, input) { }

    public override void Process(Note note) {
        GameManager.Instance.AudioManager.PlayNote(note);
        note.QueueFree();
    }

    public override bool IsCompatible(Vector2I input, Note note) {
        return base.IsCompatible(input, note) &&
               note != null && note.Instrument != InstrumentType.None;
    }
}