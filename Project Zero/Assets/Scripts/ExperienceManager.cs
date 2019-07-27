using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public int level = 1;
    public float exp = 0;
    public float quadraticTermInLevelFormula = 10;
    public float linearTermInLevelFormula = 1000;

    internal float nextLevelProgression = 0f;

    [Space(10)]
    public float experienceGivenOnDeath;

    UnitController unitController;

    public void Awake()
    {
        unitController = GetComponent<UnitController>();
        unitController.OnKillCallback += CreditsKill;
    }

    public void CreditsKill(Unit prey)
    {
        if (prey != null)
        {
            ExperienceManager experienceManager = prey.GetComponent<ExperienceManager>();
            AddExperience(experienceManager.experienceGivenOnDeath);
        }
    }

    public void AddExperience(float amount)
    {
        exp += amount;
        UpdateExperience();
    }

    public void UpdateExperience()
    {
        level = GetLevelByExp(exp);
        float levelExp = GetExpByLevel(level);
        float nextLevelExp = GetExpByLevel(level + 1);
        nextLevelProgression = Mathf.InverseLerp(levelExp, nextLevelExp, exp);
    }

    public int GetLevelByExp(float exp)
    {
        // exp = a * level^2 + b * level
        // a * level^2 + b * level - exp = 0
        float a = quadraticTermInLevelFormula;
        float b = linearTermInLevelFormula;
        float c = -exp;
        
        //delta = b^2 -4ac
        float delta = b*b - 4*a*c;

        // level = (-b +- sqrt(delta)) / 2*a
        float level = (-b + Mathf.Sqrt(Mathf.Abs(delta))) / (2*a);

        return Mathf.FloorToInt(level);
    }

    public float GetExpByLevel(int level)
    {
        return quadraticTermInLevelFormula * level * level + linearTermInLevelFormula * level;
    }

    public void Start()
    {
        exp = GetExpByLevel(level);
    }

    //only to debug
    public void Update()
    {
        UpdateExperience();
    }
}
