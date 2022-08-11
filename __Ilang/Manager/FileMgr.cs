using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Ilang {
    public class FileMgr : Singleton<FileMgr> {
        BinaryFormatter _bf;
        FileStream _file;
        //MemoryStream _ms;
        //bool _loaded;

        public string dataPath { private set; get; }

        // Use this for initialization
        new void Awake() {
            base.Awake();
            name = "[FileManager]";
            _bf = new BinaryFormatter();
            dataPath = Application.persistentDataPath;
        }

        // Update is called once per frame
        public void SaveTo<T>(ref T container, string path) {
            _file = File.Create(path);
            _bf.Serialize(_file, container);
            _file.Close();
        }

        public void LoadTo<T>(ref T container, string path) {
            if (File.Exists(path)) {
                _file = File.Open(path, FileMode.Open);
                container = (T)_bf.Deserialize(_file);
                _file.Close();
            }
        }

        //public void LoadToAndroid<T>(ref T container, string path) {
        //    WWW file = new WWW(path);
        //    while (!file.isDone) {; }
        //    _ms = new MemoryStream(file.bytes);
        //    container = (T)_bf.Deserialize(_ms);
        //    _ms.Close();
        //}

        //IEnumerator LoadToAndroidCoroutine(string path) {
        //    WWW file = new WWW(path);
        //    yield return file;
        //    _ms = new MemoryStream(file.bytes);
        //    _loaded = true;
        //}
    }
}