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
            "#..##....#....##...#",
            "#..#.....#.....#...#",
            "#..#.....O.....#...#",
            "#........#.........#",
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
                        Instantiate(wallPrefab, pos, Quaternion.identity, transform);
                        break;
                    case '.':
                    case ' ':
                        Instantiate(floorPrefab, pos, Quaternion.identity, transform);
                        break;
case 'O': // Obstacle placeholder marker (2D-friendly)
    Instantiate(floorPrefab, pos, Quaternion.identity, transform);

    // Create a small red square using a SpriteRenderer (no external assets)
    GameObject marker = new GameObject("ObstacleMarker");
    var sr = marker.AddComponent<SpriteRenderer>();

    // Reuse the same sprite as your Floor prefab (should be the built-in Square)
    var floorSR = floorPrefab.GetComponent<SpriteRenderer>();
    if (floorSR != null) sr.sprite = floorSR.sprite;

    sr.color = new Color(0.9f, 0.2f, 0.2f, 1f); // red
    marker.transform.position = pos;
    marker.transform.localScale = Vector3.one * 0.5f;
    marker.transform.SetParent(transform);
    break;

                                    }
            }
        }
CenterMaze(layout);

    }
void CenterMaze(string[] layout)
{
    // Compute total width and height in world units
    float width = layout[0].Length * cellSize;
    float height = layout.Length * cellSize;

    // Move the entire maze so that its center is at (0,0)
    Vector3 offset = new Vector3(-width / 2f + cellSize / 2f, height / 2f - cellSize / 2f, 0f);
    transform.position = offset;
}

}
