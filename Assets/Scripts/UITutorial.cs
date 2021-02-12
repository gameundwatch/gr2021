using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITutorial : MonoBehaviour
{

	public Text TextTutorial;
	public GameObject Bicycle;

	private float _dt;
	private bool _playonce;

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{

		_dt = Bicycle.GetComponent<BycicleController>().distanceTravelled;

		if ( _dt < 10f )
		{
			TextTutorial.text = "Press LSHIFT or RSHIFT.";
		}

		if ( 10f <= _dt && _dt < 300f )
		{
			TextTutorial.text = "By pressing the left and right shift keys alternately, the bike will move forward.";
		}

		if ( 300f <= _dt && _dt < 480f )
		{
			TextTutorial.text = "Press SPACE to put the breaks.";
		}

		if ( 480f <= _dt && _dt < 600f )
		{
			TextTutorial.text = "The handle is automatic.";
		}

		if ( 600f <= _dt && _dt < 700f )
		{
			TextTutorial.text = "Next, try driving accurately.";
		}

		if (700f <= _dt && _dt < 800f )
		{
			TextTutorial.text = "As you heard it, The music is changed by your bicycle speed.";
		}

		if (800f <= _dt && _dt < 1000f )
		{
			TextTutorial.text = "if you too fast, the pitch is up.";
		}

		if (1000f <= _dt && _dt < 1200f )
		{
			TextTutorial.text = "if you too slow, the pitch is down.";
		}

		if (1200f <= _dt && _dt < 1400f )
		{
			TextTutorial.text = "Reduce the amount of time you spend stationary at traffic lights and get through the course as accurately \nand quickly as possible.";
		}

		if (1400 <= _dt )
		{
			TextTutorial.text = "";
		}
		
	}

}
