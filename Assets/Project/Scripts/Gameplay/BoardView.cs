using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoardView {
    
}

public class BoardView : MonoBehaviour, IBoardView
{
    private IBoardController _boardController;

    public IBoardController BoardController {
        get { return _boardController ??= new BoardController(this); }
    }
}
