using System.Collections.Generic;
using Godot;

/// <summary>
/// Class representing a confetti, emitting particles on demand
/// </summary>
public partial class Confetti : Node2D {
    List<CpuParticles2D> _particles = new();
    
    public override void _Ready() {
        _particles = new List<CpuParticles2D> {
            GetNode<CpuParticles2D>("blue"),
            GetNode<CpuParticles2D>("brown"),
            GetNode<CpuParticles2D>("fuchsia"),
            GetNode<CpuParticles2D>("orange"),
            GetNode<CpuParticles2D>("turquoise")
        };
    }

    /// <summary>
    /// Emit confetti particles
    /// </summary>
    public void Emit() {
        foreach (var particles in _particles) {
            particles.Emitting = true;
        }
    }
}