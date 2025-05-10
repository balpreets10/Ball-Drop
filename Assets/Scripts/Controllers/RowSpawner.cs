using BallDrop.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BallDrop
{
    public class RowSpawner : MonoBehaviour
    {
        private int CurrentRowCounter = 0;
        private bool GameEnded;
        private List<IRow> ActiveRows = new List<IRow>();
        public static GameObject CurrentRow;

        IRowData rowDataObject;
        Dictionary<float, CubeType> RowData;
        WaitForSeconds WaitBetweenRowSpawn = new WaitForSeconds(0.05f);

        private void OnEnable()
        {
            MyEventManager.SpawnRow.AddListener(SpawnRow);
            MyEventManager.RemoveRow.AddListener(RemoveRow);
            MyEventManager.SetPlayerPosAfterRevival.AddListener(SetBallNewPosition);
            MyEventManager.EndGame.AddListener(EndGame);
        }

        private void OnDisable()
        {
                MyEventManager.SpawnRow.RemoveListener(SpawnRow);
                MyEventManager.RemoveRow.RemoveListener(RemoveRow);
                MyEventManager.SetPlayerPosAfterRevival.RemoveListener(SetBallNewPosition);
                MyEventManager.EndGame.RemoveListener(EndGame);
        }

        private IEnumerator Start()
        {
            CurrentRow = null;
            if (GameData.Instance.gameMode == GameMode.Classic)
            {
                rowDataObject = GetComponent<ClassicRowData>();
            }
            else
            {
                rowDataObject = GetComponent<ArcadeRowData>();
            }
            yield return new WaitForEndOfFrame();
            StartSpawningRows();
        }

        private void StartSpawningRows()
        {
            GetNextRowData();
            StartCoroutine(SpawnRows(4));
        }

        public void GetNextRowData()
        {
            RowData = rowDataObject.GetRowData(CurrentRowCounter, GameData.Instance.levelData.Level);
        }


        //Spawn Rows Initially at the start of Level
        private IEnumerator SpawnRows(int rowsToSpawn)
        {
            for (int i = 0; i < rowsToSpawn; i++)
            {
                SpawnRow();
                yield return WaitBetweenRowSpawn;
            }
            MyEventManager.OnRowsSpawned.Dispatch();
        }

        public void SpawnRow()
        {
            if (!GameEnded)
            {
                if (CurrentRowCounter == 0)
                {
                    GameData.Instance.PlayerStartingPosition = new Vector3(0, 8, RowData.Keys.ToList()[0]);
                }
                if (RowData != null)
                {
                    IRow row = ObjectPool.Instance.GetRow().GetComponent<IRow>();
                    Vector3 currentSpawnPosition;

                    // If no rows has been spawned yet spawn at 0 position
                    if (CurrentRow == null)
                        currentSpawnPosition = Vector3.zero;
                    else
                        currentSpawnPosition = new Vector3(0, CurrentRow.transform.position.y - GameData.Instance.SpaceBetweenRows, 0);

                    ActivateRow(row, currentSpawnPosition, RowData);
                }
            }
        }

        private void ActivateRow(IRow row, Vector3 currentSpawnPosition, Dictionary<float, CubeType> rowData)
        {
            List<float> positions = rowData.Keys.ToList();
            List<CubeType> cubes = rowData.Values.ToList();

            row.ActivateAndSetPosition(positions, cubes, currentSpawnPosition);
            ActiveRows.Add(row);
            CurrentRow = row.GetGameObject();
            rowDataObject.ActivateCoin(CurrentRowCounter, positions, currentSpawnPosition, CurrentRow.transform);

            float x = CurrentRow.transform.position.x;
            float y = CurrentRow.transform.position.y - 3;
            float z = Random.Range(0, 7);
            rowDataObject.ActivatePowerUp(CurrentRowCounter, new Vector3(x, y, z));

            CurrentRowCounter++;
            GetNextRowData();
        }

        private void EndGame()
        {
            GameEnded = true;
        }

        public void RemoveRow(IRow row)
        {
            ActiveRows.Remove(row);
        }

        public void SetBallNewPosition(GameObject ball)
        {
            IRow row = ActiveRows[1];
            Vector3 pos = Vector3.zero;
            if (row == null)
            {
                ball.GetComponent<Bounce>().LevelComplete();
                return;
            }
            foreach (ICube cube in row.GetRowCubes())
            {
                if (cube.GetBaseCubeType() == CubeType.Normal || cube.GetBaseCubeType() == CubeType.Landing)
                {
                    pos = cube.GetGameObject().transform.position + new Vector3(0, 1, 0);
                    break;
                }
            }
            ball.transform.SetPositionAndRotation(pos, Quaternion.identity);
            ActiveRows[0].OnPassedPlayer(false);
            ball.GetComponent<Bounce>().ChangeStateToDown();
        }
    }
}