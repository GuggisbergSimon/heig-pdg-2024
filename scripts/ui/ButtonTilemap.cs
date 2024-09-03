using Godot;
using heigpdg2024.scripts.managers;

namespace heigpdg2024.scripts.ui;

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