using Godot;

namespace heigpdg2024.scripts.resources;

public enum DurationNotation {
    Whole, //Ronde
    Half, //Blanche
    Quarter, //Noire
    Eighth //Croche
}

/// <summary>
/// Resource representing the different durations available and their representation
/// </summary>
[GlobalClass]
public partial class Duration : Resource {
    [Export] public DurationNotation Notation { get; set; }
    [Export] public Texture2D Sprite { get; set; }
    [Export] public Texture2D SpriteWhiteDot { get; set; }

    public Duration() : this(DurationNotation.Half, null) { }

    public Duration(DurationNotation notation, Texture2D sprite) {
        Notation = notation;
        Sprite = sprite;
    }
}