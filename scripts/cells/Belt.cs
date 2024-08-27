using Godot;

namespace heigpdg2024.scripts.cells;

public partial class Belt : Transit {
    public override void Process(Note note) {
        //TODO implement
    }

    public Belt(Cell input, Cell output, bool isBusy) : base(input, output, isBusy) {
    }
}