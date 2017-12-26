using UnityEngine;
using System.Collections;

public class FieldMapDataFactory : MonoBehaviour {

	public FieldMapData CreateTutorial1()
	{
		int[,] horizontalFields = new int[2, 1] {
			{ 1 },
			{ 0 }
		};

		int[,] verticalFields = new int[1, 2] {
			{ 0, 1 }
		};

		return new FieldMapData (horizontalFields, verticalFields);
	}

	public FieldMapData CreateTutorial2()
	{
		int[,] horizontalFields = new int[3, 2] {
			{ 1, 0 },
			{ 0, 1 },
			{ 0, 0 }
		};

		int[,] verticalFields = new int[2, 3] {
			{ 0, 1, 0 },
			{ 0, 0, 1 }
		};

		return new FieldMapData (horizontalFields, verticalFields);
	}

	public FieldMapData CreateTutorial3()
	{
		int[,] horizontalFields = new int[3, 2] {
			{ 1, 1 },
			{ 0, 1 },
			{ 0, 1 }
		};

		int[,] verticalFields = new int[2, 3] {
			{ 0, 0, 1 },
			{ 0, 1, 0 }
		};

		return new FieldMapData (horizontalFields, verticalFields);
	}

	public FieldMapData CreateRandom()
	{
		int[,] horizontalFields = new int[4, 3] {
			{ 1, 1, 1 },
			{ 0, 0, 1 },
			{ 0, 0, 1 },
			{ 0, 0, 0 }
		};

		int[,] verticalFields = new int[3, 4] {
			{ 0, 1, 0, 1 },
			{ 0, 0, 1, 0 },
			{ 0, 0, 1, 1 }
		};

		return new FieldMapData (horizontalFields, verticalFields);
	}
}
