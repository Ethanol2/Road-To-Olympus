using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject
{
    [SerializeField] private string displayName;
    public string DisplayName => displayName;
    [SerializeField] private Sprite sprite;
    public Sprite Sprite => sprite;

    [Space]
    [SerializeField, Range(0f, 2f)] private float spawnChance = 0.5f;
    public float SpawnChance => spawnChance;
    [SerializeField] private Type itemType = Type.Junk;
    public Type ItemType => itemType;
    [SerializeField] private float hungerGain = 0.2f;
    public float HungerGain => hungerGain;

    [Space]
    [SerializeField] private int attackBonus = 1;
    public int AttackBonus => attackBonus;
    [SerializeField] private int defenseBonus = 1;
    public int DefenseBonus => defenseBonus;
    [SerializeField] private int speedBonus = 1;
    public int SpeedBonus => speedBonus;
    [SerializeField] private int knowledgeBonus = 1;
    public int KnowledgeBonus => knowledgeBonus;

    [Space]
    [SerializeField] private int moneyValue = 1;
    public int MoneyValue => moneyValue;

    public enum Type
    {
        Eadible, Weapon, Armor, Boots, Helmet, Shield, Junk, Book
    }

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(displayName)) displayName = this.name;
    }
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(Item))]
public class ItemEditor : UnityEditor.Editor
{
    private static Vector2 chanceParameters = new Vector2(0f, 1f);

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        GUILayout.Space(15f);

        //GUILayout.BeginHorizontal();

        chanceParameters.x = UnityEditor.EditorGUILayout.Slider("From", chanceParameters.x, 0f, chanceParameters.y);
        chanceParameters.y = UnityEditor.EditorGUILayout.Slider("To", chanceParameters.y, chanceParameters.x, 2f);

        if (GUILayout.Button("Generate Spawn Chance"))
        {
            float chance = Random.Range(chanceParameters.x, chanceParameters.y);
            serializedObject.FindProperty("spawnChance").floatValue = chance;
        }

        //GUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
