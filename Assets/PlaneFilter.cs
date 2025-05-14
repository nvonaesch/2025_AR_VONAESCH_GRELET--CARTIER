using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation ;


[RequireComponent(typeof(ARPlaneManager))]
public class PlaneFilter : MonoBehaviour
{

    public ARPlaneManager planeManager ;

    [SerializeField] private float minFloorHeight = 1.0f ;
    [SerializeField] private float normalThreshold = 0.9f ;
    
    void Awake(){
        planeManager = GetComponent<ARPlaneManager>() ;
    }

    void OnEnable(){
        planeManager.planesChanged += OnPlanesChanged ;
    }

    void OnDisable(){
        planeManager.planesChanged -= OnPlanesChanged ;
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args){
        foreach (var plane in args.added){
            FilterPlane(plane) ;
        }
        foreach(var plane in args.updated){
            FilterPlane(plane) ;
        }
    }

    void FilterPlane(ARPlane plane){
        Vector3 normal = plane.transform.up ;
        float dotUp = Vector3.Dot(normal, Vector3.up) ;
        float yPos = plane.transform.position.y ;

        bool isFloor = dotUp > normalThreshold && yPos < minFloorHeight ;
        bool isWall = Mathf.Abs(dotUp) < (1.0f - normalThreshold) ;
        
        if (isFloor || isWall){
            plane.gameObject.SetActive(true) ;
        }
        else{
            plane.gameObject.SetActive(false) ;
        }
    }
}
