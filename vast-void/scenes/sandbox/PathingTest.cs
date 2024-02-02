using Godot;
using VastVoid.Components;
using VastVoid.Managers;

public partial class PathingTest : Node2D
{
	[Export]ProgressMeter _meter1;

	public override void _Ready()
	{
		SignalBus.Instance.ClockShortTicked += () => _meter1.CurrentValue++;
	}

	public override void _Process(double delta)
	{
	}
}
