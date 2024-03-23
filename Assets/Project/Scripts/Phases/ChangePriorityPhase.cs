using System.Collections;

public class ChangePriorityPhase : Phase {
    public ChangePriorityPhase(IMatchView matchView) : base(matchView) {
    }

    public override IEnumerator Start() {
        if (GameManager.Instance.playerList.Count == 0) yield break;

        GameManager.Instance.currentPriority =
            (GameManager.Instance.currentPriority + 1) % GameManager.Instance.playerList.Count;

        GameManager.Instance.ChangePhase(new RechargePhase(matchView));

        matchView.SetCurrentPhaseText("Changing priority phase");
    }
}