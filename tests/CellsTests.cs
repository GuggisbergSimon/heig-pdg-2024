using GdUnit4;
using static GdUnit4.Assertions;
using Godot;
using heigpdg2024.scripts.cells;
using heigpdg2024.scripts.resources;

namespace Tests;

/// <summary>
/// Test suite for objects in the cells namespace, such as Merger, Source, Speaker and Transit classes
/// </summary>
[TestSuite]
public class CellsTests {
    [TestCase]
    public void Merger_CanBeCreated() {
        var merger = new Merger(new Vector2(0, 0), false, new Vector2I(0, 1), new Vector2I(0, -1), new Vector2I(1, 0));
        AssertBool(merger.IsBusy).IsFalse();
    }

    [TestCase]
    public void Merger_IsCompatible_ReturnsTrue() {
        var inputUp = new Vector2I(0, 1);
        var inputDown = new Vector2I(0, -1);
        var merger = new Merger(new Vector2(0, 0), false, inputUp, inputDown, new Vector2I(1, 0));
        AssertBool(merger.IsCompatible(-inputDown)).IsTrue();
        AssertBool(merger.IsCompatible(-inputUp)).IsTrue();
    }

    [TestCase]
    public void Merger_IsCompatible_ReturnsFalse() {
        var merger = new Merger(new Vector2(0, 0), false, new Vector2I(0, 1), new Vector2I(0, -1), new Vector2I(1, 0));
        AssertBool(merger.IsCompatible(new Vector2I(1, 0))).IsFalse();
    }

    [TestCase]
    public void Source_CanBeCreated() {
        var source = new Source(new Vector2(0, 0), new Vector2I(0, 1), DurationNotation.Quarter);
        AssertObject(source).IsNotNull();
        AssertVector(source.Position).Equals(new Vector2(0, 0));
        AssertVector(source.Output).Equals(new Vector2I(0, 1));
        AssertString(source.Duration.ToString()).Equals(DurationNotation.Quarter.ToString());
    }

    [TestCase]
    public void Speaker_CanBeCreated() {
        var speaker = new Speaker(new Vector2(0, 0), false, new Vector2I(0, 1));
        AssertObject(speaker).IsNotNull();
        AssertBool(speaker.IsBusy).IsFalse();
    }

    [TestCase]
    public void Speaker_IsCompatible_ReturnsTrue() {
        var inputVector = new Vector2I(0, 1);
        var speaker = new Speaker(new Vector2(0, 0), false, inputVector);
        AssertBool(speaker.IsCompatible(-inputVector)).IsTrue();
        AssertBool(speaker.IsCompatible(inputVector)).IsFalse();
    }

    [TestCase]
    public void Transit_CanBeCreated() {
        var transit = new Transit(new Vector2(0, 0), false, new Vector2I(0, 1), new Vector2I(1, 0), note => { });
        AssertObject(transit).IsNotNull();
        AssertBool(transit.IsBusy).IsFalse();
    }

    [TestCase]
    public void Transit_IsCompatible_ReturnsTrue() {
        var inputVector = new Vector2I(0, 1);
        var transit = new Transit(new Vector2(0, 0), false, inputVector, new Vector2I(1, 0), note => { });
        AssertBool(transit.IsCompatible(-inputVector)).IsTrue();
        AssertBool(transit.IsCompatible(inputVector)).IsFalse();
    }
}