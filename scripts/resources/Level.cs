using System;
using Godot;

[GlobalClass]
public partial class Level : Resource {
	[Export] private int[] _toolsIntArray;

	[Export] public int Tier { get; set; }
	[Export] public int Tempo { get; set; }
	public BlockType[] Tools {
		get => Array.ConvertAll(_toolsIntArray, value => (BlockType)value);
		set => _toolsIntArray = Array.ConvertAll(value, item => (int)item);
	}
	[Export] public Requirement[] Requirements { get; set; }

	public Level() : this(0, 120, new[] { BlockType.Instrument1 }, null) { }

	public Level(int tier, int tempo, BlockType[] tools, Requirement[] requirements) {
		Tier = tier;
		Tempo = tempo;
		Tools = tools;
		Requirements = requirements;
	}

	public override string ToString() {
		return Tier + " - " + Tempo + " - " + string.Join(", ", Tools) + " - " + Requirements;
	}
}
