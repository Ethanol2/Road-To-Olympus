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
        if (item is FoodItem food)
        {
            inventoryManager.EatItem(food, this);
        }
        else if (item is CombatItem equipment)
        {
            inventoryManager.EquipDequip(equipment, !itemEquipped, this);
        }
    }

    public void Init(InventoryManager manager)
    {
        inventoryManager = manager;
    }
    public void AddItem(Item newItem)
    {
        item = newItem;

        this.name = item.DisplayName;

        icon.sprite = item.Sprite;
        text.text = item.DisplayName;

        speed.gameObject.SetActive(false);
        attack.gameObject.SetActive(false);
        defense.gameObject.SetActive(false);
        food.gameObject.SetActive(false);

        if (item is FoodItem foodItem)
        {
            interactButton.gameObject.SetActive(true);
            interactText.text = "Eat";

            food.SetValue(foodItem.HungerGain * 10, true, true);
        }
        else if (item is CombatItem equipment)
        {
            interactButton.gameObject.SetActive(true);
            interactText.text = "Equip";

            speed.SetValue(equipment.SpeedBoost, true, true);
            attack.SetValue(equipment.AttackBoost, true, true);
            defense.SetValue(equipment.DefenseBoost, true, true);
        }
        else
        {
            interactButton.gameObject.SetActive(false);
        }

        knowledge.SetValue(item.KnowledgeBonus, true, true);
        cost.SetValue(item.MoneyValue, false, true);
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
