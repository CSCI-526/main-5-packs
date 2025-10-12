using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    // public Transform spawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
