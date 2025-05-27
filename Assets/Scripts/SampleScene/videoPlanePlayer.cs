using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlanePlayer : MonoBehaviour
{
    public GameObject videoPlane;    
    public float distanceFromCamera = 2.0f;

    private VideoPlayer videoPlayer;

    void Start()
    {
        Camera cam = Camera.main;
        videoPlane.transform.position = cam.transform.position + cam.transform.forward * distanceFromCamera;
        videoPlane.transform.rotation = Quaternion.LookRotation(-cam.transform.forward);

        videoPlayer = videoPlane.GetComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.loopPointReached += OnVideoFinished;

        videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Destroy(videoPlane);
    }
}
