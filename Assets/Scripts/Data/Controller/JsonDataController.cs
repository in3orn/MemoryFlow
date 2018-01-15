using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace Dev.Krk.MemoryFlow.Data.Controller
{
    public class JsonDataController<DataType> : MonoBehaviour
    {
        public UnityAction OnInitialized;

        [SerializeField]
        private string fileName = "*.json";

        private DataType data;

        private bool initialized;

        public DataType Data { get { return data; } }

        public bool Initialized { get { return initialized; } }

        void Start()
        {
            StartCoroutine(InitData());
        }

        private IEnumerator InitData()
        {
            WWW www = LoadFile(fileName);
            yield return www;

            data = JsonUtility.FromJson<DataType>(www.text);

            initialized = true;
            if (OnInitialized != null) OnInitialized();
        }

        private WWW LoadFile(string fileName)
        {
            string filePath = GetFilePath(fileName);
            return new WWW(filePath);
        }

        private string GetFilePath(string fileName)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return "jar:file://" + Application.dataPath + "!/assets/" + fileName;
            }
            else
            {
                return "file://" + Path.Combine(Application.streamingAssetsPath, fileName);
            }
        }
    }
}
