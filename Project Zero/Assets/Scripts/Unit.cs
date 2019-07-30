using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats;

[RequireComponent(typeof(ExperienceManager))]
public class Unit : MonoBehaviour
{
    internal ExperienceManager experienceManager;
    internal UnitController unitController;

    public bool alive = true;
    public int id;

    [SerializeField]
    float health;
    public float Health
    {
        get
        {
            return health;
        }
        private set
        {
            if (value <= 0f)
            {
                health = 0f;
                Die();
            }
            else
            {
                health = value;
            }
        }
    }

    [SerializeField]
    float mana;
    public float Mana
    {
        get
        {
            return mana;
        }
        private set
        {
            mana = value;
        }
    }

    public float radius = 0.5f;
    [SerializeField]
    public Stats stats;

    public BaseStats unitStats;
    public BaseStats baseStats;
    public BaseStats statsGain;
    public BaseStats equipStats;

    BaseStats newStats;

    float physicalResistance = 0f;
    float magicResistance = 0f;

    public void UpdateStats()
    {
        newStats = baseStats + (experienceManager.level * statsGain) + equipStats;
        BaseStats.UpdateDerivedStats(ref newStats);

        //maintaining proportion of life and mana
        health = health / unitStats.MaxHealth * newStats.MaxHealth;
        mana = mana / unitStats.MaxMana * newStats.MaxMana;

        unitStats = newStats;

        //Update resistances
        physicalResistance = PhysicalResistanceFormula(unitStats.Armor);
        magicResistance = MagicResistanceFormula(unitStats.MagicArmor);
    }

    public void TakeDamage(float damage, DamageType damageType, Unit damageDealer)
    {
        if (!alive) return;

        switch (damageType)
        {
            case DamageType.Physical:
                {
                    Health -= damage * (1 - physicalResistance);
                    break;
                }
            case DamageType.Magic:
                {
                    Health -= damage * (1 - magicResistance);
                    break;
                }
            case DamageType.Pure:
                {
                    Health -= damage;
                    break;
                }
        }

        if (alive == false)
        {
            //Credit the death
            unitController.Die(damageDealer);
        }
    }

    public static float PhysicalResistanceFormula (float armor)
    {
        if (armor >= 0f)
        {
            return 1 - 1 / (armor / 50 + 1);
        }
        else
        {
            return -Mathf.Abs(armor) / 100f;
        }
    }

    public static float MagicResistanceFormula(float magicArmor)
    {
        if (magicArmor >= 0f)
        {
            return 1 - 1 / (magicArmor / 50 + 1);
        }
        else
        {
            return -Mathf.Abs(magicArmor) / 100f;
        }
    }

    public void PayManaCost(float manaCost)
    {
        Mana -= manaCost;
    }

    public void Awake()
    {
        experienceManager = GetComponent<ExperienceManager>();
        unitController = GetComponent<UnitController>();
    }

    public void Start()
    {
        UpdateStats();
        health = unitStats.MaxHealth;
        mana = unitStats.MaxMana;
    }

    public void Update()
    {
        UpdateStats();
    }

    public void Die()
    {
        if (alive)
        {
            alive = false;
        }
    }

    public void Spawn()
    {
        health = unitStats.MaxHealth;
        mana = unitStats.MaxMana;
        alive = true;
    }
}