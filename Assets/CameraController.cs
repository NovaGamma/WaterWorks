using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;
    public float movementSpeed;
    public float movementTime;
    public Vector3 newPosition;
    public Vector3 zoomAmount;
    public Vector3 newZoom;
    
    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;
    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
        newZoom = cameraTransform.localPosition;

        
    }

    // Update is called once per frame
    void Update()
    {
        HandledMovementInput();
        HandleMouseInput();
        
    }

    void HandleMouseInput(){
        if(Input.mouseScrollDelta.y != 0){
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }
        if(Input.GetMouseButtonDown(0)){
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;
            if(plane.Raycast(ray, out entry)){
                dragStartPosition = ray.GetPoint(entry);
            }
        }

         if(Input.GetMouseButton(0)){
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;
            if(plane.Raycast(ray, out entry)){
                dragCurrentPosition = ray.GetPoint(entry);
                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }

    }

    void HandledMovementInput(){

    if(Input.GetKey(KeyCode.UpArrow)){
       newPosition += (transform.forward * movementSpeed);
    }
    if(Input.GetKey(KeyCode.DownArrow)){
       newPosition += (transform.forward * -movementSpeed);
    }

    transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime *movementTime);
    }
}
