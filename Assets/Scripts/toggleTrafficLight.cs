using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleTrafficLight : MonoBehaviour {

	[SerializeField]
	private IntersectionSequences intersectionSequences;	// Using DataBase
	private Dictionary<TrafficLightData, int> numOfTraffics = new Dictionary<TrafficLightData, int>(); // 一つの交差点における信号機のDictionaryを作成

	// Use this for initialization
	void Start () {

		TrafficLight = this.gameObject;
		RedLight = transform.GetChild(5).gameObject;
		YellowLight = transform.GetChild(8).gameObject;
		GreenLight = transform.GetChild(1).gameObject;

		PedesRedLight = transform.GetChild(4).gameObject;
		PedesGreenLight = transform.GetChild(3).gameObject;

		FollowArrowLight = transform.GetChild(7).gameObject;
		LeftArrowLight = transform.GetChild(2).gameObject;
		RightArrowLight = transform.GetChild(6).gameObject;

		CarAlloff();
		PedesAlloff();
		ArrowAlloff();

		// 親の取得、参照

		parentIntersection = transform.parent.gameObject;
		interSectionController = parentIntersection.GetComponent<InterSectionController>();
		intersectionSequences = interSectionController.GetIntersectionSequences();
		currentTime =0;

		if (intersectionSequences == null)
		{
			Debug.Log("IntersectionData is not FOUND");			
		}

		trueID = trafficLightID -1 ; // 内部処理用の変数にTrafficLightIDを変換 

	}
	


	// Update is called once per frame
	void Update () {

  // 親のcurrentStep取得
		currentStep = interSectionController.GetCurrentStep();


		// 信号機の状態の反映、(車、歩行者、矢印)
		switch (stateCarLight)
		{
			case "Red": 
				litRed = true;
				litYellow = false;
				litGreen = false;
				break;
			case "Yellow":
				litRed = false;
				litYellow = true;
				litGreen = false;
				break;
			case "Green":
				litRed = false;
				litYellow = false;
				litGreen = true;
				break;
			default:
				CarAlloff();
				break;
		}

		switch (statePedesLight)
		{
			case "Red": 
				litPedesRed = true;
				litPedesGreen = false;
				break;
			case "Yellow":
				litPedesRed = false;

				PedesGreenClock -= Time.deltaTime;
				if (PedesGreenClock <= 0.0)
				{
					PedesGreenClock = 0.3f;
					litPedesGreen = !litPedesGreen;
				}
				break;
			case "Green":
				litPedesRed = false;
				litPedesGreen = true;
				break;
			default:
				PedesAlloff();
				break;
		}

		if (stateArrow / 4 == 1){
			litLeft = true;
		}	else {
			litLeft = false;
		}
		if ((stateArrow % 4) / 2 == 1){
			litFollow = true;
		}	else {
			litFollow = false;
		}
		if ((stateArrow % 2) / 1 == 1){
			litRight = true;
		}	else {
			litRight = false;
		}

		//信号機の点灯、消灯処理
		if (litRed == true)
		{
			RedLight.GetComponent<Renderer>().material = onRed;
		}	else {
			RedLight.GetComponent<Renderer>().material = offRed;
		}
		
		if (litYellow == true)
		{
			YellowLight.GetComponent<Renderer>().material = onYellow;
		}	else {
			YellowLight.GetComponent<Renderer>().material = offYellow;
		}

		if (litGreen == true)
		{
			GreenLight.GetComponent<Renderer>().material = onGreen;
		}	else {
			GreenLight.GetComponent<Renderer>().material = offGreen;
		}

		if (litPedesRed == true)
		{
			PedesRedLight.GetComponent<Renderer>().material = onPedesRed;
		}	else {
			PedesRedLight.GetComponent<Renderer>().material = offPedesRed;
		}

		if (litPedesGreen == true)
		{
			PedesGreenLight.GetComponent<Renderer>().material = onPedesGreen;
		}	else {
			PedesGreenLight.GetComponent<Renderer>().material = offPedesGreen;
		}
		
		if (litFollow == true)
		{
			FollowArrowLight.GetComponent<Renderer>().material = onArrow;
		}	else {
			FollowArrowLight.GetComponent<Renderer>().material = offArrow;
		}
		if (litLeft == true)
		{
			LeftArrowLight.GetComponent<Renderer>().material = onArrow;
		}	else {
			LeftArrowLight.GetComponent<Renderer>().material = offArrow;
		}
		if (litRight == true)
		{
			RightArrowLight.GetComponent<Renderer>().material = onArrow;
		}	else {
			RightArrowLight.GetComponent<Renderer>().material = offArrow;
		}

		// 信号機のステップ処理

		StateController(currentStep);

	}

	// variables

	private float PedesGreenClock; // 歩行者信号の青点滅用

	// ====
	// 信号機のマテリアル
	public Material onRed,onYellow,onGreen,onPedesRed,onPedesGreen,onArrow;
	public Material offRed,offYellow,offGreen,offPedesRed,offPedesGreen,offArrow;

	GameObject TrafficLight;

	// 信号機の電球オブジェクト
	GameObject RedLight,YellowLight,GreenLight,
	PedesRedLight,PedesGreenLight,
	FollowArrowLight,LeftArrowLight,RightArrowLight;

	// ====
	// 信号機の点灯、消灯に関する変数
	private bool litRed,litYellow,litGreen; 
	private bool litPedesRed,litPedesGreen;
	private bool litFollow,litLeft,litRight;

	// ====
	// 信号機の状態 
	public string stateCarLight; // Red,Yellow,Green,Alloff
	public string statePedesLight; // Red,Yellow,Green,Alloff
	public int stateArrow; 
	// 矢印は左、直進、右のそれぞれのOnOffを1,0で示した際に導出される2進数を
	// 10進数int型に置き換えたもの
	// ex. 左、直進が点灯 -> 110 -> 7

	// ====
	// 信号機の識別
	public int trafficLightID;
	private int trueID;

	// ====
	// ステップ
	public int currentTime; //現在のStep内経過時間（秒）
	public int currentStep; //現在のStep

	// 親と付随する変数等の参照
	GameObject parentIntersection;
	InterSectionController interSectionController;

	// ====
	// 全消灯処理
	public void CarAlloff() {
		litRed =false;
		litYellow =false;
		litGreen =false;
	}

	public void PedesAlloff(){
		litPedesRed =false;
		litPedesGreen =false;
	}

	public void ArrowAlloff(){
		stateArrow =0;
		litFollow =false;
		litLeft =false;
		litRight =false;
	}

	public void StateController(int step){
		// 自動車信号機の状態
		switch (intersectionSequences.GetLightDatas()[trueID].GetCarLightSteps(step))
		{
				case 0: stateCarLight = "Red";
				break;
				case 1: stateCarLight = "Yellow";
				break;
				case 2: stateCarLight = "Green";
				break;
		}
		// 歩行者信号機の状態
		switch (intersectionSequences.GetLightDatas()[trueID].GetPedesLightSteps(step))
		{
				case 0: statePedesLight = "Red";
				break;
				case 1: statePedesLight = "Yellow";
				break;
				case 2: statePedesLight = "Green";
				break;
		}
		// 矢印の状態
		stateArrow = intersectionSequences.GetLightDatas()[trueID].GetArrowLightSteps(step);		

	}

}
