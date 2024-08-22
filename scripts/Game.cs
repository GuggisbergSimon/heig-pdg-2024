using Godot;
using System;

public partial class Game : Node2D {
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        GD.Print(
            "Camera zoom " + GetNode<Camera2D>("Camera2D").Zoom.X);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
    }

    public override void _Input(InputEvent @event) {
        // Mouse in viewport coordinates.
        if (Input.IsActionJustPressed("PrimaryAction")) {
            GD.Print("test primary");
        } else if (Input.IsActionJustPressed("SecondaryAction")) {
            GD.Print("test secondary");
            GameManager.Instance.GotoScene("res://scenes/Rick.tscn");
        }

        //else if (@event is InputEventMouseMotion eventMouseMotion) {
        //    GD.Print("Mouse Motion at: ", eventMouseMotion.Position);
        //}

        // Print the size of the viewport.
        //GD.Print("Viewport Resolution is: ", GetViewport().GetVisibleRect().Size);
    }
}