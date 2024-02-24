using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Character/Character")]
public class Character : ScriptableObject
{
    [SerializeField] private string displayName = "Unknown Stranger";
    public string DisplayName => displayName;

    [SerializeField, TextArea] private string description = "A person of average height and build. They have clothing worn by most people in the region";
    public string Description => description;

    [SerializeField] private string possessivePronoun = "Their";
    public string PossessivePronoun => possessivePronoun;

    [SerializeField] private string personalPronoun = "They";
    public string PersonalPronoun => personalPronoun;
    [SerializeField] private TerrainInfo[] favouriteAreas = new TerrainInfo[0];
    [SerializeField, Range(0f, 2f)] private float spawnChance = 1f;
    [SerializeField] private bool uniqueEncounter = false;

    [Header("-1 is hostile, 0 is neutral, 1 is friendly")]
    [SerializeField] private float startingRelashionship = 0f;
    public float StartingRelashionship => startingRelashionship;

    [Space]
    [SerializeField] private Sprite image;
    public Sprite Image => image;

    [Header("Build")]
    [SerializeField] private CharacterStats stats;
    public CharacterStats Stats => stats;
    [SerializeField] private CombatItem helmet;
    public CombatItem Helmet => helmet;
    [SerializeField] private CombatItem armour;
    public CombatItem Armour => armour;
    [SerializeField] private CombatItem shield;
    public CombatItem Shield => shield;
    [SerializeField] private CombatItem boots;
    public CombatItem Boots => boots;
    [SerializeField] private CombatItem weapon;
    public CombatItem Weapon => weapon;
    [SerializeField] private CombatItem rangedWeapon;
    public CombatItem RangedWeapon => rangedWeapon;
    [SerializeField] private Attack[] meleeAttacks = new Attack[0];
    public Attack[] MeleeAttacks => meleeAttacks;

    [Space]
    [SerializeField] private List<Item> inventory = new List<Item>();
    public List<Item> Inventory => inventory;

    public int Attack
    {
        get
        {
            return (stats ? stats.Attack : 0) + (weapon ? weapon.AttackBoost : 0); 
        }
    }
    public int RangedAttack
    {
        get
        {
            return (stats ? stats.Attack : 0) + (rangedWeapon ? rangedWeapon.AttackBoost : 0);
        }
    }
    public int TotalDefense
    {
        get
        {
            return
                (stats ? stats.Defense : 0) +
                (armour ? armour.DefenseBoost : 0) +
                (helmet ? helmet.DefenseBoost : 0) +
                (boots ? boots.DefenseBoost : 0) +
                (shield ? shield.DefenseBoost : 0);
        }
    }

    public enum Relashionship
    {
        Friendly, Familiar, Neutral, Cool, Hostile, Unkown
    }

    private void OnValidate()
    {
        if (rangedWeapon && rangedWeapon.EquipType != CombatItem.EquipableType.RangeWeapon) { rangedWeapon = null; }
        if (weapon && weapon.EquipType != CombatItem.EquipableType.Weapon) { weapon = null; }
        if (armour && armour.EquipType != CombatItem.EquipableType.Armour) { armour = null; }
        if (helmet && helmet.EquipType != CombatItem.EquipableType.Helmet) { helmet = null; }
        if (shield && shield.EquipType != CombatItem.EquipableType.Shield) { shield = null; }
        if (boots && boots.EquipType != CombatItem.EquipableType.Boots) { boots = null; }
    }
    
    public float GetSpawnChance(TerrainInfo currentTerrain)
    {
        float mod = 1f;
        if (favouriteAreas.Contains<TerrainInfo>(currentTerrain))
        {
            mod = 1.5f;
        }

        return spawnChance * mod;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Character))]
public class CharacterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        GUILayout.Space(10f);
        if (GUILayout.Button("Create Stats Object"))
        {
            CharacterStats stats = ScriptableObject.CreateInstance<CharacterStats>();
            stats.name = target.name + "'s Stats";

            AssetDatabase.AddObjectToAsset(stats, AssetDatabase.GetAssetPath(target));
            serializedObject.FindProperty("stats").objectReferenceValue = stats;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        if (GUILayout.Button("Delete Stats Object"))
        {
            SerializedProperty statsProp = serializedObject.FindProperty("stats");
            Undo.DestroyObjectImmediate(statsProp.objectReferenceValue);
            statsProp.objectReferenceValue = null;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif