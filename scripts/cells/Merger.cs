using Godot;

namespace heigpdg2024.scripts.cells;

public class Merger : Processor {
    private Vector2I InputDown { get; }
    private Note _note;
    private Vector2I _output;
	
    public Merger(Vector2 position, bool isBusy, Vector2I inputUp,
        Vector2I inputDown, Vector2I output) : base(position, isBusy,
        inputUp) {
        GD.Print("Merger created + " + position.X + " " + position.Y);
        _output = output;
        InputDown = inputDown;
    }

    public override void Process(Note note) {
        //TODO implement
        /*
         * si la cell apres le merger est libre
         *     s'il y a une note en haut et en bas
         *        si c'est la meme note en haut et en bas
         *           passer une fois la note en sortie
         *        sinon
         *           merger les deux notes
         *     sinon
         *        bloquer
         * sinon
         *      bloquer
         */
        
        if (_note == null) {
            GD.Print("coucou1");
            _note = note;
        }
        else {
            GD.Print("coucou2");
            Processor output = GameManager.Instance.Tilemap.GetInput(Position, _output);
            if (output != null && output.IsBusy == false) {
                note.AddNote(_note);
                GD.Print("coucou3");
                note.MoveByTempo(Position);
                output.Process(note);
                GameManager.Instance.Tilemap.SetBusy(Position + (Input * 16), false);
                GameManager.Instance.Tilemap.SetBusy(Position + (InputDown * 16), false);

            }   
        }
            
		
    }
	
    public void ClearMemory() {
        _note = null;
    }
}