using Godot;

namespace heigpdg2024.scripts.cells;

public class Speaker : Input {
    public Speaker(Vector2 position, bool isBusy) : base(position, isBusy) { }

    public override void Process(Note note) {
        //TODO implement
        GD.Print("Playing note : " + note);
        note.QueueFree();
    }
}