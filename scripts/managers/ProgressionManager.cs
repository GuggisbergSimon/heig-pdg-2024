using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class ProgressionManager : Node {
	[Export] private Level[] _levelsResources;

	private static readonly BlockType[] _defaultTools = {
		BlockType.Belt,
		BlockType.Source,
		BlockType.Speaker
	};

	private static Dictionary<BlockType, bool> _toolsUnlocked = new Dictionary<BlockType, bool> {
		{ BlockType.Belt, true },
		{ BlockType.Source, true },
		{ BlockType.Speaker, true },
		{ BlockType.Instrument1, false },
		{ BlockType.ShiftUp, false },
		{ BlockType.ShiftDown, false },
		{ BlockType.SpeedUp, false },
		{ BlockType.SpeedDown, false },
		{ BlockType.Merger, false },
		{ BlockType.Instrument2, false },
	};

	private List<Requirement> _todos = new();

	[Signal]
	public delegate void LevelChangeEventHandler();

	public int CurrentTier { get; private set; }

	public override void _Ready() {
		base._Ready();
	   // UpdateTodos();
	}

	public void TryRequirement(Note note) {
		foreach (var requirement in _todos) {
			if (requirement.Duration != note.Duration.Notation || requirement.Instrument != note.Instrument ||
				requirement.Pitches.Length != note.Pitches.Count) {
				continue;
			}

			List<PitchNotation> requiredPitches = requirement.Pitches.Select(pitch => (PitchNotation)pitch).ToList();
			requiredPitches.Sort();
			note.Pitches.Sort();
			if (requiredPitches.Where((t, i) => t != note.Pitches[i]).Any()) {
				return;
			}

			_todos.Remove(requirement);
			break;
		}
		
		if (_todos.Count == 0) {
			LevelUp();
		}
	}

	public void LevelUp() {
		LevelChangement(CurrentTier++);
	}

	public void LevelDown() {
		LevelChangement(CurrentTier--);
	}

	private void LevelChangement(int oldTier) {
		Level level = _levelsResources.FirstOrDefault(lvl => lvl.Tier == CurrentTier);
		if (level == null) {
			CurrentTier = oldTier;
			return;
		}
		GameManager.Instance.Tempo = level.Tempo;
		UpdateTools(level);
		UpdateTodos(level);
		EmitSignal(SignalName.LevelChange);
		
	}

	public Dictionary<BlockType, bool> GetTools() {
		return _toolsUnlocked;
	}

	private void UpdateTools(Level level) {
		foreach (var tool in _toolsUnlocked.Keys.ToList()) {
				_toolsUnlocked[tool] = false;
		}

		foreach (var tool in _defaultTools) {
			_toolsUnlocked[tool] = true;
		}

		if (level != null) {
			foreach (BlockType tool in level.Tools) {
				_toolsUnlocked[tool] = true;
			}
		}
	}
	
	private void UpdateTodos(Level level) {
			if (level != null) {
				_todos = level.Requirements != null ? level.Requirements.ToList() : new List<Requirement>();
			}
			else {
				_todos.Clear();
			}
	}

	public string[] GetLevelRequirements() {
			// Trouver le niveau correspondant au CurrentTier
			Level level = _levelsResources.FirstOrDefault(lvl => lvl.Tier == CurrentTier);

			// Vérifier si le niveau existe et a des requirements
			if (level != null && level.Requirements != null) {
				// Transformer chaque requirement en chaîne de caractères
				return level.Requirements.Select(req => req.ToString()).ToArray();
			}

			// Retourner un tableau vide si aucun niveau n'est trouvé ou si aucun requirement n'est présent
			return new string[0];
		}
}
