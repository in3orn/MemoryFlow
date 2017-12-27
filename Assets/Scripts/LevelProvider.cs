using UnityEngine;
using System.Collections;

public class LevelProvider : MonoBehaviour {

	[SerializeField]
	private FieldMapDataFactory fieldMapDataFactory;

	[SerializeField]
	private JsonFieldMapDataFactory jsonFieldMapDataFactory;

	public FieldMapData GetMapData(int level) {
		switch (level)
		{
		case 0:
			return fieldMapDataFactory.CreateTutorial1();
		case 1:
			return fieldMapDataFactory.CreateTutorial2 ();
		case 2:
			return fieldMapDataFactory.CreateTutorial3 ();
		default:
			return getRandomData ();
		}
	}

	private FieldMapData getRandomData() {
		FieldMapData data = jsonFieldMapDataFactory.Create (0);

		if (data.HorizontalFields.GetLength (0) == data.VerticalFields.GetLength (1)) {
			float r = Random.value;
			if (r < 0.25f) {
				data.ReflectByDiagonal ();
			}
			else if (r < 0.5f) {
				data.ReflectByContrdiagonal ();
			}
			else if (r < 0.75) {
				data.ReflectByCenter ();
			}
		}

		return data;
	}
}
