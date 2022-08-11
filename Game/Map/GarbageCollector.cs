using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using Ilang;

public class GarbageCollector : MonoBehaviour
{
    [SerializeField]
    Tilemap _map;

    Camera2D _cam;

    void Awake() {
        _cam = Camera.main.GetComponent<Camera2D>();

        _map.WorldToCell(new Vector3());
    }

    void Update() {
        float beginPos = -1 * (MapPiece.FramedMapWidth / 2.0f - 0.5f);
        var pos = transform.position - new Vector3(beginPos, 0.0f, 0.0f);

        for (int i = 0; i < MapPiece.FramedMapWidth; i++) {
            _map.SetTile(_map.WorldToCell(pos), null);
            pos += Vector3.left;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        var target = collision.gameObject;
        if (target.GetComponent<Projectile2D>()) {
            target.SetActive(false);
        }
    }
    void LateUpdate() {
        var pos = transform.position;
        pos.y = _cam.transform.position.y + _cam.Size.y / 2.0f + 2;
        transform.position = pos;
    }
}
