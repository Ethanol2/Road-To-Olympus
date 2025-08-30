using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EncounterManager : MonoBehaviour
{
    public static EncounterManager Instance { get; private set; }

    [SerializeField] private Encounter[] genericEncounters;
    [SerializeField] private Encounter[] significantEncounters;

    [Space]
    [SerializeField] private Encounter current;
    [SerializeField] private Character[] characters;

    [Header("Settings")]
    [SerializeField, Range(0f, 1f)] private float travelEnounterChance = 0.5f;
    [SerializeField, Range(0f, 1f)] private float foragingEncounterChance = 0.2f;

    [Header("UI")]
    [SerializeField] private Transform encounterUI;
    [SerializeField] private TMP_Text title;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text description;

    [Space]
    [SerializeField] private Button runButton;
    private TMP_Text runButtonText;
    [SerializeField] private Button talkButton;
    private TMP_Text talkButtonText;
    [SerializeField] private Button fightButton;
    private TMP_Text fightButtonText;

    public Encounter CurrentEncounter => current;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    private void Start()
    {
        encounterUI.gameObject.SetActive(false);

        runButtonText = runButton.GetText();
        talkButtonText = talkButton.GetText();
        fightButtonText = fightButton.GetText();

        runButton.onClick.AddListener(RunButton);
        fightButton.onClick.AddListener(FightButton);
        talkButton.onClick.AddListener(TalkButton);
    }
    private void FightButton()
    {

    }
    private void TalkButton()
    {

    }
    private void RunButton()
    {
        encounterUI.gameObject.SetActive(false);
    }

    public bool CheckForEncounter(Milestone milestone = null, bool foraging = false)
    {
        if (milestone == null) { milestone = ProgressTracker.Instance.CurrentPoint; }

        float chance = milestone.EncounterChance * (foraging ? foragingEncounterChance : travelEnounterChance) * 0.05f;
        float roll = Random.Range(0f, 1f);

        if (roll > chance)
        {
            return false;
        }

        DisplayEncounter(Randomizer.GetRandom(genericEncounters) as Encounter);
        return true;
    }
    public void DisplayEncounter(Encounter encounter)
    {
        encounterUI.gameObject.SetActive(true);
        title.text = encounter.DisplayName;
        image.gameObject.SetActive(encounter.Sprite);
        image.sprite = encounter.Sprite;
        description.text = encounter.Description;
    }

}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(EncounterManager))]
public class EncounterManagerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        GUILayout.Space(10f);
        if (GUILayout.Button("Add Encounters"))
        {
            UnityEditor.SerializedProperty gE = serializedObject.FindProperty("genericEncounters");
            UnityEditor.SerializedProperty sE = serializedObject.FindProperty("significantEncounters");

            gE.arraySize = 0;
            sE.arraySize = 0;

            string[] encounters = UnityEditor.AssetDatabase.FindAssets("t:Encounter");
            foreach (string eGUID in  encounters)
            {
                Encounter encounter = UnityEditor.AssetDatabase.LoadAssetAtPath<Encounter>(UnityEditor.AssetDatabase.GUIDToAssetPath(eGUID));
                if (encounter != null)
                {
                    if (encounter.Type == Encounter.EncounterType.Generic)
                    {
                        gE.arraySize++;
                        gE.GetArrayElementAtIndex(gE.arraySize - 1).objectReferenceValue = encounter;
                    }
                    else
                    {
                        sE.arraySize++;
                        sE.GetArrayElementAtIndex(sE.arraySize - 1).objectReferenceValue = encounter;
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
