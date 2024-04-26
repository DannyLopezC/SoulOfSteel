public interface IArmCardController : IEquipmentCardController {
}

public class ArmCardController : EquipmentCardController, IArmCardController {
    private readonly IArmCardView _view;

    public ArmCardController(IArmCardView view) : base(view) {
        _view = view;
    }
}