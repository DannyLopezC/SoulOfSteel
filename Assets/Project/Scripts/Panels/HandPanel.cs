using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHandPanel
{
    void ResetAnimationReferenceParent();
    GameObject GetGo();
    public Transform AnimationReference { get; set; }
}

public class HandPanel : MonoBehaviour, IHandPanel
{
    [SerializeField] private bool isMiddle;
    public Transform AnimationReference { get; set; }

    void Start()
    {
        if (isMiddle) GameManager.Instance.MiddlePanel = this;
        else GameManager.Instance.HandPanel = this;
    }

    public void ResetAnimationReferenceParent()
    {
        AnimationReference.SetParent(transform.parent);
    }

    public GameObject GetGo()
    {
        return gameObject;
    }
}