using Godot;

public enum DurationNotation {
    Whole, //Ronde
    Half, //Blanche
    Quarter, //Noire
    Eighth //Croche
}

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