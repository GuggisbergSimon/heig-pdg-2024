using Godot;
using System;

public partial class Camera : Camera2D
{
    
    private int MAX_SPEED = 10;
    private int _velocity_x = 0;
    private int _velocity_y = 0;
    
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        Position += new Vector2(_velocity_x, _velocity_y);
	}
    
    public override void _Input(InputEvent @event)
    {
        if(Input.IsActionJustPressed("ui_up")) {
            _velocity_y = -MAX_SPEED;
        }
        else if(Input.IsActionJustPressed("ui_down"))
        {
            _velocity_y = MAX_SPEED;
        }
        else if(Input.IsActionJustPressed("ui_left"))
        {
            _velocity_x = -MAX_SPEED;
        }
        else if(Input.IsActionJustPressed("ui_right"))
        {
            _velocity_x = MAX_SPEED;
        }
        else if(Input.IsActionJustReleased("ui_up") || Input.IsActionJustReleased("ui_down"))
        {
            _velocity_y = 0;
        }
        else if(Input.IsActionJustReleased("ui_left") || Input.IsActionJustReleased("ui_right"))
        {
            _velocity_x = 0;
        }
    }
}
