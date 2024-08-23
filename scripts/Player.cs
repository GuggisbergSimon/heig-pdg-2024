using Godot;

public partial class Player : Node {
    public override void _Input(InputEvent @event) {
        if (Input.IsActionJustPressed("PrimaryAction")) {
            GD.Print("test primary");
        }
        else if (Input.IsActionJustPressed("SecondaryAction")) {
            GD.Print("test secondary");
            GameManager.Instance.GotoScene("res://scenes/Rick.tscn");
        }
    }
}