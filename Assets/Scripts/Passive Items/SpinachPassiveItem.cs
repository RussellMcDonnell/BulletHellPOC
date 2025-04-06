using UnityEngine;

public class SpinachPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        PlayerStats.CurrentMight *= 1 + PassiveItemData.Multiplier / 100f;
    }
}
