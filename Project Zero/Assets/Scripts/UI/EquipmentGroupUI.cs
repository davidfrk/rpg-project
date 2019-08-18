using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.UI
{
    public class EquipmentGroupUI : MonoBehaviour
    {
        public void Toggle()
        {
            if (isActiveAndEnabled)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        public void Open()
        {
            gameObject.SetActive(true);
            AudioManager.instance.PlaySound(AudioManager.UISound.OpenShop);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            AudioManager.instance.PlaySound(AudioManager.UISound.CloseShop);
        }
    }
}
