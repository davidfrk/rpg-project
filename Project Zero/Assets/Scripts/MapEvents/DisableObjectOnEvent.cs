using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.MapEvent
{
    public class DisableObjectOnEvent : MonoBehaviour
    {
        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
