using Godot;

public partial class DevButtonsContainer : PanelContainer {
    public override void _Ready() {
        if (!OS.HasFeature("debug")) {
            Hide();
        }
        
        GetNode<Button>("MarginContainer/VBoxContainer/LevelUpButton").Pressed += GameManager.Instance.ProgressionManager.LevelUp;
        GetNode<Button>("MarginContainer/VBoxContainer/LevelDownButton").Pressed += GameManager.Instance.ProgressionManager.LevelDown;
    }
}