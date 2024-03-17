using System.Collections.Generic;
using UnityEngine;

public interface IBoardController {
}

public class BoardController : IBoardController {
    private readonly IBoardView _view;

    private List<List<int>> _boardStatus;
    private int _boardSize = 10;
    private List<QuadrantView> quadrants;

    public BoardController(IBoardView view) {
        _view = view;
    }
}