using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class AttackedCardScr : MonoBehaviour,IDropHandler {
	
public void OnDrop(PointerEventData eventData)
{
	if (GameManagerScr.Instance.BotTurn)
		return;
		CardControlerScr card = eventData.pointerDrag.GetComponent<CardControlerScr>();

		CardControlerScr Enemycard = GetComponent<CardControlerScr>(); 
	    if(Enemycard = card)
        {
			Debug.Log("БЛЯТЬ");
		}
	if (card && card.Card.CanAttack && Enemycard.Card.IsPlaced)
	{
			Enemycard = GetComponent<CardControlerScr>();
			if (GameManagerScr.Instance.EnemyFieldCards.Exists(x => x.Card.IsProvocation) &&
		    (!Enemycard.Card.IsProvocation))
		{
				Debug.Log("Не провокацию бьеш сынок");
			return;
		}
		GameManagerScr.Instance.CardFight(card, GetComponent<CardControlerScr>());
	}

}
}

