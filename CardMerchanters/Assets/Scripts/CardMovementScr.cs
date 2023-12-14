using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class CardMovementScr : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
	public CardControlerScr CC;
	private Camera MainCamera;
	private Vector3 offset;
	public Transform DefaultParent;
	public Transform DefaultTempCardParent;
	private GameObject TempCardGO;
	public bool IsDraggable;
	void Awake()
	{
		MainCamera = Camera.allCameras[0];
		TempCardGO = GameObject.Find("TempCardGO");
	}

	public void OnBeginDrag(PointerEventData eventData)
	{

		offset = transform.position - MainCamera.ScreenToWorldPoint(eventData.position);
		DefaultParent = DefaultTempCardParent = transform.parent;
		IsDraggable = !GameManagerScr.Instance.BotTurn && ((DefaultParent.GetComponent<DropPlaceScr>().type == FieldType.My_Hand &&
		                                                    GameManagerScr.Instance.Drip >= CC.Card.dripcost) ||
		                                                   (DefaultParent.GetComponent<DropPlaceScr>().type == FieldType.My_Field &&
		                                                    CC.Card.CanAttack));
		if (!IsDraggable)
			return;
		if(CC.Card.CanAttack)
		{
			GameManagerScr.Instance.HightTargets(true);
		}
		else if (CC.Card.IsSpell)
		{
			if (CC.Card.Effect == Card.Spells.DamageTarget)
			{
				GameManagerScr.Instance.HightAtttackSpellTargets(true);
			}
			else
			if (CC.Card.Effect == Card.Spells.HealTarget)
			{
				GameManagerScr.Instance.HightHealSpellTargets(true);
			}
		}
		TempCardGO.transform.SetParent(DefaultParent);
		TempCardGO.transform.SetSiblingIndex(transform.GetSiblingIndex());
		transform.SetParent(DefaultParent.parent);
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!IsDraggable)
			return;
		var newPos = MainCamera.ScreenToWorldPoint(eventData.position);
		transform.position = newPos + offset;
		if(!CC.Card.IsSpell)
		{
			if (TempCardGO.transform.parent != DefaultTempCardParent)
				TempCardGO.transform.SetParent(DefaultTempCardParent);

			if (DefaultParent.GetComponent<DropPlaceScr>().type != FieldType.My_Field)
				CheckPosition();
		}
	}
	public void OnEndDrag(PointerEventData eventData)
	{
		if (!IsDraggable)
			return;
		
		GameManagerScr.Instance.HightTargets(false);
		GameManagerScr.Instance.HightHealSpellTargets(false);
		GameManagerScr.Instance.HightAtttackSpellTargets(false);
		GameManagerScr.Instance.RenewAttack();
		transform.SetParent(DefaultParent);
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		transform.SetSiblingIndex(TempCardGO.transform.GetSiblingIndex());
		TempCardGO.transform.SetParent(GameObject.Find("Canvas").transform);
		TempCardGO.transform.localPosition = new Vector3(4259, 0);
	}
	void CheckPosition()
	{
		int newIndex = DefaultTempCardParent.childCount;

		for (int i = 0; i < DefaultTempCardParent.childCount; i++)
		{
			if (transform.position.x < DefaultTempCardParent.GetChild(i).position.x)
			{
				newIndex = i;

				if (TempCardGO.transform.GetSiblingIndex() < newIndex)
					newIndex--;

				break;
			}
		}
		TempCardGO.transform.SetSiblingIndex(newIndex);
	}

	public void MoveToField(Transform field)
	{
		transform.DOMove(field.position, .2f);
	}
	
	public void MoveToTarget(Transform target)
	{
		StartCoroutine(MoveToTargetCor(target));
		
	}

	IEnumerator MoveToTargetCor(Transform target)
	{
		Vector3 pos = transform.position;
		transform.DOMove(target.position,.15f);
		yield return new WaitForSeconds(.15f);
		transform.DOMove(pos, .15f);
		yield return new WaitForSeconds(.15f);
		
	}
}
