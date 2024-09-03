using Godot;
using System.Collections.Generic;
using heigpdg2024.scripts.managers;
using heigpdg2024.scripts.resources;
using System.IO;
using System.Linq;


namespace heigpdg2024.scripts.ui;

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
                if (tool.Key == BlockType.Belt) {
                    child.GrabFocus();
                }
            }
            else {
                TextureRect child = new TextureRect();
                child.SetTexture(GD.Load<Texture2D>("res://assets/question_mark.png"));
                // child.SetCustomMinimumSize(new Vector2(64, 64));
                AddChild(child);
            }
        }
    }
}