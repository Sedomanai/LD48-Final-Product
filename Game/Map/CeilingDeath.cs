using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Ilang;

public class CeilingDeath : MonoBehaviour
{
    Camera2D _cam;

    public UnityEvent OnCeilingDeath; 

    void Awake() {
        _cam = Camera.main.GetComponent<Camera2D>();
    }

    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.GetComponent<Mole>()) {
            var jump = collision.gameObject.GetComponent<Jump2DModule>();
            if (jump.IsGround) {
                //Debug.Log("jump death?");
                OnCeilingDeath.Invoke();
            }
        }
    }

    void LateUpdate() {
        var pos = transform.position;
        pos.y = _cam.transform.position.y + _cam.Size.y / 2.0f;
        transform.position = pos;
    }
}
