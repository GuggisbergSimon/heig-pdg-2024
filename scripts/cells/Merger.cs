using Godot;

namespace heigpdg2024.scripts.cells;

public class Merger : Transit {
    private Note _noteQueued;

    public Merger(Cell input, Cell output, bool isBusy) : base(input, output, isBusy) {
    }
    
    public override void Process(Note note) {
        //TODO implement
        if (_noteQueued == null) {
            _noteQueued = note;
        }
        else {
            if (!_noteQueued.AddNote(note)) {
                GD.Print("Error merging two incompatibles notes");
            }
            else {
                _output.Process(note);
            }
        }
    }
}