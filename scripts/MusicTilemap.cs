using System.Collections.Generic;
using Godot;
using heigpdg2024.scripts.cells;

public partial class MusicTilemap : TileMapLayer {
    [Export] private int _sourceId;
    [Export] private int _terrainset;
    [Export] private int _terrain;

    private Vector2I _atlasCoords;
    private Vector2I _lastCell;
    private Vector2I _beltCoords = Vector2I.Zero;
    private Vector2I _sourceCoords = new(1, 6);
    private Godot.Collections.Dictionary<Vector2I, bool> _busyCells = new();

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
            0 => _beltCoords,
            1 => new Vector2I(0, 3),
            2 => new Vector2I(0, 4),
            3 => _sourceCoords,
            _ => -Vector2I.One
        };
    }

    public override void _Input(InputEvent @event) {
        //TODO detect clicks through UI layer
        Vector2I cellCoords = LocalToMap(GetGlobalMousePosition());
        if (!Godot.Input.IsActionPressed("PrimaryAction") && !Godot.Input.IsActionPressed("SecondaryAction")) {
            return;
        }

        //Only one action per cell
        if (_lastCell == cellCoords &&
            !Godot.Input.IsActionJustPressed("PrimaryAction") &&
            !Godot.Input.IsActionJustPressed("SecondaryAction")) {
            return;
        }

        //TODO disable when outside window or hovering UI
        if (Godot.Input.IsActionPressed("PrimaryAction")) {
            if (_atlasCoords.Equals(_beltCoords)) {
                //Handles Belt
                //Only adjacent cells
                if (_lastCell.DistanceSquaredTo(cellCoords) > 1) {
                    _lastCell = cellCoords;
                    return;
                }

                //SetCellsTerrainPath(new Array<Vector2I>(new[] { _lastCell, cellCoords }), _terrainset, _terrain);
                if (!_busyCells.TryAdd(cellCoords, false)) {
                    _busyCells[cellCoords] = false;
                }
            }
            else if (_atlasCoords.Equals(_sourceCoords)) {
                GameManager.Instance.RegisterSource(new Source(MapToLocal(cellCoords), Vector2I.Right));
                SetCell(cellCoords, _sourceId, _atlasCoords);
                if (!_busyCells.TryAdd(cellCoords, false)) {
                    _busyCells[cellCoords] = false;
                }
            }
            else {
                //Handles all other cases
                SetCell(cellCoords, _sourceId, _atlasCoords);
                if (!_busyCells.TryAdd(cellCoords, false)) {
                    _busyCells[cellCoords] = false;
                }
            }
        }
        else if (Godot.Input.IsActionPressed("SecondaryAction")) {
            //Resets the cell
            SetCell(cellCoords);
            _busyCells.Remove(cellCoords);
        }

        _lastCell = cellCoords;
    }

    public Processor GetInput(Vector2 worldPos, Vector2I cellOffset) {
        Vector2I cellCoord = LocalToMap(worldPos);
        cellCoord += cellOffset;
        TileData data = GetCellTileData(cellCoord);
        if (data == null) {
            return null;
        }

        Vector2 cellPos = MapToLocal(cellCoord);
        bool isBusy = _busyCells[cellCoord];
        string type = data.GetCustomData("type").AsString();
        Vector2I input = data.GetCustomData("input").AsVector2I();
        if (type.Equals("speaker")) {
            return new Speaker(cellPos, isBusy, input);
        }

        Vector2I output = data.GetCustomData("output").AsVector2I();
        return type switch {
            "belt" => new Belt(cellPos, isBusy, input, output),
            "up" => new UpDown(cellPos, isBusy, input, output, true),
            "down" => new UpDown(cellPos, isBusy, input, output, false),
            "merger" => new Merger(cellPos, isBusy, input, output),
            _ => null
        };
    }

    public Processor GetInput(Vector2 worldPos) {
        return GetInput(worldPos, Vector2I.Zero);
    }
    
    public void SetBusy(Vector2 cellPos, bool busy) {
        _busyCells[LocalToMap(cellPos)] = busy;
    }

    public void OnLevelUpButtonPressed() {
        GameManager.Instance.ProgressionManager.levelUp();
    }

    public void OnLevelDownButtonPressed() {
        GameManager.Instance.ProgressionManager.levelDown();
    }
}