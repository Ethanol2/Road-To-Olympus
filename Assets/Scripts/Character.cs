using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character")]
public class Character : ScriptableObject
{
    [SerializeField] private string displayName = "Unknown Stranger";
    public string DisplayName => displayName;

    [SerializeField, TextArea] private string description = "A person of average height and build. They have clothing worn by most people in the region";
    public string Description => description;

    [SerializeField] private string possessivePronoun = "Their";
    public string PossessivePronoun => possessivePronoun;

    [SerializeField] private string personalPronoun = "They";
    public string PersonalPronoun => personalPronoun;


    [Header("-1 is hostile, 0 is neutral, 1 is friendly")]
    [SerializeField] private float startingRelashionship = 0f;
    public float StartingRelashionship => startingRelashionship;

    [Space]
    [SerializeField] private Sprite image;
    public Sprite Image => image;

    [Header("Build")]
    [SerializeField] private CharacterStats stats;
    public CharacterStats Stats => stats;
    [SerializeField] private CombatItem helmet;
    public CombatItem Helmet => helmet;
    [SerializeField] private CombatItem armour;
    public CombatItem Armour => armour;
    [SerializeField] private CombatItem shield;
    public CombatItem Shield => shield;
    [SerializeField] private CombatItem boots;
    public CombatItem Boots => boots;
    [SerializeField] private CombatItem weapon;
    public CombatItem Weapon => weapon;
    [SerializeField] private CombatItem rangedWeapon;
    public CombatItem RangedWeapon => rangedWeapon;

    [Space]
    [SerializeField] private List<Item> inventory = new List<Item>();
    public List<Item> Inventory => inventory;

    public int Attack
    {
        get
        {
            return (stats ? stats.Attack : 0) + (weapon ? weapon.AttackBoost : 0); 
        }
    }
    public int RangedAttack
    {
        get
        {
            return (stats ? stats.Attack : 0) + (rangedWeapon ? rangedWeapon.AttackBoost : 0);
        }
    }
    public int TotalDefense
    {
        get
        {
            return
                (stats ? stats.Defense : 0) +
                (armour ? armour.DefenseBoost : 0) +
                (helmet ? helmet.DefenseBoost : 0) +
                (boots ? boots.DefenseBoost : 0) +
                (shield ? shield.DefenseBoost : 0);
        }
    }

    public enum Relashionship
    {
        Friendly, Familiar, Neutral, Cool, Hostile, Unkown
    }
}
