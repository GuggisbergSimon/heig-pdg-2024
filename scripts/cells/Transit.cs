using Godot;

namespace heigpdg2024.scripts.cells;

public abstract class Transit : Input {
    protected Input Output;

    public Transit(Vector2 position, bool isBusy, Input output): base(position, isBusy) {
        Output = output;
    }
}