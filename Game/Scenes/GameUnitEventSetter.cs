using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ilang;

public class GameUnitEventSetter : MonoBehaviour
{
    [SerializeField]
    Mole _mole;
    [SerializeField]
    AudioClip _stageBgm;
    [SerializeField]
    CurtainScript _curtain;

    // Start is called before the first frame update
    public void AddGold(int value) {
        Game.Instance.goldInt.AddValue(value);
    }

    public void StartUp() {
        Game.Instance.ChangeState(Game.eState.StartUp);
        if (_stageBgm)
            SoundMgr.Instance.PlayBGM(_stageBgm);
    }

    public void Overworld() {
        TimeMgr.Instance.UnpauseGame();
        Game.Instance.ChangeState(Game.eState.Overworld);
    }
    public void Playing() {
        Game.Instance.ChangeState(Game.eState.Playing);
    }
    public void GameOver() {
        TimeMgr.Instance.PauseGame();
        Game.Instance.ChangeState(Game.eState.GameOver);
        _mole.DeathEvent();
        StartCoroutine(GameOverCO());
    }
    
    IEnumerator GameOverCO() {
        yield return new WaitForSeconds(1.0f);
        _curtain.FadeOut();
    }

    public void PlayAudioClip(AudioClip clip) {
        SoundMgr.Instance.PlaySFX(clip);
    }
}
