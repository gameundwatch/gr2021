using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrafficLightData", menuName = "GraduationResearch/TrafficLightData")]
public class TrafficLightData : ScriptableObject {

	[SerializeField]
	private int lightID;

	// ステップの内容
	// 信号機の色は Red = 0 Yellow = 1 Green = 2;
	// 矢印は左、直進、右のそれぞれのOnOffを1,0で示した際に導出される2進数を
	// 10進数int型に置き換えたもの
	// 左、直進が点灯 -> 110 -> 7

	[SerializeField]
	private int[] carLightSteps;

	[SerializeField]
	private int[] pedesLightSteps;

	[SerializeField]
	private int[] arrowLightSteps;

	// Getter

	public int GetLightID(){
		return lightID;
	}

	public int GetCarLightSteps(int step){
		return carLightSteps[step];
	}
	public int GetPedesLightSteps(int step){
		return pedesLightSteps[step];
	}
	public int GetArrowLightSteps(int step){
		return arrowLightSteps[step];
	}

	public int GetStepsSize() {
		return carLightSteps.Length;
	}

}
