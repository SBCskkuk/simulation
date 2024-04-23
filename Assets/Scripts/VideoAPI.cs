using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;
using System.Collections;

public class VideoAPI : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        StartCoroutine(GetVideoUrl());
    }

    IEnumerator GetVideoUrl()
    {
        string apiUrl = "https://your-api-url.com/getVideo";
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            string videoUrl = request.downloadHandler.text;
            videoPlayer.url = videoUrl;
            videoPlayer.Play();
        }
    }
}
