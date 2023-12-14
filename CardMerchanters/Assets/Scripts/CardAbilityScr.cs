using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbilityScr : MonoBehaviour
{
    public CardControlerScr CC;

    public void OnCast()
    {
        foreach (var ability in CC.Card.Abilities)
        {
            switch (ability)
            {
                case Card.Ability_Type.Provocation:
                    CC.Info.HightlightAsProvocation();
                    break;
            }
        }
    }

    public void OnDamageDeal()
    {
        Debug.Log("drip");
        foreach (var ability in CC.Card.Abilities)
        {
            switch (ability)
            {
                case Card.Ability_Type.WindFury:
                    if (CC.Card.TimesAttack == 1)
                    {
                        CC.Card.CanAttack = true;
                        if (CC.IsPlayerCard)
                            CC.Info.Atttack(true);
                    }
                    else
                    {
                        CC.Card.TimesAttack = 0;
                    }
                    break;
            }
        }
    }
    public void OnNewTurn()
    {
        CC.Card.TimesAttack = 0;
        foreach (var ability in CC.Card.Abilities)
        {
            switch (ability)
            {
                case Card.Ability_Type.Regeneration:
                    if (CC.Card.currentHP + 2 >= CC.Card.HP)
                    {
                        CC.Card.currentHP = CC.Card.HP;
                    }
                    else
                    {
                        CC.Card.currentHP += 2;
                    }
                    CC.Info.RefreshData();
                    break;
            }
        }
    }
}
