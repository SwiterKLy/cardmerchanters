using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

class IDGenerate
{
	private int Id = 0;

	public int GetId()
	{
		Id++;
		return Id;
	}

}
public abstract class Card
{
	public enum Ability_Type
	{
		NoAbility,
		Regeneration,
		WindFury,
		Provocation
	}
	
	public List<Ability_Type> Abilities;
	protected int ID;
	public string Name;
	public Sprite Logo;
	public int dripcost;
	public int Attack;
	public int HP;
	public int currentHP;
	public bool CanAttack;
	public bool IsAlive;
	public bool IsPlaced;
	public int TimesAttack;
	public int TimesTakeDamage;
	public bool IsPprovocation;

	public bool IsSpell;
	public bool HasAbility;
	public bool Enemy = false;
	public Card GetCopy()
	{
		Card card = this;
		card.Abilities = new List<Ability_Type>(Abilities);
		return card;
	}
	public bool IsProvocation
	{
		get { return Abilities.Exists(x => x == Ability_Type.Provocation);}
	}
	abstract public void GainDMG(int dmg);
	
	public enum Spells
	{
		DamageTarget,
		HealTarget,
	}
	public enum TargetType
	{
		No_Target,
		Ally_Card_Target,
		Enemy_Card_Target
	}
	abstract public void BecomeFriend();
	abstract public void CheckProvocation(bool x); 
	protected internal Spells Effect;
	public static TargetType Target;
	protected internal int SpellValue;
}

public class BattlerCard : Card
{
	public BattlerCard(int id,string name,string logoPath,int attack,int hp
		,int Ddripcost,Ability_Type ability = 0)

	{
		IsSpell = false;
		ID = id;
		Name = name;
		Logo = Resources.Load<Sprite>(logoPath);
		Attack = attack;
		HP = hp;
		dripcost = Ddripcost;
		currentHP = hp;
		CanAttack = false;
		IsAlive = true;
		IsPlaced = false;
		Abilities = new List<Ability_Type>();
		if (ability != 0)
		{
			Abilities.Add(ability);
			HasAbility = true;
		}
		else
		{
			if (ability == Ability_Type.NoAbility)
			{
				HasAbility = false;
			}
		}
		IsPprovocation = IsProvocation;
		

		TimesAttack = 0;
	}
	override public void GainDMG(int dmg)
	{
		Debug.Log(Name);
		currentHP = currentHP - dmg;
		if (currentHP <= 0)
		{
			IsAlive = false;
		}
	}

	override public void BecomeFriend()
	{

		Enemy = false;
	}
	override public void CheckProvocation(bool x)
    {
		if (x == true)
		{ IsPprovocation = true; }
		else
		IsPprovocation = IsProvocation;
	}
}

public class SpellCard : Card
{
	public SpellCard(int id, string name, string logoPath, int Ddripcost, Spells spell = 0, TargetType target = 0,
		int spellValue = 0)
	{
		IsSpell = true;
		ID = id;
		Name = name;
		Logo = Resources.Load<Sprite>(logoPath);
		Attack = 0;
		HP = 0;
		dripcost = Ddripcost;
		currentHP = 0;
		CanAttack = false;
		IsAlive = true;
		IsPlaced = false;
		Abilities = new List<Ability_Type>();
		Effect = spell;
		Target = target;
		SpellValue = spellValue;
		TimesAttack = 0;
	}

	public override void GainDMG(int dmg)
	{
	}
	override public void BecomeFriend()
	{
	
	}
	override public void CheckProvocation(bool x)
	{
	}
}

