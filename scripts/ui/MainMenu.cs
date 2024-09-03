using Godot;
using heigpdg2024.scripts.managers;

namespace heigpdg2024.scripts.ui;

/// <summary>
/// Class representing the main menu of the game
/// </summary>
public partial class MainMenu : Control {
    public override void _Ready() {
        Hide();
    }

    /// <summary>
    /// Show the main menu
    /// </summary>
    public void OnShowMainMenu() {
        GameManager.Instance.Pause();
        Show();
    }

    /// <summary>
    /// Hide the main menu
    /// </summary>
    public void OnHideMainMenu() {
        GameManager.Instance.Play();
        Hide();
    }
    
    /// <summary>
    /// Quit the game
    /// </summary>
    public void OnQuit() {
        GetTree().Quit();
    }
}