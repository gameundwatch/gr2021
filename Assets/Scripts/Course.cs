using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Course : MonoBehaviour
{

  [SerializeField]
  private float[] _sections; // 通過する交差点のPathCreator上の位置

  [SerializeField]
  public GameObject[] _intersections; // 通過する交差点のGameObjectを格納

  [SerializeField]
  private int[] _using_paths_ID; // 通過する交差点における通過点のtrafficLightsIDを格納

  [SerializeField]
  private int[] _passable_ArrowLight; //通過可能な矢印信号について格納;

  void Start()
  {

  }

  void Update()
  {

  }

  public float GetSection(int i) {
		return _sections[i];
	}
	public GameObject GetInterSection(int i){
		return _intersections[i];
	}
	public int GetUsingPath(int i){
    // 配列内データと一致させるため -1
		return _using_paths_ID[i] - 1;
	}

  public int GetSectionsValue() {
    return _sections.Length;
  }

}
