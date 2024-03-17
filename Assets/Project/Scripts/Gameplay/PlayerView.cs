using UnityEngine;

public interface IPlayerView {
    
}

public class PlayerView : MonoBehaviour, IPlayerView {
    private IPlayerController _playerController;

    public IPlayerController PlayerController {
        get { return _playerController ??= new PlayerController(this); }
    }
}
