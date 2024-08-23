using System.Collections.Generic;
using Godot;

public partial class NoteVisualizer : Node {
    [Export] private List<Texture2D> _sprites;
    [Export] private PackedScene _sprite;
    private Note _note;
    public override void _Ready() {
        base._Ready();
        _note = new Note();
        foreach (var pitch in _note.Pitches) {
            var spriteInstance = _sprite.Instantiate<Sprite2D>();
            spriteInstance.Texture = _sprites[(int)pitch];
            AddChild(spriteInstance);
        }
    }
}