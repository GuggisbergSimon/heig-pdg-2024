using Godot;
using System.Collections.Generic;
using System.IO;

public partial class ToolsContainer : HBoxContainer {
    public override void _Ready() {
        base._Ready();
        UpdateOptions();
        GameManager.Instance.ProgressionManager.LevelChange += UpdateOptions;
    }

    public void UpdateOptions() {
        foreach (Node child in GetChildren()) {
            RemoveChild(child);
            child.QueueFree();
        }

        foreach (KeyValuePair<BlockType, bool> tool in GameManager.Instance.ProgressionManager.GetTools()) {
            if (tool.Value) {
                ToolButton child = new ToolButton(tool.Key);
                AddChild(child);
            }
            else {
                //TODO add "?" texture for locked tools
                Button child = new Button();
                child.Disabled = true;
                child.Text = "?";
                AddChild(child);
            }
        }
    }
}