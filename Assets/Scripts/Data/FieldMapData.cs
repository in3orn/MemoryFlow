using System;

public struct FieldMapData
{
	private int[,] horizontalFields;
	private int[,] verticalFields;

	public FieldMapData(int[,] horizontalFields, int[,] verticalFields) {
		this.horizontalFields = horizontalFields;
		this.verticalFields = verticalFields;
	}

	public int[,] HorizontalFields { get { return horizontalFields; } }
	public int[,] VerticalFields { get { return verticalFields; } }
}