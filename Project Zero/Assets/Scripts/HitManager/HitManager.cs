using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour
{
    private static HitManager instance;

    public DamageUI damageUI;
    public GameObject bloodEffect;

    void Awake()
    {
        instance = this;
    }

    public static void Crit(Unit owner, Unit target, float damage)
    {
        DamageUI ui = Instantiate<DamageUI>(instance.damageUI);
        ui.UpdateUI(owner, target, damage);

        GameObject hitEffect = Instantiate(instance.bloodEffect);
        hitEffect.transform.position = target.transform.position + Vector3.up;
        hitEffect.transform.rotation = Quaternion.LookRotation(owner.transform.forward, Vector3.up);
    }
}
