using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance;
    public static int dayNight = 0;
    public static int season = 0;
    public float timeInDay = 30;
    public Animator anim;
    public Room currentRoom;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentRoom.Invoke("Set", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Switch", false);
        if (Input.GetKeyDown(KeyCode.Q))
        {
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
        currentRoom.Set();
    }
}
