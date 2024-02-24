using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Food Item")]
public class FoodItem : Item
{
    [SerializeField] private float hungerGain = 0.2f;
    public float HungerGain => hungerGain;

    public override string RandomizableType => "FoodItem";

}
