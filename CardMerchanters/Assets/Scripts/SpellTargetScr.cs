using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellTargetScr : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (GameManagerScr.Instance.BotTurn)
            return;
        CardControlerScr spell = eventData.pointerDrag.GetComponent<CardControlerScr>();
        CardControlerScr target = eventData.pointerDrag.GetComponent<CardControlerScr>();
        if (spell.Card.IsSpell && GetComponent<CardControlerScr>().Card.IsPlaced && GameManagerScr.Instance.Drip >= spell.Card.dripcost)
        {
            GameManagerScr.Instance.ReduceDrip(true, spell.Card.dripcost);
            spell.UseSpell(GetComponent<CardControlerScr>());
            GameManagerScr.Instance.CheckCardsForAvaliability();
        }
    }

}
