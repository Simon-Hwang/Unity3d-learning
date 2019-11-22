using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform trans;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup; // detect event trigger
    public Vector3 originalPosition; // recorde current postion 
    private GameObject lastEnter = null; // recorde last frame location
    private Color lastEnterNormalColor;// recorde last color
    private Color highLightColor = Color.cyan;//highlight 
    void Start()
    {
        trans = this.transform;
        rectTransform = this.transform as RectTransform;
        canvasGroup = GetComponent<CanvasGroup>();
        originalPosition = trans.position;
    }
    void Update()
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;// event trigger ignore itself so that it can detect lowwer object 
        lastEnter = eventData.pointerEnter;
        lastEnterNormalColor = lastEnter.GetComponent<Image>().color;
        originalPosition = trans.position;
        gameObject.transform.SetAsLastSibling();///sibling first
    }

    public void OnDrag(PointerEventData eventData) 
    {
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            rectTransform.position = globalMousePos;
        }
        GameObject curEnter = eventData.pointerEnter;
        bool inItemGrid = EnterItemGrid(curEnter);
        if (inItemGrid)
        {
            Image img = curEnter.GetComponent<Image>();
            lastEnter.GetComponent<Image>().color = lastEnterNormalColor;
            if (lastEnter != curEnter)
            {
                lastEnter.GetComponent<Image>().color = lastEnterNormalColor;
                lastEnter = curEnter;//record current good 
            }
            img.color = highLightColor; // highligh 
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject curEnter = eventData.pointerEnter;
        if (curEnter == null)
        {
            trans.position = originalPosition; // reset
        }
        else
        {
            if (curEnter.name == "Panel")// move to the bag
            {
                trans.position = curEnter.transform.position;
                originalPosition = trans.position;
                curEnter.GetComponent<Image>().color = lastEnterNormalColor;//recover its color
            }
            else
            {
                if (curEnter.name == eventData.pointerDrag.name && curEnter != eventData.pointerDrag)
                {
                    Vector3 targetPostion = curEnter.transform.position;
                    curEnter.transform.position = originalPosition;
                    trans.position = targetPostion;
                    originalPosition = trans.position;
                }
                else//move to another bag
                {
                    trans.position = originalPosition;
                }
            }
        }
        lastEnter.GetComponent<Image>().color = lastEnterNormalColor;//reset color
        canvasGroup.blocksRaycasts = true;//reset
    }
    bool EnterItemGrid(GameObject go) //judge whether point at specific bag
    {
        if (go == null)
        {
            return false;
        }
        return go.name == "Panel";
    }
}
