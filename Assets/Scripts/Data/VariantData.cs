using System;

[Serializable]
public class VariantData : ICloneable
{
    public int Id;
	public string Name;

    public int PathLength;
    public int NumOfTurns;

    public RowData[] HorizontalFields;
	public RowData[] VerticalFields;

    public object Clone()
    {
        return new VariantData
        {
            Id = this.Id,
            Name = this.Name,
            PathLength = this.PathLength,
            NumOfTurns = this.NumOfTurns,
            HorizontalFields = CloneFields(this.HorizontalFields),
            VerticalFields = CloneFields(this.VerticalFields)
        };
    }

    private RowData[] CloneFields(RowData[] fields)
    {
        RowData[] result = new RowData[fields.Length];
        for(int i = 0; i < fields.Length; i++)
        {
            result[i] = fields[i].Clone() as RowData;
        }
        return result;
    }
}