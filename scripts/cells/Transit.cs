namespace heigpdg2024.scripts.cells;

public abstract class Transit : Cell {
    protected Cell _input;
    protected Cell _output;
    protected bool _isBusy;

    public Transit(Cell input, Cell output, bool isBusy) {
        _input = input;
        _output = output;
        _isBusy = isBusy;
    }
}