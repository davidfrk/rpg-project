using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rpg.UI
{
    public class ButtonUI : Button
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            UIController.UIClick();
        }
    }
}
