using Godot;

namespace heigpdg2024.scripts.cells;

public partial class Source : Node2D {
    private PackedScene _noteScene =
        GD.Load<PackedScene>("res://scenes/Note.tscn");

    private readonly Input _output;
    private Vector2 _position;

    public Source(Vector2 position, Input output) {
        _position = position;
        _output = output;
    }

    public override void _Process(double delta) {
        if (!_output.IsBusy) {
            var newNote = (Note)_noteScene.Instantiate();
            newNote.Position = _output.Position;

            GameManager.Instance.AddChild(newNote);
            // GetTree().CurrentScene.AddChild(newNote); // Add the note to the scene ? Or should we add it to the GameManager ?

            GameManager.Instance.RegisterNote(newNote);
        }
    }
}