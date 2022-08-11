using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Ilang;


[RequireComponent(typeof(Rigidbody2D))]
public class ScrollTilemap : MonoBehaviour
{
    [SerializeField]
    float _scrollRate = 20.0f;
    [SerializeField]
    GameObject _mapPieces;
    [SerializeField]
    MapPiece _ceremonyMapPiece;

    List<List<MapPiece>> _mapPieceByOpenings;

    //Dictionary<int, List<MapPiece>> _mapPieceByOpenings = new Dictionary<int, List<MapPiece>>();
    Transform _molePos;
    Camera2D _cam;
    Vector3Int _writer;
    MapPiece _prevPiece = null;
    MapPiece _currPiece = null;
    float rate = 0.0f;
    Tilemap _tilemap;
    Rigidbody2D _body;
    ItemReplacer _itemr;

    void Awake() {
        _cam = Camera.main.GetComponent<Camera2D>();
        _tilemap = GetComponentInChildren<Tilemap>();
        _body = GetComponent<Rigidbody2D>();
        _itemr = GetComponent<ItemReplacer>();
        _molePos = GetComponentInChildren<Mole>().transform;
        _writer = new Vector3Int(-9, -13, 0);
        _mapPieceByOpenings = new List<List<MapPiece>>();

        for (int i = 0; i < MapPiece.EntryTypeCount; i++) {
            _mapPieceByOpenings.Add(new List<MapPiece>());
        }

        var pieces = _mapPieces.GetComponentsInChildren<MapPiece>(true);
        for (int i = 0; i < pieces.Length; i++) {
            var piece = pieces[i];
            piece.index = i;
            for (int j = 0; j < MapPiece.EntryTypeCount; j++) {
                if (piece.openingType.HasFlag((MapPiece.eEntryType)(1 << j))) {
                    _mapPieceByOpenings[j].Add(piece);
                }
            }
        }
    }

    /**************** Enable this code when there are items in starting position. So far there is none.
    void Start() {
        //ReplaceItem();
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
    /**/

    void FillNext() {
        _currPiece = null;

        if (Game.Instance.ceremonyEligible  && !Game.Instance.ceremonyDone) {
            _currPiece = _ceremonyMapPiece;
            Game.Instance.ceremonyDone = true;
        } else {
            List<MapPiece> eligible = null;

            if (!_prevPiece) { // in the beginning
                int entry = Random.Range(0, MapPiece.EntryTypeCount);
                eligible = _mapPieceByOpenings[entry];
            } else {
                List<int> entries = new List<int>();
                for (int i = 0; i < MapPiece.EntryTypeCount; i++) {
                    if (_prevPiece.closingType.HasFlag((MapPiece.eEntryType)(1 << i))) {
                        entries.Add(i);
                    }
                }

                var key = entries[Random.Range(0, entries.Count)];
                if (_mapPieceByOpenings.Count > key) // double check
                    eligible = _mapPieceByOpenings[key];
            }

            if (eligible != null) { // double check, null should not happen
                while (true) {
                    int select = Random.Range(0, eligible.Count);
                    _currPiece = eligible[select];
                    if (eligible.Count > 1 && _currPiece == _prevPiece) {
                        eligible.RemoveAt(select);
                    } else 
                        break;
                }
            }
        }

        FillNextTiles();
        _prevPiece = _currPiece;
    }

    void FillNextTiles() {
        var tiles = _currPiece.Tiles;
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

    bool IsReadyToFill(float writerWorldY) {
        return ((_molePos.position.y - writerWorldY) < (_cam.Size.y / 2.0f + 2));
    }

    void Update() {
        float writerWorldY = _tilemap.CellToWorld(_writer).y + 1.0f;  //+ new Vector3(0.0f, 1.0f, 0.0f);
        if (!TimeMgr.Instance.Paused && IsReadyToFill(writerWorldY)) {
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
