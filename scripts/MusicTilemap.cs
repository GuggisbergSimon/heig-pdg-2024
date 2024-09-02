using System.Collections.Generic;
using Godot;
using heigpdg2024.scripts.cells;

public partial class MusicTilemap : TileMapLayer {
    [Export] private int _sourceId;
    [Export] private PackedScene _noteScene;
    private BlockType _selectedTool = BlockType.Belt;
    private Vector2I _atlasCoords;
    private Vector2I _lastCellCoords;
    private Vector2I _lastDirection;
    private readonly Dictionary<Vector2I, bool> _busyCells = new();
    private readonly Dictionary<Vector2I, int> _directionIndexes = new();
    private readonly Dictionary<Vector2I, Merger> _mergers = new();
    private readonly Dictionary<Vector2I, Source> _sources = new();

    #region Godot methods

    public override void _Ready() {
        //There can be only one MusicTilemap per active scene
        GameManager.Instance.RegisterTilemap(this);
        GameManager.Instance.TimerTempo.Timeout += OnTempo;

        _directionIndexes.Add(Vector2I.Right, 0);
        _directionIndexes.Add(Vector2I.Up, 1);
        _directionIndexes.Add(Vector2I.Left, 2);
        _directionIndexes.Add(Vector2I.Down, 3);
    }

    #endregion

    #region public methods

    public void UpdateTool(BlockType tool) {
        _selectedTool = tool;
        _atlasCoords = tool switch {
            BlockType.Belt => Vector2I.Zero,
            BlockType.Source => new Vector2I(1, 7),
            BlockType.Speaker => new Vector2I(1, 4),
            BlockType.Merger => new Vector2I(1, 5),
            BlockType.ShiftUp => new Vector2I(0, 4),
            BlockType.ShiftDown => new Vector2I(0, 5),
            BlockType.SpeedUp => new Vector2I(2, 4),
            BlockType.SpeedDown => new Vector2I(2, 5),
            BlockType.Instrument1 => new Vector2I(3, 4),
            BlockType.Instrument2 => new Vector2I(3, 5),
            _ => Vector2I.Zero
        };
    }

    public void OnPressed() {
        Vector2I cellCoords = LocalToMap(GetGlobalMousePosition());
        DeleteAt(cellCoords);
        if (Input.IsActionJustPressed("PrimaryAction")) {
            if (_selectedTool == BlockType.Belt) {
                //Add basic "->" belt
                var data = GetCellTileData(cellCoords);
                if (data != null && data.GetCustomData("type").AsString().Equals("belt")) {
                    _lastDirection = -data.GetCustomData("input").AsVector2I();
                }
                else {
                    _lastDirection = Vector2I.Right;
                }

                int directionIndex = _directionIndexes[_lastDirection];
                SetCell(cellCoords, _sourceId, new Vector2I(directionIndex, directionIndex));
                SetBusy(cellCoords, false);
            }
            else {
                CreateAt(cellCoords);
            }
        }

        _lastCellCoords = cellCoords;
    }

    public void OnDragged() {
        Vector2I cellCoords = LocalToMap(GetGlobalMousePosition());
        if (_lastCellCoords.Equals(cellCoords) &&
            !Input.IsActionJustPressed("PrimaryAction") &&
            !Input.IsActionJustPressed("SecondaryAction")) {
            return;
        }

        if (Input.IsActionPressed("PrimaryAction")) {
            if (_selectedTool == BlockType.Belt) {
                //Only adjacent cells
                if (_lastCellCoords.DistanceSquaredTo(cellCoords) > 1) {
                    _lastCellCoords = cellCoords;
                    return;
                }

                Vector2I direction = cellCoords - _lastCellCoords;
                if (direction == Vector2I.Zero) {
                    return;
                }

                //Update _lastCellCoords based on direction
                SetCell(_lastCellCoords, _sourceId,
                    new Vector2I(_directionIndexes[_lastDirection], _directionIndexes[direction]));
                SetCell(cellCoords, _sourceId,
                    new Vector2I(_directionIndexes[direction], _directionIndexes[direction]));
                _lastDirection = direction;
                SetBusy(cellCoords, false);
            }
            else {
                CreateAt(cellCoords);
            }
        }
        else if (Input.IsActionPressed("SecondaryAction")) {
            DeleteAt(cellCoords);
        }

        _lastCellCoords = cellCoords;
    }

