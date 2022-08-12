using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ilang
{
   public class Ending : MonoBehaviour
    {
        [SerializeField]
        CurtainScript script;

        bool safeSwitch = false;
        void OnTriggerStay2D(Collider2D collision) {
            if (collision.gameObject.GetComponent<Mole>()) {
                if (!safeSwitch) {
                    safeSwitch = true;
                    script.FadeOut();
                }
            }
        }
    }
}
