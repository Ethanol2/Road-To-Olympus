using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class MapGenerator : MonoBehaviour
{
    public bool GenerateOnStart = true;
    public static MapGenerator Instance;

    [Space]
    [SerializeField] private TerrainInfo[] terrainCards;
    [SerializeField] private CameraController cam;
    [SerializeField] private int segmentsToGenerate = 5;
    [SerializeField] private bool randomFlip = true;

    [Space]
    [SerializeField] private SpriteGenerator[] cloudGenerators;
    [SerializeField] private SpriteGenerator[] treeGenerators;
    [SerializeField] private SpriteGenerator[] rockGenerators;
    [SerializeField] private SpriteGenerator[] shrubGenerators;

    [Space]
    [SerializeField] private Transform olympus;
    [SerializeField] private float startScale = 3f;

    private List<int> terrainCardOrder = new List<int>();
    private int currentTerrainIndex = 0;

    private TerrainInfo Current
    {
        get
        {
            return terrainCards[terrainCardOrder[currentTerrainIndex]];
        }
    }

    public UnityEvent OnGenerated;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (GenerateOnStart) Generate();
    }
    public void Generate()
    {
        StartCoroutine(GenerateRoutine());
    }
    public void GenerateEnvironment()
    {
        Vector2Int rockSettings = Current.RockCount;
        foreach (SpriteGenerator generator in rockGenerators)
        {
            generator.SetCam(cam.transform);
            generator.Generate(rockSettings.x, rockSettings.y);
        }

        Vector2Int treeSettings = Current.TreesCount;
        foreach (SpriteGenerator generator in treeGenerators)
        {
            generator.SetCam(cam.transform);
            generator.Generate(treeSettings.x, treeSettings.y);
        }

        Vector2Int shrubSettings = Current.ShrubCount;
        foreach (SpriteGenerator generator in shrubGenerators)
        {
            generator.SetCam(cam.transform);
            generator.Generate(shrubSettings.x, shrubSettings.y);
        }

        foreach (SpriteGenerator generator in cloudGenerators)
        {
            generator.SetCam(cam.transform);
            generator.Generate();
        }
    }
    private IEnumerator GenerateRoutine()
    {
        terrainCardOrder.Clear();
        int prior = -1;
        while (terrainCardOrder.Count < segmentsToGenerate)
        {
            int card;
            do
            {
                card = Random.Range(0, terrainCards.Length);
            }
            while (card == prior);

            prior = card;
            terrainCardOrder.Add(prior);
        }

        int index = 0;
        Vector3 tPos = new Vector3(0f, -0.01f, 190f);
        foreach (int card in terrainCardOrder)
        {
            Transform newTerrain = GameObject.Instantiate(terrainCards[card].gameObject, this.transform).transform;
            newTerrain.name = "Terrain " + index;

            bool flipped = randomFlip && Random.Range(0f, 1f) >= 0.5f;

            Vector3 euler = new Vector3(0f, flipped ? 180f : 0f, 0f);

            newTerrain.eulerAngles = euler;
            newTerrain.position = (tPos * index) + (flipped ? tPos : Vector3.zero);

            newTerrain.GetComponent<TerrainInfo>().PopulateMap(flipped);

            index++;
        }
        currentTerrainIndex = 0;

        Transform backTerain = GameObject.Instantiate(terrainCards[Random.Range(0, terrainCards.Length)].gameObject, this.transform).transform;
        backTerain.name = "Back Terrain";
        backTerain.position = tPos * -1f;

        yield return new WaitForSeconds(Time.deltaTime);

        GenerateEnvironment();

        olympus.localScale = Vector3.one * startScale;
        olympus.position = tPos * segmentsToGenerate;
    }
}
