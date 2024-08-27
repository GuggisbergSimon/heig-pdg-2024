namespace heigpdg2024.scripts.cells;

public abstract class UpDown : Transit {
    private bool _isUp;
    public UpDown(Cell input, Cell output, bool isBusy, bool isUp) : base(input, output, isBusy) {
        _isUp = isUp;
    }
}