namespace Termway.Helper
{
    using UnityEngine;

    public static class RectTransformExtensions
    {
        public static void Left(this RectTransform rt, float left)
        {
            rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        }

        public static void Bottom(this RectTransform rt, float bottom)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        }

        public static void Right(this RectTransform rt, float right)
        {
            rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        }

        public static void Top(this RectTransform rt, float top)
        {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        }

        public static void Width(this RectTransform rt, float width)
        {
            rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
        }

        public static void Height(this RectTransform rt, float height)
        {
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
        }

        public static void PosX(this RectTransform rt, float x)
        {
            rt.anchoredPosition = new Vector2(x, rt.anchoredPosition.y);
        }

        public static void PosY(this RectTransform rt, float y)
        {
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, y);
        }
    }
}