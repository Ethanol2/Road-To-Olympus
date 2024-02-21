using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TerrainInfo : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private MapPoint[] mapPoints = new MapPoint[0];

    [Header("Generation Settings")]
    [SerializeField] private Vector2Int treesCount = new Vector2Int(60, 100);
    [SerializeField] private Vector2Int shrubCount = new Vector2Int(150, 200);
    [SerializeField] private Vector2Int rockCount = new Vector2Int(5, 50);

    [Header("Encounter Settings")]
    [SerializeField, Range(0f, 2f)] private float generalForagingChance = 1f;
    [SerializeField, Range(0f, 2f)] private float eadibleChance = 1f;
    [SerializeField, Range(0f, 2f)] private float junkChance = 1f;
    [SerializeField, Range(0f, 2f)] private float equipmentChance = 1f;

    [Space]
    [SerializeField, Range(0f, 1f)] private float villageChance = 0.2f;

    // Properties
    public Vector2Int TreesCount => treesCount;
    public Vector2Int ShrubCount => shrubCount;
    public Vector2Int RockCount => rockCount;

    public float ForagingChance => junkChance;
    public float HuntingChance => eadibleChance;
    public float EquipmentChance => equipmentChance;
    public float GeneralForagingChance => generalForagingChance;
    public float VillageChance => villageChance;

    public void PopulateMap(bool flipped)
    {
        MapPoint[] points = mapPoints;
        if (flipped)
        {
            MapPoint[] pointsBackwards = new MapPoint[points.Length - 1];
            int index = 0;
            for (int k = points.Length - 1; k >= 1; k--)
            {
                pointsBackwards[index++] = points[k];
            }
            points = pointsBackwards;
        }
        else
        {
            List<MapPoint> temp = new List<MapPoint>(points);
            temp.RemoveAt(temp.Count - 1);
            points = temp.ToArray();
        }
        ProgressTracker.Instance.AddMapPoints(points);
        foreach(MapPoint point in  points)
        {
            point.TerrainInfo = this;
            point.ForagingChance = generalForagingChance;
        }
    }
}
