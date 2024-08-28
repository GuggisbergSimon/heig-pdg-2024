using Godot;

namespace heigpdg2024.scripts.cells;

public class Belt : Transit {
    
    public override void Process(Note note) {
        switch (IsBusy) {
            case false:
                IsBusy = true;
                note.MoveByTempo(Position);
                break;
            case true when !Output.IsBusy:
                Output.Process(note);
                break;
        }
    }

    public Belt(Vector2 position, bool isBusy, Input output) : base(position, isBusy, output) { }
}