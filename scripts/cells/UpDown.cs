using Godot;

namespace heigpdg2024.scripts.cells;

public class UpDown : Transit {
    private bool _isUp;
    public UpDown(Vector2 position, bool isBusy, Input output, bool isUp) : base(position, isBusy, output) {
        _isUp = isUp;
    }

    public override void Process(Note note) {
        switch (IsBusy) {
            case false:
                IsBusy = true;
                note.MoveByTempo(Position);
                break;
            case true when !Output.IsBusy:
                if (_isUp) {
                    note.PitchesUp();
                }
                else {
                    note.PitchesDown();
                }
                Output.Process(note);
                break;
        }
    }
}