using Godot;
using System;

public partial class ToolButton : TextureButton
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        //connect mouse entered and exited signals
        Connect("mouse_entered",  new Callable(this, nameof(OnMouseEntered)));
        Connect("mouse_exited",  new Callable(this, nameof(OnMouseExited)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    
    public void OnMouseEntered() {
        Scale = new Vector2(1.1f, 1.1f);
    }
    
    public void OnMouseExited()
    {
        Scale = new Vector2(1, 1);
    }
}
