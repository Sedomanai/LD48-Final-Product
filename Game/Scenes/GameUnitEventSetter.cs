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
    AudioClip _bgm;
    // Start is called before the first frame update
    public void AddGold(int value) {
        Game.Instance.goldInt.AddValue(value);
    }

    public void StartUp() {
        Game.Instance.ChangeState(Game.eState.StartUp);
    }

    public void Overworld() {
        Game.Instance.ChangeState(Game.eState.Overworld);
    }
    public void Playing() {
        Game.Instance.ChangeState(Game.eState.Playing);
        //SoundMgr.Instance.PlayBGM(_bgm);
    }
    public void GameOver() {
        TimeMgr.Instance.PauseGame();
        _mole.DeathEvent();
        Game.Instance.ChangeState(Game.eState.GameOver);
        StartCoroutine(CoGameOver());
    }
    IEnumerator CoGameOver() {
        yield return new WaitForSeconds(1.0f);
        CurtainMgr.Instance.Play("close", () => {
            SceneManager.LoadSceneAsync("Stage Buffer");
        });
    }
}
