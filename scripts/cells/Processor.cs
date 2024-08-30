using Godot;

namespace heigpdg2024.scripts.cells;

public abstract class Processor {
    protected Vector2I Input;

    protected Processor(Vector2 position, bool isBusy, Vector2I input) {
        Position = position;
        IsBusy = isBusy;
        Input = input;
    }

    protected Vector2 Position { get; }
    public bool IsBusy { get; }

    public abstract void Process(Note note);

    public virtual bool IsCompatible(Vector2I input) {
        return !IsBusy && input.Equals(-Input);
    }

    public Vector2 GetPosition() {
        return Position;
    }
}