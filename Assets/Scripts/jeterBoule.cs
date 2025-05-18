using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

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

        //ajout
        Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;

        GameObject ball = Instantiate(ballPrefab, spawnPosition/*spawnPoint.position*/, Quaternion.identity);
        ball.tag = "Ball";
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.TransformDirection(direction) * throwForce, ForceMode.Impulse);

        Destroy(ball, 3f);
    }
}
