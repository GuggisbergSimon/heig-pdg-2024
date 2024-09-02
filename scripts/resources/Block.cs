using Godot;

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
        switch (type) {
            case BlockType.Belt:
                return Vector2I.Zero;
            case BlockType.Source:
                return new Vector2I(2, 7);
            case BlockType.Speaker:
                return new Vector2I(1, 4);
            case BlockType.Merger:
                return new Vector2I(1, 5);
            case BlockType.ShiftUp:
                return new Vector2I(0, 4);
            case BlockType.ShiftDown:
                return new Vector2I(0, 5);
            case BlockType.SpeedUp:
                return new Vector2I(2, 4);
            case BlockType.SpeedDown:
                return new Vector2I(2, 5);
            case BlockType.Instrument1:
                return new Vector2I(3, 4);
            case BlockType.Instrument2:
                return new Vector2I(3, 5);
            default:
                return Vector2I.Zero;
        }
    }
}

[GlobalClass]
public partial class Block : Resource {
    [Export] public BlockType Type { get; set; }
    [Export] public Texture2D Sprite { get; set; }

    public Block() : this(BlockType.Source, null) { }

    public Block(BlockType type, Texture2D sprite) {
        Type = type;
        Sprite = sprite;
    }
}