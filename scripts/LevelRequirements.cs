using Godot;
using System;

public partial class LevelRequirements : HBoxContainer
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        GameManager.Instance.ProgressionManager.LevelChange += UpdateRequirements;

        UpdateRequirements();
    }

    private void UpdateRequirements() {
        foreach (Node child in GetChildren()) {
            RemoveChild(child);
            child.QueueFree();
        }
        foreach (string requirement in GameManager.Instance.ProgressionManager.getLevelRequirements()) {
            AddRequirementBox(requirement);
        }
    }

    // Method to dynamically add requirement boxes
    private void AddRequirementBox(string requirement)
    {
        Label requirementLabel = new Label();
        requirementLabel.Text = requirement;
        requirementLabel.AddThemeColorOverride("font_color", Colors.Black);
        AddChild(requirementLabel);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}