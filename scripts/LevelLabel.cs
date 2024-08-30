using Godot;
using System;

public partial class LevelLabel : Godot.Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GameManager.Instance.ProgressionManager.LevelChange += UpdateLabel;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    
    private void UpdateLabel() {
        Text = "Level " + GameManager.Instance.ProgressionManager.CurrentTier;
    }
}
