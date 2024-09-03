using Godot;
using heigpdg2024.scripts.notes;
using heigpdg2024.scripts.resources;

namespace heigpdg2024.scripts.managers;

/// <summary>
/// Class representing an audio manager, to be used to play notes through a sampler
/// </summary>
public partial class AudioManager : Node {
    private static Node _piano;
    private static Node _guitar;
    
    private static AudioStreamPlayer _place;
    private static AudioStreamPlayer _remove;
    private static AudioStreamPlayer _select;
    private static AudioStreamPlayer _levelUp;

    public override void _Ready() {
        _piano = GetNode("Sampler/Piano");
        _guitar = GetNode("Sampler/Guitar");
        _place = GetNode<AudioStreamPlayer>("UISounds/PlaceSound");
        _remove = GetNode<AudioStreamPlayer>("UISounds/RemoveSound");
        _select = GetNode<AudioStreamPlayer>("UISounds/SelectSound");
        _levelUp = GetNode<AudioStreamPlayer>("UISounds/LevelUpSound");
    }

    /// <summary>
    /// Plays a note through the sampler matching the criteria
    /// </summary>
    /// <param name="note">The note to play</param>
    public void PlayNote(Note note) {
        var duration = note.Duration;
        var pitches = note.Pitches;

        foreach (var pitch in pitches) {
            switch (note.Instrument) {
                case InstrumentType.Piano:
                    _piano.Call("play_note", pitch.ToString(), 4, (int)duration.Notation);
                    break;
                case InstrumentType.Guitar:
                    _guitar.Call("play_note", pitch.ToString(), 4, (int)duration.Notation);
                    break;
                case InstrumentType.None:
                    // Do nothing
                    break;
                default:
                    GD.PushWarning("Instrument not found");
                    break;
            }
        }
    }

    public void PlayPlaceSound() {
        _place.Play();
    }

    public void PlayRemoveSound() {
        _remove.Play();
    }

    public void PlaySelectSound() {
        _select.Play();
    }
    
    public void PlayLevelUpSound() {
        _levelUp.Play();
    }
}