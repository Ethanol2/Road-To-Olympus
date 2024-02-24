using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Item/Item")]
public class Item : ScriptableObject, IRandomizable
{
    [SerializeField] private string displayName;
    public string DisplayName => displayName;
    [SerializeField] private Sprite sprite;
    public Sprite Sprite => sprite;

    [Space]
    [SerializeField, Range(0f, 2f)] private float spawnChance = 0.5f;
    public float SpawnChance => spawnChance;
    [SerializeField] private int knowledgeBonus = 1;
    public int KnowledgeBonus => knowledgeBonus;

    [Space]
    [SerializeField] private int moneyValue = 1;
    public int MoneyValue => moneyValue;

    public float Chance => spawnChance;

    public virtual string RandomizableType => "Item";

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
