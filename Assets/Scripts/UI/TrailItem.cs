using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrailItem : MonoBehaviour
{
    Sprite sprite;

    public void ApplyTrail()
    {
        sprite = transform.GetChild(0).GetComponent<Image>().sprite;
        Debug.Log("Selected trail : " + sprite);

        //Code to set trail material...
    }
}
