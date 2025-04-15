using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    protected PlayerStats PlayerStats;
    public PassiveItemScriptableObject PassiveItemData;

    protected virtual void ApplyModifier()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerStats = GetComponentInParent<PlayerStats>();    
        ApplyModifier();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
