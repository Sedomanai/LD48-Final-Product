using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ilang
{
    public class KinematicEquation : MonoBehaviour
    {
        // 1. s = t * (u + v) / 2
        // 2. v = u + at
        // 3. s = ut + at^2 / 2
        // 4. s = vt - at^2 / 2
        // 5. v^2 = u^2 + 2as

        // s, v, u, a, t
        public static float S_UAT(float u, float a, float t) {
            return u * t + a * Mathf.Pow(t, 2) / 2.0f;
        }
        public static float U_SVA(float s, float v, float a) {
            return Mathf.Pow(Mathf.Pow(v, 2) - 2 * a * s, 0.5f);
        }
        public static float U_SVT(float s, float v, float t) {
            return ((2.0f / s) * t - v);
        }
        public static float U_SAT(float s, float a, float t) {
            return (s / t - a * t / 2.0f);
        }
        public static float V_SUA(float s, float u, float a) {
            return Mathf.Pow(Mathf.Pow(u, 2) + 2 * a * s, 0.5f);
        }
        public static float V_SUT(float s, float u, float t) {
            return (2.0f * s / t - u);
        }
        public static float V_SAT(float s, float a, float t) {
            return (s / t + a * t / 2.0f);
        }
        
    }
}

