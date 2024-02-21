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

    public enum EquipableType
    {
        Weapon, Armor, Boots, Helmet, Shield
    }
}
