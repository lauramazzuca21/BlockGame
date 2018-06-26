using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	[SerializeField]
    private Text _scoreText;

	private Timer _timer; //variable needed so that we can access the CurrentCoutdownValue and modify it depending on the tiles cleared
	private int _score;
	private int _highestScore; /* variable to check runtime if the present score is the highest
	                            * the value of highest score is stored with PlayerPrefs and retrived at the beginning
	                            * of the game. If the current score is greater than the one stored, it is updated. */

    // Use this for initialization
    void Start()
    {
		_score = 0;
		UpdateScoreText();

		_highestScore = PlayerPrefsManager.GetHighestScore();

		_timer = FindObjectOfType<Timer>();
        StartCoroutine(_timer.StartCountdown(Constants.BASE_COUNTDOWN));
    }

    public void UpdateScoreAndTimer(int tilesCleared)
    {
		_score += ((tilesCleared - 1) * 80) + (((tilesCleared - 2) / 5) ^ 2);

		if (_score > _highestScore) PlayerPrefsManager.SetHighestScore(_score);

		UpdateScoreText();

		float power = Mathf.Pow((tilesCleared - 2f) / 3f, 2f);
		_timer.CurrentCountdownValue += 10f + (power * 20f);
       
    }

	private void UpdateScoreText() 
	{
		_scoreText.text = _score.ToString();
	}
}
