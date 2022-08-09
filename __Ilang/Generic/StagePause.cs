using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ilang
{
    public class StagePause : MonoBehaviour
    {
        public void Pause() {
            TimeMgr.Instance.PauseGame();
        }

        public void Unpause() {
            TimeMgr.Instance.UnpauseGame();
        }

    }

}
