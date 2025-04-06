using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemScriptableObjectScript", menuName = "Scriptable Objects/Passive Item")]
public class PassiveItemScriptableObjectScript : ScriptableObject
{
    [SerializeField]
    private float _multiplier;
    public float Multiplier { get => _multiplier; private set => _multiplier = value; } 
    
}
