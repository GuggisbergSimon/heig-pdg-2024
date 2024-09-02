using Godot;
using heigpdg2024.scripts.managers;

namespace heigpdg2024.scripts.ui;

/// <summary>
/// Class representing the interactable tilemap, through an invisible button layered all over the screen
/// </summary>
public partial class ButtonTilemap : Button {
    private bool _isDrawing;

    public override void _Ready() {
        base._Ready();

        ButtonDown += () => {
            GameManager.Instance.Tilemap.OnPressed();
            _isDrawing = true;
        };
        ButtonUp += () => {
            _isDrawing = false;
        };
    }

    public override void _Input(InputEvent @event) {
        if (!_isDrawing) {
            return;
        }
        
        GameManager.Instance.Tilemap.OnDragged();
    }
}