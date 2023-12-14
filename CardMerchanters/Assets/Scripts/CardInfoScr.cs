using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardInfoScr : MonoBehaviour
{
   public CardControlerScr CC;
    public Image Logo, CardColor;
    public Text Name,Attack,HP;
    [FormerlySerializedAs("Dripcost")] public Text Dripcost;
    public Text Ability,SpellAbility;
    public GameObject HideObj, CardBG;
   public Color Normal, Target,agro,SpellTargetCol;


   public void HideCardInfo()
   {
      HideObj.SetActive(true);
      Attack.text = null;
      HP.text = null;
      Dripcost.text = null;
      Ability.text = null;
   }
   public void ShowCardInfo()
   {
      Logo.sprite = CC.Card.Logo;
      Logo.preserveAspect = false;
      Name.text = CC.Card.Name;
      HideObj.SetActive(false);  
      if(!CC.Card.IsSpell)
      {
         Attack.text = CC.Card.Attack.ToString();
         HP.text = CC.Card.currentHP.ToString();
        
      }
      else
      {
         Attack.text = "";
         HP.text = "";
      }
      Dripcost.text = CC.Card.dripcost.ToString();
      if (CC.Card.Abilities.Exists(x =>x == Card.Ability_Type.Provocation))
      {
         Ability.text = "Провокація";
      }
      else
      if (CC.Card.Abilities.Exists(x =>x == Card.Ability_Type.WindFury))
      {
         Ability.text = "Двійна атака";
      }
      else
      if (CC.Card.Abilities.Exists(x =>x == Card.Ability_Type.Regeneration))
      {
         Ability.text = "Регенерація";
      }
      else if (CC.Card.IsSpell)
      {
            CardBG.SetActive(false);
        CardColor.color = new Color(255,255,255,0);
         if (CC.Card.Effect == Card.Spells.DamageTarget)
         {
            SpellAbility.text = "Наносить " + CC.Card.SpellValue.ToString() + "  шкоди карті";
         }
         else 
         if (CC.Card.Effect == Card.Spells.HealTarget)
         {
            SpellAbility.text = "+" + CC.Card.SpellValue.ToString() + " здоров`я карті";
         }
         
      }
      else
         Ability.text = "";
   }

   public void Atttack(bool hightlight)
   {
      if(hightlight)
         CardColor.color = new Color(0.2f, 0.72f, 0.42f);
      else
      CardColor.color = new Color(255f, 255f, 255f);
   }
   public void HighlightDripAvailability(int currentMana)
   {
      GetComponent<CanvasGroup>().alpha = currentMana >= CC.Card.dripcost ? 
         1 :
         .5f;
   }

   public void RefreshData()
   {
      Attack.text = CC.Card.Attack.ToString();
      HP.text = CC.Card.currentHP.ToString();
   }

   public void HightlightAsTarget(bool hightlight)
   {
      if (CC.Card.IsProvocation)
      {
         CardColor.color = hightlight ? Target : agro;
      }
      else
         CardColor.color = hightlight ? Target : Normal;
   }
   public void HightlightAsSpellTarget(bool hightlight)
   {
      if (CC.Card.IsProvocation)
      {
         CardColor.color = hightlight ? SpellTargetCol : agro;
      }
      else
         CardColor.color = hightlight ? SpellTargetCol : Normal;
   }
   public void HightlightAsProvocation()
   {
      CardColor.color = agro;
   }
   private void Start()
   {
      //ShowCardInfo(CardManager.AllCards[transform.GetSiblingIndex()]);
   }
  
}
