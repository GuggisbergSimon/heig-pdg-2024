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