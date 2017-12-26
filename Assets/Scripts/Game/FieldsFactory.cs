using UnityEngine;

public class FieldsFactory : MonoBehaviour
{
    [SerializeField]
    private GameObject pattern;

    [SerializeField]
    private Level level;

    public Field[,] Create(int[,] fieldTypes, Field.TypeEnum fieldType)
    {
        Field[,] fields = new Field[fieldTypes.GetLength(0), fieldTypes.GetLength(1)];

        for (int y = 0; y < fieldTypes.GetLength(0); y++)
        {
            for (int x = 0; x < fieldTypes.GetLength(1); x++)
            {
                GameObject instance = Instantiate(pattern, level.gameObject.transform) as GameObject;
                Field field = instance.GetComponent<Field>();
				Vector2 position = new Vector2 (x * Field.SIZE, y * Field.SIZE);
				if (fieldType == Field.TypeEnum.Horizontal) {
					position += Vector2.right * Field.SIZE / 2;
				} else {
					position += Vector2.up * Field.SIZE / 2;
					field.transform.Rotate (0f, 0f, 90f);
				}
                field.Init(position, fieldTypes[y, x] > 0);
                    
                fields[y, x] = field;
            }
        }

        return fields;
    }
}
