using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milestone : MonoBehaviour
{
    public bool HasVillage = false;
    public TerrainInfo TerrainInfo;

    [Space]
    [SerializeField] private float foragingModifier = 1f;

    private void Start()
    {
        this.gameObject.SetActive(false);
    }
    public string GetDescription()
    {
        if (HasVillage)
        {
            return "There's a village";
        }
        else
        {
            return "Nothing much here...";
        }
    }
    public float ForagingChance => TerrainInfo ? TerrainInfo.ForagingChance * foragingModifier : 1f;
    public float EncounterChance => TerrainInfo ? TerrainInfo.EncounterChance : 1f;
    public float ForagingModifier { get { return foragingModifier; } set {  foragingModifier = value; } }
}
