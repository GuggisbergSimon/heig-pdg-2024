using Godot;

public enum DurationNotation {
    Semibreve, //Ronde
    Minim, //Blanche
    Crotchet, //Noire
    Quaver //Croche
}

[GlobalClass]
public partial class Duration : Resource {
    [Export] public DurationNotation Notation { get; set; }

    [Export] public Texture2D Sprite { get; set; }
    
    [Export] public Texture2D SpriteWhiteDot { get; set; }

    public Duration() : this(DurationNotation.Minim, null) { }
    
    public Duration(DurationNotation notation, Texture2D sprite) {
        Notation = notation;
        Sprite = sprite;
    }
}