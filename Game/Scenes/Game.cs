using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ilang;


// You can change everything from here, you can show a Logo or a loading screen before moving on to the first scene
// The first scene MUST be this Logo scene however. This is because of singleton oddities. (By that I mean never revisit the Logo scene)
// Don't forget to change the name and add your scenes

public class Game : Singleton<Game>
{
    public enum eState
    {
        Logo,
        StartUp,
        Overworld,
        Playing,
        GameOver,
        Ending
    }
    eState _state;

    public int shoeLevel = 0; // 1 = walk faster, 2 = jump higher, 3 = double jump
    public int digLevel = 0; // 1 = dig faster, 2 = dig stone 3 times, 3 = 10 times // 
    public int powerLevel = 0; // 1 = dig stronger, 3 = dig range + 1 // juice

    public eState State { get { return _state; } }


    public GameObject ground, stone, bomb;
    public TextProInt goldInt;
    public TextProInt stoneInt;
    public TextProInt bombInt;

    public TextProChange groundText;
    public int maxDepth = 0;
    public bool worm = false;

    public bool ceremonyEligible = false;
    public bool ceremonyDone = false;
    public bool gameFinished = false;

    public GameStatsSO stats;

    public void ShopMode(bool value) {
        ground.gameObject.SetActive(!value);
        bomb.gameObject.SetActive(!value);
        stone.gameObject.SetActive(!value);
    }

    override protected void Awake() {
        shoeLevel = digLevel = 0;
        base.Awake();
        _state = eState.Logo;
    }

    public void resetStoneInt() {
        stoneInt.SetValue(stats.resetStoneIntValue);
    }

    public void finishGame() {
        ceremonyDone = ceremonyEligible = false;
        gameFinished = true;
    }

    void Start() {
        //debug
        //goldInt.SetValue(1000);
    }

    public void ChangeState(eState state) {
        _state = state;
    }
}
