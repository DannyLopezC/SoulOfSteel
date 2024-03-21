using Unity.VisualScripting;
using UnityEngine;

public interface IPhase {
    void Start();
    void End();
}

public abstract class Phase : IPhase {

    public Phase() {
        
    }

    public abstract void Start();

    public abstract void End();
}