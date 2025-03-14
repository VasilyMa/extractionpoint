using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    public abstract Sprite Icon { get; }
    public abstract string Discription { get;  }
    public abstract void BecomeInteractive();
    public abstract void StopBeingInteractive();
}