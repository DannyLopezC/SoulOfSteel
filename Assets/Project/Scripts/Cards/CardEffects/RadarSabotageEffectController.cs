using Photon.Pun;
using Photon.Realtime;
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

        PlayerView enemyPlayer = GameManager.Instance.playerList.Find(p => p.PlayerController.GetPlayerId() != originId);
        enemyPlayer.photonView.RPC("RpcHideBoard", RpcTarget.AllBuffered, originId); 

        EffectManager.Instance.WaitForEffectCardAnimation();
    }

    public void Deactivate(int originId)
    {
        PlayerView enemyPlayer = GameManager.Instance.playerList.Find(p => p.PlayerController.GetPlayerId() != originId);
        enemyPlayer.photonView.RPC("RpcShowBoard", RpcTarget.AllBuffered, originId);
    }
}
