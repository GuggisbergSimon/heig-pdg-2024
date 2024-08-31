using Godot;

namespace heigpdg2024.scripts.cells;

public class Source {
    public Vector2I Output { get; }
    public Vector2 Position { get; }
    public DurationNotation Duration { get; }

    public Source(Vector2 position, Vector2I output, DurationNotation duration) {
        Position = position;
        Output = output;
        Duration = duration;
    }
}