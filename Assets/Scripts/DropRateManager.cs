using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public string Name;
        public GameObject ItemPrefab;
        
        [Range(0f, 100f)]
        [Tooltip("The drop rate of the item. 0% means it will never drop, 100% means it will always drop.")]
        public float DropRate;
    }

    public List<Drops> DropsList;

    private void OnDestroy()
    {
        float randomNumber = UnityEngine.Random.Range(0f, 100f);
        List<Drops> possibleDrops = new List<Drops>();

        foreach (Drops drop in DropsList)
        {
            if (randomNumber <= drop.DropRate)
            {
                possibleDrops.Add(drop);
            }
        }

        // Check if there are any possible drops
        if (possibleDrops.Count > 0)
        {
            // Select a random drop from the possible drops
            int randomIndex = UnityEngine.Random.Range(0, possibleDrops.Count);
            Drops drop = possibleDrops[randomIndex];
            Instantiate(drop.ItemPrefab, transform.position, Quaternion.identity);
        }
    }
}
