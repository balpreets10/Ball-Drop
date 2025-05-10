using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrailPanel : MonoBehaviour
{
    public GameObject TrailItem;
    public GameObject Parent;
    public Sprite LockSprite;

    void Start()
    {
        LoadItems();
    }

    private void LoadItems()
    {
        //foreach (Sprite sprite in DownloadedObjectPool.TrailTextures)   //Get downloaded sprites from DLC pool
        //{
        //    GameObject go = Instantiate(TrailItem, Parent.transform);
        //    if (CheckLocked())
        //        go.GetComponent<Image>().sprite = sprite;
        //    else
        //        go.GetComponent<Image>().sprite = LockSprite;
        //    go.GetComponent<Image>().preserveAspect = true;
        //}
    }

    private bool CheckLocked()
    {
        // Code to check if the trail is locked or not....
        return true;
    }
}
