using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGraph : MonoBehaviour
{
    public static RoomGraph instance;
    public GameObject[] roomPrefabs;
    public int spacingX = 1, spacingZ = 1;
    public Dictionary<Vector2Int, Room> rooms = new Dictionary<Vector2Int, Room>();
    List<Vector2Int> spotsClaimed = new List<Vector2Int>();
    // Start is called before the first frame update
    void Start()
    {
        rooms.Add(Vector2Int.up * -25, Manager.instance.currentRoom);
        instance = this;
        Room r0 = Instantiate(roomPrefabs[0], new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Room>();
        spotsClaimed.Add(Vector2Int.zero);
        rooms.Add(Vector2Int.zero, r0);
        r0.pos = Vector2Int.zero;
        r0.downOpen.SetActive(true);
        r0.downClosed.SetActive(false);
        r0.gameObject.SetActive(false);
        int i = 1;
        while (roomPrefabs.Length > i)
        {
            Vector2Int from = spotsClaimed[Mathf.FloorToInt(spotsClaimed.Count * Random.value)];
            Vector2Int to = from + (Random.value > 0.2f ? (Random.value > 0.5f ? Vector2Int.right * spacingX : Vector2Int.left * spacingX) : Vector2Int.up * spacingZ);
            if (rooms.ContainsKey(to))
            {

            }
            else
            {
                Room r = Instantiate(roomPrefabs[i], new Vector3(to.x, 0, to.y), Quaternion.identity).GetComponent<Room>();
                spotsClaimed.Add(to);
                rooms.Add(to, r);
                rooms[to].pos = to;
                i++;
            }
            if (to.x > from.x)
            {
                rooms[to].leftOpen.SetActive(true);
                rooms[to].leftClosed.SetActive(false);
                rooms[from].rightOpen.SetActive(true);
                rooms[from].rightClosed.SetActive(false);
            }
            else if (to.x < from.x)
            {
                rooms[from].leftOpen.SetActive(true);
                rooms[from].leftClosed.SetActive(false);
                rooms[to].rightOpen.SetActive(true);
                rooms[to].rightClosed.SetActive(false);
            }
            else
            {
                rooms[from].upOpen.SetActive(true);
                rooms[from].upClosed.SetActive(false);
                rooms[to].downOpen.SetActive(true);
                rooms[to].downClosed.SetActive(false);
            }
            rooms[to].gameObject.SetActive(false);
        }
    }
}
