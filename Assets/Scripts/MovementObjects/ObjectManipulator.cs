using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManipulator : MonoBehaviour
{
    public GameObject ARObject;
    [SerializeField] private Camera aRCamera;
    private bool isARObjectSelected;
    private string tagARObjects = "ARObject";
    private Vector2 initialTouchPosition;

    [SerializeField] private float speedMovement = 8.0f;
    [SerializeField] private float speedRotation = 6.0f;
    [SerializeField] private float scaleFactor = 0.9f;

    private float screenFactor = 0.01f;

    private float touchDistance;
    private Vector2 touchPositionDiff;

    private float rotationTolerance = 1f;
    private float scaleTolerance = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0) 
        {
            Touch touchOne = Input.GetTouch(0);

            //Objet Movement
            if (Input.touchCount == 1) 
            {
                if (touchOne.phase == TouchPhase.Began) 
                {
                    initialTouchPosition = touchOne.position;
                    isARObjectSelected = CheckTouchOnARObject(initialTouchPosition);
                }
                if (touchOne.phase == TouchPhase.Moved && isARObjectSelected) 
                {
                    Vector2 diffPosition = (touchOne.position - initialTouchPosition) * screenFactor;

                    ARObject.transform.position = ARObject.transform.position + 
                        new Vector3(diffPosition.x * speedMovement, diffPosition.y * speedMovement, 0);

                    initialTouchPosition = touchOne.position;

                }
            }

            if (Input.touchCount == 2) 
            {
                Touch touchTwo = Input.GetTouch(1);

                if (touchOne.phase == TouchPhase.Began || touchTwo.phase == TouchPhase.Began) 
                {
                    touchPositionDiff = touchTwo.position - touchOne.position;
                    touchDistance = Vector2.Distance(touchTwo.position, touchOne.position);
                }

                if (touchOne.phase == TouchPhase.Moved || touchTwo.phase == TouchPhase.Moved) 
                {
                    Vector2 currentTouchPositionDiff = touchTwo.position - touchOne.position;
                    float currentTouchDistance = Vector2.Distance(touchTwo.position, touchOne.position);

                    float diffDistance = currentTouchDistance - touchDistance;

                    if (Mathf.Abs(diffDistance) > scaleTolerance) 
                    {
                        Vector3 newScale = ARObject.transform.localScale + Mathf.Sign(diffDistance) * Vector3.one * scaleFactor;
                        ARObject.transform.localScale = Vector3.Lerp(ARObject.transform.localScale, newScale, 0.05f);
                    }

                    float angle = Vector2.SignedAngle(touchPositionDiff, currentTouchPositionDiff);

                    if (Mathf.Abs(angle) > rotationTolerance) 
                    {
                        ARObject.transform.rotation = Quaternion.Euler(0, ARObject.transform.rotation.eulerAngles.y - Mathf.Sign(angle) * speedRotation, 0);
                    }

                    touchDistance = currentTouchDistance;
                    touchPositionDiff = currentTouchPositionDiff;
                }
            }
        }
    }

    private bool CheckTouchOnARObject(Vector2 touchPosition) 
    {
        Ray ray = aRCamera.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out RaycastHit hitARObject)) 
        {
            if (hitARObject.collider.CompareTag(tagARObjects)) 
            {
                ARObject = hitARObject.transform.gameObject;
                return true;
            }
        }
        return false;
    }
}
