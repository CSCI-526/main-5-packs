using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    void Start()
    {
      GameObject spawnMarker = GameObject.Find("PlayerSpawn");
        if (spawnMarker != null && playerPrefab != null)
        {
            Instantiate(playerPrefab, spawnMarker.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("PlayerSpawn not found or PlayerPrefab not assigned!");
        }
    }


    void Update()
    {
        
    }
}
