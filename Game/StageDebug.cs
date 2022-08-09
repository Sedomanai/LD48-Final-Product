using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ilang
{
    public class StageDebug : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        Mole _mole;
        [SerializeField]
        int _goldDebug;

        [SerializeField]
        CeilingDeath _death;

        void Awake() {
            if (_goldDebug > 0) {
                Game.Instance.goldInt.AddValue(_goldDebug);
            }
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.P)) { 
                Game.Instance.ChangeState(Game.eState.Playing);
                _death.OnCeilingDeath.Invoke();
            }
        }
#endif
    }
}
