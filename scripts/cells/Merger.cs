using Godot;

namespace heigpdg2024.scripts.cells;

public class Merger : Transit {
    private Note _noteQueued;
    private bool _hasMerged;

    public Merger(Vector2 position, bool isBusy, Input output) : base(position, isBusy, output) { }

    public override void Process(Note note) {
        //TODO implement
        if (_noteQueued == null) {
            _noteQueued = note;
            note.MoveByTempo(Position);
        }
        else {
            if (!_hasMerged) {
                if (!_noteQueued.AddNote(note)) {
                    GD.Print("Error merging two incompatibles notes");
                }
                else {
                    IsBusy = true;
                    note.MoveByTempo(Position);
                }
            }
            else if (!Output.IsBusy) {
                Output.Process(note);
                IsBusy = false;
            }
        }
    }
}