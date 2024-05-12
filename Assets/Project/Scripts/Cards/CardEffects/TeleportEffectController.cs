using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITeleportEffectController : IEffectController
{
}

public class TeleportEffectController : EffectController, ITeleportEffectController
{
    private PlayerView _player;

    public override void Activate(int originId)
    {
        GameManager.Instance.LocalPlayerInstance.PlayerController.SetDoingEffect(true);

        _player = GameManager.Instance.playerList.Find(p => p.PlayerController.GetPlayerId() == originId);
        EffectManager.Instance.OnCellsSelectedEvent += StopEffect;
        EffectManager.Instance.OnSelectedCellEvent += SetTeleport;
        _player.SelectCells(1);
    }

    private void SetTeleport(Vector2 index, bool select)
    {
        if (CellType.Normal != GameManager.Instance.boardView.GetBoardStatus()[(int)index.y][(int)index.x]
                .CellController.GetCellType()) return;

        //Here i need to move the player to the desired position
        _player.PlayerController.SetCurrentCell(index);
        _player.GetComponent<PlayerMovement>().MoveToCell(index);

    }

    private void StopEffect(List<Vector2> cellsSelected)
    {
        EffectManager.Instance.OnAllEffectsFinished();
        GameManager.Instance.LocalPlayerInstance.PlayerController.SetDoingEffect(false);
        EffectManager.Instance.OnSelectedCellEvent -= SetTeleport;
        EffectManager.Instance.OnCellsSelectedEvent -= StopEffect;
    }
}
