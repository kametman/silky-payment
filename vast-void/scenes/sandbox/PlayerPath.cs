using Godot;
using VastVoid.Components;
using VastVoid.Managers;

public partial class PlayerPath : Node2D
{
	private IdleClock _clock1;

	[Export]private Path2D _environmentPath;
	[Export]private Sprite2D _playerSprite;
	[Export]private float _playerSpeed = 50f;

	[Export]private Button _clockButton;
	
	private int _currentPoint = 0;
	private bool _isPlayerMoving = false;
	private bool _isTravelPaused;

	public override void _Ready()
	{
		var firstPoint = _environmentPath.Curve.GetPointPosition(0);
		_playerSprite.Position = firstPoint;

		InitializeClock();

		SignalBus.Instance.ClockShortTicked += () => MoveToNextPointOnPath();
	}

	private void InitializeClock()
	{
		_clock1 = GetNode<IdleClock>("/root/IdleClock");
		_clock1.PauseClock();
		_clock1.SetClockParams(1, 5);
		_clock1.StartClock();
	}

	private void MoveToNextPointOnPath()
	{
		if (_isPlayerMoving) { return; }
		if (_isTravelPaused) 
		{ 
			_clockButton.Disabled = false;
			return; 
		}

		_isPlayerMoving = true;
		//_clock1.PauseClock();
		var nextPoint = _environmentPath.Curve.GetPointPosition(++_currentPoint % _environmentPath.Curve.PointCount);
		var travelTime = (nextPoint - _playerSprite.Position).Length() / _playerSpeed;


		var moveTween = CreateTween();
		moveTween.TweenProperty(_playerSprite, "position", nextPoint, travelTime);
		moveTween.TweenCallback(Callable.From(() => 
		{ 
			_isPlayerMoving = false; 
			if (!_isTravelPaused) { _clock1.StartClock(); }
			_clockButton.Disabled = false;
		}));
	}

	public override void _Process(double delta)
	{
		_clockButton.Text = _isTravelPaused ? "PROCEED" : "PAUSE";
	}

	public void OnClockButtonPressed()
	{
		_isTravelPaused = !_isTravelPaused;
		if (_isTravelPaused) 
		{ 
			_clockButton.Disabled = true; 
			_clock1.PauseClock();
		}
		else
		{
			_clock1.StartClock();
		}

	}
}
