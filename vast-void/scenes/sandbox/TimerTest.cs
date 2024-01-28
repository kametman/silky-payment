using Godot;
using VastVoid.Components;
using VastVoid.Managers;

public partial class TimerTest : Node2D
{
	private IdleClock _clock1;

	[Export]private float _tickLength = 1;
	[Export]private ulong _bigTickCount = 5;

	[Export]private Label _smTickCountLabel;
	[Export]private Label _lgTickCountLabel;

	private int _smTickCount, _lgTickCount;

	public override void _Ready()
	{
		_smTickCount = 0;
		_lgTickCount = 0;

		InitializeEvents();		
		InitializeClock();
	}

	private void InitializeEvents()
	{
		SignalBus.Instance.ClockShortTicked += () => _smTickCount++;
		SignalBus.Instance.ClockLongTicked += () => _lgTickCount++;
	}

	private void InitializeClock()
	{
		_clock1 = GetNode<IdleClock>("/root/IdleClock");
		_clock1.PauseClock();
		_clock1.SetClockParams(_tickLength, _bigTickCount);
		_clock1.StartClock();
	}

	public override void _Process(double delta)
	{
		_smTickCountLabel.Text = $"S{_smTickCount}";
		_lgTickCountLabel.Text = $"L{_lgTickCount}";
	}
}
