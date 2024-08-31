using Godot;

public partial class LevelRequirements : HBoxContainer {
    public override void _Ready() {
        base._Ready();

        GameManager.Instance.ProgressionManager.LevelChange += UpdateRequirements;

        UpdateRequirements();
    }

    private void UpdateRequirements() {
        foreach (Node child in GetChildren()) {
            RemoveChild(child);
            child.QueueFree();
        }

        foreach (string requirement in GameManager.Instance.ProgressionManager.GetLevelRequirements()) {
            AddRequirementBox(requirement);
        }
    }

    // Method to dynamically add requirement boxes
    private void AddRequirementBox(string requirement) {
        Label requirementLabel = new Label();
        requirementLabel.Text = requirement;
        requirementLabel.AddThemeColorOverride("font_color", Colors.Black);
        AddChild(requirementLabel);
    }
}