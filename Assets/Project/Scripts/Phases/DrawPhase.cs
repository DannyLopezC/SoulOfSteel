using System.Collections;
using UnityEngine;

public class DrawPhase : Phase {
    public DrawPhase(IMatchView matchView) : base(matchView) {
    }

    public override IEnumerator Start() {
        GameManager.Instance.playerList.ForEach(player => player.PlayerController.DrawCards(5));

        matchView.SetCurrentPhaseText("drawing cards");

        GameManager.Instance.ChangePhase(new ChangePriorityPhase(matchView));
        yield break;
    }

    public override IEnumerator End() {
        yield break;
    }
}