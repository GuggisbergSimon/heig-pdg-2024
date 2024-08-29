using Godot;

namespace heigpdg2024.scripts.cells;

public abstract class Processor {
    protected Vector2I Input;
    protected Vector2 Position { get; }
    public bool IsBusy { get; }

    protected Processor(Vector2 position, bool isBusy, Vector2I input) {
        Position = position;
        IsBusy = isBusy;
        Input = input;
    }
    
    public abstract void Process(Note note);
    
    public bool IsCompatible(Vector2I input) {
        return !IsBusy && input.Equals(Input);
    }
}