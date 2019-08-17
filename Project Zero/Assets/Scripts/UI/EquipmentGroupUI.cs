using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.UI
{
    public class EquipmentGroupUI : MonoBehaviour
    {
        public void Toggle()
        {
            gameObject.SetActive(!isActiveAndEnabled);
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
