using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroText : MonoBehaviour
{
    [TextArea]
    public string[] texts;
    int currentlyOn = 0;
    public TMPro.TextMeshProUGUI text;
    bool typing = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TypeText());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (typing)
            {
                StopAllCoroutines();
                text.text = texts[currentlyOn];
                typing = false;
            }
            else
            {
                currentlyOn++;
                if (currentlyOn >= texts.Length)
                {
                    Destroy(gameObject);
                }
                else
                {
                    StartCoroutine(TypeText());
                }
            }
        }
        switch (currentlyOn)
        {
            case 0:
                break;
            default:
                break;
        }
    }

    IEnumerator TypeText()
    {
        typing = true;
        char[] chars = texts[currentlyOn].ToCharArray();
        text.text = "";
        while (text.text.Length < chars.Length)
        {
            text.text += chars[text.text.Length];
            yield return null;
        }
        typing = false;
    }
}
