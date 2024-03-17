using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMatchView {
    
}

public class MatchView : MonoBehaviour, IMatchView
{
    private IMatchController _matchController;

    public IMatchController MatchController {
        get { return _matchController ??= new MatchController(this); }
    }
    
}
