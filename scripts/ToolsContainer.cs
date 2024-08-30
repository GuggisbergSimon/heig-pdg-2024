using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public partial class ToolsContainer : HBoxContainer {
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        base._Ready();
        UpdateOptions();
        GameManager.Instance.ProgressionManager.LevelChange += UpdateOptions;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
    }

    public void UpdateOptions() {
        foreach (Node child in GetChildren()) {
            RemoveChild(child);
            child.QueueFree();
        }

        foreach (KeyValuePair<BlockType, bool> tool in GameManager.Instance.ProgressionManager.getTools()) {
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
                child.SetCustomMinimumSize(new Vector2(64, 64));
                AddChild(child);
            }
        }
    }
}