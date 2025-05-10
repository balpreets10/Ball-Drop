using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace BallDrop
{
    [CustomEditor(typeof(UnlockableItems), true)]
    public class CustomToggleEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}