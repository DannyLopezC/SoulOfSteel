using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PhotonView pv;
    public Movement currentMovement;
    public Nickname nickname;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        GameManager.Instance.OnMovementSelectedEvent += DoMove;
    }

    private void DoMove(Movement movement, PlayerView player, int movementIterations)
    {
        if (GameManager.Instance.LocalPlayerInstance.PlayerController.GetMoving()) return;
        player.PlayerController.SetMoving(true);
        currentMovement = movement;

        StartCoroutine(StartMoving(movement, player, movementIterations));
    }

    private IEnumerator StartMoving(Movement movement, PlayerView player, int movementIterations)
    {
        for (int i = 0; i < movementIterations; i++) {
            int currentDegrees = player.PlayerController.GetCurrentDegrees();
            Vector2 currentCell = player.PlayerController.GetCurrentCell();

            List<Movement.MovementInfo> steps = movement.steps;
            int nextDegrees = movement.degrees[0];

            foreach (Movement.MovementInfo step in steps) {
                int currentSteps = step.Steps;
                Vector2 nextCell = currentCell;

                while (currentSteps > 0) {
                    yield return new WaitForSeconds(0.5f);

                    int adjustedDirection;

                    switch (step.Direction) {
                        case "↑":
                            adjustedDirection = currentDegrees;
                            break;
                        case "→":
                            adjustedDirection = (currentDegrees - 90) % 360;
                            break;
                        case "←":
                            adjustedDirection = (currentDegrees + 90) % 360;
                            break;
                        default:
                            Debug.Log($"not valid direction");
                            yield break;
                    }

                    switch (adjustedDirection) {
                        case 180:
                            nextCell.x -= 1;
                            break;
                        case 0:
                            nextCell.x += 1;
                            break;
                        case 90:
                            nextCell.y -= 1;
                            break;
                        case 270:
                            nextCell.y += 1;
                            break;
                    }

                    nextCell = new Vector2(
                        Mathf.Clamp(nextCell.x, 0, GameManager.Instance.boardView.BoardController.GetBoardCount() - 1),
                        Mathf.Clamp(nextCell.y, 0, GameManager.Instance.boardView.BoardController.GetBoardCount() - 1));

                    MoveToCell(nextCell);
                    currentSteps--;
                    yield return null;
                }

                player.PlayerController.SetCurrentCell(nextCell);
                player.PlayerController.SetCurrentDegrees(nextDegrees);
                Rotate(player.transform, nextDegrees);

                if (i == movementIterations - 1) {
                    player.PlayerController.SetMoving(false);
                    GameManager.Instance.OnMovementTurnDone();
                }

                if (!EffectManager.Instance.gravitationalImpulseEffectActive) {
                    if (GameManager.Instance.boardView.GetBoardStatus()[(int)nextCell.y][(int)nextCell.x].CellController
                            .GetCellType() == CellType.Blocked) {
                        if (!GameManager.Instance.testing) {
                            player.photonView.RPC("RpcReceivedDamage", RpcTarget.AllBuffered, 2,
                                player.PlayerController.GetPlayerId());
                        }
                        else {
                            //receive damage when hitting walls
                            player.PlayerController.ReceivedDamage(3, player.PlayerController.GetPlayerId());
                        }

                        nextCell = new Vector2(nextCell.x + 1, nextCell.y);
                        MoveToCell(nextCell);
                        player.PlayerController.SetCurrentCell(nextCell);
                    }
                }
            }
        }
    }

    public void MoveToCell(Vector2 index)
    {
        if (pv.IsMine) {
            transform.position = GameManager.Instance.boardView.GetCellPos(index);

            CellView currentCell = GameManager.Instance.boardView.GetBoardStatus()[(int)index.y][(int)index.x];

            if (currentCell.CellController.GetCellType() == CellType.Mined &&
                !EffectManager.Instance.gravitationalImpulseEffectActive) {
                PlayerView currentPlayer = GetComponent<PlayerView>();
                if (!GameManager.Instance.testing) {
                    currentPlayer.photonView.RPC("RpcReceivedDamage", RpcTarget.AllBuffered, 3,
                        currentPlayer.PlayerController.GetPlayerId());
                    GameManager.Instance.LocalPlayerInstance.photonView.RPC("RpcPutMines",
                        RpcTarget.AllBuffered, (int)index.y, (int)index.x, false);
                }
                else {
                    currentPlayer.PlayerController.ReceivedDamage(3, currentPlayer.PlayerController.GetPlayerId());
                }

                currentCell.CellController.SetType(CellType.Normal);
                Debug.Log($"turn off mine");
            }
        }
    }

    public void Rotate(Transform t, int direction)
    {
        nickname.transform.SetParent(transform.parent);
        t.rotation = Quaternion.Euler(0, 0, direction);
        nickname.transform.SetParent(transform);
    }

    private void OnDestroy()
    {
        if (GameManager.HasInstance()) GameManager.Instance.OnMovementSelectedEvent -= DoMove;
    }
}