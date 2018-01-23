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

        public Vector2[] MapSizes = { Vector2.one, Vector2.one * 2, Vector2.one * 3, new Vector2(3, 4), Vector2.one * 4, new Vector2(4, 5), Vector2.one * 5, new Vector2(5, 6), Vector2.one * 6 };

        public int MaxPoolSize = 10000;

        public float MinLengthToTurnsRatio = 1.0f;
        public float MaxLengthToTurnsRatio = 1.75f;

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
            if (MaxPoolSize < 1)
            {
                errorString = "Max Size must be positive.";
                return false;
            }

            if (MinLengthToTurnsRatio < 1)
            {
                errorString = "Min Length To Turn Ratio must be 1+.";
                return false;
            }

            if (MaxLengthToTurnsRatio < MinLengthToTurnsRatio)
            {
                errorString = "Max Length To Turn Ratio must be bigger than Min Length To Turns Ratio.";
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
            pathFinder.MinLengthToTurnsRatio = MinLengthToTurnsRatio;
            pathFinder.MaxLengthToTurnsRatio = MaxLengthToTurnsRatio;

            int prevCount = 0;

            foreach (Vector2 mapSize in MapSizes)
            {
                Debug.Log("Start process for maps: " + (int)mapSize.x + "x" + (int)mapSize.y);
                result.AddRange(pathFinder.FindPaths((int)mapSize.x, (int)mapSize.y));
                Debug.Log("Maps created: " + (result.Count - prevCount));
                prevCount = result.Count;
            }

            return result.ToArray();
        }
    }
}