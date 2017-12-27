using UnityEngine;
using System.IO;
using System.Collections;

public class JsonFieldMapDataFactory : MonoBehaviour {

	[SerializeField]
	string fileName;

	void Awake() {
		Random.InitState ((int)Time.deltaTime);
	}

	public FieldMapData Create(int levelName) {
		string filePath = Path.Combine (Application.streamingAssetsPath, fileName);
		string json = File.ReadAllText (filePath);

		LevelsData data = JsonUtility.FromJson<LevelsData> (json);

		int r = Mathf.RoundToInt (Random.value * (data.Levels.Length-1));
		LevelData level = data.Levels [r];

		int[,] horizontalFields = rowsToArray (level.HorizontalFields);
		int[,] verticalFields = rowsToArray (level.VerticalFields);

		return new FieldMapData (horizontalFields, verticalFields);
	}

	private int[,] rowsToArray(RowData[] rows) {
		int rowSize = rows.Length;
		int colSize = rows [0].Fields.Length;

		int[,] result = new int[rowSize, colSize];

		for (int i = 0; i < rowSize; i++) {
			RowData row = rows [i];
			for (int j = 0; j < colSize; j++) {
				result [i, j] = row.Fields [j];
			}
		}

		return result;
	}
}
