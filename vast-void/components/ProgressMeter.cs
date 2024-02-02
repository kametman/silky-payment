using Godot;
using System;

namespace VastVoid.Components;

public partial class ProgressMeter : Node2D
{
	private int _maxValue = 100;
	[Export]public int MaxValue
	{
		get { return _maxValue; }
		set 
		{
			_maxValue = value;
			CalculateProgressValue();
		}
	}

	private int _currentValue = 0;
	[Export]public int CurrentValue
	{
		get { return _currentValue; }
		set 
		{
			_currentValue = value;
			CalculateProgressValue();
		}
	}

	[Export]private Sprite2D _progressSprite;

	private bool _initialized = false;
	private float _progressValue;

	public override void _Ready()
	{
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
}
