using Godot;

public partial class LevelLabel : Label {
    public override void _Ready() {
        GameManager.Instance.ProgressionManager.LevelChange += UpdateLabel;
    }

    private void UpdateLabel() {
        Text = "Level " + GameManager.Instance.ProgressionManager.CurrentTier;
    }
}