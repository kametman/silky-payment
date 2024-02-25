using System;
using System.Collections.Generic;
using Godot;
using Shuffled.Components;

namespace Shuffled.Managers;

public partial class CardFactory : Node
{
	private PackedScene _playingCardPrefab = ResourceLoader.Load<PackedScene>("uid://cgkvw0mvdfi4r");
	private Dictionary<Guid, List<int>> _cardDecks;

	private CardFactory(): base()
	{
		_cardDecks = new Dictionary<Guid, List<int>>();
	}

	public Guid CreateNewDeck()
	{
		var deckID = Guid.NewGuid();

		var newDeck = new List<int>();
		for (var i = 0; i < 52; i++)
		{
			newDeck.Insert((int)(GD.Randi() % newDeck.Count), i);
		}
		_cardDecks.Add(deckID, newDeck);

		return deckID;
	}

	public PlayingCard GetRandomCard(bool isFaceDown = false)
	{
		var newCard = _playingCardPrefab.Instantiate<PlayingCard>();
		newCard.Init((int)(GD.Randi() % 52), isFaceDown);

		return newCard;
	}

	public int GetCardCountFromDeck(Guid deckID)
	{
		if (_cardDecks.ContainsKey(deckID)) { return _cardDecks[deckID].Count; }
		else { return -1; }
	}

	public PlayingCard GetCardFromDeck(Guid deckID)
	{
		throw new NotImplementedException();
	}

	public string GetDescriptionFromValue(int cardValue)
	{
		var value = cardValue % 52;
		var suitDescription = GetSuitDescription(value / 13);
		var rankDescription = GetRankDescription(value % 13);

		return $"{rankDescription} of {suitDescription}";
	}

	private string GetSuitDescription(int suitValue)
	{
		if (suitValue == 0) { return "Hearts"; }
		else if (suitValue == 1) { return "Diamonds"; }
		else if (suitValue == 2) { return "Clubs"; }
		else if (suitValue == 3) { return "Spades"; }
		else { return "blank"; }
	}

	private string GetRankDescription(int rankValue)
	{
		if (rankValue == 0) { return "Ace"; }
		else if (rankValue == 1) { return "Two"; }
		else if (rankValue == 2) { return "Three"; }
		else if (rankValue == 3) { return "Four"; }
		else if (rankValue == 4) { return "Five"; }
		else if (rankValue == 5) { return "Six"; }
		else if (rankValue == 6) { return "Seven"; }
		else if (rankValue == 7) { return "Eight"; }
		else if (rankValue == 8) { return "Nine"; }
		else if (rankValue == 9) { return "Ten"; }
		else if (rankValue == 10) { return "Jack"; }
		else if (rankValue == 11) { return "Queen"; }
		else if (rankValue == 12) { return "King"; }
		else { return "zero"; }
	}

	private static CardFactory _instance = null;
	public static CardFactory Instance
	{
		get
		{
			if (_instance == null) { _instance = new CardFactory(); }

			return _instance;
		}
	}
}
