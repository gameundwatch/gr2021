using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpeedMeter : MonoBehaviour

{

	public Text TextSpeed;
	public Text TextOptimalSpeed;
	public Text TextTime;
	public Slider SliderLRWeight;
	public GameObject Bicycle;

	private int _minute;
	private float _second;


	// Start is called before the first frame update
	void Start()
	{
			float weightL = -1.0f;
			float weightR =  1.0f;


			//スライダーの最大値の設定
			SliderLRWeight.minValue = weightL;
			SliderLRWeight.maxValue = weightR;

			//スライダーの現在値の設定

			TextSpeed.gameObject.SetActive(false);			
			TextOptimalSpeed.gameObject.SetActive(false);
			SliderLRWeight.gameObject.SetActive(false);
			TextTime.gameObject.SetActive(false);
			

	}

	// Update is called once per frame
	void Update()
	{

		if(Input.GetKeyDown(KeyCode.S) && TextSpeed.IsActive())
		{
			TextSpeed.gameObject.SetActive(false);
		} else if (Input.GetKeyDown(KeyCode.S) && !TextSpeed.IsActive())
		{
			TextSpeed.gameObject.SetActive(true);
		}

		if(Input.GetKeyDown(KeyCode.P) && SliderLRWeight.IsActive())
		{
			SliderLRWeight.gameObject.SetActive(false);
		} else if (Input.GetKeyDown(KeyCode.P) && !SliderLRWeight.IsActive())
		{
			SliderLRWeight.gameObject.SetActive(true);
		}

		if(Input.GetKeyDown(KeyCode.O) && TextOptimalSpeed.IsActive())
		{
			TextOptimalSpeed.gameObject.SetActive(false);
		} else if (Input.GetKeyDown(KeyCode.O) && !TextOptimalSpeed.IsActive())
		{
			TextOptimalSpeed.gameObject.SetActive(true);
		}

		_minute = (int)Bicycle.GetComponent<BycicleController>().travelledTime / 60;
		_second = Bicycle.GetComponent<BycicleController>().travelledTime % 60;

		// 5 units (Bezier Path) = 1 meter
		TextSpeed.text = string.Format("{0:000}",Bicycle.GetComponent<BycicleController>().speed);
		TextOptimalSpeed.text = string.Format("{0:000}",Bicycle.GetComponent<BycicleController>().optimalSpeed);
		SliderLRWeight.value = Bicycle.GetComponent<BycicleController>().posLR;
		TextTime.text = string.Format("{0:00}",_minute) + ":" +  string.Format("{0:00.00}",_second);

	}
}
