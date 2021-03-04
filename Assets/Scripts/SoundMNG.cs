using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SoundMNG : MonoBehaviour
{
    // Audio Source の設定
    [SerializeField]
    private AudioSource MainAudioSource;

    [SerializeField]
    private AudioClip Silent;

    // BGM設定
    [SerializeField]
    private AudioClip[] Sounds;

    public string SoundPath;

    // フェードイン時間の設定（秒）
    [SerializeField]
    private float FadeInTime;

    // フェードアウト時間の設定（秒）
    [SerializeField]
    private float FadeOutTime;

    // 現在のボリューム計算用
    private float SoundTime;

    // MAXボリューム
    private float MaxVol;

    // 再生中の曲番号
    private int SoundNum;

    // BGM状態管理
    public enum BGM_STATE
    {
        WAIT,
        FADE_IN,
        NOW_PLAY,
        FADE_OUT,
        FADE_STOP,
        PAUSE,
        END,
    }
    private BGM_STATE _bgm_state;
    private BGM_STATE _state_work;

		[SerializeField]
		private BycicleController _bycicle_rider;

    //-------------------------------------------------//

    // 数値の初期化
    private void Awake()
    {
        StartCoroutine (StreamPlayAudioFile());

        MainAudioSource.volume = 0.0f;

        SoundTime = 0.0f;
        MaxVol = 1.0f;
        SoundNum = 0;

        _bgm_state = BGM_STATE.WAIT;
        _state_work = BGM_STATE.WAIT;

    }

    // 実行中の処理
    private void Update()
    {
        switch (_bgm_state)
        {
            // フェードイン処理
            case BGM_STATE.FADE_IN:
                if (FadeInTime <= 0.0f)
                {
                    MainAudioSource.volume = MaxVol;
                    _bgm_state = BGM_STATE.NOW_PLAY;
                }
                else if (FadeInTime >= Sounds[SoundNum].length)
                {
                    SoundTime += Time.deltaTime;
                    if (SoundTime >= Sounds[SoundNum].length * 0.9f)
                    {
                        SoundTime = Sounds[SoundNum].length * 0.9f;
                        _bgm_state = BGM_STATE.NOW_PLAY;
                    }
                    MainAudioSource.volume = SoundTime / (Sounds[SoundNum].length * 0.9f) * MaxVol;
                }
                else
                {
                    SoundTime += Time.deltaTime;
                    if(SoundTime >= FadeInTime)
                    {
                        SoundTime = FadeInTime;
                        _bgm_state = BGM_STATE.NOW_PLAY;
                    }
                    MainAudioSource.volume = SoundTime / FadeInTime * MaxVol;
                }
                break;

            // 通常再生中処理
            case BGM_STATE.NOW_PLAY:
                if (Sounds[SoundNum].length - MainAudioSource.time <= FadeOutTime)
                {
                    SoundTime = FadeOutTime;
                    _bgm_state = BGM_STATE.FADE_OUT;
                }
                break;

            // フェードアウト処理
            case BGM_STATE.FADE_OUT:
                if(FadeOutTime > 0.0f)
                {
                    SoundTime -= Time.deltaTime;
                    if (SoundTime <= 0.0f)
                    {
                        SoundTime = 0.0f;
                    }
                    MainAudioSource.volume = SoundTime / FadeOutTime * MaxVol;
                }

                if (!MainAudioSource.isPlaying)
                {
                    _bgm_state = BGM_STATE.END;
                }
                break;

            // フェードアウト停止処理
            case BGM_STATE.FADE_STOP:
                if (FadeOutTime > 0.0f)
                {
                    SoundTime -= Time.deltaTime;
                    if (SoundTime <= 0.0f)
                    {
                        SoundTime = 0.0f;
                        MainAudioSource.Stop();
                    }
                    MainAudioSource.volume = SoundTime / FadeOutTime * MaxVol;
                }
                else
                {
                    StopSoundNow();
                }

                if (!MainAudioSource.isPlaying)
                {
                    _bgm_state = BGM_STATE.END;
                }
                break;

            // BGM_STATE == ENDの場合、WAITに送る

            case BGM_STATE.END:
							_bgm_state = BGM_STATE.WAIT;
						break;                
        }

				// Speedに応じて音楽を変化させる

			// TempoChanger(_bycicle_rider.speed - _bycicle_rider.optimalSpeed + 1.0f );
            TempoChanger(_bycicle_rider.speed / _bycicle_rider.optimalSpeed);
    }

    // 指定番号の再生（-1でランダム再生）
    public void StartSoundNum(int _num)
    {
        // 再生する曲番号の取得
        SoundNum = _num;
        
        // 最大曲数判定
        if (0 <= SoundNum && SoundNum < Sounds.Length)
        {
            MainAudioSource.clip = Sounds[SoundNum];
        }
        else
        {
            // 最大曲数を越える指定の場合　または -1 などが指定された場合はランダムで曲を選ぶ
            SoundNum = UnityEngine.Random.Range(0, Sounds.Length);
            MainAudioSource.clip = Sounds[SoundNum];
        }
        
        MainAudioSource.Stop();
        SoundTime = 0.0f;
        MainAudioSource.volume = 0.0f;
        _bgm_state = BGM_STATE.FADE_IN;
        MainAudioSource.Play();
    }

    // 途中再生（-1でランダム再生）
    public void StartSoundNum(int _num, float _time)
    {
        // 再生する曲番号の取得
        SoundNum = _num;

        // 最大曲数判定
        if (0 <= SoundNum && SoundNum < Sounds.Length)
        {
            MainAudioSource.clip = Sounds[SoundNum];
        }
        else
        {
            // 最大曲数を越える指定の場合　または -1 などが指定された場合はランダムで曲を選ぶ
            SoundNum = UnityEngine.Random.Range(0, Sounds.Length);
            MainAudioSource.clip = Sounds[SoundNum];
        }

        MainAudioSource.Stop();
        MainAudioSource.volume = MaxVol;

        if (_time < Sounds[SoundNum].length)
        {
            MainAudioSource.time = _time;
        }
        else
        {
            Debug.Log("時間指定が曲の最大時間を超えています");
            MainAudioSource.time = 0;
        }

        _bgm_state = BGM_STATE.NOW_PLAY;
        MainAudioSource.Play();
    }

    // 曲の一時停止
    public void SoundPause()
    {
        _state_work = _bgm_state;
        _bgm_state = BGM_STATE.PAUSE;
        MainAudioSource.Pause();
    }

    // 曲の再開
    public void SoundUnPause()
    {
        _bgm_state = _state_work;
        MainAudioSource.UnPause();
    }

    // 即停止
    public void StopSoundNow()
    {
        MainAudioSource.Stop();
        _bgm_state = BGM_STATE.END;
    }

    // フェード停止
    public bool StopSoundFadeOut()
    {
        bool _re = false;

        if (_bgm_state == BGM_STATE.FADE_IN ||
            _bgm_state == BGM_STATE.NOW_PLAY)
        {
            SoundTime = FadeOutTime;
            _bgm_state = BGM_STATE.FADE_STOP;
            _re = true;
        }

        return _re;
    }

    //------------- Get & Set -------------//

    // 現在のステータス取得
    public BGM_STATE GetNowBgmState()
    {
        return _bgm_state;
    }

    // 現在の再生BGM番号の取得
    public int GetSoundNum()
    {
        return SoundNum;
    }

    // 現在の再生時間取得
    public float GetAudioSourceTime()
    {
        return MainAudioSource.time;
    }

    // 現在の曲の最大再生時間の取得
    public float GetNowPlaySoundMaxTime()
    {
        return Sounds[SoundNum].length;
    }

    // フェード時間設定
    public void SetFadeTime(float _InTime,float _OutTime)
    {
        FadeInTime = _InTime;
        FadeOutTime = _OutTime;
    }

    // 最大音量変更
    public void SetMaxVol(float _vol)
    {
        // ボリュームの最大値超過チェック
        if(_vol >= 1.0f)
        {
            _vol = 1.0f;
        }

        // ボリュームの反映
        MaxVol = _vol;

        // 現在のボリュームが最大値以上だった場合変更する
        if(MainAudioSource.volume >= MaxVol)
        {
            MainAudioSource.volume = MaxVol;
        }

        // 通常再生状態でMaxVolが現在のボリューム値を上回ったら変更する
        if(_bgm_state == BGM_STATE.NOW_PLAY)
        {
            if(MainAudioSource.volume < MaxVol)
            {
                MainAudioSource.volume = MaxVol;
            }
        }
    }

    public void TempoChanger (float spd)
    {
        MainAudioSource.pitch = (spd - 1.0f) * 0.25f + 1.0f;
    }

    IEnumerator StreamPlayAudioFile()
    {
        string mPath = GameTitle.GetMusicPath();
        if ( string.IsNullOrEmpty(mPath) ){
            Sounds[0] = Silent;
            yield break;
        }

        Debug.Log(mPath);
        if (mPath.Substring(mPath.Length - 3) == "ogg" || mPath.Substring(mPath.Length - 3) == "OGG") {
            // OGG VORVIS
            using (var uwr = UnityWebRequestMultimedia.GetAudioClip("file:///" + mPath , AudioType.OGGVORBIS )) {
                yield return uwr.SendWebRequest();
                if (uwr.isNetworkError || uwr.isHttpError) {
                    Debug.LogError(uwr.error);
                    yield break;
                }

            Sounds[0] = DownloadHandlerAudioClip.GetContent(uwr);
            // オーディオクリップを使う
            }

        } else if (mPath.Substring(mPath.Length - 3) == "wav" || mPath.Substring(mPath.Length - 3) == "WAV") {

            // WAV 
            using (var uwr = UnityWebRequestMultimedia.GetAudioClip("file:///" + mPath , AudioType.WAV )) {
                yield return uwr.SendWebRequest();
                if (uwr.isNetworkError || uwr.isHttpError) {
                    Debug.LogError(uwr.error);
                    yield break;
                }

            Sounds[0] = DownloadHandlerAudioClip.GetContent(uwr);
            // オーディオクリップを使う
            }

        }
    }

}
