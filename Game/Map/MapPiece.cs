using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Ilang;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapPiece : MonoBehaviour
{
    [System.Flags]
    public enum eEntryType {
        None = 0, A = 1, B = 2, C = 4, D = 8, E = 16, F = 32, G = 64, H = 128
    }

    [SerializeField]
    IlangTile _dirtTile;
    IlangTile[] _tiles;

    public eEntryType openingType;
    public eEntryType closingType;

    static int[,] EntryTypes = {
            { 0, 0, 0, 0,  1, 1, 1, 1,  1, 1, 1, 1,  1, 1, 1, 1  },
            { 1, 1, 0, 0,  0, 1, 1, 1,  1, 1, 1, 1,  1, 1, 1, 1  },
            { 1, 1, 1, 1,  0, 0, 0, 0,  1, 1, 1, 1,  1, 1, 1, 1  },
            { 1, 1, 1, 1,  1, 1, 0, 0,  0, 1, 1, 1,  1, 1, 1, 1  },
            { 1, 1, 1, 1,  1, 1, 1, 0,  0, 0, 0, 1,  1, 1, 1, 1  },
            { 1, 1, 1, 1,  1, 1, 1, 1,  1, 0, 0, 0,  1, 1, 1, 1  },
            { 1, 1, 1, 1,  1, 1, 1, 1,  1, 1, 1, 0,  0, 0, 0, 1  },
            { 1, 1, 1, 1,  1, 1, 1, 1,  1, 1, 1, 1,  1, 0, 0, 0  },
        };


    Tilemap _tilemap;
    void Awake() {
        gameObject.SetActive(false);
        _tilemap = GetComponent<Tilemap>();
        List<IlangTile> tiles = new List<IlangTile>();
        for (int i = 0; i < 10; i++) {
            for (int j = 0; j < 18; j++) {
                tiles.Add(_tilemap.GetTile<IlangTile>(new Vector3Int(j, -i-1, 0)));
            }
        } _tiles = tiles.ToArray();
    }

    public IlangTile[] Tiles { get { return _tiles; } }

    void FindTileType() {
        _tilemap = GetComponent<Tilemap>();
        openingType = closingType = 0;

        for (int i = 0; i < 8; i++) {
            if (CheckOpening(i, -1))
                openingType |= (eEntryType)(1 << i);
            if (CheckOpening(i, -5))
                closingType |= (eEntryType)(1 << i);
        }
    }

    bool CheckOpening(int entryType, int yPos) {
        bool[] openings = new bool[16];

        for (int i = 0; i < 16; i++) {
            openings[i] = false;
            if (_tilemap.GetTile<IlangTile>(new Vector3Int(i + 1, yPos, 0)).quirks.index == 0) {
                if (EntryTypes[entryType, i] == 0) {
                    openings[i] = true;
                }
            }
        }

        //check if opening exists
        bool check = false;
        bool prev = false;
        for (int j = 0; j < 16; j++) {
            var curr = openings[j];
            if (prev && curr) {
                check = true;
                break;
            }
            prev = openings[j];
        }

        return check;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MapPiece))]
    public class MapPieceEditor : Editor
    {
        public MapPiece map { get { return target as MapPiece; } }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            var o = (eEntryType)EditorGUILayout.EnumFlagsField("Opening Type", map.openingType);
            var c = (eEntryType)EditorGUILayout.EnumFlagsField("Closing Type", map.closingType);
            if (GUILayout.Button("Find Tile Type")) {
                map.FindTileType();
            }

            if (EditorGUI.EndChangeCheck()) {
                EditorUtility.SetDirty(map);
            }

            serializedObject.ApplyModifiedProperties();

        }
    }
#endif
}
