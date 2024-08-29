using Godot;

namespace heigpdg2024.scripts.cells;

public class Speaker : Processor {
    public Speaker(Vector2 position, bool isBusy, Vector2I input) : base(position, isBusy, input) { }

    public override void Process(Note note) {
        //TODO implement
        GD.Print("Playing note : " + note);
        note.QueueFree();
    }
}