using Godot;
using VastVoid.Managers;

namespace VastVoid.Components;

public partial class IdleClock : Node
{
	[Export]private float _tickLength = 0.25f;
	[Export]private ulong _bigTickCount = 10;
	[Export]private Timer _clockTimer;

	private ulong _tickCount;
	private bool _clockPause;

	public bool IsClockPaused
	{
		get { return _clockTimer?.IsStopped() ?? false; }
	}

    public override void _Ready()
    {
        _tickCount = 0;
		_clockTimer.WaitTime = _tickLength;
    }

	public void StartClock()
	{
		_clockTimer.Start();
	}

	public void PauseClock()
	{
		_clockPause = true;
	}

    public void OnClockTimerTimeout()
	{
		_tickCount++;
		SignalBus.Instance.EmitSignal(SignalBus.CLOCK_SHORT_TICKED);
		if (_tickCount % _bigTickCount == 0) { SignalBus.Instance.EmitSignal(SignalBus.CLOCK_LONG_TICKED); }

		if (_clockPause)
		{
			_clockTimer.Stop();
			_clockPause = false;
		}
	}
}
