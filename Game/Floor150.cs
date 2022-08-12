using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ilang { 
    public class Floor150 : MonoBehaviour
    {
        [SerializeField]
        UnityEvent endEvent;

        bool safeSwitch = false;
        void OnTriggerStay2D(Collider2D collision) {
            if (collision.gameObject.GetComponent<Mole>() && !Game.Instance.gameFinished) {
                if (!safeSwitch) {
                    safeSwitch = true;
                    endEvent.Invoke();
                }
            }
        }
    }
}
