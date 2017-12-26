using UnityEngine;
using System.Collections;

public class LevelProvider : MonoBehaviour {

	[SerializeField]
	private FieldMapDataFactory fieldMapDataFactory;

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
			return fieldMapDataFactory.CreateRandom ();
		}
	}
}
