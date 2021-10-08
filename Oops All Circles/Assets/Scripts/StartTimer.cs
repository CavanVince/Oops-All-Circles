using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTimer : MonoBehaviour
{
    private float timer = 3;
    private bool countingDown;
    private Text timerText;

    private void Start()
    {
        timerText = GetComponent<Text>();
        countingDown = true;
    }

    /// <summary>
    /// Lowers the start game timer
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartCountdown()
    {
        while (countingDown)
        {
            timerText.text = timer.ToString();
            yield return new WaitForSeconds(1);
            timer--;

            if (timer <= 0)
            {
                timerText.text = "Go!";
                countingDown = false;
            }
        }
    }
}
