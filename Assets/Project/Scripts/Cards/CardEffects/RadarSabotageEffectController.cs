using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRadarSabotageEffectController: IEffectController
{

}

public class RadarSabotageEffectController : EffectController, IRadarSabotageEffectController
{
    public override void Activate(int originId)
    {
        GameManager.Instance.LocalPlayerInstance.PlayerController.SetDoingEffect(true);

        IPlayerController enemyPlayer = GameManager.Instance.playerList.Find(p => p.PlayerController.GetPlayerId() != originId).PlayerController;
        enemyPlayer.HideBoardCells();

        EffectManager.Instance.WaitForEffectCardAnimation();
    }

    public void Deactivate(int originId)
    {
        IPlayerController enemyPlayer = GameManager.Instance.playerList.Find(p => p.PlayerController.GetPlayerId() == originId).PlayerController;
        enemyPlayer.ShowBoardCells();
    }
}
