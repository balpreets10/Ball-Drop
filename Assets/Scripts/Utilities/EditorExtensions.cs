#if UNITY_EDITOR

using UnityEditor;

#endif

using UnityEngine;

namespace BallDrop
{
    public class EditorExtensions
    {
#if UNITY_EDITOR

        public static bool Foldout(bool foldout, GUIContent content, bool toggleOnLabelClick, GUIStyle style)
        {
            Rect position = GUILayoutUtility.GetRect(40f, 40f, 16f, 16f, style);
            // EditorGUI.kNumberW == 40f but is internal
            return EditorGUI.Foldout(position, foldout, content, toggleOnLabelClick, style);
        }

        public static bool Foldout(bool foldout, string content, bool toggleOnLabelClick, GUIStyle style)
        {
            return Foldout(foldout, new GUIContent(content), toggleOnLabelClick, style);
        }

#endif
    }
}