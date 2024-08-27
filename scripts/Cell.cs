using Godot;
namespace heigpdg2024.scripts;

public abstract class Cell {
    private Vector2I position;

    public abstract void Process(Note note);
}