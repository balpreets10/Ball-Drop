using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;
using System.Reflection;

namespace BallDrop
{
    public static class ExtensionMethods
    {
        public static void DestroyChildren(this Transform t)
        {
            bool isPlaying = Application.isPlaying;

            while (t.childCount != 0)
            {
                Transform child = t.GetChild(0);

                if (isPlaying)
                {
                    child.SetParent(null);
                    UnityEngine.Object.Destroy(child.gameObject);
                }
                else UnityEngine.Object.DestroyImmediate(child.gameObject);
            }
        }

        public static void DeactivateChildren(this Transform t)
        {
            bool isPlaying = Application.isPlaying;

            for (int i = 0; i < t.childCount; i++)
            {
                GameObject go = t.GetChild(i).gameObject;

                if (isPlaying)
                {
                    go.SetActive(false);
                }
            }
        }

        public static List<Transform> GetChildren(this Transform t)
        {
            List<Transform> children = new List<Transform>();
            bool isPlaying = Application.isPlaying;

            for (int i = 0; i < t.childCount; i++)
            {
                children.Add(t.GetChild(i));
            }
            return children;
        }

        public static void SetActive(this Transform obj, bool value)
        {
            obj.gameObject.SetActive(value);
        }

        public static void SetActive(this MonoBehaviour obj, bool value)
        {
            obj.gameObject.SetActive(value);
        }

        public static void SetParent(this Transform transform, Transform parent, Vector3 localPosition, Vector3 localScale, bool worldPositionStays)
        {
            transform.SetParent(parent, worldPositionStays);
            transform.localPosition = localPosition;
            transform.localScale = localScale;
        }

        //
        public static bool IsNullOrEmpty<T>(this T[] array) where T : class
        {
            if (array == null || array.Length == 0)
                return true;
            else
                return array.All(item => item == null);
        }

        public static bool Contains<T>(this T[] array, T element)
        {
            for (int i = array.Length - 1; i != -1; --i)
            {
                if (element.Equals(array[i]))
                    return true;
            }
            return false;
        }

        public static Transform GetSiblingAtIndex(this Transform t, int index)
        {
            return t.parent.GetChild(index);
        }

        public static bool activeInHierarchy(this Transform t)
        {
            return t.gameObject.activeInHierarchy;
        }

        public static bool activeInHierarchy(this MonoBehaviour t)
        {
            return t.gameObject.activeInHierarchy;
        }


    }
}