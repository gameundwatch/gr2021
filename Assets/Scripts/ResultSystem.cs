using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultSystem : MonoBehaviour
{
	private float _score, _second;
	private int _minute;

	public Text ResultTime;

	// Start is called before the first frame update
	void Start()
	{
		ShowResultTime ();
	}

	// Update is called once per frame
	void Update()
	{
			
	}

	void ShowResultTime ()
	{
	_score = PlayerPrefs.GetFloat("SCORETIME");
	Debug.Log(_score);
	_minute = (int)_score / 60;
	_second = _score % (int)60;

	ResultTime.text = string.Format("{0:00}",_minute) + ":" +  string.Format("{0:00.00}",_second);

	}

}
