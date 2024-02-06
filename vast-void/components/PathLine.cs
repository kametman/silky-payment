using Godot;
using System;

namespace VastVoid.Components;

public partial class PathLine : Node2D
{
	[Export]private Sprite2D _startDotSprite;
	[Export]private Sprite2D _endDotSprite;
	[Export]private Line2D _line2D;

	private Vector2 _pointA;
	private Vector2 _pointB;
	private float _dotDistance;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public void Init(Vector2 pointA, Vector2 pointB, float dotDistance)
	{
		_pointA = pointA;
		_pointB = pointB;

		CreatePathLine();
	}

	private void CreatePathLine()
	{
		_startDotSprite.Position = _pointA;
		_endDotSprite.Position = _pointB;
		_line2D.AddPoint(_pointA);
		_line2D.AddPoint(_pointB);

		


	}
}
