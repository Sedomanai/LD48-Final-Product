using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGame : MonoBehaviour
{
    void Awake() {
        Game.Instance.finishGame();
    }
}
