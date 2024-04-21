using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public PhotonView pv;

    private void Start() {
        pv = GetComponent<PhotonView>();
        GameManager.Instance.OnMovementSelectedEvent += DoMove;
        MoveToCell(new Vector2(5, 5));
    }

    private void DoMove(Movement movement, PlayerView player) {
        StartCoroutine(StartMoving(movement, player));
    }

    private IEnumerator StartMoving(Movement movement, PlayerView player) {
        int currentDegrees = player.PlayerController.GetCurrentDegrees();
        Vector2 currentCell = player.PlayerController.GetCurrentCell();

        List<Movement.MovementInfo> steps = movement.steps;
        int nextDegrees = movement.degrees[0];

        foreach (Movement.MovementInfo step in steps) {
            int currentSteps = step.Steps;

            while (currentSteps > 0) {
                yield return new WaitForSeconds(0.5f);

                Vector2 nextCell = currentCell;

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
                        nextCell.y += 1;
                        break;
                    case 270:
                        nextCell.y -= 1;
                        break;
                }

                MoveToCell(nextCell);
                currentSteps--;
                yield return null;
            }
        }
    }

    private void MoveToCell(Vector2 index) {
        if (!GameManager.Instance.LocalPlayerInstance.PlayerController.GetMoving()) return;

        if (pv.IsMine) {
            transform.position = GameManager.Instance.boardView.GetCellPos(index);
        }
    }

    private void OnDestroy() {
        if (GameManager.HasInstance()) GameManager.Instance.OnMovementSelectedEvent -= DoMove;
    }
}