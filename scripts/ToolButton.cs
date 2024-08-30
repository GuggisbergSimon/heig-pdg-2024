using Godot;
using System;

public partial class ToolButton : TextureButton {

    private BlockType _type;

    public ToolButton(BlockType type) {
        _type = type;
        Block b = (Block)GD.Load("res://resources/" + _type + ".tres");
        SetTextureNormal(b.Sprite);
        StretchMode = TextureButton.StretchModeEnum.KeepAspectCentered;
        SetCustomMinimumSize(new Vector2(64, 64));
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        //connect mouse entered and exited signals
        Connect("mouse_entered", new Callable(this, nameof(OnMouseEntered)));
        Connect("mouse_exited", new Callable(this, nameof(OnMouseExited)));
        Pressed += OnClick;
    }
    
    public override void _ExitTree() {
        Disconnect("mouse_entered", new Callable(this, nameof(OnMouseEntered)));
        Disconnect("mouse_exited", new Callable(this, nameof(OnMouseExited)));
        Pressed -= OnClick;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        if (HasFocus()) {
            // Add a border
            Modulate = new Color(1, 0, 0, 1);
        }
        else {
            Modulate = new Color(1, 1, 1, 1);
        }
    }

    private void OnClick() {
        GameManager.Instance.Tilemap.UpdateTool(_type);
    }

    public void OnMouseEntered() {
        Scale = new Vector2(1.1f, 1.1f);
    }

    public void OnMouseExited() {
        Scale = new Vector2(1, 1);
    }
}