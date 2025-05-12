using BallDrop.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop.Base
{
    public class RowBase : GameComponent, IRow
    {
        private bool RowPassed = false;
        private List<ICube> rowCubes = new List<ICube>();
        public BoxCollider exitCollider;
        private WaitForSeconds colliderEnablingDelay = new WaitForSeconds(0.11f);

        public void OnEnable()
        {
            MyEventManager.Game.EndGame.AddListener(EndGame);
        }

        private void OnDisable()
        {
            MyEventManager.Game.EndGame.RemoveListener(EndGame);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(GameStrings.Player))
            {
                if (!RowPassed)
                {
                    RowPassed = true;
                    GameData.Instance.rowPassPosition = other.transform.position;
                    OnPassedPlayer(true);
                }
            }
            if (other.CompareTag(GameStrings.TopBoundary))
            {
                OnPassedPlayer(false);
            }
        }

        public void OnPassedPlayer(bool animate)
        {
            ActivateNext();
            MyEventManager.Game.Rows.OnRowPassed.Dispatch();
            Deactivate(animate);
        }

        public void ActivateAndSetPosition(List<float> positions, List<CubeType> cubes, Vector3 rowPosition)
        {
            Activate();
            rowCubes.Clear();
            RowPassed = false;
            transform.SetPositionAndRotation(rowPosition, Quaternion.identity);
            for (int i = 0; i < positions.Count; i++)
            {
                ICube cube = ObjectPool.Instance.GetCube(cubes[i]).GetComponent<ICube>();
                if (cube != null)
                {
                    cube.ActivateAndSetPosition(transform, positions[i]);
                    rowCubes.Add(cube);
                }
            }
            StartCoroutine(EnableCollider());
        }

        private IEnumerator EnableCollider()
        {
            yield return colliderEnablingDelay;
            if (exitCollider != null)
                exitCollider.enabled = true;
        }

        private void EndGame()
        {
            Deactivate(false);
        }

        public void ChangeInvisibleCube(float zPos)
        {
            if (!RowPassed)
            {
                int subtype = UnityEngine.Random.Range(0, (int)CubeType.Invisible);
                ICube cube = ObjectPool.Instance.GetCube((CubeType)subtype).GetComponent<ICube>();
                if (cube != null)
                {
                    cube.ActivateAndSetPosition(transform, zPos);
                    rowCubes.Add(cube);
                }
            }
        }

        public void Deactivate(bool animate)
        {
            MyEventManager.Game.Rows.RemoveRow.Dispatch(this);
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(DeactivateChildren(animate));
            }
            exitCollider.enabled = false;
        }

        public IEnumerator DeactivateChildren(bool animate)
        {
            if (GetComponentInChildren<CoinBase>() != null)
                GetComponentInChildren<CoinBase>().Deactivate();
            foreach (ICube icube in rowCubes)
            {
                icube.GetGameObject().transform.parent = ObjectPool.Instance.PooledObjectsHolder;
                if (icube != null)
                {
                    if (animate)
                    {
                        GameData.Instance.playerGameData.AddCubeData(icube.GetBaseCubeType());
                        icube.FlipAndDeactivate();
                    }
                    else
                    {
                        icube.Deactivate();
                    }
                }
                yield return null;
            }
            yield return new WaitForEndOfFrame();
            Deactivate();
        }

        public List<ICube> GetRowCubes()
        {
            return rowCubes;
        }

        public void AddToRowCubes(ICube cube)
        {
            rowCubes.Add(cube);
        }

        //public void RemoveFromCubes(ICube cube)
        //{
        //    rowCubes.Remove(cube);
        //}

        private void ActivateNext()
        {
            MyEventManager.Game.Rows.SpawnRow.Dispatch();
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}