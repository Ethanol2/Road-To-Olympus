using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPoint : MonoBehaviour
{
    public bool HasVillage = false;
    public TerrainInfo TerrainInfo;
    public float ForagingChance = 0.5f;
    public float ForagingModifier = 1f;

    private void Start()
    {
        this.gameObject.SetActive(false);
    }
}
