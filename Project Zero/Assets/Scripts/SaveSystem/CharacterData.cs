using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.ProgressionSystem;

public class CharacterData
{
    public float health;
    public float mana;
    public float exp;
    public int[] statsLevels;
    public int skillPoints;
    public int[] equipments;
    public int[] inventory;
    public float[] position;

    public void GetData(Unit unit)
    {
        //Stats
        health = unit.Health;
        mana = unit.Mana;

        statsLevels = new int[4];
        statsLevels[0] = unit.stats.StrLevel;
        statsLevels[1] = unit.stats.DexLevel;
        statsLevels[2] = unit.stats.IntLevel;
        statsLevels[3] = unit.stats.WillLevel;

        //Level
        ExperienceManager experienceManager = unit.GetComponent<ExperienceManager>();
        if (experienceManager)
        {
            exp = experienceManager.Exp;
        }

        //SkillTree
        SkillTree skillTree = unit.GetComponent<SkillTree>();
        if (skillTree != null)
        {
            skillPoints = skillTree.SkillPoints;
        }

        //Position
        position = new float[3];
        position[0] = unit.transform.position.x;
        position[1] = unit.transform.position.y;
        position[2] = unit.transform.position.z;
    }

    public void Apply(Unit unit)
    {
        //Stats
        unit.stats.StrLevel = statsLevels[0];
        unit.stats.DexLevel = statsLevels[1];
        unit.stats.IntLevel = statsLevels[2];
        unit.stats.WillLevel = statsLevels[3];
        unit.stats.UpdateBaseStats();

        //Level
        ExperienceManager experienceManager = unit.GetComponent<ExperienceManager>();
        if (experienceManager)
        {
            experienceManager.Exp = exp;
        }

        //SkillTree
        SkillTree skillTree = unit.GetComponent<SkillTree>();
        if (skillTree != null)
        {
            skillTree.SkillPoints = skillPoints;
        }

        //Position
        unit.unitController.Teleport(new Vector3(position[0], position[1], position[2]), Quaternion.identity);

        //LastStats
        unit.UpdateStats();
        unit.Health = health;
        unit.Mana = mana;
    }
}
