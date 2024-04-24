using UnityEngine;

namespace AsteroidsSurvival.Utils
{
    public static class MyExtensions
    {
        public static bool IsNumber(this string str)
        {
            int i;
            return int.TryParse(str, out i);
        }

        public static void Log(this string str)
        {
            Debug.Log("Log: " + str);
        }

    }
}
