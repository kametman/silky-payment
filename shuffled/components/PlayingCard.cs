using Godot;

namespace Shuffled.Components;

public partial class PlayingCard : Node2D
{
	private bool _isFaceDown = true;
	[Export]public bool IsFaceDown
	{
		get { return _isFaceDown; }
		set
		{
			_isFaceDown = value;
			FlipCard();
		}
	}

	private int _cardValue;
	[Export]public int CardValue
	{
		get { return _cardValue; }
		private set
		{
			_cardValue = value % 52;
			SetCardFront();
		}
	}

	[Export]private float _flipSpeed = 0.1f;

	private Sprite2D _cardFrontSprite;
	private Sprite2D _cardBackSprite;

	public override void _Ready()
	{
		_cardFrontSprite = GetNode<Sprite2D>("CardFrontSprite");
		_cardBackSprite = GetNode<Sprite2D>("CardBackSprite");

		SetCardFront();
		_cardFrontSprite.Visible = !_isFaceDown;
		_cardBackSprite.Visible = _isFaceDown;
	}

	public void Init(int cardValue, bool isFaceDown)
	{
		CardValue = cardValue % 52;
		_isFaceDown = isFaceDown;
	}

	private void FlipCard()
	{
		if (_cardFrontSprite == null || _cardFrontSprite.IsQueuedForDeletion()) { return; }
		if (_cardBackSprite == null || _cardBackSprite.IsQueuedForDeletion()) { return; }
		if (_cardFrontSprite.Visible == !_isFaceDown && _cardBackSprite.Visible == IsFaceDown) { return; }

		var flipTweenA = CreateTween();
		flipTweenA.TweenProperty(_cardFrontSprite, "scale", Vector2.Down, _flipSpeed);
		flipTweenA.TweenProperty(_cardBackSprite, "scale", Vector2.Down, _flipSpeed);
		flipTweenA.TweenCallback(Callable.From(() => 
		{
			_cardFrontSprite.Visible = !_isFaceDown;
			_cardBackSprite.Visible = _isFaceDown;

			var flipTweenB = CreateTween();
			flipTweenB.TweenProperty(_cardFrontSprite, "scale", Vector2.One, _flipSpeed);
			flipTweenB.TweenProperty(_cardBackSprite, "scale", Vector2.One, _flipSpeed);
		}));
	}

	private void SetCardFront()
	{
		if (_cardFrontSprite == null || _cardFrontSprite.IsQueuedForDeletion()) { return; }

		_cardFrontSprite.Frame = _cardValue;
	}
}
