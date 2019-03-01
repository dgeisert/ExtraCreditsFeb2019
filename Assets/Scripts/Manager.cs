using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance;
    public static int dayNight = 0;
    public static int season = 0;
    public static List<SeasonsObject> seasonsObjects = new List<SeasonsObject>();
    public float timeInDay = 30;
    public Animator anim;
    float dayTime;
    // Start is called before the first frame update
    void Start()
    {
        dayTime = Time.time;
        Set();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Switch", false);
        if (dayTime + timeInDay < Time.time)
        {
            dayTime = Time.time;
            Transition();
        }
    }

    void Transition()
    {
        if (dayNight == 1)
        {
            season++;
            if (season == 4)
            {
                season = 0;
            }
            dayNight = 0;
            anim.SetInteger("Season", season);
        }
        else
        {
            dayNight = 1;
        }
        anim.SetInteger("DayNight", dayNight);
        anim.SetBool("Switch", true);
        Set();
    }

    void Set()
    {
        foreach (SeasonsObject so in seasonsObjects)
        {
            so.Set();
        }
    }
}
