using System.Collections;
using UnityEngine;

public class BattlePhase : Phase {
    private bool _allAttacksSelected;

    public BattlePhase(IMatchView matchView) : base(matchView) {
    }

    public override IEnumerator Start() {
        matchView.SetCurrentPhaseText("battle phase");

        GameManager.Instance.playerList.ForEach(p => p.SelectMovement());

        while (!_allAttacksSelected) {
            bool localAllMovementSelected = true;
            foreach (PlayerView player in GameManager.Instance.playerList) {
                if (!player.PlayerController.GetMovementSelected()) {
                    localAllMovementSelected = false;
                    break;
                }
            }

            _allAttacksSelected = localAllMovementSelected;

            yield return null;
        }

        GameManager.Instance.OnAllMovementSelected();
        GameManager.Instance.movementTurn = GameManager.Instance.currentPriority;
    }

    public void BattleFinished() {
        GameManager.Instance.ChangePhase(new FinalPhase(matchView));
    }
}