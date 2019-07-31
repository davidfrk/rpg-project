using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Stats;

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
                health = Mathf.Clamp(value, 0f, lastMaxHealth);
            }
        }
    }
    float lastMaxHealth;
    public float MaxHealth
    {
        get
        {
            return lastMaxHealth;
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
            mana = Mathf.Clamp(value, 0f, lastMaxMana);
        }
    }
    float lastMaxMana;
    public float MaxMana
    {
        get
        {
            return lastMaxMana;
        }
    }

    public float radius = 0.5f;
    [SerializeField]
    public Stats stats;

    float physicalResistance = 0f;
    float magicResistance = 0f;

    public void UpdateStats()
    {
        stats.UpdateLevel(experienceManager.level);
        stats.UpdateDerivedStats();

        if (alive)
        {
            Health += stats.HpRegen.Value * Time.deltaTime;
            Mana += stats.ManaRegen.Value * Time.deltaTime;
        }

        //maintaining proportion of life and mana
        health = health / lastMaxHealth * stats.MaxHealth.Value;
        mana = mana / lastMaxMana * stats.MaxMana.Value;

        lastMaxHealth = stats.MaxHealth.Value;
        lastMaxMana = stats.MaxMana.Value;

        //Update resistances
        physicalResistance = PhysicalResistanceFormula(stats.Armor.Value);
        magicResistance = MagicResistanceFormula(stats.MagicArmor.Value);
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
        stats.InitStats();
        UpdateStats();
        lastMaxHealth = stats.MaxHealth.Value;
        lastMaxMana = stats.MaxMana.Value;
        health = lastMaxHealth;
        mana = lastMaxMana;
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
        health = lastMaxHealth;
        mana = lastMaxMana;
        alive = true;
    }
}