using UnityEngine;

public class MazeBuilder : MonoBehaviour
{
    [Header("Maze Settings")]
    public float cellSize = 1f;

    [Header("Prefabs")]
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    [Header("Ingredient Prefabs")]
    public GameObject chiliPickupPrefab;
    public GameObject butterPickupPrefab;
    [Header("Obstacle Prefabs")]
    public GameObject iceWallPrefab;
    public GameObject stickyZonePrefab;
    [Header("Ability Durations")]
    public float chiliDurationSeconds = 0f;
    public float butterDurationSeconds = 12f;

    void Start()
    {
        string[] maze =
        {
            "####################",
            "#..C..#..I.....C..##",
            "#.#.#.#.###.##.#..##",
            "#..#..~~~..#.#I..#.#",
            "#..#B..#..##.~.##..#",
            "#...#..#....##...###",
            "#..##..#..B.#..#..##",
            "#S..#......#..#...##",
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
                        SpawnWall(pos);
                        break;

                    case 'S':
                        SpawnFloor(pos);
                        CreateSpawnMarker(pos);
                        break;

                    case 'C':
                        SpawnFloor(pos);
                        SpawnIngredient(pos, IngredientType.Chili, chiliDurationSeconds);
                        break;

                    case 'B':
                        SpawnFloor(pos);
                        SpawnIngredient(pos, IngredientType.Butter, butterDurationSeconds);
                        break;

                    case 'I':
                        SpawnFloor(pos);
                        SpawnIceWall(pos);
                        break;

                    case '~':
                        SpawnFloor(pos);
                        SpawnStickyZone(pos);
                        break;

                    case '.':
                    case ' ':
                    default:
                        SpawnFloor(pos);
                        break;
                }
            }
        }
        CenterMaze(layout);
    }

    void SpawnWall(Vector2 position)
    {
        if (wallPrefab == null) return;
        GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity, transform);
        wall.layer = LayerMask.NameToLayer("Wall");
    }

    void SpawnFloor(Vector2 position)
    {
        if (floorPrefab == null) return;
        Instantiate(floorPrefab, position, Quaternion.identity, transform);
    }

    void SpawnIngredient(Vector2 position, IngredientType type, float durationSeconds)
    {
        GameObject prefab = type == IngredientType.Chili ? chiliPickupPrefab : butterPickupPrefab;
        GameObject ingredient = prefab != null ? Instantiate(prefab, position, Quaternion.identity, transform) : CreateRuntimeIngredient(type, position);

        if (!ingredient.TryGetComponent(out IngredientPickup pickup))
        {
            pickup = ingredient.AddComponent<IngredientPickup>();
        }

        pickup.Configure(type, durationSeconds);
    }

    GameObject CreateRuntimeIngredient(IngredientType type, Vector2 position)
    {
        GameObject ingredient = new GameObject($"{type}Pickup");
        ingredient.transform.SetParent(transform);
        ingredient.transform.position = position;

        SpriteRenderer sr = ingredient.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 1;

        if (floorPrefab != null && floorPrefab.TryGetComponent(out SpriteRenderer floorSR))
        {
            sr.sprite = floorSR.sprite;
        }

        sr.color = type == IngredientType.Chili
            ? new Color(0.88f, 0.24f, 0.16f, 1f)
            : new Color(0.99f, 0.91f, 0.47f, 1f);

        float pickupScale = Mathf.Max(0.1f, cellSize * 0.6f);
        ingredient.transform.localScale = new Vector3(pickupScale, pickupScale, 1f);

        CircleCollider2D circleCollider = ingredient.AddComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;

        return ingredient;
    }

    void SpawnIceWall(Vector2 position)
    {
        GameObject source = iceWallPrefab != null ? iceWallPrefab : wallPrefab;
        if (source == null) return;

        GameObject ice = Instantiate(source, position, Quaternion.identity, transform);

        if (ice.TryGetComponent(out SpriteRenderer sr))
        {
            sr.color = new Color(0.62f, 0.84f, 1f, 1f);
        }

        if (!ice.TryGetComponent(out IceWall iceWall))
        {
            iceWall = ice.AddComponent<IceWall>();
        }

        ice.layer = LayerMask.NameToLayer("Wall");
    }

    void SpawnStickyZone(Vector2 position)
    {
        GameObject zone = stickyZonePrefab != null ? Instantiate(stickyZonePrefab, position, Quaternion.identity, transform) : CreateRuntimeStickyZone(position);
        if (!zone.TryGetComponent<StickyZone>(out _))
        {
            zone.AddComponent<StickyZone>();
        }

        float zoneScale = Mathf.Max(0.1f, cellSize);
        zone.transform.localScale = new Vector3(zoneScale, zoneScale, 1f);
    }

    GameObject CreateRuntimeStickyZone(Vector2 position)
    {
        GameObject zone = new GameObject("StickyZone");
        zone.transform.SetParent(transform);
        zone.transform.position = position;

        SpriteRenderer sr = zone.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 0;

        if (floorPrefab != null && floorPrefab.TryGetComponent(out SpriteRenderer floorSR))
        {
            sr.sprite = floorSR.sprite;
        }

        sr.color = new Color(0.98f, 0.8f, 0.2f, 0.65f);

        BoxCollider2D collider = zone.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;

        return zone;
    }

    void CreateSpawnMarker(Vector2 position)
    {
        GameObject spawnMarker = new GameObject("PlayerSpawn");
        spawnMarker.transform.position = position;
        spawnMarker.transform.SetParent(transform);
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
