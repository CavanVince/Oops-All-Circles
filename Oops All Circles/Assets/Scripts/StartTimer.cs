using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTimer : MonoBehaviour
{
    private float timer = 3;
    private float fadeTimer = 1;
    private bool countingDown;

    private Text timerText;
    private PlayerController player;

    public PlayerController Player 
    {
        get 
        {
            return player;
        }

        set 
        {
            player = value;
        }
    }

    private void Start()
    {
        timerText = GetComponent<Text>();
        countingDown = true;
        Player = null;
    }

    /// <summary>
    /// Lowers the start game timer
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartCountdown()
    {
        //Decrement the timer and adjust the text to match the time
        while (countingDown)
        {
            timerText.text = timer.ToString();
            yield return new WaitForSeconds(1);
            timer--;

            //If the timer is zero, change the text and start to fade it out
            if (timer <= 0)
            {
                player.enabled = true;
                timerText.text = "Go!";
                countingDown = false;
                StartCoroutine(FadeOut());
            }
        }
    }

    /// <summary>
    /// Fades out the timer text
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeOut() 
    {
        //Loop for a set amount of time and fade the text alpha channel
        while (timerText.color.a > 0) 
        {
            timerText.color = new Color(timerText.color.r, timerText.color.g, timerText.color.b, fadeTimer);
            yield return new WaitForSeconds(.05f);
            fadeTimer -= .1f;
        }

        //If the alpha is zero, remove the text and set the alpha back to 1
        if (timerText.color.a <= 0) 
        {
            timerText.text = "";
            timerText.color = new Color(timerText.color.r, timerText.color.g, timerText.color.b, 1);
        }
    }
}