    public Processor GetProcessor(Vector2 worldPos, Vector2I cellOffset) {
        var cellCoords = LocalToMap(worldPos);
        cellCoords += cellOffset;
        var data = GetCellTileData(cellCoords);
        if (data == null) return null;

        var cellPos = MapToLocal(cellCoords);
        var isBusy = _busyCells[cellCoords];
        var type = data.GetCustomData("type").AsString();
        var input = data.GetCustomData("input").AsVector2I();
        if (type.Equals("speaker"))
            return new Speaker(cellPos, isBusy, input);

        var output = data.GetCustomData("output").AsVector2I();
        return type switch {
            "belt" => new Transit(cellPos, isBusy, input, output,
                note => { }),
            "pitchUp" => new Transit(cellPos, isBusy, input, output,
                note => note.PitchChange(1)),
            "pitchDown" => new Transit(cellPos, isBusy, input, output,
                note => note.PitchChange(-1)),
            "durationUp" => new Transit(cellPos, isBusy, input, output,
                note => note.DurationChange(1)),
            "durationDown" => new Transit(cellPos, isBusy, input, output,
                note => note.DurationChange(-1)),
            "instrument1" => new Transit(cellPos, isBusy, input, output,
                note => note.InstrumentChange(InstrumentType.Piano)),
            "instrument2" => new Transit(cellPos, isBusy, input, output,
                note => note.InstrumentChange(InstrumentType.Guitar)),
            "merger" => _mergers.GetValueOrDefault(cellCoords),
            _ => null
        };
    }

    public Processor GetProcessor(Vector2 worldPos) {
        return GetProcessor(worldPos, Vector2I.Zero);
    }

    public void SetBusy(Vector2 cellPos, bool busy) {
        SetBusy(LocalToMap(cellPos), busy);
    }

    #endregion

    #region private methods

    private void OnTempo() {
        foreach (var source in _sources) {
            var output = GetProcessor(source.Value.Position, source.Value.Output);
            if (output == null || output.IsBusy || !output.IsCompatible(source.Value.Output)) {
                continue;
            }

            var note = _noteScene.Instantiate<Note>();
            note.Initialize(source.Value.Duration, PitchNotation.G);
            note.Position = source.Value.Position;
            AddChild(note);
            output.Process(note);
        }
    }

    private void CreateAt(Vector2I cellCoords) {
        if (_selectedTool == BlockType.Source) {
            Source s = new Source(MapToLocal(cellCoords), Vector2I.Right, DurationNotation.Half);
            if (!_sources.TryAdd(cellCoords, s)) {
                _sources[cellCoords] = s;
            }

            SetCell(cellCoords, _sourceId, _atlasCoords);
            SetBusy(cellCoords, false);
        }
        else if (_selectedTool == BlockType.Merger) {
            Merger m = new Merger(MapToLocal(cellCoords), false, Vector2I.Up, Vector2I.Down, Vector2I.Right);
            if (!_mergers.TryAdd(cellCoords, m)) {
                _mergers[cellCoords] = m;
            }

            SetCell(cellCoords, _sourceId, _atlasCoords);
            if (!_busyCells.TryAdd(cellCoords, false))
                _busyCells[cellCoords] = false;
        }
        else {
            //Handles all other cases
            SetCell(cellCoords, _sourceId, _atlasCoords);
            SetBusy(cellCoords, false);
        }
    }

    private void DeleteAt(Vector2I cellCoords) {
        //Resets the cell
        SetCell(cellCoords);
        _busyCells.Remove(cellCoords);
        _sources.Remove(cellCoords);
        _mergers.Remove(cellCoords);
    }

    private void SetBusy(Vector2I cellCoords, bool busy) {
        if (!_busyCells.TryAdd(cellCoords, busy)) {
            _busyCells[cellCoords] = busy;
        }
    }

    #endregion
}