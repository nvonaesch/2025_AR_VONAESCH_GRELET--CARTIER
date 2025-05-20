using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class jeterBoule : MonoBehaviour
{
    public GameObject ballPrefab;
    public float throwForce = 3f;

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

                case TouchPhase.Moved:
                    if (currentBall != null)
                    {
                        Vector3 touchPosition = new Vector3(touch.position.x, touch.position.y, 0.5f);
                        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
                        worldPosition.x += 0.05f;
                        worldPosition.y += 0.05f;
                        currentBall.transform.position = worldPosition;
                    }
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
        Vector3 touchPosition = new Vector3(startTouchPos.x, startTouchPos.y, 0.5f);
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(touchPosition);

        spawnPosition.x += 0.05f;
        spawnPosition.y += 0.05f;

        currentBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        currentBall.tag = "Ball";

        currentRb = currentBall.GetComponent<Rigidbody>();
        if (currentRb != null)
            currentRb.isKinematic = true;
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
