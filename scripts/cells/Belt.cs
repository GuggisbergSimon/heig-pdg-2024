using Godot;
namespace heigpdg2024.scripts.cells;

public partial class Belt : Cell {
    public Vector2I Input { get; }
    public Vector2I Output { get; }
    public bool IsBusy { get; }

    public Belt(Vector2I input, Vector2I output, bool isBusy) {
        Input = input;
        Output = output;
        IsBusy = isBusy;
    }
    
    public override void Process(Note note) {
        //TODO implement
    }
}
