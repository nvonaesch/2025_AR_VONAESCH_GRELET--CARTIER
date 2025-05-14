using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jeterBoule : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnPoint;
    public float throwForce = 10f;

    private Vector2 startTouchPos;
    private Vector2 endTouchPos;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPos = touch.position;
                    break;

                case TouchPhase.Ended:
                    endTouchPos = touch.position;
                    ThrowBall();
                    break;
            }
        }
    }

    void ThrowBall()
    {
        Vector2 swipe = endTouchPos - startTouchPos;
        Vector3 direction = new Vector3(swipe.x, swipe.y, 1).normalized;

        GameObject ball = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.TransformDirection(direction) * throwForce, ForceMode.Impulse);
    }
}
