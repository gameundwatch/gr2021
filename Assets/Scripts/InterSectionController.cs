using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterSectionController : MonoBehaviour {

	[SerializeField]
	private IntersectionSequences intersectionSequences;	// Using DataBase

	// Use this for initialization
	void Start () {
		clock = 0.0f;
		lightStates   = new int[intersectionSequences.GetLightsV()];

		remainRedTime = new float[intersectionSequences.GetLightsV()];
		DefaultRemainRedTime = new float[intersectionSequences.GetLightsV()];

		remainGreenTime = new float[intersectionSequences.GetLightsV()];
		DefaultRemainGreenTime = new float[intersectionSequences.GetLightsV()];


		for (int i = 0; i < remainRedTime.Length; i++) {
			for (int step = 0; step < intersectionSequences.havingLights[i].GetStepsSize() ; step++ ){
				if (intersectionSequences.havingLights[i].GetCarLightSteps(step) != 2 && intersectionSequences.havingLights[i].GetArrowLightSteps(step) == 0 ) // 矢印なし、かつ青信号でない場合
				{
					DefaultRemainRedTime[i] += intersectionSequences.GetStepTime(step); // ステップの時間を加算する
				}
			}
			// Debug.Log( intersectionSequences.GetName() + i + " : " + remainRedTime[i]);
		}

		for (int i = 0; i < remainGreenTime.Length; i++) {
			for (int step = 0; step < intersectionSequences.havingLights[i].GetStepsSize() ; step++ ){
				if (intersectionSequences.havingLights[i].GetCarLightSteps(step) == 2 ) // 青信号の場合
				{
					DefaultRemainGreenTime[i] += intersectionSequences.GetStepTime(step); // ステップの時間を加算する
				}
			}
			// Debug.Log( intersectionSequences.GetName() + i + " : " + remainGreenTime[i]);
		}

		for( int i =0 ; i < remainRedTime.Length ; i++ ) {
			remainRedTime[i] = DefaultRemainRedTime[i];
		}
		for( int i =0 ; i < remainRedTime.Length ; i++ ) {
			remainGreenTime[i] = DefaultRemainGreenTime[i];
		}

	}
	
	// Update is called once per frame
	void Update () {
		
		clock += Time.deltaTime;
		StepUp();
		currentClock = (int)clock;

		// 赤信号の残り秒数計算

		for( int i =0 ; i < remainRedTime.Length ; i++ ) {
			if ( intersectionSequences.havingLights[i].GetCarLightSteps(currentStep) == 2 ) {
				// 青信号の場合にはデフォルト値にリセット
				remainRedTime[i] = DefaultRemainRedTime[i];
			} else if (intersectionSequences.havingLights[i].GetCarLightSteps(currentStep) != 2) {
				// 赤信号の場合には秒おきに減る
				remainRedTime[i] -= Time.deltaTime;
			}
		}

		// 青信号の残り秒数計算

		for( int i =0 ; i < remainGreenTime.Length ; i++ ) {
			if ( intersectionSequences.havingLights[i].GetCarLightSteps(currentStep) == 2 ) {
				// 赤信号の場合には秒おきに減る
				remainGreenTime[i] -= Time.deltaTime;
			} else if (intersectionSequences.havingLights[i].GetCarLightSteps(currentStep) != 2) {
				// 青信号の場合にはデフォルト値にリセット
				remainGreenTime[i] = DefaultRemainGreenTime[i];

			}
		}

		CatchLightStates();

	}

	private float clock;

	public int currentStep;
	public int currentClock;

	[SerializeField]
	private int[] lightStates;

	[SerializeField]
	private float[] DefaultRemainRedTime;
	[SerializeField]
	private float[] remainRedTime;
	
	[SerializeField]
	private float[] DefaultRemainGreenTime;
	[SerializeField]
	private float[] remainGreenTime;


	public int GetCurrentStep(){
		return currentStep;
	}
	public IntersectionSequences GetIntersectionSequences(){
		return intersectionSequences;
	}

	public void StepUp () {
		if(clock > intersectionSequences.GetStepTime(currentStep)) {
			if (currentStep +1 == intersectionSequences.GetStepTime().Length)
			{
				currentStep =0;
			} else {
				currentStep +=1;
			}
			clock = 0.0f;
		}
	}

	public void CatchLightStates () {

		for (int i = 0; i < lightStates.Length; i++) {
			lightStates[i] = intersectionSequences.havingLights[i].GetCarLightSteps(currentStep);
		}

	}

	public int GetLightStates (int direction) {
		return lightStates[direction];
	}

	public float GetRRT (int direction) {
		return remainRedTime[direction];
	}
	public float GetRGT (int direction) {
		return remainGreenTime[direction];
	}

}
