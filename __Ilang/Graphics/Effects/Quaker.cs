using UnityEngine;
using System.Collections;

namespace Ilang {
    public class Quaker : MonoBehaviour {
        IEnumerator _quake;
        float _magnitude, _duration;

        [SerializeField]

        #if UNITY_EDITOR
        [Boolean(3)]
        #endif
        byte constraints;

        void Awake() {
            _quake = BeginQuake();
            StartQuake(0.1f, 5);
        }

        public void StartQuake(float magnitude, float duration) {
            _magnitude = magnitude;
            _duration = duration;

            StartCoroutine(_quake);
        }

        public void StopQuake() {
            StopCoroutine(_quake);
        }

        IEnumerator BeginQuake() {
            Vector3 origin = transform.localPosition;
            float m, t = 0;

            while (t < 1) {
                origin = transform.localPosition;
                Vector3 fixedMagnitude = Random.insideUnitSphere * _magnitude;

                fixedMagnitude.x = (1 == (constraints & 1)) ? 0 : fixedMagnitude.x;
                fixedMagnitude.y = (2 == (constraints & 2)) ? 0 : fixedMagnitude.y;
                fixedMagnitude.z = (4 == (constraints & 4)) ? 0 : fixedMagnitude.z;

                transform.localPosition = origin + fixedMagnitude;

                m = Mathf.MoveTowards(_magnitude, 0, t);
                t += Time.deltaTime / _duration;
                yield return null;
            }

            transform.localPosition = origin;
        }
    }
}