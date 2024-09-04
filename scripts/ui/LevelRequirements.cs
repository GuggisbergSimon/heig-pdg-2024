using Godot;
using heigpdg2024.scripts.managers;
using System.Collections.Generic;

namespace heigpdg2024.scripts.ui;

/// <summary>
/// Class representing a UI Container holding the requirements to be displayed
/// </summary>
public partial class LevelRequirements : VBoxContainer {
    private Dictionary<string, Label> _requirementLabels = new Dictionary<string, Label>();

    public override void _Ready() {
        base._Ready();

        GameManager.Instance.ProgressionManager.LevelChange += UpdateRequirements;
        GameManager.Instance.ProgressionManager.RequirementCompleted += OnRequirementCompleted;

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

        _requirementLabels.Clear();

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

        // Store the label for later access
        _requirementLabels[requirement] = requirementLabel;
    }

    /// <summary>
    /// Handle the requirement completed signal by updating the UI
    /// </summary>
    /// <param name="requirement">The completed requirement</param>
    private void OnRequirementCompleted(string requirement) {
        if (_requirementLabels.TryGetValue(requirement, out Label label)) {
            label.Text += " ✔️";
        }
    }
}