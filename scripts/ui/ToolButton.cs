using Godot;
using heigpdg2024.scripts.managers;
using heigpdg2024.scripts.resources;

namespace heigpdg2024.scripts.ui;

/// <summary>
/// Class representing a UI Button holding the informations related to a given Tool
/// </summary>
public partial class ToolButton : TextureButton {
    private BlockType _type;

    /// <summary>
    /// ToolButton constructor
    /// </summary>
    /// <param name="type">The tool to displays as a button</param>
    public ToolButton(BlockType type) {
        _type = type;
        Block b = (Block)GD.Load("res://resources/block/" + _type + ".tres");
        SetTextureNormal(b.Sprite);
        StretchMode = StretchModeEnum.KeepAspectCentered;
        SetCustomMinimumSize(new Vector2(64, 64));
    }

    public override void _Ready() {
        // Connect mouse entered and exited signals
        Connect("mouse_entered", new Callable(this, nameof(OnMouseEntered)));
        Connect("mouse_exited", new Callable(this, nameof(OnMouseExited)));
        Pressed += OnClick;
    }

    public override void _ExitTree() {
        Disconnect("mouse_entered", new Callable(this, nameof(OnMouseEntered)));
        Disconnect("mouse_exited", new Callable(this, nameof(OnMouseExited)));
        Pressed -= OnClick;
    }
    
    public override void _Process(double delta) {
        if (GameManager.Instance.Tilemap._selectedTool == _type) {
            // Make the button display in red
            Modulate = new Color(1, 0, 0);
        }
        else {
            Modulate = new Color(1, 1, 1);
        }
    }

    /// <summary>
    /// Update the tool of the tilemap with the tool of this button
    /// </summary>
    private void OnClick() {
        GameManager.Instance.Tilemap.UpdateTool(_type);
        GameManager.Instance.AudioManager.PlaySelectSound();
    }

    /// <summary>
    /// Scale up the button when hovering over it
    /// </summary>
    public void OnMouseEntered() {
        Scale = new Vector2(1.1f, 1.1f);
    }

    /// <summary>
    /// Scale down the button to normal scale when stopping hovering over it
    /// </summary>
    public void OnMouseExited() {
        Scale = new Vector2(1, 1);
    }
}