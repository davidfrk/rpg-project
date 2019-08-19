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
        set
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
        set
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
    public float range = 0.15f;

    [SerializeField]
    public Stats stats;

    [SerializeField]
    float physicalResistance = 0f;
    [SerializeField]
    float magicResistance = 0f;
    float attackSpeed = 1f;
    public float AttackSpeed
    {
        get
        {
            return attackSpeed;
        }
    }

    float movementSpeed = 1f;
    public float MovementSpeed
    {
        get
        {
            return movementSpeed;
        }
    }

    public CharacterCrit crit = new CharacterCrit();

    public void UpdateStats()
    {
        stats.UpdateLevel(experienceManager.Level);
        stats.UpdateDerivedStats();

        if (alive)
        {
            Health += stats.HpRegen.Value * Time.deltaTime;
            Mana += stats.ManaRegen.Value * Time.deltaTime;
        }

        //maintaining proportion of life and mana
        health = health / lastMaxHealth * stats.MaxHealth.Value;
        mana = mana / lastMaxMana * stats.MaxMana.Value;

        UpdateMaxStats();

        //Update resistances
        physicalResistance = PhysicalResistanceFormula(stats.Armor.Value);
        magicResistance = MagicResistanceFormula(stats.MagicArmor.Value);
        attackSpeed = AttackSpeedFormula(stats.AttackSpeed.Value);
    }

    private void UpdateMaxStats()
    {
        lastMaxHealth = stats.MaxHealth.Value;
        lastMaxMana = stats.MaxMana.Value;
    }

    public void TakeDamage(float damage, DamageType damageType, Unit damageDealer)
    {
        if (!alive) return;

        switch (damageType)
        {
            case DamageType.Physical:
                {
                    Health -= damage * stats.DamageTakenMultiplier.Value * (1 - physicalResistance);
                    break;
                }
            case DamageType.Magic:
                {
                    Health -= damage * stats.DamageTakenMultiplier.Value * (1 - magicResistance);
                    break;
                }
            case DamageType.Pure:
                {
                    Health -= damage;
                    break;
                }
        }

        if (damageDealer != null)
        {
            unitController.CallOnBeingAttackedEvent(damageDealer);
        }
        
        if (alive == false)
        {
            //Credit the death
            unitController.Die(damageDealer);
        }
    }

    public float CritMult
    {
        get
        {
            return 1f + stats.Int.Value / 100f;
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

    public static float AttackSpeedFormula(float attackSpeed)
    {
        return 0.5f + Mathf.Sqrt(attackSpeed) / 20f;
    }

    public void PayManaCost(float manaCost)
    {
        Mana -= manaCost;
    }

    public void Regen(Resource resource, float amount, Unit healer)
    {
        switch (resource)
        {
            case Resource.Health:
                {
                    Health += amount;
                    break;
                }
            case Resource.Mana:
                {
                    Mana += amount;
                    break;
                }
        }
    }

    public void RegenToFull()
    {
        Health = lastMaxHealth;
        Mana = lastMaxMana;
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
        //UpdateMaxStats();
        
        if (alive)
        {
            health = lastMaxHealth;
            mana = lastMaxMana;
        }
        else
        {
            health = 0;
            mana = 0;
            unitController.Die(null);
        }

        //Temporário até design de critico
        crit.AddCrit(new Crit(stats.baseStats.crit, this));
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

    public enum Resource
    {
        Health,
        Mana,
    }
}