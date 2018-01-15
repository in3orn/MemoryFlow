using UnityEngine;
using System.IO;
using System.Collections;
using Dev.Krk.MemoryFlow.Data.Controller;

public class JsonFieldMapDataFactory : MonoBehaviour
{
    [SerializeField]
    private LevelsDataController levelsDataController;

    public FieldMapData Create(int level)
    {
        LevelData levelData = levelsDataController.Data.Levels[level];

        int variant = Mathf.FloorToInt(Random.value * (levelData.Variants.Length - 0.01F));

        VariantData variantData = levelData.Variants[variant];

        int[,] horizontalFields = RowsToArray(variantData.HorizontalFields);
        int[,] verticalFields = RowsToArray(variantData.VerticalFields);

        return new FieldMapData(horizontalFields, verticalFields);
    }

    private int[,] RowsToArray(RowData[] rows)
    {
        int rowSize = rows.Length;
        int colSize = rows[0].Fields.Length;

        int[,] result = new int[rowSize, colSize];

        for (int i = 0; i < rowSize; i++)
        {
            RowData row = rows[i];
            for (int j = 0; j < colSize; j++)
            {
                result[i, j] = row.Fields[j];
            }
        }

        return result;
    }
}
