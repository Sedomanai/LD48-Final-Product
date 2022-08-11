using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ilang
{
    public class TempSound : MonoBehaviour
    {

        [SerializeField]
        AudioClip _clip;
        // Start is called before the first frame update
        void Start() {
            SoundMgr.Instance.PlayBGM(_clip);
        }

        // Update is called once per frame
        void Update() {

        }
    }

}
