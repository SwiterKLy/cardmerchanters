using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngineInternal.Input;
using Random = UnityEngine.Random;

public class Game
{
	public List<Card> EnemyDeck;
	public List<Card> MyDeck;
	public Game()
	{
		EnemyDeck = BuildEnemyDeck();
		MyDeck = BuildMyDeck();
	}

	private List<Card> BuildMyDeck()
	{
		List<Card> list = new List<Card>();
		for (int i = 0; i < 15; i++)
			list.Add(CardManager.MyDeck[i].GetCopy());
		return list;
	}
	private List<Card> BuildEnemyDeck()
	{
		List<Card> list = new List<Card>();
		for (int i = 0; i < 15; i++)
			list.Add(CardManager.EnemyDeck[i].GetCopy());
		return list;
	}
	
}

public class GameManagerScr : MonoBehaviour
{
	public int score = 0;
	public int bestscore = 0;
	public AudioSource win, lose,main;
	public static GameManagerScr Instance;
	private Game CurrentGame;
	public CardManagerScr CardList;

	public Transform EnemyHand, MyHand,MyField,EnemyField;

	public GameObject CardPref;

	private int Turn = 1;
	private int TurnTime = 30;

	public Text TurnTimeText;

	public Button EndTurnBtn;

	public bool BotTurn = true;

	public int MyMaxCum = 0, EnemyMaxCum = 0;

	[FormerlySerializedAs("MyCum")] public int Drip = 0;
	public int EnemyCum = 0;
	public Text MyCumTxt, EnemyCumTxt;

	public int MyHP, EnemyHP;
	
	public Text MyHPTxt, EnemyHPTxt;

	public GameObject ResultBack;

	public Text ResultText;
	public Text ScoreText;

	public int MyCountOfCards = 15;
	public int EnemyCountOfCards = 15;

	public AttackedHeroScr MyHero,EnemyHero;

	public List<CardControlerScr> MyHandCards = new List<CardControlerScr>();
	public List<CardControlerScr> EnemyHandCards = new List<CardControlerScr>();
	public List<CardControlerScr> MyFieldCards = new List<CardControlerScr>();
	public List<CardControlerScr> EnemyFieldCards = new List<CardControlerScr>();

	public void CreateCardPref(Card card, Transform hand)
	{
		GameObject cardGO = Instantiate(CardPref, hand, false);
		CardControlerScr cardC = cardGO.GetComponent<CardControlerScr>();
		cardC.Init(card,hand == MyHand);
		if(cardC.IsPlayerCard)
			MyHandCards.Add(cardC);
		else
			EnemyHandCards.Add(cardC);
	}
	private void Awake()
	{
		CardList = GetComponent<CardManagerScr>();
		if(Instance == null)
		Instance = this;
	}

	public Image Avatar;
	public Sprite img1;
	public Sprite img2;
	public Sprite img3;
	private void Start()
	{
		StartG();
		bestscore = PlayerPrefs.GetInt("My");
		if (PlayerPrefs.GetInt("Img") == 1)
		{
			Avatar.sprite = img1;
		}
		else
   if (PlayerPrefs.GetInt("Img") == 2)
		{
			Avatar.sprite = img2;
		}
		else
			   if (PlayerPrefs.GetInt("Img") == 3)
		{
			Avatar.sprite = img3;
		}
	}
	public void RestartG()
	{
		StopAllCoroutines();
		foreach(var card in MyFieldCards)
			Destroy(card.gameObject);
		foreach(var card in EnemyFieldCards)
			Destroy(card.gameObject);
		foreach(var card in MyHandCards)
			Destroy(card.gameObject);
		foreach(var card in EnemyHandCards)
			Destroy(card.gameObject);
		
		MyCountOfCards = 15;
	    EnemyCountOfCards = 15;
		MyHandCards.Clear();
		EnemyHandCards.Clear();
		MyFieldCards.Clear();
		EnemyFieldCards.Clear();
		CardList.ClearDeck();
		StartG();
		win.Stop();
		lose.Stop();
		main.Play();
	}
	public void StartG()
	{
		Turn = 0;
		EndTurnBtn.interactable = true;
		CurrentGame = new Game();
		ChangeTurn();
		GiveHandCards(CurrentGame.EnemyDeck,EnemyHand,5);
		GiveHandCards(CurrentGame.MyDeck,MyHand,4);
		MyMaxCum = EnemyMaxCum = Drip = EnemyCum = 1;
		MyHP = EnemyHP = 30;
		CheckCardsForAvaliability();
		RenewHP();
		UpdateCum();
		ResultBack.SetActive(false);
		StartCoroutine(TurnFunc());
	}

