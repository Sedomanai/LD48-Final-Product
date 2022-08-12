using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ilang;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using TMPro;

public class DigModule : MonoBehaviour
{
    [SerializeField]
    ObjectPool _dirtParticle;
    [SerializeField]
    ObjectPool _stoneParticle;
    [SerializeField]
    ObjectPool _starParticle;

    [SerializeField]
    Tilemap baseMap;

    List<Transform> _digpos;
    Transform _slashpos;

    bool _digging = false;
    public bool Digging { get { return _digging; } }

    public UnityEvent OnDig;
    public UnityEvent OnSlash;
    public UnityEvent OnActualDig;

    int _digDepth = 0;

    void Awake() {
        _digpos = new List<Transform>();
        var children = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform tr in children) {
            if (tr.name == "LB Dig" || tr.name == "RB Dig") {
                _digpos.Add(tr);
            }
            if (tr.name == "R Dig")
                _slashpos = tr;
        }
    }

    public void DigStart() {
        _digging = true;
    }
    public void DigEnd() {
        _digging = false;
    }

    public void Slash() {
        if (_slashpos) {
            HashSet<IlangTile> visited = new HashSet<IlangTile>();
            DigBody(_slashpos, visited, true);
            OnSlash.Invoke();
        }
    }

    public void Dig() {
        HashSet<IlangTile> visited = new HashSet<IlangTile>();
        for (int i = 0; i < _digpos.Count; i++) {
            DigBody(_digpos[i], visited, false);
        }
        OnDig.Invoke();
    }

    void Update() {
        if (!TimeMgr.Instance.Paused && Game.Instance.groundText) {
            _digDepth = -baseMap.WorldToCell(transform.position).y - 3;

            if (_digDepth < 0)
                _digDepth = 0;

            var max = Mathf.Max(Game.Instance.maxDepth, _digDepth);
            if (max <= _digDepth) {
                Game.Instance.groundText.ChangeText(_digDepth.ToString());
                Game.Instance.maxDepth = _digDepth;

            } else {
                Game.Instance.groundText.ChangeText(_digDepth.ToString() + "/" + max.ToString());
            }

            if (_digDepth > 120) {
                Game.Instance.ceremonyEligible = true;
            }
        }
    }

    void DigBody(Transform dig, HashSet<IlangTile> visited, bool slash) {
        var cpos = baseMap.WorldToCell(dig.position);

        Dig(cpos, visited);
        if (Game.Instance.digLevel > 1) {
            if (slash) {
                if (transform.position.x < cpos.x) {
                    Dig(cpos + new Vector3Int(1, 0, 0), visited);
                } else {
                    Dig(cpos + new Vector3Int(-1, 0, 0), visited);
                }
            } else {
                Dig(cpos + new Vector3Int(0, -1, 0), visited);
            }
        }
    }

    void Dig(Vector3Int cpos, HashSet<IlangTile> visited) {
        var wpos = baseMap.CellToWorld(cpos) + new Vector3(0.5f, 0.5f, 0);
        var tile = baseMap.GetTile<IlangTile>(cpos);


        if (visited.Contains(tile))
            return;

        if (tile) {
            bool instantiateStar = false;
            if (tile.quirks.index == 0) {
                baseMap.SetTile(cpos, null);
                var obj = _dirtParticle.InstantiateFromPool();
                baseMap.RefreshTile(cpos);
                obj.transform.position = wpos;
                OnActualDig.Invoke();
            } else if (tile.quirks.index == 1) {
                if (Game.Instance.digLevel > 2 && Game.Instance.stoneInt.Value > 0) {
                    Game.Instance.stoneInt.AddValue(-1);
                    var obj = _stoneParticle.InstantiateFromPool();
                    baseMap.SetTile(cpos, null);
                    baseMap.RefreshTile(cpos);
                    obj.transform.position = wpos;
                    OnActualDig.Invoke();
                } else {
                    instantiateStar = true;
                }
            } else {
                instantiateStar = true;
            }

            if (instantiateStar) {
                var obj = _starParticle.InstantiateFromPool();
                obj.transform.position = wpos;
                obj.transform.localScale = new Vector3(gameObject.transform.localScale.x, 1, 1);
            }
        }
    }
}
