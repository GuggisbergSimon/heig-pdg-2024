using Godot;
using heigpdg2024.scripts.managers;
using heigpdg2024.scripts.notes;

namespace heigpdg2024.scripts.cells;

public delegate void ProcessorCallback(Note note);

public class Transit : Processor {
    private readonly Vector2I _output;
    private readonly ProcessorCallback _callback;

    public Transit(Vector2 position, bool isBusy, Vector2I input, Vector2I output, ProcessorCallback callback) : base(
        position, isBusy, input) {
        _output = output;
        _callback = callback;
    }

    public override void Process(Note note) {
        switch (IsBusy) {
            case false:
                GameManager.Instance.Tilemap.SetBusy(Position, true);
                note.MoveByTempo(Position);
                break;
            case true:
                Processor output = GameManager.Instance.Tilemap.GetProcessor(Position, _output);
                if (output != null && output.IsCompatible(_output)) {
                    _callback(note);
                    output.Process(note);
                    GameManager.Instance.Tilemap.SetBusy(Position, false);
                }

                break;
        }
    }
}