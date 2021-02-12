using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SoundTest : MonoBehaviour
{
    [SerializeField]
    AudioSource MusicSource;
    [SerializeField]
    AudioClip Silent;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine (CheckImportedMusic());

    }

    // Update is called once per frame
    void Update()
    {

        if (!MusicSource.isPlaying)
            MusicSource.Play();

        if (GameTitle.GetCallToggle()){
            GameTitle.callToggle = false;
            StartCoroutine (CheckImportedMusic());
        }

    }

    IEnumerator CheckImportedMusic()
    {
        string mPath = "";
        mPath += GameTitle.GetMusicPath();

        if(mPath.Length < 4){
            mPath = "";
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

            MusicSource.clip = DownloadHandlerAudioClip.GetContent(uwr);
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

            MusicSource.clip = DownloadHandlerAudioClip.GetContent(uwr);
            // オーディオクリップを使う
            }


        }

    }

    public void CallCheckImportMusic () {
            StartCoroutine (CheckImportedMusic());
  
    }

}
