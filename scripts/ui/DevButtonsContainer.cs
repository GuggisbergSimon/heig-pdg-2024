using Godot;
using heigpdg2024.scripts.managers;

namespace heigpdg2024.scripts.ui;

/// <summary>
/// Class representing the UI container holding the developer buttons to be used in debug mode only
/// </summary>
public partial class DevButtonsContainer : PanelContainer {
    public override void _Ready() {
        if (!OS.HasFeature("debug")) {
            Hide();
        }

        GetNode<Button>("MarginContainer/VBoxContainer/LevelUpButton").Pressed +=
            GameManager.Instance.ProgressionManager.LevelUp;
        GetNode<Button>("MarginContainer/VBoxContainer/LevelDownButton").Pressed +=
            GameManager.Instance.ProgressionManager.LevelDown;
    }
}