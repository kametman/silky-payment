using System;
using System.Collections.Generic;
using Godot;
using Shuffled.Components;

namespace Shuffled.Managers;

public partial class CardManager : Node
{
	private PackedScene _playingCardPrefab = ResourceLoader.Load<PackedScene>("uid://cgkvw0mvdfi4r");
	private Dictionary<Guid, List<int>> _cardDecks;

	private CardManager(): base()
	{
		_cardDecks = new Dictionary<Guid, List<int>>();
	}

	public Guid CreateNewDeck(Guid? deckId = null)
	{
		var newDeckId = deckId ?? Guid.NewGuid();

		_cardDecks.Add(newDeckId, GetShuffledDeck());

		return newDeckId;
	}

	private List<int> GetShuffledDeck()
	{
		var shuffledDeck = new List<int> { 0, };
		for (var i = 1; i < 52; i++)
		{
			shuffledDeck.Insert((int)(GD.Randi() % shuffledDeck.Count), i);
		}

		return shuffledDeck;
	}

	public void ResetDeck(Guid deckId)
	{
		if (!_cardDecks.ContainsKey(deckId)) { CreateNewDeck(deckId); }
		else
		{
			if (GetCardCountFromDeck(deckId) == 52) { return; }

			_cardDecks[deckId].Clear();
			_cardDecks[deckId].AddRange(GetShuffledDeck());
		}
	}

	public PlayingCard GetRandomCard(bool isFaceDown = false)
	{
		var newCard = CreateCardWithValue((int)(GD.Randi() % 52), isFaceDown);

		return newCard;
	}

	private PlayingCard CreateCardWithValue(int cardValue, bool isFaceDown = false)
	{
		var newCard = _playingCardPrefab.Instantiate<PlayingCard>();
		newCard.Init(cardValue, isFaceDown);

		return newCard;
	}

	public int GetCardCountFromDeck(Guid deckID)
	{
		if (_cardDecks.ContainsKey(deckID)) { return _cardDecks[deckID].Count; }
		else { return -1; }
	}

	public PlayingCard GetCardFromDeck(Guid deckID, bool isFaceDown = false)
	{
		if (!_cardDecks.ContainsKey(deckID)) { return null; }
		if (GetCardCountFromDeck(deckID) == 0) { return null; }

		var cardValue = _cardDecks[deckID][0];
		_cardDecks[deckID].RemoveAt(0);
		var newCard = CreateCardWithValue(cardValue, isFaceDown);

		return newCard;
	}

	public PlayingCard[] GetCardsFromDeck(Guid deckID, int cardCount, bool isFaceDown = false)
	{
		if (!_cardDecks.ContainsKey(deckID)) { return null; }
		if (cardCount == 0) { return null; }
		if (GetCardCountFromDeck(deckID) < cardCount) { return null; }

		var returnedCards = new PlayingCard[cardCount];
		for (var i = 0; i < cardCount; i++)
		{
			returnedCards[i] = GetCardFromDeck(deckID, isFaceDown);
		}

		return returnedCards;
	}

	public int GetSuitValue(int cardValue)
	{
		return cardValue % 52 / 13;
	}

	public int GetRankValue(int cardValue)
	{
		return cardValue % 52 % 13 + 1;
	}

	public string GetDescriptionFromValue(int cardValue)
	{
		var suitDescription = GetSuitDescription(GetSuitValue(cardValue));
		var rankDescription = GetRankDescription(GetRankValue(cardValue));

		return $"{rankDescription} of {suitDescription}";
	}

	public string GetSuitDescription(int suitValue)
	{
		if (suitValue == 0) { return "Hearts"; }
		else if (suitValue == 1) { return "Diamonds"; }
		else if (suitValue == 2) { return "Clubs"; }
		else if (suitValue == 3) { return "Spades"; }
		else { return "blank"; }
	}

	public string GetRankDescription(int rankValue)
	{
		if (rankValue == 1) { return "Ace"; }
		else if (rankValue == 2) { return "Two"; }
		else if (rankValue == 3) { return "Three"; }
		else if (rankValue == 4) { return "Four"; }
		else if (rankValue == 5) { return "Five"; }
		else if (rankValue == 6) { return "Six"; }
		else if (rankValue == 7) { return "Seven"; }
		else if (rankValue == 8) { return "Eight"; }
		else if (rankValue == 9) { return "Nine"; }
		else if (rankValue == 10) { return "Ten"; }
		else if (rankValue == 11) { return "Jack"; }
		else if (rankValue == 12) { return "Queen"; }
		else if (rankValue == 13) { return "King"; }
		else { return "zero"; }
	}

	private static CardManager _instance = null;
	public static CardManager Instance
	{
		get
		{
			if (_instance == null) { _instance = new CardManager(); }

			return _instance;
		}
	}
}
