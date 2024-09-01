using Godot;

namespace heigpdg2024.scripts.cells;

public class Speaker : Processor {
    public Speaker(Vector2 position, bool isBusy, Vector2I input) : base(position, isBusy, input) { }

    public override void Process(Note note) {
        if (note.Instrument == InstrumentType.None) {
            note.MoveByTempo(Position, Callable.From(note.QueueFree));
            return;
        }
        GameManager.Instance.AudioManager.PlayNote(note);
        GameManager.Instance.ProgressionManager.TryRequirement(note);
        note.MoveByTempo(Position, Callable.From(note.QueueFree));
    }
}