using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardControlerScr : MonoBehaviour
{
	public Card Card;
	public BattlerCard BattlerCard;
	public bool IsPlayerCard;
	public CardInfoScr Info;
	public CardMovementScr Movement;
	private GameManagerScr gameManager;
	public CardAbilityScr Ability;

	public void Init(Card сard, bool IsplayerCard)
	{
		Card = сard;
		gameManager = GameManagerScr.Instance;
		IsPlayerCard = IsplayerCard;
		if (IsPlayerCard)
		{
			Info.ShowCardInfo();
			GetComponent<AttackedCardScr>().enabled = false;
		}
		else
			Info.HideCardInfo();
	}

	public void OnCast()
	{
		if (Card.IsSpell && SpellCard.Target != SpellCard.TargetType.No_Target)
			return;
		if (IsPlayerCard)
		{
			gameManager.MyHandCards.Remove(this);
			gameManager.MyFieldCards.Add(this);
			gameManager.ReduceDrip(true,Card.dripcost);
			gameManager.CheckCardsForAvaliability();
		}
		else
		{
			gameManager.EnemyHandCards.Remove(this);
			gameManager.EnemyFieldCards.Add(this);
			gameManager.ReduceDrip(false,Card.dripcost);
			Info.ShowCardInfo();
		}

		Card.IsPlaced = true;
		Info.HightlightAsTarget(false);
		if (Card.HasAbility == false)
		{
			Debug.Log("Bez abilki");
			//Ability.OnCast();
		}

		if (Card.IsSpell)
		{
			UseSpell(null);
		}
	}
	public void OnTakeDamage(CardControlerScr attacker = null)
	{
		CheckForAlive();
	}
	public void OnDamageDeal()
	{
		Card.TimesAttack++;
		Debug.Log(Card.TimesAttack);
		Card.CanAttack = false;
		Info.Atttack(false);
		if(Card.HasAbility == true)
		{
			Ability.OnDamageDeal();
		}
	}

	public void UseSpell(CardControlerScr target)
	{
		Debug.Log("Я работаю");
		switch (Card.Effect)
		{
				
			case Card.Spells.DamageTarget :
				DealDamageToCard(target,Card.SpellValue);
				break;
			case Card.Spells.HealTarget :
				DealDamageToCard(target,-Card.SpellValue);
				break;
		}

		if (target != null)
		{
			target.Ability.OnCast();
			target.CheckForAlive();
		}
		DestroyCard();
	}

	private void DealDamageToCard(CardControlerScr card,int damage)
	{
		Debug.Log("Я нанес урон цели");
		card.Card.GainDMG(damage);
		card.CheckForAlive();
		card.OnTakeDamage();
	}
	public void CheckForAlive()
	{
		if (Card.IsAlive)
			Info.RefreshData();
		else
			DestroyCard();
	}
	void DestroyCard()
	{
		
		Movement.OnEndDrag(null);
		RemoveCardFromList(gameManager.EnemyFieldCards);
		RemoveCardFromList(gameManager.EnemyHandCards);
		RemoveCardFromList(gameManager.MyFieldCards);
		RemoveCardFromList(gameManager.MyHandCards);
		Destroy(gameObject);
	}

	void RemoveCardFromList(List<CardControlerScr> list)
	{
		if (list.Exists(x => x == this))
			list.Remove(this);
	}
}
