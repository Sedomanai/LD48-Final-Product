using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/* 
* 게임 오브젝트 활성화/비활성화 스크립트
* Generic GameObject (Dis)Activation Script
* 
* Used to toggle activation on a gameobject after a few seconds (upon awake or enable)
* ex) Bullets, scene start, etc.
* 
*/

namespace Ilang {
    public class ActionTrigger : MonoBehaviour
    {
        public float waitSeconds;
        public eAutomation automateOption;
        public UnityEvent onWaitEnded;

        void Awake() {
            if (automateOption == eAutomation.OnAwake) {
                Trigger();
            }
        }

        void OnEnable() {
            if (automateOption == eAutomation.OnEnable) {
                Trigger();
            }
        }

        void OnDisable() {
            if (automateOption == eAutomation.OnDisable) {
                Trigger();
            }
        }

        public void Trigger() {
            StartCoroutine(CoToggle());
        }

        IEnumerator CoToggle() {
            yield return new WaitForSeconds(waitSeconds);
            onWaitEnded.Invoke();
        }
    }
}