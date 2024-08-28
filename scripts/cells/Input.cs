using Godot;

namespace heigpdg2024.scripts.cells;

public abstract class Input {
    public Vector2 Position { get; }
    public bool IsBusy { get; protected set; }

    public Input(Vector2 position, bool isBusy) {
        Position = position;
        IsBusy = isBusy;
    }

    public abstract void Process(Note note);
}