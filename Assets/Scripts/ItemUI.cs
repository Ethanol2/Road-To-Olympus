using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private InventoryManager inventoryManager;
    public Item Item => item;

    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text text;

    [Space]
    [SerializeField] private UIStat speed;
    [SerializeField] private UIStat knowledge;
    [SerializeField] private UIStat attack;
    [SerializeField] private UIStat defense;
    [SerializeField] private UIStat food;
    [SerializeField] private UIStat cost;

    [Space]
    [SerializeField] private Button interactButton;
    [SerializeField] private TMP_Text interactText;
    [SerializeField] private bool itemEquipped = false;
    [SerializeField] private Image background;

    [SerializeField] private Color normalColour = Color.grey;
    [SerializeField] private Color equipedColour = Color.blue;

    private void Start()
    {
        interactButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (item.ItemType == Item.Type.Eadible)
        {
            inventoryManager.EatItem(item, this);
            return;
        }

        inventoryManager.EquipDequip(item, !itemEquipped, this);
    }

    public void Init(InventoryManager manager)
    {
        inventoryManager = manager;
    }
    public void AddItem(Item newItem)
    {
        item = newItem;

        this.name = item.name;

        icon.sprite = item.Sprite;
        text.text = item.name;

        speed.SetValue(item.SpeedBonus, true, true);
        knowledge.SetValue(item.KnowledgeBonus, true, true);
        attack.SetValue(item.AttackBonus, true, true);
        defense.SetValue(item.DefenseBonus, true, true);
        food.SetValue(item.HungerGain * 10, true, true);
        cost.SetValue(item.MoneyValue, false, true);

        switch (item.ItemType)
        {
            case Item.Type.Eadible:
                interactButton.gameObject.SetActive(true);
                interactText.text = "Eat";
                break;
            case Item.Type.Boots:
            case Item.Type.Helmet:
            case Item.Type.Shield:
            case Item.Type.Weapon:
            case Item.Type.Armor:
                interactButton.gameObject.SetActive(true);
                interactText.text = "Equip";
                break;
            default:
                interactButton.gameObject.SetActive(false);
                break;
        }
    }
    public void MarkEquiped()
    {
        Debug.Log($"[ItemUI] Equipping {item.name}");

        this.transform.SetAsFirstSibling();
        background.color = equipedColour;
        interactText.text = "Remove";
        itemEquipped = true;
    }
    public void MarkDequipped()
    {
        Debug.Log($"[ItemUI] Dequipping {item.name}");

        this.transform.SetAsLastSibling();
        background.color = normalColour;
        interactText.text = "Equip";
        itemEquipped = false;
    }
}
