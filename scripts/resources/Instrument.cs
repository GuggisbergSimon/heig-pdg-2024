using Godot;

namespace heigpdg2024.scripts.resources;

public enum InstrumentType {
	None,
	Piano,
	Guitar
}

[GlobalClass]
public partial class Instrument : Resource {
	[Export] public InstrumentType Type { get; set; }
	[Export] public Color Color { get; set; }

	public Instrument() : this(InstrumentType.None, Colors.Black) { }

	public Instrument(InstrumentType type, Color color) {
		Type = type;
		Color = color;
	}
}
