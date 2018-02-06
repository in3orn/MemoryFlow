﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Dev.Krk.MemoryFlow.Game.Level;

//TODO refactor - to much responsibilities
namespace Dev.Krk.MemoryFlow.Game
{
    public class LevelController : MonoBehaviour
    {
        public UnityAction OnPlayerFailed;
        public UnityAction<Vector2> OnPlayerMoved;

        public UnityAction OnLevelStarted;
        public UnityAction OnLevelEnded;

        public UnityAction OnLevelCompleted;
        public UnityAction OnLevelFailed;

        public enum StateEnum
        {
            Idle = 0,
            Showing,
            Playing,
            Finished,
            Failed
        }

        private StateEnum state;

        [SerializeField]
        private MapDataProvider levelProvider;

        [SerializeField]
        private FieldMap fieldMap;

        [SerializeField]
        private Player player;

        [SerializeField]
        private Finishable finish;

        [SerializeField]
        private GameObject center;

        [SerializeField]
        private float finishDuration = 1f;

        private Vector2 playerActualPosition;

        private Queue<Vector2> queuedMoves;

        private Queue<Field> queuedFields;

        private Vector2 offset;

        private List<Field> oldFields;

        public int HorizontalLength
        {
            get { return fieldMap.HorizontalLength; }
        }

        public int VerticalLength
        {
            get { return fieldMap.VerticalLength; }
        }

        public StateEnum State { get { return state; } }

        void Awake()
        {
            queuedMoves = new Queue<Vector2>(5);
            queuedFields = new Queue<Field>(5);

            oldFields = new List<Field>();
        }

        void Start()
        {
            player.OnMoved += ProcessNextMove;

            fieldMap.OnShown += StartGame;
            fieldMap.OnHidden += FinishGame;

            finish.OnFinished += FinishLevel;
        }

        public void Init(int flow, int level)
        {
            if (level == 0)
            {
                player.Init(Vector2.zero, fieldMap.ShowInterval, fieldMap.HideInterval);
                playerActualPosition = player.transform.position;
                offset = Vector2.zero;
            }

            fieldMap.Init(levelProvider.GetMapData(flow, level), offset);

            Vector2 mapEnd = offset + new Vector2(fieldMap.HorizontalLength, fieldMap.VerticalLength);
            finish.Init(mapEnd * Field.SIZE, fieldMap.ShowInterval, fieldMap.HideInterval);

            queuedFields.Clear();
            queuedMoves.Clear();

            InitCenter();

            state = StateEnum.Idle;
            fieldMap.ShowPreview();
            ShowActors();

            if (OnLevelStarted != null) OnLevelStarted();
        }

        public void Reset()
        {
            Clear();
            ResetOldFields();
        }

        private void ResetOldFields()
        {
            foreach (Field field in oldFields)
            {
                Destroy(field.gameObject);
            }
            oldFields.Clear();
        }

        public void Clear()
        {
            if (fieldMap.HorizontalFields != null)
            {
                offset.x += fieldMap.HorizontalLength;
                offset.y += fieldMap.VerticalLength;

                MoveToOld(fieldMap.HorizontalFields);
                MoveToOld(fieldMap.VerticalFields);
                fieldMap.Clear();
            }
        }

        private void MoveToOld(Field[,] fields)
        {
            for (int y = 0; y < fields.GetLength(0); y++)
            {
                for (int x = 0; x < fields.GetLength(1); x++)
                {
                    oldFields.Add(fields[y, x]);
                }
            }
        }

        private void InitCenter()
        {
            center.transform.position = (offset + new Vector2(fieldMap.HorizontalLength, fieldMap.VerticalLength) * 0.5f) * Field.SIZE;
        }

        public bool CanMoveLeft()
        {
            return CanMove() && CanMoveLeft(player);
        }

        public bool CanMoveRight()
        {
            return CanMove() && CanMoveRight(player);
        }

        public bool CanMoveUp()
        {
            return CanMove() && CanMoveUp(player);
        }

        public bool CanMoveDown()
        {
            return CanMove() && CanMoveDown(player);
        }

        public bool CanMove()
        {
            return state == StateEnum.Showing || state == StateEnum.Playing;
        }

