using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGenerator : MonoBehaviour
{
    public bool GenerateOnStart = true;

    [Space]
    [SerializeField] private Sprite[] sprites = new Sprite[0];
    [SerializeField] private int poolSize = 50;
    [SerializeField] private int minSpriteCount = 0;
    [SerializeField] private int maxSpriteCount = 50;

    [Space]
    [SerializeField] private float minScale = 1f;
    [SerializeField] private float maxScale = 1.2f;

    [Space]
    [SerializeField] private bool matchTerrainElevation = true;
    [SerializeField] private bool upIsTerrainNormal = false;

    [Space]
    [SerializeField] private bool colourByDistance = true;
    [SerializeField] private float minDist = 0f;
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private Color closeColour = Color.white;
    [SerializeField] private Color farColour = Color.blue;

    [Space]
    [SerializeField] private bool objectsMove = false;
    [SerializeField] private bool objectsLoop = true;
    [SerializeField] private float minSpeed = 2f;
    [SerializeField] private float maxSpeed = 10f;

    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    private List<(Transform, float)> movingObjects = new List<(Transform, float)>();
    private BoxCollider spawnArea;
    private Transform cam;

    // Start is called before the first frame update
    void Awake()
    {
        spawnArea = GetComponent<BoxCollider>();

        for (int k = 0; k < poolSize; k++)
        {
            SpriteRenderer newRenderer = new GameObject().AddComponent<SpriteRenderer>();
            newRenderer.transform.SetParent(this.transform);
            newRenderer.name = $"{this.name} {k}";
            newRenderer.gameObject.SetActive(false);
            spriteRenderers.Add(newRenderer);
        }

        if (GenerateOnStart) { Generate(); }
    }
    private void Update()
    {
        if (movingObjects.Count == 0) return;

        for (int k = movingObjects.Count - 1; k >= 0; k--)
        {
            Transform obj = movingObjects[k].Item1;
            float speed = movingObjects[k].Item2;

            Vector3 position = obj.position;
            position.x += speed * Time.deltaTime;

            if (!spawnArea.bounds.Contains(position))
            {
                if (objectsLoop)
                {
                    position.x += spawnArea.bounds.extents.x * 2f * (speed < 0f ? 1f : -1f);
                    position = spawnArea.bounds.ClosestPoint(position);
                }
                else
                {
                    obj.gameObject.SetActive(false);
                    movingObjects.RemoveAt(k);
                }
            }

            spriteRenderers[k].transform.LookAt(cam.transform.position);
            obj.position = position;
        }    
    }

    public void SetCam(Transform cam) => this.cam = cam;
    public void Generate(int min, int max)
    {
        Generate(Random.Range(min, max));
    }
    [ContextMenu("Generate")]
    public void Generate()
    {
        Generate(Random.Range(minSpriteCount, maxSpriteCount));
    }
    public void Generate(int count)
    {
        count = Mathf.Clamp(count, 0, maxSpriteCount);

        Vector3 raycastHeight = new Vector3(0f, 20f, 0f);
        movingObjects.Clear();

        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.gameObject.SetActive(false);
        }
        for (int k = 0; k < count; k++)
        {
            spriteRenderers[k].sprite = sprites[Random.Range(0, sprites.Length)];
            spriteRenderers[k].gameObject.SetActive(true);

            Vector3 position = GetRandomPointInsideCollider(spawnArea);

            if (matchTerrainElevation)
            {
                RaycastHit hit;
                if (Physics.Raycast(position + raycastHeight, Vector3.down, out hit, 40f))
                {
                    position = hit.point;
                    if (upIsTerrainNormal)
                    {
                        spriteRenderers[k].transform.up = hit.normal;
                    }
                }
                else
                {
                    spriteRenderers[k].gameObject.SetActive(false);
                    continue;
                }
            }
            spriteRenderers[k].transform.position = position;

            spriteRenderers[k].transform.localScale = Vector3.one * Random.Range(minScale, maxScale);
            spriteRenderers[k].transform.LookAt(cam.transform.position);

            if (colourByDistance) spriteRenderers[k].color = Color.Lerp(closeColour, farColour, (Vector3.Distance(position, cam.transform.position) - minDist) / maxDistance);

            if (objectsMove) movingObjects.Add((spriteRenderers[k].transform, Random.Range(minSpeed, maxSpeed)));
        }
    }
    public Vector3 GetRandomPointInsideCollider(BoxCollider boxCollider)
    {
        Vector3 extents = boxCollider.size / 2f;
        Vector3 point = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        );

        return boxCollider.transform.TransformPoint(point);
    }
    public void SetSpriteColours(Color colour)
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.color = colour;
        }
    }
}
