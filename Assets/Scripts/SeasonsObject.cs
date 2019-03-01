using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonsObject : MonoBehaviour
{
    public int season = -1, day = -1;

    void Start()
    {
        Manager.seasonsObjects.Add(this);
    }
    public void Set()
    {
        bool set = season < 0 || season == Manager.season;
        set = set && (day < 0 || day == Manager.dayNight);
        gameObject.SetActive(set);
    }
}
