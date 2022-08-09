using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ilang
{
    public class StageEnd : MonoBehaviour
    {
        [SerializeField]
        float _fadeOutSeconds = 0.0f;
        [SerializeField]
        string _closingCurtainState;
        [SerializeField]
        string _nextStageName;


        public void End() {
            //SoundMgr.Instance.FadeOut(_fadeOutSeconds);
            StartCoroutine(CoEnd());
        }

        IEnumerator CoEnd() {
            if (_closingCurtainState != "")
                CurtainMgr.Instance.Play(_closingCurtainState, LoadNextScene);
            else {
                yield return new WaitForSeconds(_fadeOutSeconds);
                LoadNextScene();
            }
        }
        void LoadNextScene() {
            AsyncOperation op = SceneManager.LoadSceneAsync(_nextStageName);
        }
    }
}