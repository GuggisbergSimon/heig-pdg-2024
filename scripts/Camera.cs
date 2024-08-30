using Godot;
using System;

public partial class Camera : Camera2D {
    private int MAX_KEYBOARD_SPEED = 10;
    private int MAX_MOUSE_SPEED = 3;
    private int MOUSE_BUFFER = 5;
    private int _velocity_x = 0;
    private int _velocity_y = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        Position += new Vector2(_velocity_x, _velocity_y);
    }

    public override void _Input(InputEvent @event) {
        if (Input.IsActionJustPressed("ui_up")) {
            _velocity_y = -MAX_KEYBOARD_SPEED;
        }
        else if (Input.IsActionJustPressed("ui_down")) {
            _velocity_y = MAX_KEYBOARD_SPEED;
        }
        else if (Input.IsActionJustPressed("ui_left")) {
            _velocity_x = -MAX_KEYBOARD_SPEED;
        }
        else if (Input.IsActionJustPressed("ui_right")) {
            _velocity_x = MAX_KEYBOARD_SPEED;
        }
        else if (Input.IsActionJustReleased("ui_up") || Input.IsActionJustReleased("ui_down")) {
            _velocity_y = 0;
        }
        else if (Input.IsActionJustReleased("ui_left") || Input.IsActionJustReleased("ui_right")) {
            _velocity_x = 0;
        }
        else if (@event is InputEventMouseMotion mouseEvent) {
            Vector2 viewportSize = GetWindow().Size;
            Vector2 mousePosition = mouseEvent.Position;

            if (mousePosition.X <= MOUSE_BUFFER) {
                _velocity_x = -MAX_MOUSE_SPEED;
            }
            else if (mousePosition.X >= viewportSize.X - MOUSE_BUFFER) {
                _velocity_x = MAX_MOUSE_SPEED;
            }
            else {
                _velocity_x = 0;
            }

            if (mousePosition.Y <= MOUSE_BUFFER) {
                _velocity_y = -MAX_MOUSE_SPEED;
            }
            else if (mousePosition.Y >= viewportSize.Y - MOUSE_BUFFER) {
                _velocity_y = MAX_MOUSE_SPEED;
            }
            else {
                _velocity_y = 0;
            }
        }
    }
}