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
			return jsonFieldMapDataFactory.Create (0);
		}
	}
}
