using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] private int health = 0;
    public int Health
    {
        get => health; set
        {
            health = value; if (health <= 0)
            {
                health = 0;
                OnNoHealth.Invoke();
            }
            OnStatsChanged.Invoke();
        }
    }

    [SerializeField] private int money = 0;
    public int Money
    {
        get => money; set
        {
            money = value; if (money < 0) money = 0;
            OnStatsChanged.Invoke();
        }
    }

    [SerializeField] private float hunger = 0f;
    public float Hunger
    {
        get => hunger; set
        {
            hunger = value; if (hunger <= 0)
            {
                hunger = 0;
                OnEmptyStomach.Invoke();
            }
            OnStatsChanged.Invoke();
        }
    }

    [SerializeField] private float rest = 0f;
    public float Rest
    {
        get => rest; set
        {
            rest = value; 
            if (rest <= 0)
            {
                rest = 0;
                OnMaxTired.Invoke();
            }
            else if (rest > 1f)
            {
                rest = 1f;
            }
            OnStatsChanged.Invoke();
        }
    }

    [SerializeField] private int knowledge = 0;
    public int Knowledge
    {
        get => knowledge; set
        {
            knowledge = value;
            OnStatsChanged.Invoke();
        }
    }

    [SerializeField] private int speed = 10;
    public int Speed
    {
        get => speed; set
        {
            speed = value;
            OnStatsChanged.Invoke();
        }
    }

    [Space]
    [SerializeField] private int attack = 1;
    public int Attack
    {
        get => attack; set
        {
            attack = value;
            OnStatsChanged.Invoke();
        }
    }

    [SerializeField] private int defense = 0;
    public int Defense
    {
        get => defense; set
        {
            defense = value;
            OnStatsChanged.Invoke();
        }
    }

    public UnityEvent OnStatsChanged;
    public UnityEvent OnNoHealth;
    public UnityEvent OnEmptyStomach;
    public UnityEvent OnMaxTired;

    public void SaveValues()
    {
        PlayerPrefs.SetInt("Health", Health);
        PlayerPrefs.SetInt("Money", Money);
        PlayerPrefs.SetFloat("Hunger", Hunger);
        PlayerPrefs.SetFloat("Rest", Rest);
        PlayerPrefs.SetInt("Knowledge", Knowledge);
        PlayerPrefs.SetInt("Speed", Speed);

        PlayerPrefs.SetInt("Attack", Attack);
        PlayerPrefs.SetInt("Defense", Defense);

        PlayerPrefs.Save();
    }
    public void LoadValues()
    {
        Health = PlayerPrefs.GetInt("Health");
        Money = PlayerPrefs.GetInt("Money");
        Hunger = PlayerPrefs.GetFloat("Hunger");
        Rest = PlayerPrefs.GetFloat("Rest");
        Knowledge = PlayerPrefs.GetInt("Knowledge", Knowledge);
        Speed = PlayerPrefs.GetInt("Speed");

        Attack = PlayerPrefs.GetInt("Attack");
        Defense = PlayerPrefs.GetInt("Defense");
    }
}

public static class RandomExtensions
{
    public static void ApplyStats(this PlayerStats mine, PlayerStats other)
    {
        mine.Attack = other.Attack;
        mine.Defense = other.Defense;
        mine.Speed = other.Speed;
        mine.Knowledge = other.Knowledge;

        mine.Health = other.Health;
        mine.Hunger = other.Hunger;
        mine.Rest = other.Rest;
        mine.Money = other.Money;
    }
    public static TMP_Text GetText(this Button button)
    {
        return button.GetComponentInChildren<TMP_Text>();
    }
}