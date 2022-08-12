using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ilang { 
    [System.Serializable]
    #if UNITY_EDITOR
    [CreateAssetMenu(fileName = "GameStatsSO", menuName = "LD48/GameStatsSO", order = 11)]
    #endif
    public class GameStatsSO : ScriptableObject
    {
        public Vector2 speed = new Vector2(3.0f, 5.0f);
        public Vector2 speedAnim = new Vector2(0.6f, 1.0f);
        public Vector2 digAnim = new Vector2(0.6f, 1.0f);
        public Vector2Int jumpCount = new Vector2Int(1, 2);
        public Vector2 jumpHeight = new Vector2(2.0f, 3.0f);
        public int resetStoneIntValue = 15;
    }
}