	private void GiveHandCards(List<Card> deck, Transform hand,int count)
	{
		for(int i = 0;i < count;i++)
		GiveCardToHand(deck,hand);
	}

	void GiveCardToHand(List<Card> deck, Transform hand)
	{
		if (deck.Count == 0 || (hand == EnemyHand && EnemyHandCards.Count >=5) || (hand == MyHand && MyHandCards.Count >=5))
			return;
		else
		{
			int index;
			if (hand == EnemyHand)
			{
				index = Random.Range(0, EnemyCountOfCards);
				EnemyCountOfCards--;
				CreateCardPref(deck[index], hand);
				deck.RemoveAt(index);
			}
			else if (hand == MyHand)
			{
				index = Random.Range(0, MyCountOfCards);
				MyCountOfCards--;
				CreateCardPref(deck[index], hand);
				deck.RemoveAt(index);
			}
			
		}
	}

	public void CardFight(CardControlerScr myCard,CardControlerScr enemyCard)
	{
		myCard.Card.GainDMG(enemyCard.Card.Attack);
		myCard.OnDamageDeal();
		enemyCard.OnTakeDamage();
		enemyCard.Card.GainDMG(myCard.Card.Attack);
		myCard.OnTakeDamage();
		myCard.CheckForAlive();
		enemyCard.CheckForAlive();
	}

	public void RenewHP()
	{
		MyHPTxt.text = MyHP.ToString();
		EnemyHPTxt.text = EnemyHP.ToString();
	}
	public void RenewAttack()
	{
		foreach (var card in MyFieldCards)
		{
			if (card.Card.CanAttack == true)
			{
				card.Info.Atttack(true);
			}
		}
	}
	public void DamageHero(CardControlerScr card,bool IsEnemyAttacked)
	{
		if (IsEnemyAttacked)
			EnemyHP = Mathf.Clamp(EnemyHP - card.Card.Attack, 0, int.MaxValue);
		else
			MyHP = Mathf.Clamp(MyHP - card.Card.Attack, 0, int.MaxValue);
		
		RenewHP();
		card.Info.Atttack(false);
		card.OnDamageDeal();
		GameResult();
	}
	IEnumerator TurnFunc()
	{
		
		TurnTime = 30;
		TurnTimeText.text = TurnTime.ToString();
		foreach(var card in EnemyFieldCards)
		{
			card.Card.CanAttack = true;
			card.Info.Atttack(false);
		}
		if (!BotTurn)
		{
			if (Turn > 2)
			{
				if (MyMaxCum < 10)
				{
					MyMaxCum++;
				}

				Drip = MyMaxCum;
			}
			UpdateCum();
			CheckCardsForAvaliability();
			foreach (var card in MyFieldCards)
			{
				card.Card.CanAttack = true;
				card.Info.Atttack(true);
				card.Ability.OnNewTurn();
			}
			while (TurnTime-- > 0)
			{
				
				TurnTimeText.text = TurnTime.ToString();
				yield return new WaitForSeconds(1);
			}
			ChangeTurn();
		}
		else
		{
			if (Turn > 3)
			{ 
			if (EnemyMaxCum < 10)
				{
			EnemyMaxCum++;
				}
			EnemyCum = EnemyMaxCum;
			}

			foreach (var card in EnemyFieldCards)
			{
				card.Card.CanAttack = true;
				card.Ability.OnNewTurn();
			}
			
			StartCoroutine(EnemyTurn(EnemyHandCards));
		}
	}
	
