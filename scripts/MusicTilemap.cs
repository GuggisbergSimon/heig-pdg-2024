using System.Collections.Generic;
using Godot;
using heigpdg2024.scripts.cells;

public partial class MusicTilemap : TileMapLayer {
    [Export] private int _sourceId;

    private Vector2I _atlasCoords;
    private Vector2I _lastCellCoords;
    private Vector2I _lastDirection;
    private Vector2I _beltCoords = Vector2I.Zero;
    private Vector2I _sourceCoords = new(1, 7);
    private Godot.Collections.Dictionary<Vector2I, bool> _busyCells = new();
    private Godot.Collections.Dictionary<Vector2I, int> _directionIndexes = new();

    public override void _Ready() {
        //There can be only one MusicTilemap per active scene
        GameManager.Instance.RegisterTilemap(this);
        ToolOptionButton toolOption = GetNode<ToolOptionButton>("ToolOptionButton");
        toolOption.ItemSelected += UpdateTool;
        toolOption.SelectedItem += UpdateTool;
        
        _directionIndexes.Add(Vector2I.Right, 0);
        _directionIndexes.Add(Vector2I.Up, 1);
        _directionIndexes.Add(Vector2I.Left, 2);
        _directionIndexes.Add(Vector2I.Down, 3);
        
    }

    private void UpdateTool(long index) {
        _atlasCoords = index switch {
            //TODO change to match final atlas spritesheet
            0 => _beltCoords,
            1 => new Vector2I(0, 4),
            2 => new Vector2I(0, 5),
            3 => _sourceCoords,
            _ => -Vector2I.One
        };
    }

    public override void _Input(InputEvent @event) {
        //TODO detect clicks through UI layer
        Vector2I cellCoords = LocalToMap(GetGlobalMousePosition());
        if (!Input.IsActionPressed("PrimaryAction") && 
            !Input.IsActionPressed("SecondaryAction")) {
            return;
        }

        //Only one action per cell or only just pressed actions
        if (_lastCellCoords.Equals(cellCoords) &&
            !Input.IsActionJustPressed("PrimaryAction") &&
            !Input.IsActionJustPressed("SecondaryAction")) {
            return;
        }

        if (Input.IsActionPressed("PrimaryAction")) {
            if (_atlasCoords.Equals(_beltCoords)) {
                if (Input.IsActionJustPressed("PrimaryAction")) {
                    //Add basic "->" belt
                    SetCell(cellCoords, _sourceId, Vector2I.Zero);
                    _lastDirection = Vector2I.Right;
                    if (!_busyCells.TryAdd(cellCoords, false)) {
                        _busyCells[cellCoords] = false;
                    }
                }
                else {
                    //Only adjacent cells
                    if (_lastCellCoords.DistanceSquaredTo(cellCoords) > 1) {
                        _lastCellCoords = cellCoords;
                        return;
                    }
                    
                    Vector2I direction = cellCoords - _lastCellCoords;
                    //Update _lastCellCoords based on direction
                    SetCell(_lastCellCoords, _sourceId, new Vector2I(_directionIndexes[_lastDirection], _directionIndexes[direction]));
                    SetCell(cellCoords, _sourceId, new Vector2I(_directionIndexes[direction], _directionIndexes[direction]));
                    _lastDirection = direction;
                    
                    if (!_busyCells.TryAdd(cellCoords, false)) {
                        _busyCells[cellCoords] = false;
                    }
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
        else if (Input.IsActionPressed("SecondaryAction")) {
            //Resets the cell
            SetCell(cellCoords);
            _busyCells.Remove(cellCoords);
        }

        _lastCellCoords = cellCoords;
    }

    public Processor GetInput(Vector2 worldPos, Vector2I cellOffset) {
        Vector2I cellCoord = LocalToMap(worldPos) + cellOffset;
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
            "pitchUp" => new UpDown(cellPos, isBusy, input, output, true),
            "pitchDown" => new UpDown(cellPos, isBusy, input, output, false),
            //"durationUp" => new UpDown(cellPos, isBusy, input, output, true),
            ///"durationDown" => new UpDown(cellPos, isBusy, input, output, false),
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