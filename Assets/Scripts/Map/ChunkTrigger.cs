using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
    private MapController _mapController;
    public GameObject TargetMap;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mapController = FindAnyObjectByType<MapController>();
        
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            _mapController.CurrentTerrainChunck = TargetMap;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            _mapController.CurrentTerrainChunck = null;
        }
    }
}
