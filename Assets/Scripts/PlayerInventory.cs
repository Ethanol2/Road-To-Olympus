using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Inventory")]
public class PlayerInventory : ScriptableObject
{
    [SerializeField] private Item[] itemReferences = new Item[0];
    public Item[] ItemReferences { get { return itemReferences; } }

    private Dictionary<Item, int> inventory = new Dictionary<Item, int>();
    private Dictionary<Item.Type, List<Item>> cachedLists = new Dictionary<Item.Type, List<Item>>();

    public int AddItem(Item item, int count = 1)
    {
        if (!inventory.ContainsKey(item)) { inventory.Add(item, 0); }
        inventory[item] += count;

        return inventory[item];
    }
    public int RemoveItem(Item item, int count = 1)
    {
        if (!inventory.ContainsKey(item)) { inventory.Add(item, 0); }
        inventory[item] -= count;

        return inventory[item];
    }
    public int GetItemCount(Item item)
    {
        if (!inventory.ContainsKey(item)) { return 0; }
        return inventory[item];
    }
    public List<Item> GetItemsOfType(Item.Type type)
    {
        if (!cachedLists.ContainsKey(type))
        {
            List<Item> newList = new List<Item>();
            foreach (Item i in itemReferences)
            {
                if (i.ItemType == type)
                {
                    newList.Add(i);
                }
            }
            cachedLists[type] = newList;
        }
        return cachedLists[type];
    }

    public Item GetRandom(float modifier = 1f)
    {
        return GetRandom(ItemReferences, modifier);
    }
    public Item GetRandomOfType(Item.Type type, float modifier = 1f)
    {
        return GetRandom(GetItemsOfType(type), modifier);
    }
    
    // Static

    // Source: https://softwareengineering.stackexchange.com/a/150642
    public static Item GetRandom(IEnumerable<Item> items, float modifier = 1f)
    {
        float totalWeight = 0f;
        Item selected = default(Item);

        foreach (Item i in items)
        {
            float weight = i.SpawnChance * modifier;

            float r = Random.Range(0f, weight + totalWeight);
            if (r >= totalWeight)
            {
                selected = i;
            }
            totalWeight += weight;
        }
        return selected;
    }
    public static Item GetRandom(IEnumerable<Item> items, Dictionary<Item.Type, float> modifiersByType)
    {
        float totalWeight = 0f;
        Item selected = default(Item);

        foreach (Item i in items)
        {
            float weight = i.SpawnChance * modifiersByType[i.ItemType];

            float r = Random.Range(0f, weight + totalWeight);
            if (r >= totalWeight)
            {
                selected = i;
            }
            totalWeight += weight;
        }
        return selected;
    }
}
#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(PlayerInventory))]
public class PlayerInventoryEditor : UnityEditor.Editor
{
    private Vector2 scroll = new Vector2();
    private PlayerInventory Inventory => target as PlayerInventory;

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        GUILayout.Space(15f);

        if (Application.isPlaying)
        {
            GUILayout.Label("Current Inventory");
            GUILayout.BeginScrollView(scroll);
            foreach (Item item in Inventory.ItemReferences)
            {
                int count = Inventory.GetItemCount(item);
                if (count == 0) { continue; }

                GUILayout.BeginHorizontal();
                UnityEditor.EditorGUILayout.ObjectField(item, typeof(Item), false);
                GUILayout.Label($"x {count}");
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }
        else
        {
            if (GUILayout.Button("Add All Items to References"))
            {
                var guids = UnityEditor.AssetDatabase.FindAssets("t:item");

                UnityEditor.SerializedProperty refs = serializedObject.FindProperty("itemReferences");
                refs.arraySize = guids.Length;

                int index = 0;
                foreach (string guid in guids)
                {
                    refs.GetArrayElementAtIndex(index).objectReferenceValue = UnityEditor.AssetDatabase.LoadAssetAtPath<Item>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));
                    index++;
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
