using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Ilang;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    Tilemap baseMap;
    [SerializeField]
    ObjectPool _dirtParticle;
    [SerializeField]
    ObjectPool _stoneParticle;
    [SerializeField]
    ObjectPool _starParticle;

    public void Boom()
    {
        var cpos = baseMap.WorldToCell(transform.position);
        for (int i = -2; i < 3; i++) {
            for (int j = -2; j < 3; j++) {
                var ai = Mathf.Abs(i);
                if (ai == 2 && ai == Mathf.Abs(j))
                    continue;
                BlowUp(cpos + new Vector3Int(i, j, 0));
            }
        }
    }

    void OnEnable() {
        GetComponent<Rigidbody2D>().isKinematic = false;
    }

    public void Disable() {
        gameObject.SetActive(false);
    }

    void BlowUp(Vector3Int cpos) {
        var wpos = baseMap.CellToWorld(cpos) + new Vector3(0.5f, 0.5f, 0);
        var tile = baseMap.GetTile<IlangTile>(cpos);


        if (tile) {
            if (tile.quirks.index == 2) {
                var obj = _starParticle.InstantiateFromPool();
                obj.transform.position = wpos;
                obj.transform.localScale = new Vector3(gameObject.transform.localScale.x, 1, 1);
            } else {
                GameObject obj = null;
                if (tile.quirks.index == 0) {
                    obj = _dirtParticle.InstantiateFromPool();
                } else if (tile.quirks.index == 1) {
                    obj = _stoneParticle.InstantiateFromPool();
                }
                obj.transform.position = wpos;
                baseMap.SetTile(cpos, null);
                baseMap.RefreshTile(cpos);
            }
        }

        GetComponent<Rigidbody2D>().isKinematic = true;
    }
}
