using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Skills;

public class SkillsBook : MonoBehaviour
{
    public static SkillsBook instance;
    public List<Skill> skills;

    void Awake()
    {
        instance = this;
    }

    static public Skill Get(int id)
    {
        if (id >= instance.skills.Count)
        {
            Debug.LogError("Skill id out of index");
            return null;
        }
        return instance.skills[id];
    }
}
