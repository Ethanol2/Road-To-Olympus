using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    public Sprite Sprite => sprite;

    [Space]
    [SerializeField] private float spawnChance = 0.5f;
    public float SpawnChance => spawnChance;
    [SerializeField] private Type itemType = Type.Junk;
    public Type ItemType => itemType;
    [SerializeField] private float hungerGain = 0.2f;
    public float HungerGain => hungerGain;

    [Space]
    [SerializeField] private int attackBonus = 1;
    public int AttackBonus => attackBonus;
    [SerializeField] private int defenseBonus = 1;
    public int DefenseBonus => defenseBonus;
    [SerializeField] private int speedBonus = 1;
    public int SpeedBonus => speedBonus;
    [SerializeField] private int knowledgeBonus = 1;
    public int KnowledgeBonus => knowledgeBonus;

    [Space]
    [SerializeField] private int moneyValue = 1;
    public int MoneyValue => moneyValue;

    public enum Type
    {
        Eadible, Weapon, Armor, Boots, Helmet, Shield, Junk, Book
    }    
}
