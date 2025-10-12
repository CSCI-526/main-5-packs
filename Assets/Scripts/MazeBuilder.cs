using UnityEngine;

public class MazeBuilder : MonoBehaviour
{
    [Header("Maze Settings")]
    public float cellSize = 1f;

    [Header("Prefabs")]
    public GameObject wallPrefab;
    public GameObject floorPrefab;

    void Start()
    {
        string[] maze =
        {
            "####################",
            "#........#.........#",
            "#..##.........##...#",
            "#..#...........#...#",
            "#..#.....O.....#...#",
            "#S.......#.........#",
            "####################"
        };

        BuildMaze(maze);
    }

    void BuildMaze(string[] layout)
    {
        for (int y = 0; y < layout.Length; y++)
        {
            string line = layout[y];
            for (int x = 0; x < line.Length; x++)
            {
                char c = line[x];
                Vector2 pos = new Vector2(x * cellSize, -y * cellSize);

                switch (c)
                {
                    case '#':
                        GameObject wall = Instantiate(wallPrefab, pos, Quaternion.identity, transform);
    			        wall.layer = LayerMask.NameToLayer("Wall");
                        break;

                    case '.':
                    
                    case ' ':
                        Instantiate(floorPrefab, pos, Quaternion.identity, transform);
                        break;
                    
                    case 'O': 
                        Instantiate(floorPrefab, pos, Quaternion.identity, transform);
                        GameObject marker = new GameObject("ObstacleMarker");
                        var sr = marker.AddComponent<SpriteRenderer>();
                        var floorSR = floorPrefab.GetComponent<SpriteRenderer>();
                        if (floorSR != null) sr.sprite = floorSR.sprite;
                        sr.color = new Color(0.9f, 0.2f, 0.2f, 1f); // red
                        marker.transform.position = pos;
                        marker.transform.localScale = Vector3.one * 0.5f;
                        marker.transform.SetParent(transform);
                        break;
                    
                    case 'S':
                    Instantiate(floorPrefab, pos, Quaternion.identity, transform);
                    GameObject spawnMarker = new GameObject("PlayerSpawn");
                    spawnMarker.transform.position = pos;
                    spawnMarker.transform.SetParent(transform);
                    break;

                }
            }
        }
        CenterMaze(layout);
    }
    void CenterMaze(string[] layout)
    {
        float width = layout[0].Length * cellSize;
        float height = layout.Length * cellSize;

        // maze center calc
        Vector3 center = new Vector3(
            (width - cellSize) / 2f,
            -(height - cellSize) / 2f,
            -10f 
        );

        // Move cam to center and fit 
        if (Camera.main != null)
        {
            Camera.main.transform.position = center;

            float verticalSize = (height / 2f) + 1f;
            float horizontalSize = ((width / 2f) + 1f) / Camera.main.aspect;
            Camera.main.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
        }
    }
}