public static class CardManager
	{
		public static List<Card> MyDeck = new List<Card>();
		public static List<Card> EnemyDeck = new List<Card>();
	}

	public class CardManagerScr : MonoBehaviour
	{

		public void Awake()
		{
			CreateDeck();
		}

		private void CreateDeck()
		{
			IDGenerate key = new IDGenerate();
			CardManager.MyDeck.Add(new BattlerCard(key.GetId(), "Гамлет", "gamlet", 1, 3, 1,
				Card.Ability_Type.Provocation));
			CardManager.MyDeck.Add(new BattlerCard(key.GetId(), "Гамлет", "gamlet", 1, 3, 1,
				Card.Ability_Type.Provocation));
			CardManager.MyDeck.Add(new BattlerCard(key.GetId(), "Гамлет", "gamlet", 1, 3, 1,
				Card.Ability_Type.Provocation));
			CardManager.MyDeck.Add(
				new BattlerCard(key.GetId(), "Шерлок", "Sherlok", 6, 7, 6, Card.Ability_Type.Regeneration));
				CardManager.MyDeck.Add(
				new BattlerCard(key.GetId(), "Шерлок", "Sherlok", 6, 7, 6, Card.Ability_Type.Regeneration));
			CardManager.MyDeck.Add(new SpellCard(key.GetId(), "Вистріл в голову", "headshot", 2,
				SpellCard.Spells.DamageTarget,
				SpellCard.TargetType.Enemy_Card_Target, 3));
			CardManager.MyDeck.Add(new SpellCard(key.GetId(), "Підготовка", "preparation", 2, SpellCard.Spells.HealTarget,
				SpellCard.TargetType.Ally_Card_Target, 2));
			CardManager.MyDeck.Add(new BattlerCard(key.GetId(), "Том", "Tom", 4, 5, 4,
				Card.Ability_Type.Provocation));
			CardManager.MyDeck.Add(new BattlerCard(key.GetId(), "Том", "Tom", 4, 5, 4,
				Card.Ability_Type.Provocation));
			CardManager.MyDeck.Add(new BattlerCard(key.GetId(), "Жуль Верн", "Jule Vern", 2, 3, 2,
				Card.Ability_Type.NoAbility));
			CardManager.MyDeck.Add(new BattlerCard(key.GetId(), "Жуль Верн", "Jule Vern", 2, 3, 2,
				Card.Ability_Type.NoAbility));
			CardManager.MyDeck.Add(
				new BattlerCard(key.GetId(), "Дон Кі Хот", "DonQiHot", 5, 1, 3, Card.Ability_Type.WindFury));
			CardManager.MyDeck.Add(
				new BattlerCard(key.GetId(), "Дон Кі Хот", "DonQiHot", 5, 1, 3, Card.Ability_Type.WindFury));

			CardManager.MyDeck.Add(new BattlerCard(key.GetId(), "Елізабет", "Elizabet", 5, 6, 5,
				Card.Ability_Type.NoAbility));
			CardManager.MyDeck.Add(new BattlerCard(key.GetId(), "Елізабет", "Elizabet", 5, 6, 5,
				Card.Ability_Type.NoAbility));
		CardManager.EnemyDeck.Add(new BattlerCard(key.GetId(), "Гамлет", "gamlet", 1, 3, 1,
			Card.Ability_Type.Provocation));
		CardManager.EnemyDeck.Add(new BattlerCard(key.GetId(), "Гамлет", "gamlet", 1, 3, 1,
			Card.Ability_Type.Provocation));
		CardManager.EnemyDeck.Add(new BattlerCard(key.GetId(), "Гамлет", "gamlet", 1, 3, 1,
			Card.Ability_Type.Provocation));
		CardManager.EnemyDeck.Add(
			new BattlerCard(key.GetId(), "Шерлок", "Sherlok", 6, 7, 6, Card.Ability_Type.Regeneration));
		CardManager.EnemyDeck.Add(
		new BattlerCard(key.GetId(), "Шерлок", "Sherlok", 6, 7, 6, Card.Ability_Type.Regeneration));
		CardManager.EnemyDeck.Add(new BattlerCard(key.GetId(), "Том", "Tom", 4, 5, 4,
			Card.Ability_Type.Provocation));
		CardManager.EnemyDeck.Add(new BattlerCard(key.GetId(), "Том", "Tom", 4, 5, 4,
			Card.Ability_Type.Provocation));
		CardManager.EnemyDeck.Add(new BattlerCard(key.GetId(), "Жуль Верн", "Jule Vern", 2, 3, 2,
			Card.Ability_Type.NoAbility));
		CardManager.EnemyDeck.Add(new BattlerCard(key.GetId(), "Жуль Верн", "Jule Vern", 2, 3, 2,
			Card.Ability_Type.NoAbility));
		CardManager.EnemyDeck.Add(
			new BattlerCard(key.GetId(), "Дон Кі Хот", "DonQiHot", 5, 1, 3, Card.Ability_Type.WindFury));
		CardManager.EnemyDeck.Add(
			new BattlerCard(key.GetId(), "Дон Кі Хот", "DonQiHot", 5, 1, 3, Card.Ability_Type.WindFury));
		CardManager.EnemyDeck.Add(
			new BattlerCard(key.GetId(), "Дон Кі Хот", "DonQiHot", 5, 1, 3, Card.Ability_Type.WindFury));
		CardManager.EnemyDeck.Add(
			new BattlerCard(key.GetId(), "Дон Кі Хот", "DonQiHot", 5, 1, 3, Card.Ability_Type.WindFury));
		CardManager.EnemyDeck.Add(new BattlerCard(key.GetId(), "Елізабет", "Elizabet", 5, 6, 5,
			Card.Ability_Type.NoAbility));
		CardManager.EnemyDeck.Add(new BattlerCard(key.GetId(), "Елізабет", "Elizabet", 5, 6, 5,
			Card.Ability_Type.NoAbility));
	}
		public void ClearDeck()
		{
			IDGenerate key = new IDGenerate();
			CardManager.MyDeck.Clear();
			CardManager.EnemyDeck.Clear();
		CardManager.MyDeck.Add(new BattlerCard(key.GetId(), "Гамлет", "gamlet", 1, 3, 1,
Card.Ability_Type.Provocation));
		CardManager.MyDeck.Add(new BattlerCard(key.GetId(), "Гамлет", "gamlet", 1, 3, 1,
			Card.Ability_Type.Provocation));
		CardManager.MyDeck.Add(new BattlerCard(key.GetId(), "Гамлет", "gamlet", 1, 3, 1,
			Card.Ability_Type.Provocation));
		CardManager.MyDeck.Add(
			new BattlerCard(key.GetId(), "Шерлок", "Sherlok", 6, 7, 6, Card.Ability_Type.Regeneration));
		CardManager.MyDeck.Add(
		new BattlerCard(key.GetId(), "Шерлок", "Sherlok", 6, 7, 6, Card.Ability_Type.Regeneration));
		CardManager.MyDeck.Add(new SpellCard(key.GetId(), "Вистріл в голову", "headshot", 2,
			SpellCard.Spells.DamageTarget,
			SpellCard.TargetType.Enemy_Card_Target, 3));
		CardManager.MyDeck.Add(new SpellCard(key.GetId(), "Підготовка", "preparation", 2, SpellCard.Spells.HealTarget,
			SpellCard.TargetType.Ally_Card_Target, 2));
		CardManager.MyDeck.Add(new BattlerCard(key.GetId(), "Том", "Tom", 4, 5, 4,
			Card.Ability_Type.Provocation));
		CardManager.MyDeck.Add(new BattlerCard(key.GetId(), "Том", "Tom", 4, 5, 4,
			Card.Ability_Type.Provocation));
		CardManager.MyDeck.Add(new BattlerCard(key.GetId(), "Жуль Верн", "Jule Vern", 2, 3, 2,
			Card.Ability_Type.NoAbility));
		CardManager.MyDeck.Add(new BattlerCard(key.GetId(), "Жуль Верн", "Jule Vern", 2, 3, 2,
			Card.Ability_Type.NoAbility));
		CardManager.MyDeck.Add(
			new BattlerCard(key.GetId(), "Дон Кі Хот", "DonQiHot", 5, 1, 3, Card.Ability_Type.WindFury));
		CardManager.MyDeck.Add(
			new BattlerCard(key.GetId(), "Дон Кі Хот", "DonQiHot", 5, 1, 3, Card.Ability_Type.WindFury));
		CardManager.MyDeck.Add(new BattlerCard(key.GetId(), "Елізабет", "Elizabet", 5, 6, 5,
			Card.Ability_Type.NoAbility));
		CardManager.MyDeck.Add(new BattlerCard(key.GetId(), "Елізабет", "Elizabet", 5, 6, 5,
			Card.Ability_Type.NoAbility));
		CardManager.EnemyDeck.Add(new BattlerCard(key.GetId(), "Гамлет", "gamlet", 1, 3, 1,
			Card.Ability_Type.Provocation));
		CardManager.EnemyDeck.Add(new BattlerCard(key.GetId(), "Гамлет", "gamlet", 1, 3, 1,
			Card.Ability_Type.Provocation));
		CardManager.EnemyDeck.Add(new BattlerCard(key.GetId(), "Гамлет", "gamlet", 1, 3, 1,
			Card.Ability_Type.Provocation));
		CardManager.EnemyDeck.Add(
			new BattlerCard(key.GetId(), "Шерлок", "Sherlock", 6, 7, 6, Card.Ability_Type.Regeneration));
		CardManager.EnemyDeck.Add(
		new BattlerCard(key.GetId(), "Шерлок", "Sherlock", 6, 7, 6, Card.Ability_Type.Regeneration));
		CardManager.EnemyDeck.Add(new BattlerCard(key.GetId(), "Том", "Tom", 4, 5, 4,
			Card.Ability_Type.Provocation));
		CardManager.EnemyDeck.Add(new BattlerCard(key.GetId(), "Том", "Tom", 4, 5, 4,
			Card.Ability_Type.Provocation));
		CardManager.EnemyDeck.Add(new BattlerCard(key.GetId(), "Жуль Верн", "Jule Vern", 2, 3, 2,
			Card.Ability_Type.NoAbility));
		CardManager.EnemyDeck.Add(new BattlerCard(key.GetId(), "Жуль Верн", "Jule Vern", 2, 3, 2,
			Card.Ability_Type.NoAbility));
		CardManager.EnemyDeck.Add(
			new BattlerCard(key.GetId(), "Дон Кі Хот", "DonQiHot", 5, 1, 3, Card.Ability_Type.WindFury));
		CardManager.EnemyDeck.Add(
			new BattlerCard(key.GetId(), "Дон Кі Хот", "DonQiHot", 5, 1, 3, Card.Ability_Type.WindFury));
		CardManager.EnemyDeck.Add(new BattlerCard(key.GetId(), "Елізабет", "Elizabet", 5, 6, 5,
			Card.Ability_Type.NoAbility));
		CardManager.EnemyDeck.Add(new BattlerCard(key.GetId(), "Елізабет", "Elizabet", 5, 6, 5,
			Card.Ability_Type.NoAbility));
	}
	}



