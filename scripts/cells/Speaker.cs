using Godot;
using heigpdg2024.scripts.managers;
using heigpdg2024.scripts.notes;
using heigpdg2024.scripts.resources;

namespace heigpdg2024.scripts.cells;

/// <summary>
/// Class representing a Speaker, playing notes aloud
/// </summary>
public class Speaker : Processor {
    /// <summary>
    /// Speaker constructor
    /// </summary>
    /// <param name="position">The center position in the world of the speaker</param>
    /// <param name="isBusy">Whether the speaker currently accepts new notes, or not</param>
    /// <param name="input">The direction in which the speaker accepts notes</param>
    public Speaker(Vector2 position, bool isBusy, Vector2I input) : base(position, isBusy, input) { }
    
    public override void Process(Note note) {
        if (note.Instrument == InstrumentType.None) {
            note.MoveByTempo(Position, Callable.From(note.QueueFree));
            return;
        }
        
        GameManager.Instance.ProgressionManager.TryRequirement(note, Position);
        note.MoveByTempo(Position, Callable.From(note.PlayNote));
    }
}