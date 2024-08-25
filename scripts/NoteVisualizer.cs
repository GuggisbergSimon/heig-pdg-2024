using Godot;

public partial class NoteVisualizer : Node {
    [Export] private Texture2D[] _sprites;
    [Export] private PackedScene _sprite;
    [Export] private PackedScene _staff;
    private Note _note;

    public override void _Ready() {
        base._Ready();

        //Test purposes
        _note = new Note(PitchNotation.G);
        _note.AddNote(new Note());
        
        AddChild(_staff.Instantiate<Sprite2D>());
        //TODO add special symbol under the staff if C or D
        foreach (var pitch in _note.Pitches) {
            var spriteInstance = _sprite.Instantiate<Sprite2D>();
            AddChild(spriteInstance);
            spriteInstance.Texture = _sprites[(int)_note.Duration];
            //TODO moving 
            spriteInstance.Position = (int)pitch * Vector2.Up;
        }
    }
}