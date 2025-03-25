using UnityEngine;

public class SetPlayModeLayout : MonoBehaviour
{
    [SerializeField] PlayModeSlotView[] _slots;

    public void Init()
    {
        _slots = GetComponentsInChildren<PlayModeSlotView>();

        foreach (var slotView in _slots)
        {
            slotView.Init();
        }
    }
}
