using Godot;
using heigpdg2024.scripts.managers;
using heigpdg2024.scripts.notes;

namespace heigpdg2024.scripts.cells;

/// <summary>
/// Class representing a Merger of notes, accepting two notes, from two inputs
/// </summary>
public class Merger : Processor {
    private Note _singleNote;
    private Vector2I Output { get; }
    private Vector2I InputDown { get; }

    /// <summary>
    /// Merger constructor
    /// </summary>
    /// <param name="position">The center position in the world of the merger</param>
    /// <param name="isBusy">Whether the merger currently accepts new notes, or not</param>
    /// <param name="inputUp">The direction in which the merger accepts notes, upwards</param>
    /// <param name="inputDown">The direction in which the merger accepts notes, downwards</param>
    /// <param name="output">The direction in which the merger outputs notes</param>
    public Merger(Vector2 position, bool isBusy, Vector2I inputUp,
        Vector2I inputDown, Vector2I output) : base(position, isBusy,
        inputUp) {
        Output = output;
        InputDown = inputDown;
    }

    public override bool IsCompatible(Vector2I output) {
        return !IsBusy && (output.Equals(-Input) || output.Equals(-InputDown));
    }

    public override void Process(Note note) {
        // Case where a merged note is idly waiting on the merger 
        if (IsBusy) {
            TryMoving(note);
        }
        else if (!note.Equals(_singleNote)) {
            // Case where a new single note enters the merger
            if (_singleNote == null) {
                note.MoveByTempo(Position);
                _singleNote = note;
            }
            // Attempts merging with another single note entering the merger
            else if (_singleNote.AddNote(note)) {
                TryMoving(_singleNote);
                _singleNote = null;
            }
        }
    }

    /// <summary>
    /// Attempts moving a note to the next processor, if it exists and is available
    /// </summary>
    /// <param name="note">The note to move</param>
    private void TryMoving(Note note) {
        var output = GameManager.Instance.Tilemap.GetProcessor(Position, Output);
        if (output == null || !output.IsCompatible(Output)) {
            GameManager.Instance.Tilemap.SetBusy(Position, true);
            IsBusy = true;
            return;
        }

        output.Process(note);
        IsBusy = false;
        GameManager.Instance.Tilemap.SetBusy(Position, false);
    }
}