using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class ToolsContainer : HBoxContainer {
    
    private static Dictionary<string, Rect2> _toolRegions = new Dictionary<string, Rect2> {
        {"Source", new Rect2(48, 48, 16, 16)},
        {"Merger", new Rect2(16, 64, 16, 16)},
        {"ShiftUp", new Rect2(0, 48, 16, 16)},
        {"ShiftDown", new Rect2(0, 64, 16, 16)},
        {"SpeedUp", new Rect2(0, 112, 16, 16)},
        {"SpeedDown", new Rect2(16, 112, 16, 16)},
    };
    
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

        foreach (KeyValuePair<string, bool> tool in GameManager.Instance.ProgressionManager.getTools()) {
            if (tool.Value) {
                ToolButton child = new ToolButton();
                // Resource r = ResourceLoader.Load("res://ressources/" + tool.Key + "Button.tres");
                // child.Theme.SetIcon(tool.Key, "Block", (Texture2D)r);
                // Theme theme = (Theme)GD.Load("res://ressources/" + tool.Key + "Button.tres");
                // child.SetTheme(theme);
                // child.Text = tool.Key;
                // AtlasTexture t = new AtlasTexture();
                // t.Atlas = (Texture2D)GD.Load("res://assets/notes_atlas.png");
                // t.Region = _toolRegions[tool.Key];
                Block b = (Block)GD.Load("res://resources/" + tool.Key + ".tres");
                child.SetTextureNormal(b.Sprite);
                child.StretchMode = TextureButton.StretchModeEnum.KeepAspectCentered;
                child.SetCustomMinimumSize(new Vector2(64, 64));
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