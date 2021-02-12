using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

[SerializeField]
private SoundMNG _music;

	// Update is called once per frame
	void Update()
	{
		
		if(_music.GetNowBgmState() == SoundMNG.BGM_STATE.WAIT){
			_music.StartSoundNum(0);
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
				Debug.Log("現在のステータス　　　:" + _music.GetNowBgmState());
				Debug.Log("再生中の曲番号　　　　:" + _music.GetSoundNum());
				Debug.Log("現在の再生時間　　　　:" + _music.GetAudioSourceTime());
				Debug.Log("現在の曲の最大再生時間:" + _music.GetNowPlaySoundMaxTime());
		}

	}
}
