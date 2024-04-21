using System.Collections;
using UnityEngine;

public class MovementPhase : Phase {
    public MovementPhase(IMatchView matchView) : base(matchView) {
    }

    public override IEnumerator Start() {
        matchView.SetCurrentPhaseText("movement phase");

        yield return new WaitForSeconds(1);

        GameManager.Instance.playerList.ForEach(p => p.PlayerController.SelectMovement());

        // GameManager.Instance.ChangePhase(new BattlePhase(matchView));

        yield break;
    }
}