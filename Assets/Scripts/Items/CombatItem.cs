using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat Item")]
public class CombatItem : Item
{
    [SerializeField] private EquipableType equipType;
    public EquipableType EquipType => equipType;

    [SerializeField] private int attackBoost = 0;
    public int AttackBoost => attackBoost;
    [SerializeField] private int defenseBoost = 0;
    public int DefenseBoost => defenseBoost;
    [SerializeField] private int speedBoost = 0;
    public int SpeedBoost => speedBoost;

    [Space]
    [SerializeField] private int hitChanceBoost = 0;
    public int HitChanceBoost => hitChanceBoost;
    [SerializeField] private int additionalHitDice = 0;
    public int AdditionalHitDixe => additionalHitDice;
    [SerializeField] private float hitRangeMultiplier = 1f;
    public float HitRangeMultiplier => hitRangeMultiplier;

    [Space]
    [SerializeField] private List<Attack> attacks = new List<Attack>();
    public List<Attack> Attacks => attacks;

    public enum EquipableType
    {
        Weapon, Armor, Boots, Helmet, Shield
    }
}
