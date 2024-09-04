using System;
using System.Collections.Generic;
using GdUnit4;
using static GdUnit4.Assertions;
using Godot;
using heigpdg2024.scripts.notes;
using heigpdg2024.scripts.resources;

namespace Tests;

/// <summary>
/// Test suite for the Note class
/// </summary>
[TestSuite]
public class NoteTests {
    private PackedScene _noteScene = GD.Load<PackedScene>("res://scenes/note/Note.tscn");

    [TestCase]
    public void Note_Initialize_SetsCorrectValues() {
        var note = _noteScene.Instantiate<Note>();
        AssertObject(note).IsNotNull();
        note.Initialize(DurationNotation.Quarter, PitchNotation.C);

        AssertString(note.Duration.Notation.ToString()).Equals(DurationNotation.Quarter.ToString());
        AssertBool(note.Pitches.Contains(PitchNotation.C)).IsTrue();

        AutoFree(note);
    }

    [TestCase]
    public void Note_DurationChange_IncreasesDuration() {
        var note = _noteScene.Instantiate<Note>();
        AssertObject(note).IsNotNull();
        note.Initialize(DurationNotation.Quarter, PitchNotation.C);

        note.DurationChange(1);

        AssertString(note.Duration.Notation.ToString()).Equals(DurationNotation.Half.ToString());

        AutoFree(note);
    }

    [TestCase]
    public void Note_DurationChange_DecreasesDuration() {
        var note = _noteScene.Instantiate<Note>();
        AssertObject(note).IsNotNull();
        note.Initialize(DurationNotation.Half, PitchNotation.C);

        note.DurationChange(-1);

        AssertString(note.Duration.Notation.ToString()).Equals(DurationNotation.Quarter.ToString());

        AutoFree(note);
    }

    [TestCase]
    public void Note_DurationChange_MaxDuration_NoChange() {
        var note = _noteScene.Instantiate<Note>();
        AssertObject(note).IsNotNull();
        note.Initialize(DurationNotation.Quarter, PitchNotation.C);

        note.DurationChange(1);
        note.DurationChange(1);

        AssertString(note.Duration.Notation.ToString()).Equals(DurationNotation.Half.ToString());

        AutoFree(note);
    }

    [TestCase]
    public void Note_DurationChange_MinDuration_NoChange() {
        var note = _noteScene.Instantiate<Note>();
        AssertObject(note).IsNotNull();
        note.Initialize(DurationNotation.Half, PitchNotation.C);

        note.DurationChange(-1);
        note.DurationChange(-1);

        AssertString(note.Duration.Notation.ToString()).Equals(DurationNotation.Quarter.ToString());

        AutoFree(note);
    }

    [TestCase]
    public void Note_PitchChange_IncreasesPitch() {
        var note = _noteScene.Instantiate<Note>();
        AssertObject(note).IsNotNull();
        note.Initialize(DurationNotation.Quarter, PitchNotation.C);

        note.PitchChange(1);

        AssertString(note.Pitches[0].ToString()).Equals(PitchNotation.D.ToString());

        AutoFree(note);
    }

    [TestCase]
    public void Note_PitchChange_DecreasesPitch() {
        var note = _noteScene.Instantiate<Note>();
        AssertObject(note).IsNotNull();
        note.Initialize(DurationNotation.Quarter, PitchNotation.D);

        note.PitchChange(-1);

        AssertString(note.Pitches[0].ToString()).Equals(PitchNotation.C.ToString());

        AutoFree(note);
    }

    [TestCase]
    public void Note_PitchChange_MaxPitch_NoChange() {
        var note = _noteScene.Instantiate<Note>();
        AssertObject(note).IsNotNull();
        note.Initialize(DurationNotation.Quarter, PitchNotation.B);

        note.PitchChange(1);

        AssertString(note.Pitches[0].ToString()).Equals(PitchNotation.B.ToString());

        AutoFree(note);
    }

    [TestCase]
    public void Note_PitchChange_MinPitch_NoChange() {
        var note = _noteScene.Instantiate<Note>();
        AssertObject(note).IsNotNull();
        note.Initialize(DurationNotation.Quarter, PitchNotation.C);

        note.PitchChange(-1);

        AssertString(note.Pitches[0].ToString()).Equals(PitchNotation.C.ToString());

        AutoFree(note);
    }

    public void Note_AddNote_SameInstrumentAndDuration_AddsNote() {
        var note1 = _noteScene.Instantiate<Note>();
        AssertObject(note1).IsNotNull();
        note1.Initialize(DurationNotation.Quarter, PitchNotation.C);
        note1.InstrumentChange(InstrumentType.Piano);

        var note2 = _noteScene.Instantiate<Note>();
        AssertObject(note2).IsNotNull();
        note2.Initialize(DurationNotation.Quarter, PitchNotation.D);
        note2.InstrumentChange(InstrumentType.Piano);

        // Ensure PitchNotation.D is properly handled
        if (!Enum.IsDefined(typeof(PitchNotation), PitchNotation.D)) {
            throw new KeyNotFoundException("PitchNotation.D is not defined in the dictionary.");
        }

        note1.AddNote(note2);
        AssertBool(note1.Pitches.Contains(PitchNotation.D)).IsTrue();

        AutoFree(note1);
        AutoFree(note2);
    }

    [TestCase]
    public void Note_AddNote_DifferentInstrument_ReturnsFalse() {
        var note1 = _noteScene.Instantiate<Note>();
        AssertObject(note1).IsNotNull();
        note1.Initialize(DurationNotation.Quarter, PitchNotation.C);
        note1.InstrumentChange(InstrumentType.Piano);

        var note2 = _noteScene.Instantiate<Note>();
        AssertObject(note2).IsNotNull();
        note2.Initialize(DurationNotation.Quarter, PitchNotation.D);
        note2.InstrumentChange(InstrumentType.Guitar);

        AssertBool(note1.AddNote(note2)).IsFalse();

        AutoFree(note1);
        AutoFree(note2);
    }

    [TestCase]
    public void Note_AddNote_DifferentDuration_ReturnsFalse() {
        var note1 = _noteScene.Instantiate<Note>();
        AssertObject(note1).IsNotNull();
        note1.Initialize(DurationNotation.Quarter, PitchNotation.C);
        note1.InstrumentChange(InstrumentType.Piano);

        var note2 = _noteScene.Instantiate<Note>();
        AssertObject(note2).IsNotNull();
        note2.Initialize(DurationNotation.Half, PitchNotation.D);
        note2.InstrumentChange(InstrumentType.Piano);

        AssertBool(note1.AddNote(note2)).IsFalse();

        AutoFree(note1);
        AutoFree(note2);
    }

    [TestCase]
    public void Note_InstrumentChange_ChangesInstrumentSuccessfully() {
        var note = _noteScene.Instantiate<Note>();
        AssertObject(note).IsNotNull();
        note.Initialize(DurationNotation.Quarter, PitchNotation.C);
        note.InstrumentChange(InstrumentType.Piano);

        note.InstrumentChange(InstrumentType.Guitar);

        AssertString(note.Instrument.ToString()).Equals(InstrumentType.Guitar.ToString());

        AutoFree(note);
    }
}