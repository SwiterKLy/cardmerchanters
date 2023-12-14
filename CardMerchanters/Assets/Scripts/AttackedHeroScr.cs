using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AttackedHeroScr : MonoBehaviour,IDropHandler {

	public enum HeroType
	{
		ENEMY,
		ME
	}

	public HeroType Type;
	public Color Normal, Target;
	
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnDrop(PointerEventData eventData)
	{
		if (GameManagerScr.Instance.BotTurn)
			return;

		CardControlerScr card = eventData.pointerDrag.GetComponent<CardControlerScr>();

		if (card && card.Card.CanAttack && Type == HeroType.ENEMY && !GameManagerScr.Instance.EnemyFieldCards.Exists(x => x.Card.IsProvocation))
		{
			GameManagerScr.Instance.DamageHero(card,true);
		}
	}
	public void HightlightAsTarget(bool hightlight)
	{
		GetComponent<Image>().color = hightlight ? Target : Normal;
	}
}
