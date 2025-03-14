using System.Collections;

using UnityEngine;

[CreateAssetMenu(fileName = "UIConfig", menuName = "Config/UI")]
public class InterfaceConfig : Config
{
    [Header("SlotView")]
    public Sprite DefaultSlotView;
    public Sprite CommonSlotView;
    public Sprite UncommonSlotView;
    public Sprite RareSlotView;
    public Sprite LegendarySlotView;
    [Space(10f)]
    [Header("AnyView")]
    public Sprite Any; //TODO remove or replace

    public override IEnumerator Init()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
