using Godot;

namespace heigpdg2024.scripts.cells;

public class Merger : Transit {
    public Merger(Vector2 position, bool isBusy, Vector2I input, Vector2I output) : base(position, isBusy, input, output) { }

    public override void Process(Note note) {
        //TODO implement
    }
}