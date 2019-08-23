using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Stats;

public class CharacterCrit
{
    private List<Crit> criticalSources = new List<Crit>();

    public void AddCrit(Crit crit)
    {
        criticalSources.Add(crit);
        criticalSources.Sort((a,b) => a.criticalDamage.CompareTo(b.criticalDamage));
    }

    public void AddCritList(List<CritBonus> critBonus, object source)
    {
        foreach(CritBonus crit in critBonus)
        {
            criticalSources.Add(new Crit(crit, source));
        }
        criticalSources.Sort((a, b) => a.criticalDamage.CompareTo(b.criticalDamage));
    }

    public void RemoveAllCritsFromSource(Object source)
    {
#pragma warning disable CS0252 // Possível comparação de referência inesperada; o lado esquerdo precisa de conversão
        criticalSources.RemoveAll(crit => (crit.source == source));
#pragma warning restore CS0252 // Possível comparação de referência inesperada; o lado esquerdo precisa de conversão
    }

    public Crit Proc()
    {
        foreach(Crit crit in criticalSources)
        {
            float random = Random.value;
            if (crit.procChance > random)
            {
                return crit;
            }
        }
        return null;
    }
}
