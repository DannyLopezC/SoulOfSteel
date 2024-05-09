using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public interface IChestCardController : IEquipmentCardController
{
    void GetEffect();
    void RemoveEffect();
}

public class ChestCardController : EquipmentCardController, IChestCardController
{
    private readonly IChestCardView _view;

    public ChestCardController(IChestCardView view) : base(view)
    {
        _view = view;
    }

    public void GetEffect()
    {
        Debug.Log("Get chest effect ID " + Id);
        switch (Id)
        {
            case 26:
                EffectManager.Instance.ActivateGravitationalImpulse(Id);
                break;
        }
    }

    public void RemoveEffect()
    {
        switch (Id)
        {
            case 26:
                EffectManager.Instance.DeactivateGravitationalImpulse(Id);
                break;
        }
    }

}
