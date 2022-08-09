using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Ilang;


[RequireComponent(typeof(Rigidbody2D))]
public class ScrollTileMap : MonoBehaviour
{
    [SerializeField]
    float _scrollRate = 20.0f;

    [SerializeField]
    Camera2D _cam;
    [SerializeField]
    Transform _molePos;
    [SerializeField]
    GameObject _mapPieces;
    [SerializeField]
    MapPiece _ceremonyMapPiece;

    Vector3Int _writer;
    Vector3 _writerWorld;

    Dictionary<int, List<MapPiece>> _mapDict = new Dictionary<int, List<MapPiece>>();


    MapPiece _prevPiece = null;
    MapPiece _nextPiece = null;

    float rate = 0.0f;


    public float ScrollRate { set { _scrollRate = value; } }
    Tilemap _tilemap;
    Rigidbody2D _body;
    ItemReplacer _itemr;

    // Start is called before the first frame update
    void Awake() {
        _tilemap = GetComponentInChildren<Tilemap>();
        _body = GetComponent<Rigidbody2D>();
        _itemr = GetComponent<ItemReplacer>();

        _writer = new Vector3Int(-9, -13, 0);

        for (int i = 0; i < 8; i++) {
            int d = 1 << i;
            _mapDict.Add(d, new List<MapPiece>());
        }

        var pieces = _mapPieces.GetComponentsInChildren<MapPiece>(true);
        foreach (var piece in pieces) {
            for (int i = 0; i < 8; i++) {
                int d = (1 << i);
                if (piece.openingType.HasFlag((MapPiece.eEntryType)(d))) {
                    _mapDict[d].Add(piece);
                }
            }
        }
    }

    void Start() {
        ReplaceItem();
    }

    void ReplaceItem() {
        for (int i = -1; i > -12; i--) {
            for (int j = -10; j < 10; j++) {
                var cpos = new Vector3Int(j, i, 0);
                var tile = _tilemap.GetTile<IlangTile>(cpos);
                if (tile && tile.quirks.index == 3) {
                    _tilemap.SetTile(cpos, null);
                    _itemr.TranslateItem(tile).transform.position = _tilemap.CellToWorld(cpos) + new Vector3(0.5f, 0.5f, 0);
                }
            }
        }
    }

    bool IsReadyToFill() {
        return ((_molePos.position.y - _writerWorld.y) < (_cam.Size.y / 2.0f + 2));
    }

    void FillNext() {
        if (Game.Instance.ceremonyEligible  && !Game.Instance.ceremonyDone) {
            _prevPiece = _ceremonyMapPiece;
            Game.Instance.ceremonyDone = true;
        } else {
            List<MapPiece> list = null;
            if (!_prevPiece) {
                int d = 1 << Random.Range(0, 8);
                list = _mapDict[d];
            } else {
                List<int> rr = new List<int>();
                for (int i = 0; i < 8; i++) {
                    int d = i << i;
                    if (_prevPiece.closingType.HasFlag((MapPiece.eEntryType)(d))) {
                        rr.Add(d);
                    }
                }
                var key = rr[Random.Range(0, rr.Count)];
                if (_mapDict.ContainsKey(key))
                    list = _mapDict[key];
            }
            if (list != null) {
                _prevPiece = list[Random.Range(0, list.Count)];
            }
        }


        var tiles = _prevPiece.Tiles;
        for (int i = 0; i < tiles.Length; i++) {
            var tile = tiles[i];
            var cpos = _writer + new Vector3Int(i % 18, -i / 18, 0);
            if (tile && tile.quirks.index == 3) {
                _itemr.TranslateItem(tile).transform.position = _tilemap.CellToWorld(cpos) + new Vector3(0.5f, 0.5f, 0);
            } else {
                _tilemap.SetTile(cpos, tiles[i]);
            }
        }
        _writer += new Vector3Int(0, -5, 0);
    }

    public void ResetRate() {
        rate = _scrollRate;
    }

    // Update is called once per frame
    void Update() {
        _writerWorld = _tilemap.CellToWorld(_writer) + new Vector3(0.0f, 1.0f, 0.0f);
        if (!TimeMgr.Instance.Paused && IsReadyToFill()) {
            FillNext();
        }

        _body.velocity = new Vector2(0.0f, 0.0f);
        if (Game.Instance.State == Game.eState.Playing) {
            if (rate < _scrollRate)
                rate = _scrollRate;
            rate += (Game.Instance.worm ? 0.033333f : 0.06666f);
            _body.velocity = new Vector2(0, rate * Time.deltaTime * TimeMgr.Instance.TimeScale);
        }
    }
}
