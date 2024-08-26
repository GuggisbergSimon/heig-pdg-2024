using Godot;

public partial class ToolOptionButton : OptionButton {
    [Signal]
    public delegate void SelectedItemEventHandler(long index);

    public override void _Ready() {
        base._Ready();
        foreach (string tool in GameManager.Instance.ProgressionManager.getTools()) {
            AddItem(tool);
        }
        GameManager.Instance.ProgressionManager.LevelChange += UpdateOptions;
    }

    public override void _Input(InputEvent @event) {
        if (!@event.IsPressed()) {
            return;
        }

        if (Input.IsActionJustPressed("ScrollLeft")) {
            Increment(-1);
        }
        else if (Input.IsActionJustPressed("ScrollRight")) {
            Increment(1);
        }
    }

    private void Increment(int value) {
        Select((Selected + value + ItemCount) % ItemCount);
        EmitSignal(SignalName.SelectedItem, Selected);
    }
    
    private void UpdateOptions() {
        Clear();
        foreach (string tool in GameManager.Instance.ProgressionManager.getTools()) {
            AddItem(tool);
        }
    }
}