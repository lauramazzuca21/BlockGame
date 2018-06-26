using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public static void SetHighestScore(int score)
    {
		PlayerPrefs.SetInt(Constants.HIGHESTSCORE_KEY, score);
    }

	public static int GetHighestScore()
    {
        return PlayerPrefs.GetInt(Constants.HIGHESTSCORE_KEY);
    }
}
