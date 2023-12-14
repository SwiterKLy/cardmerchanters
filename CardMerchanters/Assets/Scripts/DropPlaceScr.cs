using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum FieldType
{
    My_Hand,
    Enemy_Hand,
    My_Field,
    Enemy_Field
}
public class DropPlaceScr : MonoBehaviour,IDropHandler,IPointerEnterHandler,IPointerExitHandler
{
    public FieldType type;

    public void OnDrop(PointerEventData eventData)
    {   
        if (type == FieldType.My_Hand || type == FieldType.Enemy_Hand || type == FieldType.Enemy_Field)
            return;
        CardControlerScr card = eventData.pointerDrag.GetComponent<CardControlerScr>();
        if (card && !GameManagerScr.Instance.BotTurn && 
            GameManagerScr.Instance.Drip >= card.Card.dripcost && !card.Card.IsPlaced)
        {
            if (!card.Card.IsSpell)
            {
                card.Movement.DefaultParent = transform;
                card.Card.BecomeFriend();
            }
            card.OnCast();
            }
           
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || type == FieldType.Enemy_Field || type == FieldType.Enemy_Hand)
            return;

        CardMovementScr card = eventData.pointerDrag.GetComponent<CardMovementScr>();

        if (card)
            card.DefaultTempCardParent = transform;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        CardMovementScr card = eventData.pointerDrag.GetComponent<CardMovementScr>();
        
        if (card && card.DefaultTempCardParent == transform)
            card.DefaultTempCardParent = card.DefaultParent;
    }
}
