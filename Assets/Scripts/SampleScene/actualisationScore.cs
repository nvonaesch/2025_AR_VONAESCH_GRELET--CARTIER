using UnityEngine;
using TMPro ;

public class actualisationScore : MonoBehaviour
{
    public TextMeshProUGUI textDisplay ;
    private int score = 0 ;

    void Start()
    {
        UpdateText() ;
    }
    void Update()
    {
       if (Input.touchCount > 0)
       {
        Touch touch = Input.GetTouch(0) ;
        if  (touch.phase == TouchPhase.Began)
        {
            score +=1 ;
            UpdateText() ;
        }
       }

       if (Application.isEditor && Input.GetMouseButtonDown(0))
       {
            score +=1 ;
            UpdateText() ;
       }
    }

    void UpdateText()
    {
        if (textDisplay != null)
        {
            textDisplay.text = "Score : " + score ;
        }
        else
        {
            Debug.LogWarning("TextMesh non assigné") ;
        }
    }
}