        private bool CanMoveLeft(Player player)
        {
            Vector2 position = GetFieldPosition(playerActualPosition) + Vector2.left;
            return fieldMap.CanMoveLeft(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private bool CanMoveRight(Player player)
        {
            Vector2 position = GetFieldPosition(playerActualPosition);
            return fieldMap.CanMoveRight(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private bool CanMoveUp(Player player)
        {
            Vector2 position = GetFieldPosition(playerActualPosition);
            return fieldMap.CanMoveUp(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private bool CanMoveDown(Player player)
        {
            Vector2 position = GetFieldPosition(playerActualPosition) + Vector2.down;
            return fieldMap.CanMoveDown(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        public void MoveLeft()
        {
            if (CanMoveLeft())
            {
                Move(getLeftField(player), Vector2.left);
            }
        }

        public void MoveRight()
        {
            if (CanMoveRight())
            {
                Move(getRightField(player), Vector2.right);
            }
        }

        public void MoveUp()
        {
            if (CanMoveUp())
            {
                Move(getUpField(player), Vector2.up);
            }
        }

        public void MoveDown()
        {
            if (CanMoveDown())
            {
                Move(getDownField(player), Vector2.down);
            }
        }

        private void Move(Field field, Vector2 vector)
        {
            playerActualPosition += vector * Field.SIZE;

            if (queuedFields.Count == 0 && player.CanMove())
            {
                PerformMove(field, vector);
            }
            else
            {
                queuedFields.Enqueue(field);
                queuedMoves.Enqueue(vector);
            }
        }

        private void PerformMove(Field field, Vector2 vector)
        {
            if ((state == StateEnum.Showing || state == StateEnum.Playing))
            {
                if (field.Valid)
                {
                    field.Visit(player.transform.position);
                    player.Move(vector * Field.SIZE);

                    if (state == StateEnum.Showing)
                    {
                        state = StateEnum.Playing;
                        fieldMap.ShowPlayMode();
                    }

                    if (OnPlayerMoved != null) OnPlayerMoved(vector);
                }
                else
                {
                    field.Break();
                    playerActualPosition = player.transform.position;
                    queuedFields.Clear();
                    queuedMoves.Clear();
                    if (OnPlayerFailed != null) OnPlayerFailed();
                }
            }
        }

        private void ProcessNextMove()
        {
            if (queuedMoves.Count > 0)
            {
                PerformMove(queuedFields.Dequeue(), queuedMoves.Dequeue());
            }
        }

        private Field getLeftField(Player player)
        {
            Vector2 position = GetFieldPosition(playerActualPosition) + Vector2.left;
            return fieldMap.GetHorizontalField(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private Field getRightField(Player player)
        {
            Vector2 position = GetFieldPosition(playerActualPosition);
            return fieldMap.GetHorizontalField(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        public Field getUpField(Player player)
        {
            Vector2 position = GetFieldPosition(playerActualPosition);
            return fieldMap.GetVerticalField(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private Field getDownField(Player player)
        {
            Vector2 position = GetFieldPosition(playerActualPosition) + Vector2.down;
            return fieldMap.GetVerticalField(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private Vector2 GetFieldPosition(Vector2 position)
        {
            return position / Field.SIZE - offset;
        }

        private void StartGame()
        {
            if (state == StateEnum.Idle || state == StateEnum.Failed)
            {
                state = StateEnum.Showing;
            }
        }

        public void FinishLevel()
        {
            state = StateEnum.Finished;
            fieldMap.HideInvalid();
            HideActors();

            if (OnLevelCompleted != null) OnLevelCompleted();
        }

        public void FailLevel()
        {
            state = StateEnum.Failed;
            fieldMap.Hide();
            HideActors();

            if (OnLevelFailed != null) OnLevelFailed();
        }

        public void FinishGame()
        {
            StartCoroutine(FinishGameInternal());
        }

        private IEnumerator FinishGameInternal()
        {
            yield return new WaitForSeconds(finishDuration);
            if (OnLevelEnded != null) OnLevelEnded();
        }

        private void ShowActors()
        {
            player.Show();
            finish.Show();
        }

        private void HideActors()
        {
            player.Hide();
            finish.Hide();
        }
    }
}