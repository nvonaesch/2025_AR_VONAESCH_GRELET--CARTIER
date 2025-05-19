using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class jeterBoule : MonoBehaviour
{
    public GameObject ballPrefab;
    public float throwForce = 10f;

    private Vector2 startTouchPos;
    private Vector2 endTouchPos;

    private GameObject currentBall;
    private Rigidbody currentRb;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPos = touch.position;
                    SpawnBall();
                    break;

                case TouchPhase.Ended:
                    endTouchPos = touch.position;
                    ThrowBall();
                    break;
            }
        }
    }

    void SpawnBall()
    {
        // Position devant la caméra en bas de l’écran
        Vector3 screenBottom = new Vector3(Screen.width / 2, 1.5f, 0.5f);
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(screenBottom);

        currentBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        currentBall.tag = "Ball";

        currentRb = currentBall.GetComponent<Rigidbody>();
        if (currentRb != null)
            currentRb.isKinematic = true; // Empêche qu'elle tombe pendant le swipe
    }

    void ThrowBall()
    {
        if (currentBall == null || currentRb == null) return;

        Vector2 swipe = endTouchPos - startTouchPos;

        Vector3 swipeDirection = new Vector3(swipe.x, swipe.y, 0);
        Vector3 worldDirection = Camera.main.transform.forward + Camera.main.transform.TransformDirection(swipeDirection * 0.01f);

        currentRb.isKinematic = false;
        currentRb.AddForce(worldDirection.normalized * throwForce, ForceMode.Impulse);

        Destroy(currentBall, 3f);

        currentBall = null;
        currentRb = null;
    }
}
