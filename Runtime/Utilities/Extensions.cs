using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public static class Extensions
{
    /// <summary>
    /// Extension method to check if a layer is in a layermask
    /// </summary>
    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    /// <summary>
    /// Return position for a scrollable rect content so for example if we want to move the content
    /// of a scrollable rect to center an element in it MyScrollRect.content.localPosition =
    /// MyScrollRect.GetSnapToPositionToBringChildIntoView(someChild); the some child needs to be a
    /// child to the content of the scroll rect
    /// </summary>
    public static Vector2 GetSnapToPositionToBringChildIntoView(this ScrollRect instance, RectTransform child)
    {
        Canvas.ForceUpdateCanvases();
        Vector2 viewportLocalPosition = instance.viewport.localPosition;
        Vector2 childLocalPosition = child.localPosition;
        Vector2 result = new Vector2(
                0 - (viewportLocalPosition.x + childLocalPosition.x),
                0 - (viewportLocalPosition.y + childLocalPosition.y)
            );
        return result;
    }

    /// <summary>
    /// Remap from range of values to another , for example remap value in the range of 0-100 to a
    /// value in range of 0-1 Float Edition
    /// </summary>
    public static float Remap(this float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }

    /// <summary>
    /// Remap from range of values to another , for example remap value in the range of 0-100 to a
    /// value in range of 0-1 Int Edition
    /// </summary>
    public static float Remap(this int from, int fromMin, int fromMax, int toMin, int toMax)
    {
        float fromAbs = from - fromMin;
        float fromMaxAbs = fromMax - fromMin;

        float normal = fromAbs / fromMaxAbs;

        float toMaxAbs = toMax - toMin;
        float toAbs = toMaxAbs * normal;

        float to = toAbs + toMin;

        return to;
    }
}