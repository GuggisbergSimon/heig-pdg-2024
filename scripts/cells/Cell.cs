using Godot;

namespace heigpdg2024.scripts.cells;

public abstract class Cell {

    protected Vector2I _position;
    
    public abstract void Process(Note note);
}