	IEnumerator EnemyTurn(List<CardControlerScr> cards)
	{

		yield return new WaitForSeconds(1);
		
		int count = cards.Count == 1 ? 1 : Random.Range(0, cards.Count);
		for (int i = 0; i < count; i++)
		{
			if (EnemyFieldCards.Count > 5 || EnemyCum == 0 || EnemyHandCards.Count == 0)
				break;

			List<CardControlerScr> cardList = cards.FindAll(x => EnemyCum >= x.Card.dripcost);

			if (cardList.Count == 0)
				break;

			cardList[0].GetComponent<CardMovementScr>().MoveToField(EnemyField);
			yield return new WaitForSeconds(.59f);
			cardList[0].transform.SetParent(EnemyField);
			cardList[0].OnCast();
		}

		yield return new WaitForSeconds(1);
		while(EnemyFieldCards.Exists(x => x.Card.CanAttack))
		{
			var activeCard = EnemyFieldCards.FindAll(x => x.Card.CanAttack)[0];
			bool hasProvocation = MyFieldCards.Exists(x => x.Card.IsProvocation);
			if (hasProvocation || Random.Range(0,2) == 0 && MyFieldCards.Count > 0)
			{
				CardControlerScr Enemy;
				Enemy = hasProvocation ? MyFieldCards.Find(x => x.Card.IsProvocation) : MyFieldCards[Random.Range(0,MyFieldCards.Count)];
				activeCard.Movement.MoveToTarget(Enemy.transform);
				activeCard.OnDamageDeal();
			yield return new WaitForSeconds(.25f);
			CardFight(Enemy, activeCard);
			}
			else
			{
				Debug.Log("Lomayu lico");
				activeCard.GetComponent<CardMovementScr>().MoveToTarget(MyHero.transform);
				yield return new WaitForSeconds(.75f);
				DamageHero(activeCard,false);
				activeCard.OnDamageDeal();
			}
			yield return new WaitForSeconds(.2f);
		}
		yield return new WaitForSeconds(1);
		ChangeTurn();
	}
	public void ChangeTurn()
	{
		score = -40;
		StopAllCoroutines();
		Turn++;
		EndTurnBtn.interactable = BotTurn;
		if (BotTurn)
		{GiveHandCards(CurrentGame.MyDeck,MyHand,1);}
		else
		{GiveHandCards(CurrentGame.EnemyDeck,EnemyHand,1);}
		BotTurn = !BotTurn;
		StartCoroutine(TurnFunc());
	}
	public void UpdateCum()
	{
		MyCumTxt.text = Drip.ToString() + "/" + MyMaxCum.ToString();
		EnemyCumTxt.text = EnemyCum.ToString() + "/" + EnemyMaxCum.ToString();
	}

	public void ReduceDrip(bool ThisMyCum,int CumCost)
	{
		score += 20 * CumCost;
		if (ThisMyCum)
		{
			Drip = Mathf.Clamp(Drip - CumCost, 0, int.MaxValue);
		}
		else
		{
			EnemyCum = Mathf.Clamp(EnemyCum - CumCost, 0, int.MaxValue);
		}
		
		UpdateCum();
	}

	public void GameResult()
	{
		if (EnemyHP == 0)
		{
			StopAllCoroutines();
			ResultBack.SetActive(true);
			ResultText.text = "Перемога";
			ResultText.IsActive();
			main.Stop();
			win.Play();
            score += 500;
			ScoreText.text = score.ToString() + " очків";

		}
        else if (MyHP == 0)
		{
			StopAllCoroutines();
			ResultBack.SetActive(true);
			ResultText.text = "Програш";
			main.Stop();
			lose.Play();
			ScoreText.text = score.ToString() + " очків";
		}
		if (score >= bestscore)
		{
			bestscore = score;
			PlayerPrefs.SetInt("My", bestscore);
			PlayerPrefs.Save();
		}
	}

	public void CheckCardsForAvaliability()
	{
		foreach (var card in MyHandCards)
		{
			card.Info.HighlightDripAvailability(Drip);
		}
	}
	public void HightTargets(bool highlight)
	{
		List<CardControlerScr> targets = new List<CardControlerScr>();
		if (EnemyFieldCards.Exists(x => x.Card.IsProvocation))
			targets = EnemyFieldCards.FindAll(x => x.Card.IsProvocation);
		else
		{
			targets = EnemyFieldCards;
			EnemyHero.HightlightAsTarget(highlight);
		}
		foreach (var card in targets)
		{
			card.Info.HightlightAsTarget(highlight);
		}

	}
	public void HightAtttackSpellTargets(bool highlight)
	{
		List<CardControlerScr> targets = new List<CardControlerScr>();
		targets = EnemyFieldCards;
			foreach (var card in targets)
		{
			card.Info.HightlightAsTarget(highlight);
		}
	}
	public void HightHealSpellTargets(bool highlight)
	{
		List<CardControlerScr> targets = new List<CardControlerScr>();
		targets = MyFieldCards;
			foreach (var card in targets)
		{
			card.Info.HightlightAsSpellTarget(highlight);
		}

	}
}
