using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

namespace Ilang
{

    [RequireComponent(typeof(Animator))]
    public class SimpleAnimation : MonoBehaviour
    {
        [SerializeField]
        string state;
        public eAutomation automationType;

        Animator _anim;

        void Awake() {
            _anim = GetComponent<Animator>();

            if (automationType == eAutomation.OnAwake)
                _anim.Play(state);
        }

        void OnEnable() {
            if (automationType == eAutomation.OnEnable)
                _anim.Play(state);
        }

        //}

        //public void Play() {
        //    GetComponent<Animator>();
        //}

        //public enum eState
        //{
        //    Pre,
        //    loop,
        //    Post,
        //} eState _state;
        //public eState state { get { return _state; } }

        //[SerializeField]
        //AnimationClip _startClip;

        //public UnityEvent onPreBegin, onLoopBegin, onLoopEnd, onPostEnd;


        //void OnEnable() {
        //    if (_startClip) {
        //        _anim.Play(_startClip.name);
        //    }
                
        //    //_state = eState.Pre;
        //    //onPreBegin.Invoke();
        //    //if (_preClip) {
        //    //    _anim.Play(_preClip.name);
        //    //    StartCoroutine(CoActivate(_preClip.length));
        //    //} else PlayLoop();
        //}
        //void Update() {
            
        //}

        //void PlayLoop() {
        //    _state = eState.loop;
        //    _anim.Play(_clip.name);
        //    onLoopBegin.Invoke();
        //}

        //IEnumerator CoActivate(float seconds) {
        //    yield return new WaitForSeconds(seconds);
        //    PlayLoop();
        //}

        //public void EndLoop() {
        //    onLoopEnd.Invoke();
        //    if (_postClip) {
        //        _state = eState.Post;
        //        _anim.Play(_postClip.name);
        //        StartCoroutine(CoDisactivate(_postClip.length));
        //    } else {
        //        onPostEnd.Invoke();
        //    }
        //}

        //IEnumerator CoDisactivate(float seconds) {
        //    yield return new WaitForSeconds(seconds);
        //    onPostEnd.Invoke();
        //}

    }
}
