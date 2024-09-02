using Godot;

namespace heigpdg2024.scripts.cells;

public class Merger : Processor {
    private Note _singleNote;
    private Vector2I Output { get; }
    private Vector2I InputDown { get; }

    public Merger(Vector2 position, bool isBusy, Vector2I inputUp,
        Vector2I inputDown, Vector2I output) : base(position, isBusy,
        inputUp) {
        Output = output;
        InputDown = inputDown;
    }

    public override bool IsCompatible(Vector2I input) {
        return !IsBusy && (input.Equals(-Input) || input.Equals(-InputDown));
    }

    public override void Process(Note note) {
        if (IsBusy) {
            TryMoving(note);
        }
        else if (!note.Equals(_singleNote)){
            if (_singleNote == null) {
                note.MoveByTempo(Position);
                _singleNote = note;
            }
            else if (_singleNote.AddNote(note)) {
                note.MoveByTempo(Position);
                TryMoving(_singleNote);
                _singleNote = null;
            }
        }
    }
    
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