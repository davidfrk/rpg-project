using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldUI : MonoBehaviour
{
    public Text goldText;

    void Update()
    {
        goldText.text = PlayerController.localPlayer.gold.ToString();
    }
}
