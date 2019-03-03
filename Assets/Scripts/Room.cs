using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject upOpen, downOpen, leftOpen, rightOpen;
    public GameObject upClosed, downClosed, leftClosed, rightClosed;
    public Vector2Int pos;
    public List<SeasonsObject> seasonsObjects = new List<SeasonsObject>();
    public List<Enemy> enemies = new List<Enemy>();

    public void Set()
    {
        foreach (SeasonsObject so in seasonsObjects)
        {
            so.Set();
        }
        foreach (Enemy e in enemies)
        {
            if (e != null)
            {
                e.Set();
            }
        }
    }
}
