using UnityEngine;
using System.Collections.Generic;
using Dev.Krk.MemoryFlow.Data;

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

        public int MaxPoolSize { get; set; }

        private int poolSize;

        public List<MapData> FindPaths(int width, int height)
        {
            poolSize = 0;

            MapData data = CreateData(width, height);
            return FindPaths(data, 0, 0, Direction.None);
        }

        private MapData CreateData(int width, int height)
        {
            MapData data = new MapData
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

        private List<MapData> FindPaths(MapData data, int posX, int posY, Direction direction)
        {
            List<MapData> result = new List<MapData>();

            if (poolSize < MaxPoolSize)
            {
                if (IsPathCompleted(data, posX, posY))
                {
                    result.Add(data);
                    poolSize++;
                    if (poolSize % (MaxPoolSize / 10) == 0)
                    {
                        Debug.Log("Work in progress: " + poolSize + "/" + MaxPoolSize);
                    }
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
            }

            return result;
        }

        private bool IsPathCompleted(MapData data, int posX, int posY)
        {
            return posX == data.HorizontalFields[0].Fields.Length && posY == data.VerticalFields.Length;
        }

        private bool CanMoveUp(MapData data, int posX, int posY)
        {
            return posY > 0 && CanVisit(data, posX, posY - 1);
        }

        private bool CanMoveDown(MapData data, int posX, int posY)
        {
            return posY < data.VerticalFields.Length && CanVisit(data, posX, posY + 1);
        }

        private bool CanMoveLeft(MapData data, int posX, int posY)
        {
            return posX > 0 && CanVisit(data, posX - 1, posY);
        }

        private bool CanMoveRight(MapData data, int posX, int posY)
        {
            return posX < data.HorizontalFields[0].Fields.Length && CanVisit(data, posX + 1, posY);
        }

        private bool CanVisit(MapData data, int pointX, int pointY)
        {
            if (pointX > 0 && data.HorizontalFields[pointY].Fields[pointX - 1] != 0) return false;
            if (pointX < data.HorizontalFields[0].Fields.Length && data.HorizontalFields[pointY].Fields[pointX] != 0) return false;
            
            if (pointY > 0 && data.VerticalFields[pointY - 1].Fields[pointX] != 0) return false;
            if (pointY < data.VerticalFields.Length && data.VerticalFields[pointY].Fields[pointX] != 0) return false;

            return true;
        }

        private List<MapData> MoveUp(MapData data, int posX, int posY, Direction prevDirection)
        {
            MapData newData = CreateNewData(data, prevDirection, Direction.Up);
            newData.VerticalFields[posY - 1].Fields[posX] = 1;
            return FindPaths(newData, posX, posY - 1, Direction.Up);
        }

        private List<MapData> MoveDown(MapData data, int posX, int posY, Direction prevDirection)
        {
            MapData newData = CreateNewData(data, prevDirection, Direction.Down);
            newData.VerticalFields[posY].Fields[posX] = 1;
            return FindPaths(newData, posX, posY + 1, Direction.Down);
        }

        private List<MapData> MoveLeft(MapData data, int posX, int posY, Direction prevDirection)
        {
            MapData newData = CreateNewData(data, prevDirection, Direction.Left);
            newData.HorizontalFields[posY].Fields[posX - 1] = 1;
            return FindPaths(newData, posX - 1, posY, Direction.Left);
        }

        private List<MapData> MoveRight(MapData data, int posX, int posY, Direction prevDirection)
        {
            MapData newData = CreateNewData(data, prevDirection, Direction.Right);
            newData.HorizontalFields[posY].Fields[posX] = 1;
            return FindPaths(newData, posX + 1, posY, Direction.Right);
        }

        private MapData CreateNewData(MapData data, Direction prevDirection, Direction direction)
        {
            MapData result = data.Clone() as MapData;
            result.PathLength++;
            if (prevDirection != Direction.None && prevDirection != direction)
            {
                result.NumOfTurns++;
            }
            return result;
        }
    }
}