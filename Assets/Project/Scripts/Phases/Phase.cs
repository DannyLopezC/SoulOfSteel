using Unity.VisualScripting;
using UnityEngine;

public interface IPhase {
    void Start();
    void End();
}

public class Phase : IPhase {
    public Phase() {
    }

    public virtual void Start() {
    }

    public virtual void End() {
    }
}