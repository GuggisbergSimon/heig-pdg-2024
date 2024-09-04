using Godot;

namespace heigpdg2024.scripts.camera;

/// <summary>
/// Class representing a Camera
/// </summary>
public partial class Camera : Camera2D {
    [Export] private float _speed = 10f;
    [Export] private float _mouseBuffer = 5f;
    private static float MAX_KEYBOARD_SPEED = 10f;
    private static float MAX_MOUSE_SPEED = 5f;
    private Vector2 _velocity;

    public override void _Process(double delta) {
        Position += (float)delta * _speed * _velocity;
    }

    public override void _Input(InputEvent @event) {
        if (Input.IsActionJustPressed("up")) {
            _velocity.Y = -MAX_KEYBOARD_SPEED;
        }
        else if (Input.IsActionJustPressed("down")) {
            _velocity.Y = MAX_KEYBOARD_SPEED;
        }
        else if (Input.IsActionJustPressed("left")) {
            _velocity.X = -MAX_KEYBOARD_SPEED;
        }
        else if (Input.IsActionJustPressed("right")) {
            _velocity.X = MAX_KEYBOARD_SPEED;
        }
        else if (Input.IsActionJustReleased("up") || Input.IsActionJustReleased("down")) {
            _velocity.Y = 0;
        }
        else if (Input.IsActionJustReleased("left") || Input.IsActionJustReleased("right")) {
            _velocity.X = 0;
        }
        else if (@event is InputEventMouseMotion mouseEvent) {
            Vector2 viewportSize = GetWindow().Size;
            Vector2 mousePosition = mouseEvent.Position;

            if (mousePosition.X <= _mouseBuffer) {
                _velocity.X = -MAX_MOUSE_SPEED;
            }
            else if (mousePosition.X >= viewportSize.X - _mouseBuffer) {
                _velocity.X = MAX_MOUSE_SPEED;
            }
            else {
                _velocity.X = 0;
            }

            if (mousePosition.Y <= _mouseBuffer) {
                _velocity.Y = -MAX_MOUSE_SPEED;
            }
            else if (mousePosition.Y >= viewportSize.Y - _mouseBuffer) {
                _velocity.Y = MAX_MOUSE_SPEED;
            }
            else {
                _velocity.Y = 0;
            }
        }
    }
}