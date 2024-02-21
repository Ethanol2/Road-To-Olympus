using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressTracker : MonoBehaviour
{
    public static ProgressTracker Instance;
    private static float staticUITime = 0.5f;

    public PlayerStats PlayerStats;
    public PlayerInventory Inventory;
    [SerializeField] private PlayerStats startStats;

    [Space]
    [SerializeField] private float uiTime = 0.5f;

    [Space]
    [SerializeField] private List<MapPoint> mapPoints = new List<MapPoint>();
    [SerializeField] private CameraController cam;
    [SerializeField] private DayNightCycle dayNightCycle;

    [Space]
    [SerializeField] private int totalKM = 0;
    [SerializeField] private int kilometersRemaining = 0;
    [SerializeField] private int currentMapPoint = 0;
    
    [Header("Existing")]
    [SerializeField] private float tirednessPerTick = 0.001f;
    [SerializeField] private float hungerPerTick = 0.01f;
    [SerializeField] private float timeToFullyRest = 0.25f;
    [SerializeField] private Sprite restImage;

    [Header("Foraging")]
    [SerializeField] private int maxItemsPerForage = 4;
    [SerializeField] private float tirednessPerForage = 0.1f;
    [SerializeField] private float hungerPerTravel = 0.2f;
    [SerializeField] private float timePerForage = 0.14f;

    [Header("Traveling")]
    [SerializeField] private TravelUI travelUI;
    [SerializeField] private float tirednessPerTravel = 0.1f;
    [SerializeField] private float hungerPerForage = 0.15f;

    [Space]
    [Header("UI")]
    [SerializeField] private TMP_Text sceneDescription;
    [Header("Buttons")]
    [SerializeField] private Button continueTravelButton;
    private TMP_Text travelButtonText;
    [SerializeField] private Button forageButton;
    private TMP_Text forageButtonText;
    [SerializeField] private Button restButton;
    private TMP_Text restButtonText;

    public List<MapPoint> MapPoints { get {  return mapPoints; } }
    public MapPoint CurrentPoint { get => mapPoints[currentMapPoint]; }
    public static float UITime
    {
        get
        {
            if (Instance != null) { return Instance.uiTime; }
            return staticUITime;
        }
    }
    public int KilometersRemaining => kilometersRemaining;

    private void Awake()
    {
        Instance = this;
        travelButtonText = continueTravelButton.GetText();
        forageButtonText = forageButton.GetText();
        restButtonText = restButton.GetText();
    }
    // Start is called before the first frame update
    void Start()
    {
        MapGenerator.Instance.OnGenerated.AddListener(OnGenerationFinished);
        PlayerStats.ApplyStats(startStats);
        PlayerStats.OnStatsChanged.AddListener(OnStatsChange);
        dayNightCycle.OnTick.AddListener(OnTick);
        
        continueTravelButton.onClick.AddListener(Travel);
        forageButton.onClick.AddListener(Forage);
        restButton.onClick.AddListener(Rest);
    }
    private void OnTick()
    {
        PlayerStats.Hunger -= hungerPerTick;
        PlayerStats.Rest -= tirednessPerTick;
    }
    private void OnStatsChange()
    {
        travelButtonText.text = "Continue Travel";
        forageButtonText.text = "Forage";
        restButtonText.text = "Rest";
    }
    private void OnGenerationFinished()
    {
        cam.SetCamPosition(mapPoints[0]);
    }
    public void AddMapPoints(MapPoint[] points)
    {
        mapPoints.AddRange(points);
        totalKM = mapPoints.Count * 10;
        kilometersRemaining = totalKM;
    }
    private void Travel()
    {
        if (PlayerStats.Hunger < hungerPerTravel)
        {
            travelButtonText.text = "Too Hungry to Travel";
            return;
        }
        if (PlayerStats.Rest < tirednessPerTravel)
        {
            travelButtonText.text = "Too Tired to Travel";
            return;
        }

        travelUI.Travel(10, MovePlayer);
    }
    public void MovePlayer(int finalDistance) => MovePlayer(finalDistance, true);
    public void MovePlayer(int finalDistance, bool forward)
    {
        int mod = forward ? 1 : -1;
        kilometersRemaining = kilometersRemaining + (10 * -mod);
        currentMapPoint = currentMapPoint + mod;

        bool isLastPoint = false;
        if (currentMapPoint < 0) { currentMapPoint = 0; }
        else if (currentMapPoint >= mapPoints.Count - 1)
        {
            continueTravelButton.interactable = false;
            currentMapPoint = mapPoints.Count - 1;
            isLastPoint = true;
        }
        if (kilometersRemaining > totalKM) { kilometersRemaining = totalKM; }

        PlayerStats.Hunger -= hungerPerTravel;
        PlayerStats.Rest -= tirednessPerTravel;

        cam.SetCamPosition(mapPoints[currentMapPoint]);
        if (!isLastPoint)
        {
            cam.LookAt(mapPoints[currentMapPoint + 1]);
        }

        sceneDescription.text = CurrentPoint.GetDescription();

        MapGenerator.Instance.GenerateEnvironment();
    }
    public void Forage()
    {
        if (PlayerStats.Hunger < hungerPerForage) 
        {
            forageButtonText.text = "Too Hungry to Forage";
            return; 
        }
        PlayerStats.Hunger -= hungerPerForage;
        if (PlayerStats.Rest < tirednessPerForage)
        {
            forageButtonText.text = "Too Tired to Forage";
            return;
        }
        PlayerStats.Rest -= tirednessPerForage;

        dayNightCycle.AddTime(Mathf.RoundToInt(DayNightCycle.TOTAL_MINUTES * timePerForage));

        int numItemsFound = 0;
        float chance = CurrentPoint.ForagingChance * CurrentPoint.ForagingModifier;
        for (int k = 0; k < maxItemsPerForage; k++)
        {
            Debug.Log("Foraging chance: " + chance);

            float result = Random.Range(0f, 1f);
            if (result <= chance)
            {
                numItemsFound++;
                chance *= 0.85f;
            }
            else
            {
                break;
            }
        }

        CurrentPoint.ForagingModifier *= 0.75f;
        
        if (numItemsFound == 0) 
        {
            ModalController.OpenModal("Bad Luck", "You didn't find anything...");
            return; 
        }

        Debug.Log($"Items Found: {numItemsFound}");
        Dictionary<System.Type, float> mods = new Dictionary<System.Type, float>()
        {
            {typeof(CombatItem), CurrentPoint.TerrainInfo.EquipmentChance },            
            {typeof(FoodItem), CurrentPoint.TerrainInfo.HuntingChance },
            {typeof(Item), CurrentPoint.TerrainInfo.ForagingChance },
        };

        for (int k = 0; k < numItemsFound; k++)
        {
            InventoryManager.Instance.Add(PlayerInventory.GetRandom(Inventory.ItemReferences, mods), 0);
        }

    }
    public void Rest()
    {
        int minutesToFullyRest = Mathf.RoundToInt(DayNightCycle.TOTAL_MINUTES * (timeToFullyRest * (1f - PlayerStats.Rest)));
        float hoursRest = PlayerStats.Rest + 0.17f;

        ModalController.OpenModal(
            "You setup camp",
            "How long do you rest for?",
            restImage,
            $"Rest for {((float)minutesToFullyRest / 60f).ToString("0.##")} Hours",
            new System.Action(() => RestForTime(minutesToFullyRest, 1f)),
            "Rest for an Hour",
            new System.Action(() => RestForTime(60, hoursRest))
            );
    }
    private void RestForTime(int time, float restValue)
    {
        dayNightCycle.AddTime(time);
        PlayerStats.Rest = restValue;
    }
}
