using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Ilang;


public class ItemReplacer : MonoBehaviour
{
    [System.Serializable]
    public class Unit
    {
        public IlangTile tile;
        public ObjectPool pool;
    } public List<Unit> _units;

    Dictionary<IlangTile, ObjectPool> _dictionary = new Dictionary<IlangTile, ObjectPool>();

    void Awake() {
        for (int i = 0; i < _units.Count; i++) {
            _dictionary.Add(_units[i].tile, _units[i].pool);
        }
    }
    

    public GameObject TranslateItem(IlangTile tile) {
        if (_dictionary.ContainsKey(tile)) {
            return _dictionary[tile].InstantiateFromPool();
        } return null;

    }
}
