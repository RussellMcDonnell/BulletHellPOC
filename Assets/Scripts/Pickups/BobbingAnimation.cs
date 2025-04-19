using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    public float Frequency;
    public float Magnitude;
    public Vector3 Direction;
    private Vector3 _initialPosition;
    public bool IsIdle = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _initialPosition = transform.localPosition; // Store the initial position of the object        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsIdle)
        {
            // Update the position of the object based on the sine wave
            transform.position = _initialPosition + Direction * Mathf.Sin(Time.time * Frequency) * Magnitude;
        }
    }
}
