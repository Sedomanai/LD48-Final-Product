using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ilang
{
    // Checkered tile out of four tiles.
    // 타일 네 개로 이루어진 체크 모양 타일. 
#if UNITY_EDITOR
    [CreateAssetMenu(fileName = "Check Tile S", menuName = "Ilang Tile/Check Tile S", order = 12)]
#endif
    public class CheckTileS : IlangTile
    {
        [SerializeField]
        IlangTile leftTop;
        [SerializeField]
        IlangTile rightTop;
        [SerializeField]
        IlangTile leftBottom;
        [SerializeField]
        IlangTile rightBottom;

        [SerializeField]
        Vector2Int shift;
        [SerializeField]
        int scale = 1;

        public override void SetTile(Vector3Int location, ITilemap map, ref TileData data) {
            var h = Mathf.Abs((location.x + shift.x - 10000) % (2 * scale)) / scale;
            var v = Mathf.Abs((location.y + shift.y - 10000) % (2 * scale)) / scale;

            IlangTile tile = null;
            if (h == 0) {
                if (v == 0)
                    tile = leftTop;
                else tile = leftBottom;
            } else {
                if (v == 0)
                    tile = rightTop;
                else tile = rightBottom;
            }
            if (tile != null) {
                data.sprite = tile.GetSprite(location, map, quirks);
                data.colliderType = quirks.colliderType;
            }
        }
        public override Sprite GetSprite(Vector3Int location, ITilemap map, TileQuirks quirks) {
            var h = Mathf.Abs((location.x + shift.x - 10000) % (2 * scale)) / scale;
            var v = Mathf.Abs((location.y + shift.y - 10000) % (2 * scale)) / scale;

            IlangTile tile = null;
            if (h == 0) {
                if (v == 0)
                    tile = leftTop;
                else tile = leftBottom;
            } else {
                if (v == 0)
                    tile = rightTop;
                else tile = rightBottom;
            }
            return tile ? tile.GetSprite(location, map, quirks) : null;
        }

#if UNITY_EDITOR
        public override Sprite Thumbnail { get { return (leftTop != null) ? leftTop.Thumbnail : null; } }
#endif
    }
}
