using System.Linq;
using Godot;
using heigpdg2024.scripts.notes;

namespace heigpdg2024.scripts.resources;

[GlobalClass]
public partial class Requirement : Resource {
	[Export] public int[] Pitches { get; set; }
	[Export] public DurationNotation Duration { get; set; }
	[Export] public InstrumentType Instrument { get; set; }

	public Requirement() : this(null, DurationNotation.Quarter, InstrumentType.Piano) { }

	public Requirement(int[] pitches, DurationNotation duration, InstrumentType instrument) {
		Pitches = pitches;
		Duration = duration;
		Instrument = instrument;
	}

	public override string ToString() {
		string pitchString = Pitches.Aggregate("", (current, pitch) => current + (PitchNotation)pitch);
		return pitchString + " - " + Duration + " - " + Instrument;
	}
}
