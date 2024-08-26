using Godot;

public partial class MusicTilemap : TileMapLayer {
    [Export] private int _sourceId = 0;

    private Vector2I _atlasCoords;

    public override void _Ready() {
        ToolOptionButton toolOption = GetNode<ToolOptionButton>("ToolOptionButton");
        toolOption.ItemSelected += UpdateTool;
        toolOption.SelectedItem += UpdateTool;
    }

    private void UpdateTool(long index) {
        _atlasCoords = index switch {
            //TODO change to match final atlas spritesheet
            0 => Vector2I.Zero,
            1 => Vector2I.Right,
            2 => Vector2I.Down,
            3 => Vector2I.One,
            _ => -Vector2I.One
        };
    }

    public override void _Input(InputEvent @event) {
        //TODO disable when outside window or hovering UI
        //TODO only fire action once per cell
        if (Input.IsActionPressed("PrimaryAction")) {
            SetCell(LocalToMap(GetGlobalMousePosition()), 0, _atlasCoords);
        }
        else if (Input.IsActionPressed("SecondaryAction")) {
            SetCell(LocalToMap(GetGlobalMousePosition()), -1);
        }
    }
    
    public void OnLevelUpButtonPressed() {
        GameManager.Instance.ProgressionManager.levelUp();
    }
    
    public void OnLevelDownButtonPressed() {
        GameManager.Instance.ProgressionManager.levelDown();
    }
}