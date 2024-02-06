using Godot;
using System;

namespace VastVoid.Components;

[Tool]
public partial class ProgressMeter : Node2D
{
	[Export]public int MaxValue
	{
		get { return _maxValue; }
		set 
		{
			_maxValue = value;
			CalculateProgressValue();
		}
	}
	[Export]public int CurrentValue
	{
		get { return _currentValue; }
		set 
		{
			_currentValue = value;
			 CalculateProgressValue();
		}
	}
	[Export]public Color MeterColor
	{
		get { return _meterColor; }
		set 
		{
			_meterColor = value;
			UpdateMeterColor();
		}
	}

	private int _maxValue = 100;
	private int _currentValue = 100;
	private Color _meterColor = Colors.White;
	private bool _initialized = false;
	private float _progressValue;

	private Sprite2D _progressSprite;
	
	public override void _Ready()
	{
		_progressSprite = GetNode<Sprite2D>("ProgressSprite");
		UpdateMeterColor();

		_initialized = true;
	}

	public override void _Process(double delta)
	{
		_progressSprite.Scale = new Vector2(_progressValue, 1);
	}

	private void CalculateProgressValue()
	{
		if (!_initialized) { return; }
		var calculatedProgress = (float)_currentValue / _maxValue;
		var newProgressValue = Math.Clamp(calculatedProgress, 0f, 1f);

		var progressTween = CreateTween();
		if (progressTween == null) { return; }

		progressTween.TweenProperty(this, "_progressValue", newProgressValue, 0.1f);
	}

	private void UpdateMeterColor()
	{
		if (_progressSprite == null) { return; }
		_progressSprite.SelfModulate = _meterColor;
	}
}
