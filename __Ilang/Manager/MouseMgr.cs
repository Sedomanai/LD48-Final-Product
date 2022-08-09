using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ilang {
    public class MouseMgr : Singleton<MouseMgr> {
        public GameObject selected { get; set; }
        Vector3 _mousePoint, _mousePos;

        new void Awake() {
            base.Awake();
            name = "[MouseManager]";
        }

        public Vector2 GetMousePosition(Vector3 offset = default(Vector3)) {
            _mousePoint = Input.mousePosition + offset;
            _mousePos = Camera.main.ScreenToWorldPoint(_mousePoint);
            return _mousePos;
        }

        public void CastRay() {
            Camera.main.ScreenPointToRay(Input.mousePosition);
        }

    }
}