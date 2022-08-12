using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ilang
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Camera2DTrigger : MonoBehaviour
    {
        [SerializeField]
        Camera2DSubject _subject;

        BoxCollider2D _box;

        [SerializeField]
        bool _changeX;
        [SerializeField]
        bool _changeY;
        [SerializeField]
        Camera2DFlagsX _flagX = new Camera2DFlagsX();
        [SerializeField]
        Camera2DFlagsY _flagY = new Camera2DFlagsY();

        [SerializeField]
        Transform _snapPoint;

        void Awake() {
            _box = gameObject.GetComponent<BoxCollider2D>();
            _box.isTrigger = true;
            _flagX.snapPoint = _snapPoint ? _snapPoint.position.x : _flagX.snapPoint;
            _flagY.snapPoint = _snapPoint ? _snapPoint.position.y : _flagY.snapPoint;
        }

        void Update() {
            if (_box.bounds.Contains(_subject.transform.position)) {
                if (_changeX)
                    _subject.AcceptTrigger(_flagX);
                if (_changeY)
                    _subject.AcceptTrigger(_flagY);
            }
        }
        public Vector3 Center {
            get { return _box.bounds.center; }
        }


#if UNITY_EDITOR
        [CustomEditor(typeof(Camera2DTrigger))]
        public class Camera2DTriggerEditor : Editor
        {
            public Camera2DTrigger cam { get { return target as Camera2DTrigger; } }

            void OnEnable() {
                cam.gameObject.SetLayer("Camera Trigger 2D");
            }
            public override void OnInspectorGUI() {
                //base.OnInspectorGUI();
                serializedObject.Update();
                EditorGUI.BeginChangeCheck();

                if (EditorGUI.EndChangeCheck()) {
                    EditorUtility.SetDirty(cam);
                }

                cam._subject = EditorGUILayout.ObjectField("Camera Subject", cam._subject, typeof(Camera2DSubject), true) as Camera2DSubject;
                cam._snapPoint = EditorGUILayout.ObjectField("Snap Transform", cam._snapPoint, typeof(Transform), true) as Transform;
                cam._changeX = EditorGUILayout.Toggle("Change X Policy", cam._changeX);
                if (cam._changeX)
                    cam._flagX.OnInspector(!cam._snapPoint);

                cam._changeY = EditorGUILayout.Toggle("Change Y Policy", cam._changeY);
                if (cam._changeY) {
                    cam._flagY.OnInspector(!cam._snapPoint);
                    cam._flagY.OnInspector2();
                }


                serializedObject.ApplyModifiedProperties();
            }
        }
#endif
    }
}