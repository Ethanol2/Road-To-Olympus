using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Attack")]
public class Attack : ScriptableObject
{
    [SerializeField] private string displayName = "Sword Swing";
    public string DisplayName => displayName;

    [SerializeField] private string description = "A large swipe if the sword";
    public string Description => description;
    [SerializeField] private string actionText = "swing the sword";
    public string ActionText => actionText;

    [Space]
    [SerializeField] private Dice damageDice = Dice.D6;
    public Dice DamageDice => damageDice;
    [SerializeField] private int diceCount = 1;
    public int DamageDiceCount => diceCount;

    [Space]
    [SerializeField] private float minRange = 0f;
    [SerializeField] private float maxRange = 5f;

    public enum Dice
    {
        D4 = 4, D6 = 6, D8 = 8, D10 = 10, D12 = 12, D20 = 20
    }
}
