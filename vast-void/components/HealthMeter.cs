using Godot;
using System;
using VastVoid.Components;

[Tool]
public partial class HealthMeter : Node2D
{
	[Export]public int MaxHealth
	{
		get { return _maxHealth; }
		set
		{
			_maxHealth = value;
			UpdateMeters();
		}
	}
	[Export]public int CurrentHealth
	{
		get { return _currentHealth; }
		set
		{
			_currentHealth = value;
			UpdateMeters();
		}
	}

	private int _maxHealth = 100;
	private int _currentHealth = 100;

	private ProgressMeter _meterA;
	private ProgressMeter _meterB;

	public override void _Ready()
	{
		_meterA = GetNode<ProgressMeter>("MeterA");
		_meterB = GetNode<ProgressMeter>("MeterB");

		UpdateMeters();
	}

	public override void _Process(double delta)
	{
	}

	private void UpdateMeters()
	{
		if (_meterA == null) { return; }
		if (_meterB == null) { return; }

		_meterA.MaxValue = _meterB.MaxValue = _maxHealth;
		_meterA.CurrentValue = _currentHealth;
		_meterB.CurrentValue = _maxHealth - _currentHealth;
	}
}
