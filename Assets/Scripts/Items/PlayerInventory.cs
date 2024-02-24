using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Threading.Tasks;
#endif

[CreateAssetMenu(menuName = "Item/Inventory")]
public class PlayerInventory : ScriptableObject
{
    [SerializeField] private Item[] itemReferences = new Item[0];
    public Item[] ItemReferences { get { return itemReferences; } }

    private Dictionary<Item, int> inventory = new Dictionary<Item, int>();
    private Dictionary<System.Type, List<Item>> cachedLists = new Dictionary<System.Type, List<Item>>();

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
    public List<Item> GetItemsOfType<T>() where T : Item
    {
        System.Type type = typeof(T);

        if (!cachedLists.ContainsKey(type))
        {
            List<Item> newList = new List<Item>();
            foreach (Item i in itemReferences)
            {
                if (i is T item)
                {
                    newList.Add(item);
                }
            }
            cachedLists[type] = newList;
        }
        return cachedLists[type];
    }

    public Item GetRandom(float modifier = 1f)
    {
        return Randomizer.GetRandom(ItemReferences, modifier) as Item;
    }
    public Item GetRandomOfType<T>(float modifier = 1f) where T : Item 
    {
        return Randomizer.GetRandom(GetItemsOfType<T>(), modifier) as Item;
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
