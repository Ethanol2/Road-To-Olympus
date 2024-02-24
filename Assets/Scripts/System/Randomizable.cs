using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRandomizable
{
    public float Chance { get; }
    public string RandomizableType { get; }
}

public static class Randomizer
{
    // Source: https://softwareengineering.stackexchange.com/a/150642
    public static IRandomizable GetRandom(IEnumerable<IRandomizable> items, float modifier = 1f)
    {
        float totalWeight = 0f;
        IRandomizable selected = default(IRandomizable);

        foreach (IRandomizable i in items)
        {
            float weight = i.Chance * modifier;

            float r = Random.Range(0f, weight + totalWeight);
            if (r >= totalWeight)
            {
                selected = i;
            }
            totalWeight += weight;
        }
        return selected;
    }
    public static IRandomizable GetRandom(IEnumerable<IRandomizable> items, Dictionary<string, float> modifiersByType, float modifier = 1f)
    {
        float totalWeight = 0f;
        IRandomizable selected = default(IRandomizable);

        foreach (IRandomizable i in items)
        {
            float weight = i.Chance * modifiersByType[i.RandomizableType] * modifier;

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
