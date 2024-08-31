using Godot;

namespace heigpdg2024.scripts.cells;

public class Merger : Processor {
    private Note _note;
    private Vector2I Output { get; }
    private Vector2I InputDown { get; }

    public Merger(Vector2 position, bool isBusy, Vector2I inputUp,
        Vector2I inputDown, Vector2I output) : base(position, isBusy,
        inputUp) {
        Output = output;
        InputDown = inputDown;
    }

    public override bool IsCompatible(Vector2I input) {
        return !IsBusy &&
               (input.Equals(-Input) || input.Equals(-InputDown));
    }

    public override void Process(Note note) {
        if (_note == null) {
            _note = note;
        }
        else {
            var output =
                GameManager.Instance.Tilemap.GetProcessor(Position, Output);
            if (output != null && output.IsCompatible(Output)) {
                note.AddNote(_note);
                note.MoveByTempo(Position);
                output.Process(note);
                GameManager.Instance.Tilemap.SetBusy(
                    GameManager.Instance.Tilemap.MapToLocal(
                        GameManager.Instance.Tilemap.LocalToMap(Position) +
                        Input), false);
                GameManager.Instance.Tilemap.SetBusy(
                    GameManager.Instance.Tilemap.MapToLocal(
                        GameManager.Instance.Tilemap.LocalToMap(Position) +
                        InputDown), false);
            }
        }
    }

    public void ClearMemory() {
        _note = null;
    }
}