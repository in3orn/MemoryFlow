using System;

[Serializable]
public class VariantData
{
    public int Id;
	public string Name;

	public RowData[] HorizontalFields;
	public RowData[] VerticalFields;
}