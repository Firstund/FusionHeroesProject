using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncyclopediaScrollViewScript : MonoBehaviour
{
    private ScrollRect scrollRect = null;
    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();

        for (int i = 0; i < scrollRect.content.childCount - 1; i++)
        {
            scrollRect.content.sizeDelta += scrollRect.content.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta;
        }
    }
}
