using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace Ilang {
    //public class ScoutMgr : Singleton<ScoutMgr> {
    //    Queue<Unit> queue = new Queue<Unit>();
    //    IScouter _scouter;
    //    bool _isAlive;
        
    //    // Use this for initialization
    //    new void Awake() {
    //        base.Awake();
    //        name = "[Scout Manager]";
    //        DOTween.defaultEaseType = Ease.Linear;

    //        _isAlive = true;
    //        //_scouter = new Breadthfirst();
    //        _scouter = new Dijkstra();

    //        Thread thread = new Thread(Scout);
    //        thread.IsBackground = true;
    //        thread.Start();
    //    }

    //    public void Queue(Unit unit) {
    //        queue.Enqueue(unit);
    //    }

    //    void Scout() {
    //        while (_isAlive) {
    //            if (queue.Count > 0) {
    //                Unit unit = queue.Dequeue();
    //                unit.path = _scouter.Scout(unit);
    //                unit.OnScoutComplete();
    //            }
    //        }
    //    }

    //    void OnDisable() {
    //        _isAlive = false;
    //    }
    //}
}