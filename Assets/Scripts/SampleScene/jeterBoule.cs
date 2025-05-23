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
                        Vector3 touchPosition = new Vector3(touch.position.x, touch.position.y, 0.6f);
                        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
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

        // Réduire l'impact du swipe horizontal/vertical
        float swipeX = Mathf.Clamp(swipe.x * 0.001f, -0.5f, 0.5f);  // moins de courbure latérale
        float swipeY = Mathf.Clamp(swipe.y * 0.002f, 0f, 1f);       // un peu d'effet de "lancer vers le haut"

        Vector3 direction = Camera.main.transform.forward;
        direction += Camera.main.transform.right * swipeX;   // Courbure horizontale limitée
        direction += Camera.main.transform.up * swipeY;      // Lancer vers le haut

        currentRb.AddTorque(Vector3.right * 5f, ForceMode.Impulse);
        currentRb.isKinematic = false;
        currentRb.AddForce(direction.normalized * throwForce, ForceMode.Impulse);

        Destroy(currentBall, 3f);

        currentBall = null;
        currentRb = null;
    }

    void OnDestroy()
    {
        
    }

}
