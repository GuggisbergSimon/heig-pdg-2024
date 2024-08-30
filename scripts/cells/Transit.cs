using Godot;

namespace heigpdg2024.scripts.cells;

public abstract class Transit : Processor {
    protected Vector2I Output;

    protected Transit(Vector2 position, bool isBusy, Vector2I input, Vector2I output): base(position, isBusy, input) {
        Output = output;
    }
}