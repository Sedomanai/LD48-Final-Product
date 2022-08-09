using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ilang
{
    public class RepeatTrigger : MonoBehaviour
    {
        public float preheatSeconds;
        public float waitSeconds;
        public eAutomation automationType;
        public UnityEvent onRepeat;


        void Awake() {
            if (automationType == eAutomation.OnAwake) {
                Trigger();
            }
        }

        void OnEnable() {
            switch (automationType) {
                case eAutomation.OnEnable:
                    Trigger();
                    break;
                case eAutomation.OnDisable:
                    Stop();
                    break;
            }
        }

        void OnDisable() {
            switch (automationType) {
                case eAutomation.OnEnable:
                    Stop();
                    break;
                case eAutomation.OnDisable:
                    Trigger();
                    break;
            }
        }

        public void Trigger() {
            StartCoroutine(CoTrigger());
        }
        public void Stop() {
            StopCoroutine(CoTrigger());
        }


        IEnumerator CoTrigger() {
            yield return new WaitForSeconds(preheatSeconds);
            while (true) {
                onRepeat.Invoke();
                yield return new WaitForSeconds(waitSeconds);
            }
        }
    }
}