using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectData : MonoBehaviour
{
	public GameObject bicycle;

	private List<float> SpeedLog = new List<float>();
	private List<float> OptimalSpeedLog = new List<float>();

  // Start is called before the first frame update
  void Start()
  {
		SpeedLog.Clear();
		OptimalSpeedLog.Clear();
  }

  // Update is called once per frame

  void Update()
  {
		SpeedLog.Add(bicycle.GetComponent<BycicleController>().speed);
		OptimalSpeedLog.Add(bicycle.GetComponent<BycicleController>().optimalSpeed);
  }

	public float GetSpeedLog(int i) {
		return SpeedLog[i];
	}
	public float GetOptimalSpeedLog(int j) {
		return OptimalSpeedLog[j];
	}

	public int GetLogLength(){
		return SpeedLog.Count;
	}

}
