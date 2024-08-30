using Godot;

namespace heigpdg2024.scripts.cells;

public class Belt : Transit {
    
    public override void Process( Note note) {
        switch (IsBusy) {
            case false:
                GameManager.Instance.Tilemap.SetBusy(Position, true);
                note.MoveByTempo(Position);
                break;
            case true:
                Processor output = GameManager.Instance.Tilemap.GetInput(Position, Output);
                if (output != null && output.IsCompatible(Output)) {
                    output.Process(note);
                    GameManager.Instance.Tilemap.SetBusy(Position, false);
                }
                break;
        }
    }

    public Belt(Vector2 position, bool isBusy, Vector2I input, Vector2I output) : base(position, isBusy, input, output) { }
}