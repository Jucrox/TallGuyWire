/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class Window_Graph : MonoBehaviour {

    [SerializeField] private Sprite circleSprite;
    [SerializeField] private Image alertSprite;
    [SerializeField] private AudioSource alertSound;
    private RectTransform graphContainer;

    List<float> valueList = new List<float>() { 0.0f,4.4f,72.3f,115.3f,112.5f,86.6f,67.5f,66.3f,75.9f,84.2f,85.6f,82.2f,78.7f,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f,79.7f,79.8f,80.0f,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f,79.7f,79.8f,80.0f
                                                ,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f,79.7f,79.8f,80.0f,115.3f,112.5f,86.6f,67.5f,66.3f,75.9f,84.2f,85.6f,82.2f,78.7f,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f,79.8f,80.0f,77.7f,78.9f,80.4f,80.9f
                                                ,80.5f,79.9f,79.7f,79.8f,80.0f,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f,79.7f,79.8f,80.0f,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f,79.7f,79.8f,80.0f,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f,79.7f,79.8f
                                                ,115.3f,112.5f,86.6f,67.5f,66.3f,75.9f,84.2f,85.6f,82.2f,78.7f,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f,79.7f,79.8f,80.0f,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f,79.7f,79.8f,80.0f,77.7f,78.9f,80.4f
                                                ,80.9f,80.5f,79.9f,79.7f,79.8f,80.0f,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f,79.7f,79.8f,80.0f,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f,79.7f,79.8f,80.0f,77.7f,115.3f,112.5f,86.6f,67.5f,66.3f,75.9f
                                                ,84.2f,85.6f,82.2f,78.7f,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f,80.0f,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f,79.7f,79.8f,80.0f,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f,79.7f,79.8f,80.0f,77.7f,78.9f
                                                ,80.4f,80.9f,80.5f,79.9f,79.7f,79.8f,80.0f,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f,79.7f,79.8f,80.0f,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f,79.7f,79.8f,80.0f,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f
                                                ,79.7f,79.8f,80.0f,115.3f,112.5f,86.6f,67.5f,66.3f,75.9f,84.2f,85.6f,82.2f,78.7f,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f,79.8f,80.0f,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f,79.7f,79.8f,80.0f,77.7f
                                                ,78.9f,80.4f,80.9f,80.5f,79.9f,79.7f,79.8f,80.0f,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f,79.7f,79.8f,80.0f,77.7f,78.9f,80.4f,80.9f,80.5f,79.9f,79.7f,79.8f,80.0f,77.7f,78.9f,80.4f,80.9f,80.5f
                                                ,79.9f,79.7f,79.8f,80.0f,77.7f,78.9f,80.4f,115.3f,112.5f,86.6f,67.5f,66.3f,75.9f,84.2f,85.6f,82.2f,78.7f,77.7f,78.9f,80.4f,80.9f,80.5f };

    private void Awake() {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        
        ShowGraph(valueList);
    }

    public void ActiveGraph() 
    {
        ShowGraph(valueList);
    }

    private GameObject CreateCircle(Vector2 anchoredPosition, Color color) {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        gameObject.GetComponent<Image>().color = color;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(5, 5);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph(List<float> valueList) {

        StartCoroutine(DelayCreateDotConnection(valueList));
        //float graphHeight = graphContainer.sizeDelta.y;
        //float yMaximum = 160f;
        //float xSize = 4f;

        //GameObject lastCircleGameObject = null;
        //for (int i = 0; i < valueList.Count; i++) {
        //    float xPosition = xSize + i * xSize;
        //    float yPosition = (valueList[i] / yMaximum) * graphHeight;
        //    GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
        //    if (lastCircleGameObject != null) {
        //        //StartCoroutine(DelayCreateDotConnection(lastCircleGameObject, circleGameObject));
        //        CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
        //    }
        //    lastCircleGameObject = circleGameObject;
        //}
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB, Color color) {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        //gameObject.GetComponent<Image>().color = new Color(1,1,1, .5f);
        gameObject.GetComponent<Image>().color = color;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 5f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }

    IEnumerator DelayCreateDotConnection(List<float> valueList) 
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 160f;
        float xSize = 5.1f;

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            if (valueList[i] > 75 && valueList[i] < 85) 
            {
                Color color = new Color(0, 0.8396226f, 0.1830729f, 1f);
                alertSprite.color = new Color(0, 0.8396226f, 0.1830729f, 1f);
                alertSound.Pause();
                float xPosition = xSize + i * xSize;
                float yPosition = (valueList[i] / yMaximum) * graphHeight;
                GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition), color);
                if (lastCircleGameObject != null)
                {
                    CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition, color);
                }
                lastCircleGameObject = circleGameObject;
                yield return new WaitForSeconds(1f);
            }
            else
            {
                Color color = new Color(0.735849f, 0, 0, 1f);
                alertSprite.color = new Color(0.735849f, 0, 0, 1f);
                alertSound.Play();
                float xPosition = xSize + i * xSize;
                float yPosition = (valueList[i] / yMaximum) * graphHeight;
                GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition), color);
                if (lastCircleGameObject != null)
                {
                    CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition, color);
                }
                lastCircleGameObject = circleGameObject;
                yield return new WaitForSeconds(1f);
            }
            //float xPosition = xSize + i * xSize;
            //float yPosition = (valueList[i] / yMaximum) * graphHeight;
            //GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            //if (lastCircleGameObject != null)
            //{
            //    //StartCoroutine(DelayCreateDotConnection(lastCircleGameObject, circleGameObject));
            //    CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            //}
            //lastCircleGameObject = circleGameObject;
            //yield return new WaitForSeconds(1f);
        }
        
    }

    public void RestartGraph() 
    {
        if (graphContainer.transform.childCount > 0) 
        {
            for (int i = 0; i < graphContainer.transform.childCount; i++)
            {
                if (i != 0) 
                {
                    Destroy(graphContainer.transform.GetChild(i).gameObject);

                }
            }
        }
    }
}
