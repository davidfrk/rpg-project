using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Stats
{
    public interface IStat
    {
        string GetName();
        string ValueToString();
    }
}
