using UnityEngine;
using System.Collections.Generic;

/*
 * 오브젝트 풀 (Object Pooling) 디자인 참조
 * 
 */

namespace Ilang
{
    public class ObjectPool : MonoBehaviour {
        
        [SerializeField]
        GameObject _object; //지정 오브젝트
        [SerializeField]
        GameObject _container;

        List<GameObject> _pool = new List<GameObject>();

        /*
         * Get : Instantiate(오브젝트) 대용
         * 
         * 풀에 비활성된 오브젝트가 있으면
         * 그 오브젝트를 활성화/초기화 시킨 후 재사용하고
         * 없으면 새로 오브젝트를 인스턴스화 해서 풀에 넣는다
         * 
         */
        public GameObject InstantiateFromPool() {
            for (int i = 0; i < _pool.Count; i++) {
                var obj = _pool[i];
                if (!obj.activeSelf) {
                    ResetTransform(obj.transform);
                    obj.SetActive(true);
                    return obj;
                }
            } return CreateObject();
        }

        public void InstantiateFromPoolEvent() {
            for (int i = 0; i < _pool.Count; i++) {
                var obj = _pool[i];
                if (!obj.activeSelf) {
                    ResetTransform(obj.transform);
                    obj.SetActive(true);
                    return;
                }
            }

            CreateObject();
        }

        GameObject CreateObject() {
            GameObject obj = Instantiate(_object);
            obj.SetActive(true);
            _pool.Add(obj);
            ResetTransform(obj.transform);
            return obj;
        }

        void ResetTransform(Transform tr) {
            tr.position = transform.position;
            tr.eulerAngles = transform.eulerAngles;
            if (_container)
                tr.SetParent(_container.transform);
        }

        public ObjectPool SetObject(GameObject obj) {
            _object = obj;
            _pool.Clear();
            return this;
        }

        public void ClearPool() {
            _pool.Clear();
        }
    }
}