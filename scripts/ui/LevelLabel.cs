using Godot;
using heigpdg2024.scripts.managers;

namespace heigpdg2024.scripts.ui;

/// <summary>
/// Class representing the UI label updating to match current level
/// </summary>
public partial class LevelLabel : Label {
    public override void _Ready() {
        GameManager.Instance.ProgressionManager.LevelChange += UpdateLabel;
    }

    private void UpdateLabel() {
        Text = "Level " + GameManager.Instance.ProgressionManager.CurrentTier;
    }
}