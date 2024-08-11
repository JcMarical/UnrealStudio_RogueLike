using System.Collections.Generic;
using UnityEngine;

public class DungeonLayer
{
    public int MinRooms { get; set; }
    public int MaxRooms { get; set; }
    public int SmallSquareSize { get; set; }
    public int LargeSquareSize { get; set; }
    public int SpecialRoomCount { get; set; }
    public List<DungeonRoom> Rooms { get; private set; }

    public DungeonLayer(int minRooms, int maxRooms, int smallSize, int largeSize, int specialCount)
    {
        MinRooms = minRooms;
        MaxRooms = maxRooms;
        SmallSquareSize = smallSize;
        LargeSquareSize = largeSize;
        SpecialRoomCount = specialCount;
        Rooms = new List<DungeonRoom>();
    }
}

public class DungeonRoom
{
    public Vector2Int Position { get; set; }
    public Vector2Int Size { get; set; }
    public bool IsSpecial { get; set; }

    public DungeonRoom(Vector2Int position, Vector2Int size, bool isSpecial)
    {
        Position = position;
        Size = size;
        IsSpecial = isSpecial;
    }
}

//--------------------------------------------------------------------

public class DungeonGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public GameObject specialRoomPrefab;

    private List<Vector2Int> occupiedPositions = new List<Vector2Int>();

    public DungeonLayer GenerateLayer(int layerNumber)
    {
        DungeonLayer layer = GetLayerConfiguration(layerNumber);

        int roomCount = Random.Range(layer.MinRooms, layer.MaxRooms + 1);
        for (int i = 0; i < roomCount; i++)
        {
            GenerateRoom(layer, i);
        }

        return layer;
    }

    private DungeonLayer GetLayerConfiguration(int layerNumber)
    {
        switch (layerNumber)
        {
            case 1:
                return new DungeonLayer(9, 12, 4, 8, 0);
            case 2:
                return new DungeonLayer(18, 24, 5, 9, 1);
            case 3:
                return new DungeonLayer(27, 33, 6, 10, 3);
            case 4:
                return new DungeonLayer(36, 42, 7, 11, 4);
            default:
                return null;
        }
    }

    private void GenerateRoom(DungeonLayer layer, int index)
    {
        bool isSpecial = index < layer.SpecialRoomCount;

        // Determine size: special rooms can have non-standard sizes
        Vector2Int size = isSpecial ? GetSpecialRoomSize(layer) : new Vector2Int(1, 1);

        // Ensure the room is within the large square but not entirely within the small square
        Vector2Int position;
        do
        {
            position = new Vector2Int(
                Random.Range(0, layer.LargeSquareSize - size.x + 1),
                Random.Range(0, layer.LargeSquareSize - size.y + 1)
            );
        }
        while (IsInsideSmallSquare(position, size, layer.SmallSquareSize) || IsOverlapping(position, size));

        DungeonRoom room = new DungeonRoom(position, size, isSpecial);
        layer.Rooms.Add(room);
        occupiedPositions.AddRange(GetRoomPositions(position, size));
    }

    private Vector2Int GetSpecialRoomSize(DungeonLayer layer)
    {
        switch (layer.SpecialRoomCount)
        {
            case 1:
                return new Vector2Int(2, 2); // Example special room size
            case 3:
                return new Vector2Int(3, 3);
            default:
                return new Vector2Int(1, 1);
        }
    }

    private bool IsInsideSmallSquare(Vector2Int position, Vector2Int size, int smallSquareSize)
    {
        return position.x >= 0 && position.y >= 0 &&
               position.x + size.x <= smallSquareSize && position.y + size.y <= smallSquareSize;
    }

    //---------------------------------------------------------------------------------

    private bool IsOverlapping(Vector2Int position, Vector2Int size)
    {
        foreach (Vector2Int pos in GetRoomPositions(position, size))
        {
            if (occupiedPositions.Contains(pos))
                return true;
        }
        return false;
    }

    private IEnumerable<Vector2Int> GetRoomPositions(Vector2Int position, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                yield return new Vector2Int(position.x + x, position.y + y);
            }
        }
    }

    private void ConnectRooms(DungeonLayer layer)
    {
        List<DungeonRoom> connectedRooms = new List<DungeonRoom>();
        connectedRooms.Add(layer.Rooms[0]); // Start with the first room

        while (connectedRooms.Count < layer.Rooms.Count)
        {
            DungeonRoom currentRoom = connectedRooms[connectedRooms.Count - 1];
            DungeonRoom nextRoom = FindNearestUnconnectedRoom(currentRoom, layer.Rooms, connectedRooms);

            CreateCorridor(currentRoom, nextRoom);
            connectedRooms.Add(nextRoom);
        }
    }

    private DungeonRoom FindNearestUnconnectedRoom(DungeonRoom currentRoom, List<DungeonRoom> rooms, List<DungeonRoom> connectedRooms)
    {
        DungeonRoom nearestRoom = null;
        float nearestDistance = float.MaxValue;

        foreach (DungeonRoom room in rooms)
        {
            if (connectedRooms.Contains(room)) continue;

            float distance = Vector2Int.Distance(currentRoom.Position, room.Position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestRoom = room;
            }
        }

        return nearestRoom;
    }

    private void CreateCorridor(DungeonRoom from, DungeonRoom to)
    {
        // Simple straight corridor, can be more complex if needed
        Vector3 corridorStart = new Vector3(from.Position.x + from.Size.x / 2, from.Position.y + from.Size.y / 2, 1);
        Vector3 corridorEnd = new Vector3(to.Position.x + to.Size.x / 2, to.Position.y + to.Size.y / 2, 1);

        // Create the corridor game objects
        Instantiate(roomPrefab, corridorStart, Quaternion.identity);
        Instantiate(roomPrefab, corridorEnd, Quaternion.identity);
    }

    //-----------------------------------------------------------------------------------------------

    /*private void GenerateLayerSpecificRooms(DungeonLayer layer)
    {
    // Ensure specific types of rooms are included
    CreateRoomOfType(layer, "TreasureRoom");
    CreateRoomOfType(layer, "ShopRoom");
    CreateRoomOfType(layer, "BattleRoom");

    // Fill the rest of the rooms with randomized types
    for (int i = 3; i < layer.Rooms.Count; i++)
    {
        if (Random.value < 0.4f)
            layer.Rooms[i].Type = "EventRoom";
        else if (Random.value < 0.5f)
            layer.Rooms[i].Type = "BattleRoom";
        else
            layer.Rooms[i].Type = "ChallengeRoom";
    }
    }

    private void CreateRoomOfType(DungeonLayer layer, string type)
    {
    Room room = layer.Rooms.Find(r => r.Type == null);
    if (room != null)
    {
        room.Type = type;
    }
    }

        //---------------------------------------------------------

    public class DungeonManager : MonoBehaviour
    {
        public DungeonGenerator generator;

        void Start()
        {
            for (int layer = 1; layer <= 4; layer++)
            {
                DungeonLayer dungeonLayer = generator.GenerateLayer(layer);
                generator.ConnectRooms(dungeonLayer);
                generator.GenerateLayerSpecificRooms(dungeonLayer);

                VisualizeLayer(dungeonLayer);
            }
        }

        private void VisualizeLayer(DungeonLayer layer)
        {
            foreach (Room room in layer.Rooms)
            {
                Instantiate(room.IsSpecial ? specialRoomPrefab : roomPrefab, new Vector3(room.Position.x, room.Position.y, 0), Quaternion.identity);
            }
        }
    }
    */
    private void GenerateHiddenLayer()
    {
        // Example for fixed layout hidden layer
        List<DungeonRoom> hiddenRooms = new List<DungeonRoom>();

        // Define positions for a cross-shaped layout
        hiddenRooms.Add(new DungeonRoom(new Vector2Int(2, 2), new Vector2Int(1, 1), false));
        hiddenRooms.Add(new DungeonRoom(new Vector2Int(2, 3), new Vector2Int(1, 1), false));


    }
}