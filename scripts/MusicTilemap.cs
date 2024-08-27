using Godot;
using Godot.Collections;
using heigpdg2024.scripts.cells;

public partial class MusicTilemap : TileMapLayer {
    [Export] private int _sourceId;
    [Export] private int _terrainset;
    [Export] private int _terrain;

    private Vector2I _atlasCoords;
    private Vector2I _lastCell;
    private Vector2I _staffCoords = Vector2I.Zero;

    public override void _Ready() {
        //There can be only one MusicTilemap per active scene
        GameManager.Instance.RegisterTilemap(this);
        ToolOptionButton toolOption = GetNode<ToolOptionButton>("ToolOptionButton");
        toolOption.ItemSelected += UpdateTool;
        toolOption.SelectedItem += UpdateTool;
    }

    private void UpdateTool(long index) {
        _atlasCoords = index switch {
            //TODO change to match final atlas spritesheet
            0 => _staffCoords,
            1 => new Vector2I(0, 3),
            2 => new Vector2I(0, 4),
            3 => new Vector2I(3, 3),
            _ => -Vector2I.One
        };
    }

    public override void _Input(InputEvent @event) {
        Vector2I cell = LocalToMap(GetGlobalMousePosition());
        if (!Input.IsActionPressed("PrimaryAction") && !Input.IsActionPressed("SecondaryAction")) {
            return;
        }

        //Only one action per cell
        if (_lastCell == cell) {
            return;
        }

        //TODO disable when outside window or hovering UI
        if (Input.IsActionPressed("PrimaryAction")) {
            if (_atlasCoords.Equals(_staffCoords)) {
                //Handles Staff
                //Only adjacent cells
                if (_lastCell.DistanceSquaredTo(cell) > 1) {
                    _lastCell = cell;
                    return;
                }

                SetCellsTerrainPath(new Array<Vector2I>(new[] { _lastCell, cell }), _terrainset, _terrain);
            }
            else {
                //Handles all other cases
                SetCell(cell, _sourceId, _atlasCoords);
            }
        }
        else if (Input.IsActionPressed("SecondaryAction")) {
            //Resets the cell
            SetCell(cell);
        }

        _lastCell = cell;
    }

    public Cell GetCell(Vector2 worldPos) {
        TileData data = GetCellTileData(LocalToMap(worldPos));
        if (data.Terrain < 0) {
            // Production units
            //TODO differentiate others production units by data
            return new Source();
        }
        else {
            //Belt unit
            Vector2I input = data.GetCustomData("input").AsVector2I();
            Vector2I output = data.GetCustomData("output").AsVector2I();
            bool isBusy = data.GetCustomData("isBusy").AsBool();
            return new Belt(
                GameManager.Instance.Tilemap.GetCell(input + worldPos),
                GameManager.Instance.Tilemap.GetCell(output + worldPos),
                isBusy);
        }
    }

    public void OnLevelUpButtonPressed() {
        GameManager.Instance.ProgressionManager.levelUp();
    }

    public void OnLevelDownButtonPressed() {
        GameManager.Instance.ProgressionManager.levelDown();
    }
}