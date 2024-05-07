using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Random = UnityEngine.Random;

public interface IMatchController
{
    IEnumerator PrepareMatch();
}

public class MatchController : IMatchController
{
    private int _matchId;
    private List<string> _matchLog;

    private readonly IMatchView _view;

    public MatchController(IMatchView view)
    {
        _view = view;
    }

    private void SetPriority()
    {
        GameManager.Instance.currentPriority = 1;

        GameManager.Instance.OnPrioritySet(GameManager.Instance.currentPriority);

        _view.SetCurrentPhaseText($"priority = {GameManager.Instance.currentPriority}");
    }

    private void SelectQuadrant()
    {
        _view.SetCurrentPhaseText("Selecting quadrant");

        foreach (PlayerView p in GameManager.Instance.playerList) {
            Vector2 nextCell = p.PlayerController.GetPlayerId() == 1
                ? Vector2.zero
                : new Vector2(GameManager.Instance.boardView.BoardController.GetBoardCount() - 1,
                    GameManager.Instance.boardView.BoardController.GetBoardCount() - 1);

            int currentDegrees = p.PlayerController.GetPlayerId() == 1 ? 270 : 90;

            p.PlayerController.SetCurrentCell(nextCell);
            p.PlayerController.SetCurrentDegrees(currentDegrees);
            p.GetComponent<PlayerMovement>().MoveToCell(nextCell);
            p.GetComponent<PlayerMovement>().Rotate(p.transform, currentDegrees);
        }
    }

    public IEnumerator PrepareMatch()
    {
        yield return new WaitForSeconds(2);
        SetPriority();
        yield return new WaitForSeconds(2);
        SelectQuadrant();
        yield return new WaitForSeconds(2);
        GameManager.Instance.PrepareForMatch(_view);
    }
}