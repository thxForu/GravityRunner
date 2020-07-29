namespace Termway.Helper
{
    using System;
    using UnityEngine;

    public class Vector2RangeAttribute : PropertyAttribute
    {
        float minX, maxX;
        float minY, maxY;

        public Vector2RangeAttribute(float minX, float maxX, float minY, float maxY)
        {
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
        }

        public Vector2RangeAttribute(float min, float max)
        {
            this.minX = min;
            this.maxX = max;
            this.minY = min;
            this.maxY = max;
        }

        public Vector2 Clamp(Vector2 v)
        {
            return new Vector2
                (
                    Mathf.Clamp(v.x, minX, maxX),
                    Mathf.Clamp(v.y, minY, maxY)
                );
        }
    }

    public class Vector2IntRangeAttribute : PropertyAttribute
    {
        int minX, maxX;
        int minY, maxY;
        string[] tostrings;

        public Vector2IntRangeAttribute(int minX, int maxX, int minY, int maxY)
        {
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;

            tostrings = new string[] { "X ∈ [" + minX + ", " + maxX + "]", "Y ∈ [" + minY + ", " + maxY + "]" };
        }

        public Vector2IntRangeAttribute(int min, int max) : this(min, max, min, max) { }

        public Vector2Int Clamp(Vector2Int v)
        {
            return new Vector2Int
                (
                    Math.Min(Math.Max(v.x, minX), maxX),
                    Math.Min(Math.Max(v.y, minY), maxY)
                );
        }

        public int Clamp(int index, int value)
        {
            if (index == 0)
                return ClampX(value);
            else if (index == 1)
                return ClampY(value);
            throw new ArgumentOutOfRangeException("Input index should be 0 or 1 but is " + index);
        }

        public int ClampX(int x)
        {
            return Math.Min(Math.Max(x, minX), maxX);
        }

        public int ClampY(int y)
        {
            return Math.Min(Math.Max(y, minY), maxY);
        }

        public string[] ToStrings()
        {
            return tostrings;
        }
    }
}
