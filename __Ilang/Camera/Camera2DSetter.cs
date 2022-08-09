using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ilang
{
    public class Camera2DSetter : MonoBehaviour{}

#if UNITY_EDITOR

    [CustomEditor(typeof(Camera2DSetter))]
    public class Camera2DSetterEditor : Editor
    {
        void OnEnable() {
            target.CreateLayer("Camera Trigger 2D");
            target.CreateLayer("Camera Subject 2D");
            target.CreateLayer("Camera Box 2D");
            target.CreateLayer("Camera 2D");

            for (int i = 0; i < 32; i++) {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Camera Trigger 2D"), i);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Camera Subject 2D"), i);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Camera Box 2D"), i);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Camera 2D"), i);
            } 
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Camera Trigger 2D"), LayerMask.NameToLayer("Camera Subject 2D"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Camera 2D"), LayerMask.NameToLayer("Camera Box 2D"), false);
        }
    }

#endif
}