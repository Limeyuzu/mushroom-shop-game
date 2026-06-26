using Godot;

public partial class PauseMenu : CanvasLayer
{
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("pause"))
        {
            if (!GetTree().Paused && !UICanvas.Instance.IsUIOpen)
            {
                OnPauseButtonPressed();
            }
            else if (GetTree().Paused)
            {
                OnCloseButtonPressed();
            }
        }

    }

    private void OnPauseButtonPressed()
    {
        GetTree().Paused = true;
        Show();
    }

    private void OnCloseButtonPressed()
    {
        Hide();
        GetTree().Paused = false;
    }
}
