using Godot;
using VastVoid.Components;
using VastVoid.Managers;

public partial class PlayerPath : Node2D
{
	private IdleClock _clock1;

	[Export]private Path2D _environmentPath;
	[Export]private Node2D _playerNode;
	[Export]private float _playerSpeed = 50f;

	[Export]private Button _clockButton;

	private PackedScene _pathLinePrefab;
	
	private int _currentPoint = 0;
	private bool _isPlayerMoving = false;
	private bool _isTravelPaused;

	public override void _Ready()
	{
		_pathLinePrefab = ResourceLoader.Load<PackedScene>("uid://y5y1nv34jpb2");
		var firstPoint = _environmentPath.Curve.GetPointPosition(0);
		_playerNode.Position = firstPoint;

		InitializeClock();

		SignalBus.Instance.ClockShortTicked += () => MoveToNextPointOnPath();

		CreateAPathLine();
	}

	private void InitializeClock()
	{
		_clock1 = GetNode<IdleClock>("/root/IdleClock");
		_clock1.PauseClock();
		_clock1.SetClockParams(0.25f, 5);
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
		var travelTime = (nextPoint - _playerNode.Position).Length() / _playerSpeed;


		var moveTween = CreateTween();
		moveTween.TweenProperty(_playerNode, "position", nextPoint, travelTime);
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

	private void CreateAPathLine()
	{
		var firstPoint = _environmentPath.Curve.GetPointPosition(0);
		var secondPoint = _environmentPath.Curve.GetPointPosition(1);

		var newPathLine = _pathLinePrefab.Instantiate<PathLine>();
		newPathLine.Init(firstPoint, secondPoint, 10f);

		AddChild(newPathLine);
	}
}
