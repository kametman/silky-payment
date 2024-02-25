using Godot;
using VastVoid.Managers;

public partial class TimerClock : Node2D
{
	[Export]private Sprite2D _clockSprite;

	private bool _skipTick;

	public override void _Ready()
	{
		SignalBus.Instance.ClockShortTicked += () => AdvanceClockAnimation();

		_skipTick = false;
	}

	public override void _Process(double delta)
	{
	}

	private void AdvanceClockAnimation()
	{
		_skipTick = !_skipTick;

		if (_skipTick) { return; }

		_clockSprite.Frame = (_clockSprite.Frame + 1) % (_clockSprite.Hframes * _clockSprite.Vframes);
	}
}
