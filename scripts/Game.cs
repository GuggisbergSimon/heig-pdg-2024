using Godot;
using System;

public partial class Game : Node2D {
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        GD.Print("Camera zoom " + GetNode<Camera2D>("Camera2D").Zoom.X);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public override void _Input(InputEvent @event) {
        // Mouse in viewport coordinates.
        if (@event is InputEventMouseButton eventMouseButton)
            GD.Print("Mouse Click/Unclick at: ", eventMouseButton.Position);
        //else if (@event is InputEventMouseMotion eventMouseMotion) {
        //    GD.Print("Mouse Motion at: ", eventMouseMotion.Position);
        //}

        // Print the size of the viewport.
        //GD.Print("Viewport Resolution is: ", GetViewport().GetVisibleRect().Size);
    }
}