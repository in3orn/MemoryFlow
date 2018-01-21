using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using Dev.Krk.MemoryFlow.Data;
using Dev.Krk.MemoryFlow.Game.Level;

namespace Dev.Krk.MemoryFlow.Editor
{
    public class CreateMapWizard : ScriptableWizard
    {
        public string OutputFile = "Maps.json";

        public int MaxSize = 3;

        public int MaxPoolSize = 5000;

        [MenuItem("MemoryFlow/Create maps")]
        static void CreateWindow()
        {
            ScriptableWizard.DisplayWizard("Create maps", typeof(CreateMapWizard), "Create maps!");
        }

        void OnWizardUpdate()
        {
            helpString = "Creates maps using some complex algorithm.";
            isValid = ValidateInput();
        }

        private bool ValidateInput()
        {
            if (MaxSize < 1)
            {
                errorString = "Max Size must be positive.";
                return false;
            }

            return true;
        }

        void OnWizardCreate()
        {
            MapsData mapsData = new MapsData
            {
                Maps = CreateMaps()
            };

            string path = Path.Combine(Application.streamingAssetsPath, OutputFile);
            string json = JsonUtility.ToJson(mapsData);
            File.WriteAllText(path, json);

            Debug.Log("Created maps: " + mapsData.Maps.Length + " - saved in file: " + OutputFile);
        }

        private MapData[] CreateMaps()
        {
            List<MapData> result = new List<MapData>();
            PathFinder pathFinder = new PathFinder();
            pathFinder.MaxPoolSize = MaxPoolSize;

            int prevCount = 0;

            for (int i = 1; i <= MaxSize; i++)
            {
                Debug.Log("Start process for maps: " + i + "x" + i);
                result.AddRange(pathFinder.FindPaths(i, i));
                Debug.Log("Maps created: " + (result.Count - prevCount));
                prevCount = result.Count;
            }

            return result.ToArray();
        }
    }
}