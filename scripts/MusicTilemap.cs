using System.Collections.Generic;
using Godot;
using heigpdg2024.scripts.cells;

public partial class MusicTilemap : TileMapLayer {
    [Export] private int _sourceId;

    private Vector2I _atlasCoords;
    private BlockType _selectedTool = BlockType.Belt;
    private Vector2I _lastCellCoords;
    private Vector2I _lastDirection;
    private Vector2I _beltCoords = Vector2I.Zero;
    private Vector2I _sourceCoords = new(2, 7);
    private Vector2I _mergerCoords = new(1, 5);
    private Godot.Collections.Dictionary<Vector2I, bool> _busyCells = new();
    private Godot.Collections.Dictionary<Vector2I, int> _directionIndexes = new();

    public override void _Ready() {
        //There can be only one MusicTilemap per active scene
        GameManager.Instance.RegisterTilemap(this);

        _directionIndexes.Add(Vector2I.Right, 0);
        _directionIndexes.Add(Vector2I.Up, 1);
        _directionIndexes.Add(Vector2I.Left, 2);
        _directionIndexes.Add(Vector2I.Down, 3);
    }

    public void UpdateTool(BlockType tool) {
        _selectedTool = tool;
        switch (tool) {
            case BlockType.Belt:
                _atlasCoords = _beltCoords;
                break;
            case BlockType.Source:
                _atlasCoords = _sourceCoords;
                break;
            case BlockType.Speaker:
                _atlasCoords = new Vector2I(1, 4);
                break;
            case BlockType.Merger:
                _atlasCoords = _mergerCoords;
                break;
            case BlockType.ShiftUp:
                _atlasCoords = new Vector2I(0, 4);
                break;
            case BlockType.ShiftDown:
                _atlasCoords = new Vector2I(0, 5);
                break;
            case BlockType.SpeedUp:
                _atlasCoords = new Vector2I(2, 4);
                break;
            case BlockType.SpeedDown:
                _atlasCoords = new Vector2I(2, 5);
                break;
            default:
                _atlasCoords = Vector2I.Zero;
                break;
        }
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
            // TODO add Belt to Block types
            if (_selectedTool == BlockType.Belt) {
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
                    SetCell(_lastCellCoords, _sourceId,
                        new Vector2I(_directionIndexes[_lastDirection], _directionIndexes[direction]));
                    SetCell(cellCoords, _sourceId,
                        new Vector2I(_directionIndexes[direction], _directionIndexes[direction]));
                    _lastDirection = direction;

                    if (!_busyCells.TryAdd(cellCoords, false)) {
                        _busyCells[cellCoords] = false;
                    }
                }
            }
            else if (_selectedTool == BlockType.Source) {
                GameManager.Instance.RegisterSource(new Source(MapToLocal(cellCoords), Vector2I.Right));
                SetCell(cellCoords, _sourceId, _atlasCoords);
                if (!_busyCells.TryAdd(cellCoords, false)) {
                    _busyCells[cellCoords] = false;
                }
            }
            else if (_selectedTool == BlockType.Merger) {
                GameManager.Instance.RegisterMerger(
                    new Merger(MapToLocal(cellCoords), false, Vector2I.Up, Vector2I.Down, Vector2I.Right));
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
            "belt" => new Transit(cellPos, isBusy, input, output, note => { }),
            "pitchUp" => new Transit(cellPos, isBusy, input, output, note => note.PitchChange(1)),
            "pitchDown" => new Transit(cellPos, isBusy, input, output, note => note.PitchChange(-1)),
            "durationUp" => new Transit(cellPos, isBusy, input, output, note => note.DurationChange(1)),
            "durationDown" => new Transit(cellPos, isBusy, input, output, note => note.DurationChange(-1)),
            "instrument1" => new Transit(cellPos, isBusy, input, output, note => note.InstrumentChange(InstrumentType.Piano)),
            "instrument2" => new Transit(cellPos, isBusy, input, output, note => note.InstrumentChange(InstrumentType.Guitar)),
            "merger" => FindMerger(cellPos),
            _ => null
        };
    }

    public Processor GetInput(Vector2 worldPos) {
        return GetInput(worldPos, Vector2I.Zero);
    }

    // TODO améliorer
    // Nouvelle méthode pour trouver un Merger à une position spécifique
    private Merger FindMerger(Vector2 cellPos) {
        foreach (var merger in GameManager.Instance._mergers) {
            if (merger.getPosition() == cellPos) {
                return merger;
            }
        }

        return null;
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