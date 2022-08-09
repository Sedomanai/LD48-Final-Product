using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ilang
{

    // Ordered tile. Iterates through list, checks if appropriate sprite exists for the given environment, if it doesn't go to the next one.
    // 순서 적용 타일. 리스트 첫째 타일부터 현재 장소에 알맞는 스프라이트가 존재하지 않으면 다음 타일로 넘어가서 스프라이트 찾기를 반복.
    // 설명이 어려우니 동영상 참조.
#if UNITY_EDITOR
    [CreateAssetMenu(fileName = "Order Tile", menuName = "Ilang Tile/Order Tile", order = 13)]
#endif
    public class OrderTile : IlangTile
    {
        public List<IlangTile> list = new List<IlangTile>();

        public override Sprite GetSprite(Vector3Int location, ITilemap map, TileQuirks quirks) {
            for (int i = 0; i < list.Count; i++) {
                var tile = list[i];
                if (tile) {
                    var spr = tile.GetSprite(location, map, quirks);
                    if (spr) return spr;
                }
            } return null;
        }

#if UNITY_EDITOR
        public override Sprite Thumbnail { get { return (list.Count > 0 && list[0] != null) ? list[0].Thumbnail : null; } }

#endif
    }
}
