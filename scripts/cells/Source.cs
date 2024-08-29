using Godot;

namespace heigpdg2024.scripts.cells;

public class Source {
    public Vector2I Output { get; }
    public Vector2 Position { get; }

    public Source(Vector2 position, Vector2I output) {
        Position = position;
        Output = output;
    }
}