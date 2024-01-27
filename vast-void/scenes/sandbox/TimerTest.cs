using Godot;
using VastVoid.Components;
using VastVoid.Managers;

public partial class TimerTest : Node2D
{
	[Export]private IdleClock _clock1;
	[Export]private Label _smTickCountLabel;
	[Export]private Label _lgTickCountLabel;

	private int _smTickCount, _lgTickCount;

	public override void _Ready()
	{
		_smTickCount = 0;
		_lgTickCount = 0;

		SignalBus.Instance.ClockShortTicked += () => _smTickCount++;
		SignalBus.Instance.ClockLongTicked += () => _lgTickCount++;
		
		_clock1.StartClock();
	}

	public override void _Process(double delta)
	{
		_smTickCountLabel.Text = $"S{_smTickCount}";
		_lgTickCountLabel.Text = $"L{_lgTickCount}";
	}
}
