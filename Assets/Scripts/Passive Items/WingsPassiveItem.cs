using UnityEngine;

public class WingsPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        PlayerStats.CurrentMovementSpeed *= 1 + PassiveItemData.Multiplier / 100f;
    }
}
