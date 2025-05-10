using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop.Utilities
{
    public class XDebug
    {
        public static bool AddRuntimeLogs = true;

        //Add Log Masks Here
        public enum Mask
        {
            Main,
            MyEvent,
            Singleton,
        }

        //Add Color Names here, should be same as colors defined in Color Class
        public enum Color
        {
            Red,
            Green,
            Blue,
            Yellow,
            Cyan
        }

        private static List<Mask?> enabledMasks = new List<Mask?>();

        public static void Log(string log, Mask? mask = null)
        {
        }

        public static void Log(string log, Mask? mask, Color? color)
        {
            if (CanLog(mask))
                if (color != null)
                    Debug.Log("<color='" + color.ToString() + "'>" + log + "</color>");
                else
                    Debug.Log(log);
        }

        public static void LogError(string log, Mask? mask)
        {
            if (CanLog(mask))
                Debug.Log("<color='" + Color.Red.ToString() + "'>" + log + "</color>");
        }

        public void LogWarn(string log, Mask mask)
        {
            if (CanLog(mask))
                Debug.LogWarning(log);
        }

        //Checks whether a mask can be logged or not
        private static bool CanLog(Mask? mask)
        {
            if (mask != null)
                return enabledMasks.Contains(mask);
            else
                return true;
        }

        //Enables all masks in the enum Mask and enables logging of these masks
        public static void EnableAllMasks()
        {
            if (enabledMasks != null)
                enabledMasks.Clear();

            //Add Mask name here
            EnableMask(
                Mask.Main
                );
        }

        //Removes all masks so that nothing is logged
        public static void RemoveAllMasks()
        {
            if (enabledMasks != null)
                enabledMasks.Clear();
        }

        public static void EnableMask(Mask mask)
        {
            if (enabledMasks != null)
                enabledMasks.Add(mask);
        }

        public static void EnableMask(params Mask[] mask)
        {
            if (enabledMasks != null)
                foreach (Mask m in mask)
                    enabledMasks.Add(m);
        }
    }
}