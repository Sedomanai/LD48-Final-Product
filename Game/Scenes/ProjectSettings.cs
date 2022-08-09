#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;


// I really don't like configuring my project settings on the editor
// It's not very replicable and hard to manage.
// So I made this script to automatically set up basic necessities as much as I can.

// You only need to refresh this once

namespace Ilang
{
    public class ProjectSettings : MonoBehaviour { }


    [CustomEditor(typeof(ProjectSettings))]
    public class ProjectSettingsEditor : Editor
    {
        void OnEnable() {
            CommonLayers();
            Application.targetFrameRate = 60;
        }

        void CommonLayers() {
            target.CreateLayer("Camera Trigger 2D");
            target.CreateLayer("Camera Subject 2D");
            target.CreateLayer("Camera Box 2D");
            target.CreateLayer("Camera 2D");

            target.CreateLayer("Player");
            target.CreateLayer("Player Static Collider");
            target.CreateLayer("Enemy");
            target.CreateLayer("Enemy Static Collider");
            target.CreateLayer("Wall");
            target.CreateLayer("Projectile");

            target.CreateLayer("Checker");
            target.CreateLayer("Check Listener");

            for (int i = 0; i < 32; i++) {
                for (int j = 0; j <32; j++) {
                    Physics2D.IgnoreLayerCollision(i, j);
                }
            }
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Wall"), LayerMask.NameToLayer("Player"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Wall"), LayerMask.NameToLayer("Enemy"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy Static Collider"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player Static Collider"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Camera Trigger 2D"), LayerMask.NameToLayer("Camera Subject 2D"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Camera 2D"), LayerMask.NameToLayer("Camera Box 2D"), false);

            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Wall"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Enemy"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Player"), false);

            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Checker"), LayerMask.NameToLayer("Check Listener"), false);
        }
    }
}

#endif