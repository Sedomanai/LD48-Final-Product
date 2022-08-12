using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Ilang;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapPiece : MonoBehaviour
{
    public const int MapWidth = 16;
    public const int MapHeight = 5;
    public const int FramedMapWidth = MapWidth + 2;
    public const int EntryTypeCount = 8;

    // make an enum somewhere if it's really necessary
    //const int ItemIndex = 3;
    //const int GroundIndex = 0;

    [System.Flags]
    public enum eEntryType {
        None = 0, A = 1, B = 2, C = 4, D = 8, E = 16, F = 32, G = 64, H = 128
    }

    [SerializeField]
    IlangTile _dirtTile;
    IlangTile[] _tiles;

    [SerializeField]
    bool isEnding = false;

    public int index;
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


    Tilemap _mapPiece;
    void Awake() {
        gameObject.SetActive(false);
        _mapPiece = GetComponent<Tilemap>();
        List<IlangTile> tiles = new List<IlangTile>();
        int height = MapHeight + (isEnding ? 16 : 0);
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < FramedMapWidth; j++) {
                tiles.Add(_mapPiece.GetTile<IlangTile>(new Vector3Int(j, -i-1, 0)));
            }
        } _tiles = tiles.ToArray();
    }

    public IlangTile[] Tiles { get { return _tiles; } }

    void FindTileType() {
        _mapPiece = GetComponent<Tilemap>();
        openingType = closingType = 0;

        for (int i = 0; i < EntryTypeCount; i++) {
            if (CheckEntryType(i, -1))
                openingType |= (eEntryType)(1 << i);
            if (CheckEntryType(i, -5))
                closingType |= (eEntryType)(1 << i);
        }
    }

    bool CheckEntryType(int entryType, int yPos) {
        bool[] openings = new bool[MapWidth];

        for (int i = 0; i < MapWidth; i++) {
            openings[i] = false;
            var index = _mapPiece.GetTile<IlangTile>(new Vector3Int(i + 1, yPos, 0)).quirks.index;
            if ((index == 0 || index == 3) && EntryTypes[entryType, i] == 0) {
                openings[i] = true;
            }
        }

        //check if entry type is valid
        bool check = false;
        bool prev = false;
        for (int i = 0; i < MapWidth; i++) {
            var curr = openings[i];
            if (prev && curr) {
                check = true;
                break;
            }
            prev = openings[i];
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
            map.isEnding = EditorGUILayout.Toggle("Is Ending", map.isEnding);
            var o = (eEntryType)EditorGUILayout.EnumFlagsField("Opening Type", map.openingType);
            var c = (eEntryType)EditorGUILayout.EnumFlagsField("Closing Type", map.closingType);
            if (GUILayout.Button("Find Tile Type")) {
                map.FindTileType();
            }

            if (EditorGUI.EndChangeCheck()) {
                EditorUtility.SetDirty(map);
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
#endif
}
