using Godot;
using System;

public partial class DevButtonsContainer : PanelContainer
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (!OS.HasFeature("debug")) {
            Hide();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}