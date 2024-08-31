﻿using Godot;

namespace heigpdg2024.scripts.cells;

public abstract class Processor {
    public bool IsBusy { get; }
    protected Vector2 Position { get; }
    protected Vector2I Input;

    protected Processor(Vector2 position, bool isBusy, Vector2I input) {
        Position = position;
        IsBusy = isBusy;
        Input = input;
    }

    public abstract void Process(Note note);

    public virtual bool IsCompatible(Vector2I input, Note note) {
        return !IsBusy && input.Equals(-Input);
    }
}