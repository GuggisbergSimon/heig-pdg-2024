using Godot;
using heigpdg2024.scripts.notes;

namespace heigpdg2024.scripts.cells;

/// <summary>
/// Class representing a Processor of notes, accepting them from an input
/// </summary>
public abstract class Processor {
    public bool IsBusy { get; protected set; }
    protected Vector2 Position { get; }
    protected Vector2I Input;

    /// <summary>
    /// Processor constructor
    /// </summary>
    /// <param name="position">The center position in the world of the processor</param>
    /// <param name="isBusy">Whether the processor currently accepts new notes, or not</param>
    /// <param name="input">The direction in which the merger accepts notes</param>
    protected Processor(Vector2 position, bool isBusy, Vector2I input) {
        Position = position;
        IsBusy = isBusy;
        Input = input;
    }

    /// <summary>
    /// Processes a note through this processor
    /// </summary>
    /// <param name="note">the note to process</param>
    public abstract void Process(Note note);

    /// <summary>
    /// Checks whether this merger accepts new inputs, from a given one
    /// </summary>
    /// <param name="output">the direction from which a processor tries to connect to this processor</param>
    /// <returns>true if compatible</returns>
    public virtual bool IsCompatible(Vector2I output) {
        return !IsBusy && output.Equals(-Input);
    }
}