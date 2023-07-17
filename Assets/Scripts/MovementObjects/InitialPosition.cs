using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialPosition : MonoBehaviour
{
    [SerializeField] private GameObject guyWire_1;
    [SerializeField] private GameObject[] childrenGuyWire_1;

    [SerializeField] private GameObject guyWire_2;
    [SerializeField] private GameObject[] childrenGuyWire_2;

    [SerializeField] private Vector3[] initialPositionGuyWire_1;
    [SerializeField] private Vector3[] initialPositionGuyWire_2;

    [SerializeField] private Quaternion[] initialRotationGuyWire_1;
    [SerializeField] private Quaternion[] initialRotationGuyWire_2;

    [SerializeField] private Vector3[] initialScaleGuyWire_1;
    [SerializeField] private Vector3[] initialScaleGuyWire_2;

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < childrenGuyWire_1.Length; i++)
        {
            for (int j = 0; j < initialPositionGuyWire_1.Length; j++)
            {
                initialPositionGuyWire_1[j] = childrenGuyWire_1[i].transform.localPosition;
                initialRotationGuyWire_1[j] = childrenGuyWire_1[i].transform.localRotation;
                initialScaleGuyWire_1[j] = childrenGuyWire_1[i].transform.localScale;
            }
        }

        for (int i = 0; i < childrenGuyWire_2.Length; i++)
        {
            for (int j = 0; j < initialPositionGuyWire_2.Length; j++)
            {
                initialPositionGuyWire_2[j] = childrenGuyWire_2[i].transform.localPosition;
                initialRotationGuyWire_2[j] = childrenGuyWire_2[i].transform.localRotation;
                initialScaleGuyWire_2[j] = childrenGuyWire_2[i].transform.localScale;
            }
        }
    }

    public void RestartObjectsPosition() 
    {
        for (int i = 0; i < childrenGuyWire_1.Length; i++)
        {
            for (int j = 0; j < initialPositionGuyWire_1.Length; j++)
            {
                childrenGuyWire_1[i].transform.localPosition = initialPositionGuyWire_1[j];
                childrenGuyWire_1[i].transform.localRotation = initialRotationGuyWire_1[j];
                childrenGuyWire_1[i].transform.localScale = initialScaleGuyWire_1[j];
            }
        }
        for (int i = 0; i < childrenGuyWire_2.Length; i++)
        {
            for (int j = 0; j < initialPositionGuyWire_2.Length; j++)
            {
                childrenGuyWire_2[i].transform.localPosition = initialPositionGuyWire_2[j];
                childrenGuyWire_2[i].transform.localRotation = initialRotationGuyWire_2[j];
                childrenGuyWire_2[i].transform.localScale = initialScaleGuyWire_2[j];
            }
        }
    }
}
