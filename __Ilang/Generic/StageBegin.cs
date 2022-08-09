using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Ilang
{
    public class StageBegin : MonoBehaviour
    {
        [SerializeField]
        float _stageWarmUpSeconds = 0.0f;

        [SerializeField]
        AudioClip _stageBgm;
        [SerializeField]
        string _openingCurtainState;

        [SerializeField]
        UnityEvent OnStart;

        void Start() {
            StartCoroutine(CoStart());
        }

        IEnumerator CoStart() {
            yield return new WaitForSeconds(_stageWarmUpSeconds);
            if (_stageBgm)
                SoundMgr.Instance.PlayBGM(_stageBgm);
            if (_openingCurtainState != "")
                CurtainMgr.Instance.Play(_openingCurtainState, OnStart.Invoke);
            else OnStart.Invoke();
        }

        public void PlayAudioClip(AudioClip clip) {
            SoundMgr.Instance.PlaySFX(clip);
        }
    }
}