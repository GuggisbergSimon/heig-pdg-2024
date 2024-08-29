using Godot;

namespace heigpdg2024.scripts.cells;

public class UpDown : Transit {
    private readonly bool _isUp;
    public UpDown(Vector2 position, bool isBusy, Vector2I input, Vector2I output, bool isUp) : base(position, isBusy, input, output) {
        _isUp = isUp;
    }

    public override void Process(Note note) {
        switch (IsBusy) {
            case false:
                GameManager.Instance.Tilemap.SetBusy(Position, true);
                note.MoveByTempo(Position);
                break;
            case true:
                Processor output = GameManager.Instance.Tilemap.GetInput(Position, Output);
                if (output!= null && output.IsCompatible(Input)) {
                    if (_isUp) {
                        note.PitchesUp();
                    }
                    else {
                        note.PitchesDown();
                    }

                    output.Process(note);
                    GameManager.Instance.Tilemap.SetBusy(Position, false);
                }
                
                break;
        }
    }
}