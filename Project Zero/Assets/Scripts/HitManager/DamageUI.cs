using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
    public Text damageText;
    public AnimationCurve animationCurve;
    public float duration = 1f;
    public float topHeight = 1f;

    private Vector3 startingPosition;
    private float startingTime;
    private Vector3 targetPosition;
    

    public void UpdateUI(Unit owner, Unit target, float damage)
    {
        transform.position = Vector3.Lerp(owner.transform.position, target.transform.position, 0.95f) + 1.5f * Vector3.up;
        damageText.text = damage.ToString("F0");

        startingPosition = transform.position;
        startingTime = Time.time;

        Vector2 randDir = Random.insideUnitCircle;
        targetPosition = startingPosition + topHeight * Vector3.up + new Vector3(randDir.x, 0f, randDir.y);

        Destroy(gameObject, duration);
    }

    public void Update()
    {
        float time = Mathf.InverseLerp(startingTime, startingTime + duration, Time.time);
        transform.position = Vector3.Lerp(startingPosition, targetPosition, animationCurve.Evaluate(time));
        //animationCurve.Evaluate(time) * topHeight * Vector3.up + startingPosition;
    }
}
