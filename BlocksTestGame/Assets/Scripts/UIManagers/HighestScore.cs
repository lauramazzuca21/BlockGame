using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class HighestScore : MonoBehaviour
{
	[SerializeField]
	private Text _scoreText;
	// Use this for initialization
    void Start()
    {
		_scoreText.text = PlayerPrefsManager.GetHighestScore().ToString();
    }
}
