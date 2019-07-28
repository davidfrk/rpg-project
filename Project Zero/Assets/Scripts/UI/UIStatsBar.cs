using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatsBar : MonoBehaviour
{
    public RectTransform UIBar;
    public Text StatsText;
    private Vector2 size;

    void Awake()
    {
        size = UIBar.sizeDelta;
    }

    public void Set(float value, float maxValue)
    {
        StatsText.text = Mathf.FloorToInt(value) + "/" + Mathf.FloorToInt(maxValue);

        Vector2 newSize = size;
        newSize.x *= Mathf.Clamp01(value/maxValue);
        UIBar.sizeDelta = newSize;
    }
}
