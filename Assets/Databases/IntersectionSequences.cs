using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntersectionSequences", menuName = "GraduationResearch/IntersectionSequences")]
public class IntersectionSequences : ScriptableObject {

// プライベート変数

	[SerializeField]
	private int intersectionID; // 交差点の識別ID 

	[SerializeField]
	private string intersectionName; // 交差点の識別名

	[SerializeField]
	private int stepsValue; // ステップ数

	[SerializeField]
	private int lightsValue; // 信号機数

	[SerializeField]
	private int[] stepTime; // ステップ推移時間（秒）

	public List<TrafficLightData> havingLights = new List<TrafficLightData>();
	
// Getter

	public int GetID(){
		return intersectionID; // 内部処理では0からスタートするため、GetIDでは-1した値を返す。
	}
	public string GetName(){
		return intersectionName;
	}
	public int GetStepsV(){
		return stepsValue;
	}
	public int GetLightsV(){
		return lightsValue;
	}
	public int GetStepTime(int step){
		return stepTime[step];
	}
	public int[] GetStepTime(){
		return stepTime;
	}

	public List<TrafficLightData> GetLightDatas () {
		return havingLights;
	}

}
