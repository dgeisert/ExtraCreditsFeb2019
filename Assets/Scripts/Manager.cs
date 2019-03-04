using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager instance;
    public static int dayNight = 0;
    public static int season = 0;
    public float timeInDay = 30;
    public Animator anim;
    public Room currentRoom;
    public Image fader;
    public TMPro.TextMeshProUGUI gameOverText, victoryText;
    public GameObject changeSeasonChime;
    float transitionTime;
    public GameObject gameover, music, victory;
    public bool isGameover = false;
    // Start is called before the first frame update
    void Start()
    {
        dayNight = 0;
        season = 0;
        instance = this;
        currentRoom.Invoke("Set", 0.1f);
        StartCoroutine(FadeIn());
    }
    IEnumerator FadeIn()
    {
        fader.gameObject.SetActive(true);
        while (fader.color.a > 0)
        {
            fader.color -= new Color(0, 0, 0, 0.02f);
            yield return null;
        }
        fader.gameObject.SetActive(false);
    }
    public void GameOver()
    {
        if (!isGameover)
        {
            isGameover = true;
            gameover.SetActive(true);
            music.SetActive(false);
            StartCoroutine(FadeOut());
            gameOverText.gameObject.SetActive(true);
        }
    }
    public void Victory()
    {
        if (!isGameover)
        {
            isGameover = true;
            victory.SetActive(true);
            music.SetActive(false);
            StartCoroutine(FadeOut());
            victoryText.gameObject.SetActive(true);
        }
    }
    IEnumerator FadeOut()
    {
        fader.gameObject.SetActive(true);
        while (fader.color.a < 1)
        {
            fader.color += new Color(0, 0, 0, 0.02f);
            yield return null;
        }
        yield return new WaitForSeconds(5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
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
        if (transitionTime + 1f < Time.time)
        {
            changeSeasonChime.SetActive(false);
            changeSeasonChime.SetActive(true);
            transitionTime = Time.time;
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
}
