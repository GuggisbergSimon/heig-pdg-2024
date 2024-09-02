using Godot;
using heigpdg2024.scripts.managers;
using heigpdg2024.scripts.notes;

namespace heigpdg2024.scripts.cells;

/// <summary>
/// Callback to be used by the transit 
/// </summary>
public delegate void TransitCallback(Note note);

/// <summary>
/// Class representing a Transit, a processor moving notes from an input to an output with a callback method to use
/// </summary>
public class Transit : Processor {
    private readonly Vector2I _output;
    private readonly TransitCallback _callback;

    /// <summary>
    /// Transit constructor
    /// </summary>
    /// <param name="position">The center position in the world of this transit</param>
    /// <param name="isBusy">Whether the transit currently accepts new notes, or not</param>
    /// <param name="input">The direction in which the transit accepts notes</param>
    /// <param name="output">The direction of the output of this transit</param>
    /// <param name="callback">The method to be called back while processing notes</param>
    public Transit(Vector2 position, bool isBusy, Vector2I input, Vector2I output, TransitCallback callback) : base(
        position, isBusy, input) {
        _output = output;
        _callback = callback;
    }

    public override void Process(Note note) {
        switch (IsBusy) {
            // Accepts a new note
            case false:
                GameManager.Instance.Tilemap.SetBusy(Position, true);
                note.MoveByTempo(Position);
                break;
            case true:
                // Processes the note
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