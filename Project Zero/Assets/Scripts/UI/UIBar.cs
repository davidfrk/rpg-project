using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBar : MonoBehaviour
{
    public Vector2 size;
    public RectTransform rectTransform;

    public void Set(float state)
    {
        if (state == 0)
        {
            rectTransform.gameObject.SetActive(false);
        }
        else
        {
            Vector2 newSize = size;
            newSize.x *= Mathf.Clamp01(state);
            rectTransform.sizeDelta = newSize;
            rectTransform.gameObject.SetActive(true);
        }
    }
}
