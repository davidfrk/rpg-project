using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public UIBar healthBar;
    Unit unit;

    void Awake()
    {
        unit = GetComponent<Unit>();
    }
    
    void Update()
    {
        //Temporariamente até ter eventos de mudança de hp
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        float state = unit.Health / unit.MaxHealth;
        if (state == 1f || state == 0f)
        {
            healthBar.gameObject.SetActive(false);
        }
        else
        {
            healthBar.gameObject.SetActive(true);
        }

        healthBar.Set(state);
    }
}
