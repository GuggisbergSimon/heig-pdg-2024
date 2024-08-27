using Godot;
using System;

public partial class ToolsContainer : HBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        base._Ready();
        foreach (string tool in GameManager.Instance.ProgressionManager.getTools()) {
            Button child = new Button();
            child.Text = tool;
            AddChild(child);
        }
        GameManager.Instance.ProgressionManager.LevelChange += UpdateOptions;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    
    public void UpdateOptions() {
        foreach (Node child in GetChildren()) {
            RemoveChild(child);
            child.QueueFree();
        }
        foreach (string tool in GameManager.Instance.ProgressionManager.getTools()) {
            Button child = new Button();
            child.Text = tool;
            AddChild(child);
        }
    }
}
