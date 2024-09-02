using Godot;
using heigpdg2024.scripts.managers;

namespace heigpdg2024.scripts.ui;

/// <summary>
/// Class representing a UI Container holding the requirements to be displayed
/// </summary>
public partial class LevelRequirements : HBoxContainer {
    public override void _Ready() {
        base._Ready();

        GameManager.Instance.ProgressionManager.LevelChange += UpdateRequirements;

        UpdateRequirements();
    }

    /// <summary>
    /// Update requirements based on the progression manager
    /// </summary>
    private void UpdateRequirements() {
        foreach (Node child in GetChildren()) {
            RemoveChild(child);
            child.QueueFree();
        }

        foreach (string requirement in GameManager.Instance.ProgressionManager.GetLevelRequirements()) {
            AddRequirementBox(requirement);
        }
    }

    /// <summary>
    /// Add dynamically requirement boxes 
    /// </summary>
    /// <param name="requirement">The requirement to add, as a string</param>
    private void AddRequirementBox(string requirement) {
        Label requirementLabel = new Label();
        requirementLabel.Text = requirement;
        requirementLabel.AddThemeColorOverride("font_color", Colors.Black);
        AddChild(requirementLabel);
    }

}