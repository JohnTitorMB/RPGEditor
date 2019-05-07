using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    player,
    NPC,
    ennemie,
}

[System.Serializable]
public class CharacterAdvancement
{
    Character character;

    public CharacterType characterType;
    private int Level = 1;
    public int XP = 0;
    public int MaxLevel = 100;
    public int HP = 10;
    public int MaxHP = 10;
    public int MP = 10;
    public int MaxMP = 10;
    public int Attack = 10;
    public int MagicAttack = 10;
    public int Agility = 10;
    public int Defence = 10;
    public int MagicDefence = 10;
    public int luck = 10;
}


[System.Serializable]
public class Character : EntityData
{   
    public List<DialogueStatic> dialogues = new List<DialogueStatic>();

    public Character()
    {
    }

    public Character(string characterName)
    {
        name = characterName;
    }

    public Texture2D characterAvatar = null;
    public CharacterType characterType;
    public int MaxLevel = 1;
    public int Level = 1;
    public int XP = 0;
    public int HP = 0;
    public int MP = 0;

    public AnimationCurve XpPerLevelEnnemy = AnimationCurve.Linear(0, 0, 10, 10);
    public AnimationCurve XpPerLevel = AnimationCurve.Linear(0, 0, 100, 10);
    public AnimationCurve MaxHPPerLevel = AnimationCurve.Linear(0, 0, 10, 10);
    public AnimationCurve MaxMPPerLevel = AnimationCurve.Linear(0, 0, 10, 10);
    public AnimationCurve AttackPerLevel = AnimationCurve.Linear(0, 0, 10, 10);
    public AnimationCurve MagicAttackPerLevel = AnimationCurve.Linear(0, 0, 10, 10);
    public AnimationCurve AgilityPerLevel = AnimationCurve.Linear(0, 0, 10, 10);
    public AnimationCurve DefencePerLevel = AnimationCurve.Linear(0, 0, 10, 10);
    public AnimationCurve MagicDefenceLevel = AnimationCurve.Linear(0, 0, 10, 10);
    public AnimationCurve luckPerLevel = AnimationCurve.Linear(0, 0, 10, 10);
}
