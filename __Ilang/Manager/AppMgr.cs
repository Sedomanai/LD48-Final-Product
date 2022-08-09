using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Ilang {
    public class AppMgr : Singleton<AppMgr> {
        GameObject _canvas;
        Button[] _buttons;

        new void Awake() {
            base.Awake();
            name = "[AppManager]";

            _canvas = Instantiate(Resources.Load<GameObject>("Prefab/UI/QuitCanvas"), transform) as GameObject;
            _buttons = _canvas.GetComponentsInChildren<Button>();
            _buttons[0].onClick.AddListener(() => { QuitApplication(); });
            _buttons[1].onClick.AddListener(() => { RefuseQuit(); });

            _canvas.gameObject.SetActive(false);
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                _canvas.gameObject.SetActive(true);
                Time.timeScale = 0.0f;
            }
        }

        void QuitApplication() {
            Application.Quit();
        }

        void RefuseQuit() {
            _canvas.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }
}
