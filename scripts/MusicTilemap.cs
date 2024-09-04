using System.Collections.Generic;
using Godot;
using heigpdg2024.scripts.cells;
using heigpdg2024.scripts.managers;
using heigpdg2024.scripts.notes;
using heigpdg2024.scripts.resources;

namespace heigpdg2024.scripts.tiles;

/// <summary>
/// Class representing a tilemap used to produce music
/// </summary>
public partial class MusicTilemap : TileMapLayer {
    [Export] private int _sourceId;
    [Export] private PackedScene _noteScene;

    private BlockType _selectedTool = BlockType.Belt;
    private Vector2I _lastCellCoords;
    private Vector2I _lastDirection;
    private readonly Dictionary<Vector2I, bool> _busyCells = new();
    private readonly Dictionary<Vector2I, int> _directionIndexes = new();
    private readonly Dictionary<Vector2I, Merger> _mergers = new();
    private readonly Dictionary<Vector2I, Source> _sources = new();

    #region Godot methods

    public override void _Ready() {
        // Tilemap setup
        GameManager.Instance.RegisterTilemap(this);
        GameManager.Instance.TimerTempo.Timeout += OnTempo;

        // Direction setup
        _directionIndexes.Add(Vector2I.Right, 0);
        _directionIndexes.Add(Vector2I.Up, 1);
        _directionIndexes.Add(Vector2I.Left, 2);
        _directionIndexes.Add(Vector2I.Down, 3);
    }

    #endregion

    #region public methods

    /// <summary>
    /// Update tilemap tool with a new one
    /// </summary>
    /// <param name="tool">The tool to update the tilemap with</param>
    public void UpdateTool(BlockType tool) {
        _selectedTool = tool;
    }

    /// <summary>
    /// Called when the tilemap is pressed, initially
    /// </summary>
    public void OnPressed() {
        Vector2I cellCoords = LocalToMap(GetGlobalMousePosition());
        // Clears what was on the cell before drawing on it
        var data = GetCellTileData(cellCoords);
        DeleteAt(cellCoords);
        if (Input.IsActionJustPressed("PrimaryAction")) {
            if (_selectedTool == BlockType.Belt) {
                // Uses the date of previous cell, if it exists, to improve consistency
                if (data != null && data.GetCustomData("type").AsString().Equals("belt")) {
                    _lastDirection = -data.GetCustomData("input").AsVector2I();
                }
                else {
                    _lastDirection = Vector2I.Right;
                }

                int directionIndex = _directionIndexes[_lastDirection];
                SetCell(cellCoords, _sourceId, new Vector2I(directionIndex, directionIndex));
                SetBusy(cellCoords, false);
                GameManager.Instance.AudioManager.PlayPlaceSound();
            }
            else {
                CreateAt(cellCoords);
                GameManager.Instance.AudioManager.PlayPlaceSound();
            }
        }
        else if (data != null) {
            GameManager.Instance.AudioManager.PlayRemoveSound();
        }

        _lastCellCoords = cellCoords;
    }

    /// <summary>
    /// Called when the mouse is dragged over the tilemap
    /// </summary>
    public void OnDragged() {
        Vector2I cellCoords = LocalToMap(GetGlobalMousePosition());
        // Preventive return if the cell hasn't changed
        if (_lastCellCoords.Equals(cellCoords) &&
            !Input.IsActionJustPressed("PrimaryAction") &&
            !Input.IsActionJustPressed("SecondaryAction")) {
            return;
        }

        if (Input.IsActionPressed("PrimaryAction")) {
            if (_selectedTool == BlockType.Belt) {
                // Only adjacent cells
                if (_lastCellCoords.DistanceSquaredTo(cellCoords) > 1) {
                    _lastCellCoords = cellCoords;
                    return;
                }

                Vector2I direction = cellCoords - _lastCellCoords;
                // Preventive return to avoid errors
                if (direction == Vector2I.Zero) {
                    return;
                }
                
                // Update _lastCellCoords based on direction if it is valid
                if (_lastDirection != -direction) {
                    SetCell(_lastCellCoords, _sourceId,
                        new Vector2I(_directionIndexes[_lastDirection], _directionIndexes[direction]));
                }

                SetCell(cellCoords, _sourceId,
                    new Vector2I(_directionIndexes[direction], _directionIndexes[direction]));
                GameManager.Instance.AudioManager.PlayPlaceSound();
                _lastDirection = direction;
                SetBusy(cellCoords, false);
            }
            else {
                CreateAt(cellCoords);
                GameManager.Instance.AudioManager.PlayPlaceSound();
            }
        }
        else if (Input.IsActionPressed("SecondaryAction")) {
            var data = GetCellTileData(cellCoords);
            if (data != null) {
                GameManager.Instance.AudioManager.PlayRemoveSound();
            }

            DeleteAt(cellCoords);
        }

        _lastCellCoords = cellCoords;
    }

