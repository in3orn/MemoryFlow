using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dev.Krk.MemoryFlow.Game.Level
{
    public class PathFinder
    {
        private enum Direction
        {
            None,
            Left,
            Right,
            Up,
            Down
        }

        public List<VariantData> FindPaths(int width, int height)
        {
            VariantData data = CreateData(width, height);
            return FindPaths(data, 0, 0, Direction.None);
        }

        private VariantData CreateData(int width, int height)
        {
            VariantData data = new VariantData
            {
                VerticalFields = CreateRows(width + 1, height),
                HorizontalFields = CreateRows(width, height + 1)
            };

            return data;
        }

        private RowData[] CreateRows(int width, int height)
        {
            RowData[] rows = new RowData[height];

            for (int i = 0; i < height; i++)
            {
                rows[i] = new RowData
                {
                    Fields = CreateFields(width)
                };
            }

            return rows;
        }

        private int[] CreateFields(int width)
        {
            int[] fields = new int[width];
            for (int j = 0; j < width; j++)
            {
                fields[j] = 0;
            }
            return fields;
        }

        private List<VariantData> FindPaths(VariantData data, int posX, int posY, Direction direction)
        {
            List<VariantData> result = new List<VariantData>();

            if (IsPathCompleted(data, posX, posY))
            {
                result.Add(data);
            }
            else
            {
                if (CanMoveLeft(data, posX, posY))
                {
                    result.AddRange(MoveLeft(data, posX, posY, direction));
                }
                if (CanMoveUp(data, posX, posY))
                {
                    result.AddRange(MoveUp(data, posX, posY, direction));
                }
                if (CanMoveRight(data, posX, posY))
                {
                    result.AddRange(MoveRight(data, posX, posY, direction));
                }
                if (CanMoveDown(data, posX, posY))
                {
                    result.AddRange(MoveDown(data, posX, posY, direction));
                }
            }

            return result;
        }

        private bool IsPathCompleted(VariantData data, int posX, int posY)
        {
            return posX == data.HorizontalFields[0].Fields.Length && posY == data.VerticalFields.Length;
        }

        private bool CanMoveUp(VariantData data, int posX, int posY)
        {
            return posY > 0 && CanVisit(data, posX, posY - 1);
        }

        private bool CanMoveDown(VariantData data, int posX, int posY)
        {
            return posY < data.VerticalFields.Length && CanVisit(data, posX, posY + 1);
        }

        private bool CanMoveLeft(VariantData data, int posX, int posY)
        {
            return posX > 0 && CanVisit(data, posX - 1, posY);
        }

        private bool CanMoveRight(VariantData data, int posX, int posY)
        {
            return posX < data.HorizontalFields[0].Fields.Length && CanVisit(data, posX + 1, posY);
        }

        private bool CanVisit(VariantData data, int pointX, int pointY)
        {
            if (pointX > 0 && data.HorizontalFields[pointY].Fields[pointX - 1] != 0) return false;
            if (pointX < data.HorizontalFields[0].Fields.Length && data.HorizontalFields[pointY].Fields[pointX] != 0) return false;
            
            if (pointY > 0 && data.VerticalFields[pointY - 1].Fields[pointX] != 0) return false;
            if (pointY < data.VerticalFields.Length && data.VerticalFields[pointY].Fields[pointX] != 0) return false;

            return true;
        }

        private List<VariantData> MoveUp(VariantData data, int posX, int posY, Direction prevDirection)
        {
            VariantData newData = CreateNewData(data, prevDirection, Direction.Up);
            newData.VerticalFields[posY - 1].Fields[posX] = 1;
            return FindPaths(newData, posX, posY - 1, Direction.Up);
        }

        private List<VariantData> MoveDown(VariantData data, int posX, int posY, Direction prevDirection)
        {
            VariantData newData = CreateNewData(data, prevDirection, Direction.Down);
            newData.VerticalFields[posY].Fields[posX] = 1;
            return FindPaths(newData, posX, posY + 1, Direction.Down);
        }

        private List<VariantData> MoveLeft(VariantData data, int posX, int posY, Direction prevDirection)
        {
            VariantData newData = CreateNewData(data, prevDirection, Direction.Left);
            newData.HorizontalFields[posY].Fields[posX - 1] = 1;
            return FindPaths(newData, posX - 1, posY, Direction.Left);
        }

        private List<VariantData> MoveRight(VariantData data, int posX, int posY, Direction prevDirection)
        {
            VariantData newData = CreateNewData(data, prevDirection, Direction.Right);
            newData.HorizontalFields[posY].Fields[posX] = 1;
            return FindPaths(newData, posX + 1, posY, Direction.Right);
        }

        private VariantData CreateNewData(VariantData data, Direction prevDirection, Direction direction)
        {
            VariantData result = data.Clone() as VariantData;
            result.PathLength++;
            if (prevDirection != Direction.None && prevDirection != direction)
            {
                result.NumOfTurns++;
            }
            return result;
        }
    }
}