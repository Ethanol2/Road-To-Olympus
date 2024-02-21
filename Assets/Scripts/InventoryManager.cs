using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private PlayerStats stats;
    [SerializeField] private GameObject inventoryScreen;
    [SerializeField] private Button inventoryButton;

    [Space]
    [SerializeField] private ItemUI inventoryItemPrefab;
    [SerializeField] private Transform itemList;

    [Space]
    [SerializeField] private bool addStartItems = true;
    [SerializeField] private Item[] startItems = new Item[0];

    [Space]
    [SerializeField] private List<Item> equippedItems = new List<Item>();
    Dictionary<CombatItem.EquipableType, ItemUI> equippedUIs = new Dictionary<CombatItem.EquipableType, ItemUI>();

    private List<ItemUI> uiItems = new List<ItemUI>();

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        inventoryScreen.SetActive(false);
        inventoryButton.onClick.AddListener(OnClick);

        if (addStartItems)
        {
            foreach (Item item in startItems)
            {
                Add(item, 0, false);
            }
        }

        equippedUIs.Add(CombatItem.EquipableType.Weapon, null);
        equippedUIs.Add(CombatItem.EquipableType.Armor, null);
        equippedUIs.Add(CombatItem.EquipableType.Shield, null);
        equippedUIs.Add(CombatItem.EquipableType.Boots, null);
        equippedUIs.Add(CombatItem.EquipableType.Helmet, null);
    }

    private void OnClick()
    {
        inventoryScreen.SetActive(!inventoryScreen.activeInHierarchy);
    }
    private void CreateItemUI(Item item)
    {
        GameObject newUI = GameObject.Instantiate(inventoryItemPrefab.gameObject, itemList);
        ItemUI ui = newUI.GetComponent<ItemUI>();
        ui.Init(this);
        ui.AddItem(item);
        uiItems.Add(ui);
    }
    private void DestroyItemUI(Item item)
    {
        DestroyItemUI(LocateUIForItem(item));
        
    }
    private void DestroyItemUI(ItemUI ui)
    {
        if (!ui) { return; }

        uiItems.Remove(ui);
        Destroy(ui.gameObject);
        return;
    }
    private ItemUI LocateUIForItem(Item item)
    {
        for (int k = uiItems.Count - 1; k >= 0; k--)
        {
            if (uiItems[k].Item == item)
            {
                return uiItems[k];
            }
        }
        return null;
    }
    private void OpenNewItemModal(Item item)
    {
        ModalController.OpenModal(
            $"You got an Item!",
            $"The item \"{item.DisplayName}\" has been added your inventory",
            item.Sprite);
    }

    public void Add(Item item, int cost, bool showModal = true)
    {
        stats.Money -= cost;
        CreateItemUI(item);
        inventory.AddItem(item);

        stats.Knowledge += item.KnowledgeBonus;

        if (showModal) OpenNewItemModal(item);
    }
    public void Sell(Item item, int cost)
    {
        stats.Money += cost;

        stats.Knowledge -= item.KnowledgeBonus;

        DestroyItemUI(item);
        inventory.RemoveItem(item);
    }
    public void EquipDequip(CombatItem item, bool equiping, ItemUI ui = null)
    {        
        int mod = equiping ? 1 : -1;
        if (ui == null)
        {
            if (equiping)
            {
                ui = LocateUIForItem(item);
                if (ui == null)
                {
                    Add(item, 0);
                }
            }
            else
            {
                ui = LocateUIForItem(item);
                if (ui == null)
                {
                    return;
                }
            }
        }

        if (equiping)
        {
            equippedUIs[item.EquipType]?.MarkDequipped();

            equippedItems.Remove(item);
            equippedItems.Add(item);

            ui.MarkEquiped();
            equippedUIs[item.EquipType] = ui;
        }
        else if (equippedUIs[item.EquipType] == ui)
        {
            ItemUI oldUI = equippedUIs[item.EquipType];
            if (oldUI)
            {
                equippedItems.Remove(oldUI.Item);
                oldUI.MarkDequipped();
            }
            equippedUIs[item.EquipType] = ui;
        }

        stats.Attack += item.AttackBoost * mod;
        stats.Defense += item.DefenseBoost * mod;
        stats.Speed += item.SpeedBoost * mod;
    }
    public void EatItem(FoodItem item, ItemUI ui)
    {
        stats.Hunger += item.HungerGain;
        DestroyItemUI(ui);
    }
}
