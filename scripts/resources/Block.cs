using Godot;

namespace heigpdg2024.scripts.resources;

public enum BlockType {
    Belt,
    Source,
    Merger,
    ShiftUp,
    ShiftDown,
    SpeedUp,
    SpeedDown,
    Speaker,
    Instrument1,
    Instrument2,
}

static class BlockTypeMethods {
    public static Vector2I GetAtlasCoords(this BlockType type) {
        return type switch {
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
            _ => Vector2I.Zero,
        };
    }
}

/// <summary>
/// Resource representing a basic kind of block, a tool, that can be used to draw on the tilemap
/// </summary>
[GlobalClass]
public partial class Block : Resource {
    [Export] public BlockType Type { get; set; }
    [Export] public Texture2D Sprite { get; set; }

    public Block() : this(BlockType.Source, null) {
    }

    public Block(BlockType type, Texture2D sprite) {
        Type = type;
        Sprite = sprite;
    }
}