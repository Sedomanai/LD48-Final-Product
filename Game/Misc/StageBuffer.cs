using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ilang
{
    public class StageBuffer : MonoBehaviour
    {
        void Awake() {
            if (Game.Instance.digLevel == 3) {
                Game.Instance.resetStoneInt();
            }

            if (!Game.Instance.gameFinished) {
                Game.Instance.ceremonyDone = false;
                Game.Instance.ceremonyEligible = false;
            }
        }
    }
}
