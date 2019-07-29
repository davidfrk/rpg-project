using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float castRange = 3f;
    public float manaCost = 50;
    public Sprite icon;
    internal Unit owner;

    public virtual void Cast(Unit owner, Transform castTransform, Vector3 targetPosition)
    {
        
    }

    public virtual void OnCastEnd()
    {
        
    }

    public virtual bool CanCast(Unit owner)
    {
        return owner.Mana >= manaCost;
    }

    public static Collider[] FindUnitsInBox(Vector3 center, Vector3 dimensions, Quaternion orientation)
    {
        return Physics.OverlapBox(center, dimensions, orientation, LayerMask.GetMask("Unit"));
    }
}