    /// <summary>
    /// Get the processor, if there is any, located at a given position
    /// </summary>
    /// <param name="worldPos">The position in the world of the processor to get</param>
    /// <param name="cellOffset">An offset, in tile coordinates, to apply to the position</param>
    /// <returns>A processor, if there is any, null otherwise</returns>
    public Processor GetProcessor(Vector2 worldPos, Vector2I cellOffset) {
        var cellCoords = LocalToMap(worldPos);
        cellCoords += cellOffset;
        var data = GetCellTileData(cellCoords);
        if (data == null) {
            return null;
        }

        var cellPos = MapToLocal(cellCoords);
        var isBusy = _busyCells[cellCoords];
        var type = data.GetCustomData("type").AsString();
        var input = data.GetCustomData("input").AsVector2I();
        if (type.Equals("speaker")) {
            return new Speaker(cellPos, isBusy, input);
        }

        // Handles processors with output tilemap data
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

    /// <summary>
    /// Get the processor, if there is any, located at a given position
    /// </summary>
    /// <param name="worldPos">The position in the world of the processor to get</param>
    /// <returns>A processor, if there is any, null otherwise</returns>
    public Processor GetProcessor(Vector2 worldPos) {
        return GetProcessor(worldPos, Vector2I.Zero);
    }

    /// <summary>
    /// Mark a given cell as busy based on a given value
    /// </summary>
    /// <param name="cellPos">The position in the world of the cell</param>
    /// <param name="busy">Whether the cell is busy, or not</param>
    public void SetBusy(Vector2 cellPos, bool busy) {
        SetBusy(LocalToMap(cellPos), busy);
    }

    #endregion

    #region private methods

    /// <summary>
    /// Called every tempo beat
    /// </summary>
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

    /// <summary>
    /// Create a block, not a belt, at the given cell coordinates
    /// </summary>
    /// <param name="cellCoords">The coordinates of the tilemap on which to create a block</param>
    private void CreateAt(Vector2I cellCoords) {
        // Handles sources
        if (_selectedTool == BlockType.Source) {
            Source s = new Source(MapToLocal(cellCoords), Vector2I.Right, DurationNotation.Half);
            if (!_sources.TryAdd(cellCoords, s)) {
                _sources[cellCoords] = s;
            }

            SetCell(cellCoords, _sourceId, _selectedTool.GetAtlasCoords());
            SetBusy(cellCoords, false);
        }
        // Handles mergers
        else if (_selectedTool == BlockType.Merger) {
            Merger m = new Merger(MapToLocal(cellCoords), false, Vector2I.Up, Vector2I.Down, Vector2I.Right);
            if (!_mergers.TryAdd(cellCoords, m)) {
                _mergers[cellCoords] = m;
            }

            SetCell(cellCoords, _sourceId, _selectedTool.GetAtlasCoords());
            if (!_busyCells.TryAdd(cellCoords, false)) {
                _busyCells[cellCoords] = false;
            }
        }
        else {
            //Handles all other cases
            SetCell(cellCoords, _sourceId, _selectedTool.GetAtlasCoords());
            SetBusy(cellCoords, false);
        }
    }

    /// <summary>
    /// Clears a given cell from any kind of block on it
    /// </summary>
    /// <param name="cellCoords">The tilemap coordinates to clear</param>
    private void DeleteAt(Vector2I cellCoords) {
        //Resets the cell
        SetCell(cellCoords);
        _busyCells.Remove(cellCoords);
        _sources.Remove(cellCoords);
        _mergers.Remove(cellCoords);
    }

    /// <summary>
    /// Mark a given cell as busy based on a given value
    /// </summary>
    /// <param name="cellCoords">The tilemap coordinates of the cell</param>
    /// <param name="busy">Whether the cell is busy, or not</param>
    private void SetBusy(Vector2I cellCoords, bool busy) {
        if (!_busyCells.TryAdd(cellCoords, busy)) {
            _busyCells[cellCoords] = busy;
        }
    }

    #endregion
}