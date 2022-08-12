using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ilang;
using UnityEngine.Tilemaps;

public class Mole : Platformer2D
{
    Animator[] _anims;
    float direction;
    bool _justShot = false;
    bool _justDigged = false;
    bool _justSlashed = false;

    [SerializeField]
    GameStatsSO _stats;
    [SerializeField]
    bool _haveCamFollow;
    [SerializeField]
    bool initiallyFacingLeft;
    [SerializeField]
    ObjectPool _jumpSmokePool;
    [SerializeField]
    ObjectPool _bombPool;

    [SerializeField]
    AudioClip _deathClip;
    DigModule _digMod;


    protected override void Awake() {
        base.Awake();
        _anims = gameObject.GetComponentsInChildren<Animator>();
        _digMod = gameObject.GetComponent<DigModule>();
        direction = initiallyFacingLeft ? -1.0f : 1.0f;
    }

    public void DeathEvent() {
        _box.enabled = false;
        ResetAxis();
        GetComponentInChildren<Camera2DSubject>().ResetTrigger();
        GetComponent<SpriteRenderer>().sortingOrder = 5;
        SoundMgr.Instance.PlaySFX(_deathClip);
        for (uint i = 0; i < _anims.Length; i++) {
            var anim = _anims[i];
            anim.Play("death");
        }
    }

    void Update() {
        //normally this should be separated to be invoked once but it's not that important right now
        speed = (Game.Instance.shoeLevel > 0) ? _stats.speed.y : _stats.speed.x;
        _jump.jumpHeight = (Game.Instance.shoeLevel > 1) ? _stats.jumpHeight.y : _stats.jumpHeight.x;
        _jump.jumpCount = (Game.Instance.shoeLevel > 2) ? (uint)_stats.jumpCount.y : (uint)_stats.jumpCount.x;

        var state = Game.Instance.State;
        acceptControls = state == Game.eState.Playing || state == Game.eState.Overworld || state == Game.eState.Ending;
        UpdatePlatformer(!_digMod.Digging);
        if (_digMod.Digging) {
            _axis.x = 0.0f;
            _body.velocity = _axis;
        }

        if (Input.GetKeyDown(KeyCode.Space) && acceptControls && !TimeMgr.Instance.Paused) {
            if (Input.GetKey(KeyCode.DownArrow) && !_justDigged) {
                _justDigged = true;
            } else if (!_justSlashed) {
                _justSlashed = true;
            }
        }

        bool bombable =
            Game.Instance.bombInt.Value > 0 && 
            Input.GetKeyDown(KeyCode.LeftControl) && 
            acceptControls &&
            Game.Instance.State != Game.eState.Overworld &&
            !TimeMgr.Instance.Paused;

        if (bombable) {
            Game.Instance.bombInt.AddValue(-1);
            var obj = _bombPool.InstantiateFromPool();
            obj.transform.position = transform.position + new Vector3(0, 0.5f, 0);
        }
    }

    void LateUpdate() {
        if (acceptControls && !TimeMgr.Instance.Paused) {
            //facing
            if (_axis.x > 0.2f) {
                transform.localScale = new Vector3(direction, 1, 1);
            } else if (_axis.x < -0.2f) {
                transform.localScale = new Vector3(-direction, 1, 1);
            }
        }

        UpdateAnimation();
    }
    void UpdateAnimation() {
        for (uint i = 0; i < _anims.Length; i++) {
            var anim = _anims[i];
            anim.speed = 1.0f;

            if (_digMod.Digging) {
                anim.speed = (Game.Instance.digLevel > 0) ? _stats.digAnim.y : _stats.digAnim.x;
            } else anim.speed = (Game.Instance.shoeLevel > 0) ? _stats.speedAnim.y : _stats.speedAnim.x;

            anim.SetFloat("Axis X", _axis.x);
            //anim.SetFloat("Axis Y", _axis.y);
            anim.SetFloat("Axis Y", _jump.AbsoluteYAxis);

            anim.SetBool("Hit Ground", _jump.IsGround);

            if (_jump.JustJumped) {
                _jumpSmokePool.InstantiateFromPool().transform.position = transform.position;
                //anim.SetTrigger("Jump");
                anim.Play("jump_begin");
            }

            if (_justDigged) {
                anim.SetTrigger("Dig");
                _justDigged = false;
            }

            if (_justSlashed) {
                anim.SetTrigger("Slash");
                _justSlashed = false;
            }

            if (_justShot) {
                anim.SetTrigger("Shoot");
            }
        }
    }
}
