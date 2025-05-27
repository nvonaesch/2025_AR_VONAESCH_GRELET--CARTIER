using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;


public class VideoPlanePlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RawImage rawImage;

    void Start()
    {
        rawImage.enabled = false; 
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    public void PlayVideo()
    {
        Debug.Log("On est dans le script PlayVideo");
        rawImage.enabled = true;
        videoPlayer.Play();
        Debug.Log("Vidéo lancée... en théorie");
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        videoPlayer.Stop();   
        rawImage.enabled = false;
    }

    public void StopVideo()
    {
        videoPlayer.Stop();
        rawImage.enabled = false;
    }
}


