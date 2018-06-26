using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour {

	[SerializeField]
	private Text TimerText;

	public float CurrentCountdownValue { get; set; }
       
	public IEnumerator StartCountdown(float countdownValue)
    {
        CurrentCountdownValue = countdownValue;
		UpdateTimerText();
		while (CurrentCountdownValue > 0)
        {
            yield return new WaitForSeconds(1.0f);
			CurrentCountdownValue--;
			UpdateTimerText();
        }
        
		//Once the countdown gets to 00:00 the LevelManager method to load the GameOver Screen is called
		FindObjectOfType<LevelManager>().LoadScene(Constants.LOST_SCREEN);
    }
    
	private void UpdateTimerText()
	{
		int min = Mathf.FloorToInt(CurrentCountdownValue / 60);
		int sec = Mathf.RoundToInt(CurrentCountdownValue % 60);

		string minutes = min.ToString();
		string seconds = sec.ToString();
       
		if (min < 10) minutes = "0" + minutes;
		if (sec < 10) seconds = "0" + seconds;

		TimerText.text = minutes + ":" + seconds;
       
	}
}
