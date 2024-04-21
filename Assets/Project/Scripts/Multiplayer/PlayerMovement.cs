using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public PhotonView pv;
    public Movement currentMovement;

    private void Start() {
        pv = GetComponent<PhotonView>();
        GameManager.Instance.OnMovementSelectedEvent += DoMove;
    }

    private void DoMove(Movement movement, PlayerView player) {
        if (GameManager.Instance.LocalPlayerInstance.PlayerController.GetMoving()) return;
        currentMovement = movement;
        StartCoroutine(StartMoving(movement, player));
    }

    private IEnumerator StartMoving(Movement movement, PlayerView player) {
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

                Debug.Log($"{nextCell}");
                MoveToCell(nextCell);
                currentSteps--;
                yield return null;
            }

            player.PlayerController.SetCurrentCell(nextCell);
            player.PlayerController.SetCurrentDegrees(nextDegrees);
            GameManager.Instance.OnMovementFinished();
        }
    }

    public void MoveToCell(Vector2 index) {
        if (pv.IsMine) {
            index = new Vector2(
                Mathf.Clamp(index.x, index.x, GameManager.Instance.boardView.BoardController.GetBoardCount() - 1),
                Mathf.Clamp(index.y, index.y, GameManager.Instance.boardView.BoardController.GetBoardCount() - 1));
            Debug.Log($"moving {index}");
            transform.position = GameManager.Instance.boardView.GetCellPos(index);
        }
    }

    private void OnDestroy() {
        if (GameManager.HasInstance()) GameManager.Instance.OnMovementSelectedEvent -= DoMove;
    }
}