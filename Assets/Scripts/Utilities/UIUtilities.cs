using UnityEngine;

using System;

#if UNITY_EDITOR
using UnityEditor;

public class UIUtilities : EditorWindow
{
    private Transform mTransform;
    private string label = string.Empty;

    [MenuItem("Utilities/UI Utilities")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(UIUtilities));
    }

    private void OnGUI()
    {
        mTransform = (Transform)EditorGUILayout.ObjectField("Object", mTransform, typeof(Transform), true);

        if (mTransform != null)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Rename Children");
            label = GUILayout.TextField(label, GUILayout.Width(150));
            GUILayout.BeginHorizontal();
            RenameChildrenNumericallyWithPrefix();
            RenameChildrenNumericallyWithSuffix();
            GUILayout.EndHorizontal();

            GUILayout.Label("Rename Children of Children");
            GUILayout.BeginHorizontal();

            RenameChildrenOfChildrenNumericallyWithPrefix();
            RenameChildrenOfChildrenNumericallyWithSuffix();
            GUILayout.BeginHorizontal();
        }
    }

    private void RenameChildrenNumericallyWithPrefix()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("with Prefix"))
        {
            RenameChildren(label, true);
        }
    }

    private void RenameChildrenNumericallyWithSuffix()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("with Suffix"))
        {
            RenameChildren(label, false);
        }
    }

    private void RenameChildrenOfChildrenNumericallyWithPrefix()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("with Prefix"))
        {
            RenameChildrenOfChildren(label, true);
        }
    }

    private void RenameChildrenOfChildrenNumericallyWithSuffix()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("with Suffix"))
        {
            RenameChildrenOfChildren(label, false);
        }
    }

    private void RenameChildren(string label, bool prefix)
    {
        for (int i = 0; i < mTransform.childCount; i++)
        {
            if (prefix)
                mTransform.GetChild(i).name = label + i;
            else
                mTransform.GetChild(i).name = i + label;
        }
    }

    private void RenameChildrenOfChildren(string label, bool prefix)
    {
        for (int i = 0; i < mTransform.childCount; i++)
        {
            if (prefix)
                mTransform.GetChild(i).GetChild(0).name = label + i;
            else
                mTransform.GetChild(i).GetChild(0).name = i + label;
        }
    }
}

#endif