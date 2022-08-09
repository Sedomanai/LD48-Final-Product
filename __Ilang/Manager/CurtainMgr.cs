using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

/*
 * 화면 전환 스크립트
 * 셰이더 스크립트로 전환시킬 예정
 *
 * May become obsolete with shaders 
 * 
 */
namespace Ilang {
    public class CurtainMgr : Singleton<CurtainMgr> {
        Animator _anim;
        Action _onComplete;

        public bool transitioning { get; private set; }

        protected override void Awake() {
            base.Awake();
            gameObject.name = "[Curtain Manager]";
            _anim = gameObject.GetComponentInChildren<Animator>();
        }

        public void Play(string state, Action onComplete) {
            _anim.Play(state);
            _onComplete = onComplete;
            transitioning = true;
        }

        public void NextState() {
            transitioning = false;
            if (_onComplete != null)
                _onComplete.Invoke();
        }
    }
}