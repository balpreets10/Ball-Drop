using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BallDrop
{
    public class ScrollItem : MonoBehaviour
    {
        public Toggle MyToggle;

        public virtual void ShowItem()
        {
            MyToggle.isOn = true;
        }

        public virtual void HideItem()
        {

        }
    }
}