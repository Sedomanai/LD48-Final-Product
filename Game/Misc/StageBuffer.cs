using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ilang
{
    public class StageBuffer : MonoBehaviour
    {
        void Awake() {
            if (Game.Instance.digLevel == 3) {
                Game.Instance.stoneInt.SetValue(50);
            }
        }
    }
}
