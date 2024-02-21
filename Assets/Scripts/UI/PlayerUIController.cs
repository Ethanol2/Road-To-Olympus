using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;

    [Header("Stat UI")]
    [SerializeField] private UIProgressBar healthBar;
    [SerializeField] private UIStat moneyStat;
    [SerializeField] private UIProgressBar hungerBar;
    [SerializeField] private UIProgressBar restBar;

    [Space]
    [SerializeField] private UIStat knowledgeStat;
    [SerializeField] private UIStat speedStat;
    [SerializeField] private UIStat defenseStat;
    [SerializeField] private UIStat attackStat;

    private void Update()
    {
        healthBar.SetBarAmount(stats.Health);
        moneyStat.SetValue(stats.Money);
        hungerBar.SetBarAmount(stats.Hunger);
        restBar.SetBarAmount(stats.Rest);

        knowledgeStat.SetValue(stats.Knowledge);
        speedStat.SetValue(stats.Speed);
        defenseStat.SetValue(stats.Defense);
        attackStat.SetValue(stats.Attack);
    }
}
