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
        public UnityAction OnPlayerMoved;

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

            int sx = fieldMap.HorizontalLength - 1;
            int sy = fieldMap.VerticalLength - 1;

            finish.Init(new Vector2(sx, sy) * Field.SIZE, fieldMap.ShowInterval, fieldMap.HideInterval);
            player.Init(Vector2.zero, fieldMap.ShowInterval, fieldMap.HideInterval);
            playerActualPosition = player.transform.position;

            queuedFields.Clear();
            queuedMoves.Clear();

            initCenter();

            state = StateEnum.Idle;
            fieldMap.ShowPreview();
            showActors();

            if(OnLevelStarted != null) OnLevelStarted();
        }

        public void Clear()
        {
            fieldMap.Clear();
        }

        private void initCenter()
        {
            center.transform.position = new Vector2((fieldMap.HorizontalLength - 1) * Field.SIZE, (fieldMap.VerticalLength - 1) * Field.SIZE) * 0.5f;
        }

        public bool CanMoveLeft()
        {
            return CanMove() && canMoveLeft(player);
        }

        public bool CanMoveRight()
        {
            return CanMove() && canMoveRight(player);
        }

        public bool CanMoveUp()
        {
            return CanMove() && canMoveUp(player);
        }

        public bool CanMoveDown()
        {
            return CanMove() && canMoveDown(player);
        }

        public bool CanMove()
        {
            return state == StateEnum.Showing || state == StateEnum.Playing;
        }

        private bool canMoveLeft(Player player)
        {
            Vector2 position = getFieldPosition(playerActualPosition) + Vector2.left;
            return fieldMap.CanMoveLeft(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private bool canMoveRight(Player player)
        {
            Vector2 position = getFieldPosition(playerActualPosition);
            return fieldMap.CanMoveRight(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private bool canMoveUp(Player player)
        {
            Vector2 position = getFieldPosition(playerActualPosition);
            return fieldMap.CanMoveUp(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private bool canMoveDown(Player player)
        {
            Vector2 position = getFieldPosition(playerActualPosition) + Vector2.down;
            return fieldMap.CanMoveDown(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        public void MoveLeft()
        {
            if (CanMoveLeft())
            {
                move(getLeftField(player), Vector2.left);
            }
        }

        public void MoveRight()
        {
            if (CanMoveRight())
            {
                move(getRightField(player), Vector2.right);
            }
        }

        public void MoveUp()
        {
            if (CanMoveUp())
            {
                move(getUpField(player), Vector2.up);
            }
        }

        public void MoveDown()
        {
            if (CanMoveDown())
            {
                move(getDownField(player), Vector2.down);
            }
        }

        private void move(Field field, Vector2 vector)
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
                if (state == StateEnum.Showing)
                {
                    state = StateEnum.Playing;
                    fieldMap.ShowPlayMode();
                }

                if (field.Valid)
                {
                    field.Unmask();
                    player.Move(vector * Field.SIZE);
                    if (OnPlayerMoved != null) OnPlayerMoved();
                    
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