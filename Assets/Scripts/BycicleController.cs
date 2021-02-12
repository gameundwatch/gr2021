using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PathCreation;

public class BycicleController : MonoBehaviour {

	// チェックポイント系
	public PathCreator bicycleRoad;
	public EndOfPathInstruction end;

	public Course bicycleCourse;

	[SerializeField]
	private float _endpoint;

	public bool nextInterSectionState; // 0 = RED, 1 = GREEN

	[SerializeField]
	public int nextSection;

	public float distanceTravelled; // 移動距離
	public float accel; // 加速
	public float speed;
	public float friction;

	public float brake;
	public float posLR; 
	// ペダルの挙動 -1.0f から 1.0f までのfloat型であり、左のペダルが下がると減算、右のペダルが下がると加算
	public float weight;
	public float gearTorque;
	public float reactance;

	// 最適スピード計算
	public float optimalSpeed;

	public float travelledTime;

	public FileOutput fileOutput;
	public CollectData collectData;

	// Use this for initialization
	void Start () {

		posLR = 0.0f;
		accel = 0.0f;
		speed = 0.0f;
		friction = 0.002f;
		gearTorque = 0.1f;
		reactance = 0.5f;
		weight = 0.6f;
		brake = 0f;

		travelledTime = 0f;

		this.transform.position = bicycleRoad.path.GetPoint(1);
		_endpoint = bicycleRoad.path.length;

	}
	
	// Update is called once per frame
	void Update () {

		Clock();
		Pedal();
		Brake();
		Move();
		CalcOptimalSpeed();

		InterSectionController nextISC;

		if( nextSection < bicycleCourse.GetSectionsValue() ){
			nextISC = bicycleCourse.GetInterSection(nextSection).GetComponent<InterSectionController>();
			if(distanceTravelled >= bicycleCourse.GetSection(nextSection)){
				if(nextISC.GetLightStates(bicycleCourse.GetUsingPath(nextSection)) == 0)
				{
					speed = 0;
					accel = 0;
					distanceTravelled = bicycleCourse.GetSection(nextSection) - 10;
				}
				else
				{
					nextSection += 1;
				}
				Debug.Log(nextSection);
			}
		}

		if(distanceTravelled >= _endpoint)
		{
			// スコア保存
			PlayerPrefs.SetFloat("SCORETIME", travelledTime);
			PlayerPrefs.Save();

			//ログ出力
			if(SceneManager.GetActiveScene().name == "MainInterSection"){
				CollectDataAndWrite();
			}
			// シーン遷移
			SceneManager.LoadScene("ResultWindow");
			
		}

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			SceneManager.LoadScene("Menu");
		}

	}

	void Pedal(){

    if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift) )
    {
			if (posLR > -1.0f)
			{
				posLR -= weight * accel;
				accel += gearTorque;
			}
			if (posLR < -1.0f)
				posLR = -1.0f;
    }
    else if (Input.GetKey(KeyCode.RightShift) && !Input.GetKey(KeyCode.LeftShift))
		{
			if (posLR < 1.0f)
			{
				posLR += weight * accel;
				accel += gearTorque;
			}
			if (posLR > 1.0f)
				posLR = 1.0f;
		}

		if (accel >= 0.0f){
		accel = accel - (accel * reactance);
		} else {
			accel = 0.0f;
		}

	}

	void Brake(){

    if (Input.GetKey(KeyCode.Space))
		{
			brake += 0.001f;
			if (speed > 0.1f){
				speed -= (speed * brake);

			} else {
				speed = 0f;
			}
		} else {
			brake = 0f;
		}

	}

	void Move(){

		if (speed >= 0.0f){
		speed += accel;
		speed -= speed * friction;
		} else {
			speed = 0f;
		}

		distanceTravelled += speed * Time.deltaTime;
		transform.position = bicycleRoad.path.GetPointAtDistance(distanceTravelled, end);
		transform.rotation = bicycleRoad.path.GetRotationAtDistance(distanceTravelled, end);

	}

	void CalcOptimalSpeed(){

		if (nextSection != bicycleCourse.GetSectionsValue() ){

			GameObject nextInterSection = bicycleCourse.GetInterSection(nextSection);
			InterSectionController nextISC = nextInterSection.GetComponent<InterSectionController>();
			IntersectionSequences nextISS = nextISC.GetIntersectionSequences();

			float nextRRT; // 次の信号の赤信号終了までの時間
			float nextRGT;
			
			if( nextISC.GetLightStates(bicycleCourse.GetUsingPath(nextSection)) == 2)
			{
				// 次の信号が青信号である場合 if next signal is GREEN
				nextRGT = nextISC.GetRGT(bicycleCourse.GetUsingPath(nextSection));
				optimalSpeed = ( bicycleCourse.GetSection(nextSection) - distanceTravelled ) / nextRGT;

				if (optimalSpeed < speed)
				{
						optimalSpeed = speed;
				}
			} 
			else if (nextISC.GetLightStates(bicycleCourse.GetUsingPath(nextSection)) != 2)
			{
				// 次の信号が青信号でない場合 if next signal is RED
				nextRRT = nextISC.GetRRT(bicycleCourse.GetUsingPath(nextSection));
				optimalSpeed = ( bicycleCourse.GetSection(nextSection) - distanceTravelled ) / nextRRT;
				if (optimalSpeed > speed)
				{
						optimalSpeed = speed;
				}
			}

		}
		else 
		{
			optimalSpeed = speed;
		}

	}

	void Clock() {
		travelledTime += Time.deltaTime;
	}

	void CollectDataAndWrite()
	{
		int _i =0;
		string _log = "time:," + travelledTime + "\nframe,Speed,OptimalSpeed\n";

		while ( collectData.GetLogLength() != _i )
		{
			_log += ( _i + "," + collectData.GetSpeedLog(_i) + "," + collectData.GetOptimalSpeedLog(_i) + "\n" ); 
			_i++;
		}

					// ファイル出力
			fileOutput.CSVSave( _log , "SpeedLog");

	}

}
