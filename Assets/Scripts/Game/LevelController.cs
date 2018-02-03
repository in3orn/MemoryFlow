using UnityEngine;
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
        }

        void Start()
        {
            player.OnMoved += ProcessNextMove;

            fieldMap.OnShown += startGame;
            fieldMap.OnHidden += FinishGame;

            finish.OnFinished += FinishLevel;
        }

        public void Init(int flow, int level)
        {
            fieldMap.Init(levelProvider.GetMapData(flow, level));

            int sx = fieldMap.HorizontalLength;
            int sy = fieldMap.VerticalLength;

            finish.Init(new Vector2(sx, sy) * Field.SIZE, fieldMap.ShowInterval, fieldMap.HideInterval);
            player.Init(Vector2.zero, fieldMap.ShowInterval, fieldMap.HideInterval);
            playerActualPosition = player.transform.position;

            queuedFields.Clear();
            queuedMoves.Clear();

            InitCenter();

            state = StateEnum.Idle;
            fieldMap.ShowPreview();
            showActors();

            if(OnLevelStarted != null) OnLevelStarted();
        }

        public void Clear()
        {
            fieldMap.Clear();
        }

        private void InitCenter()
        {
            center.transform.position = new Vector2(fieldMap.HorizontalLength * Field.SIZE, fieldMap.VerticalLength * Field.SIZE) * 0.5f;
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
            Vector2 position = getFieldPosition(playerActualPosition) + Vector2.left;
            return fieldMap.CanMoveLeft(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private bool CanMoveRight(Player player)
        {
            Vector2 position = getFieldPosition(playerActualPosition);
            return fieldMap.CanMoveRight(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private bool CanMoveUp(Player player)
        {
            Vector2 position = getFieldPosition(playerActualPosition);
            return fieldMap.CanMoveUp(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private bool CanMoveDown(Player player)
        {
            Vector2 position = getFieldPosition(playerActualPosition) + Vector2.down;
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
            Vector2 position = getFieldPosition(playerActualPosition) + Vector2.left;
            return fieldMap.GetHorizontalField(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private Field getRightField(Player player)
        {
            Vector2 position = getFieldPosition(playerActualPosition);
            return fieldMap.GetHorizontalField(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        public Field getUpField(Player player)
        {
            Vector2 position = getFieldPosition(playerActualPosition);
            return fieldMap.GetVerticalField(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private Field getDownField(Player player)
        {
            Vector2 position = getFieldPosition(playerActualPosition) + Vector2.down;
            return fieldMap.GetVerticalField(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private Vector2 getFieldPosition(Vector2 position)
        {
            return position / Field.SIZE;
        }

        private void startGame()
        {
            if (state == StateEnum.Idle || state == StateEnum.Failed)
            {
                state = StateEnum.Showing;
            }
        }

        public void FinishLevel()
        {
            state = StateEnum.Finished;
            fieldMap.Hide();
            hideActors();

            if(OnLevelCompleted != null) OnLevelCompleted();
        }

        public void FailLevel()
        {
            state = StateEnum.Failed;
            fieldMap.Hide();
            hideActors();

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

        private void showActors()
        {
            player.Show();
            finish.Show();
        }

        private void hideActors()
        {
            player.Hide();
            finish.Hide();
        }
    }
}