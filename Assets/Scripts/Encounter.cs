using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(menuName = "Character/Encounter")]
public class Encounter : ScriptableObject, IRandomizable
{
    [SerializeField] private string displayName = "Something happens";
    public string DisplayName => displayName;

    [SerializeField, TextArea] private string description = "You see something in the distance";
    public string Description => description;

    [SerializeField] private EncounterType encounterType;
    public EncounterType Type => encounterType;

    [SerializeField] private float encounterChance = 0.5f;
    public float EncounterChance => encounterChance;

    [Space]
    [SerializeField] private Character leader;
    public Character Leader => leader;

    [Space]
    [SerializeField] private Character[] minionTypes;
    public Character[] MinionTypes => minionTypes;

    [SerializeField] private bool evenDistribution = true;
    public bool EvenDistribution => evenDistribution;
    [SerializeField] private Vector2Int minionCount = new Vector2Int(2, 5);
    public Vector2Int MinionCount => minionCount;

    [Space]
    [SerializeField, Range(0f, 1f)] private float importantLeaderChance = 0f;
    public float ImportantLeaderChance => importantLeaderChance;

    [Space]
    [SerializeField] private bool numbersHidden = false;
    public bool NumbersHidden => numbersHidden;

    // IRandomizable
    public float Chance => encounterChance;

    public string RandomizableType => encounterType.ToString();

    public enum EncounterType
    {
        Generic, Significant
    }
}
