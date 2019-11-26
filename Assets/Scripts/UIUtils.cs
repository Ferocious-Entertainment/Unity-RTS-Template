using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUtils : MonoBehaviour
{
    static Texture2D _whiteTexture;
    public static Texture2D WhiteTexture
    {
        get
        {
            if (_whiteTexture == null)
            {
                _whiteTexture = new Texture2D(1, 1);
                _whiteTexture.SetPixel(0, 0, Color.white);
                _whiteTexture.Apply();
            }

            return _whiteTexture;
        }
    }

    public static void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, WhiteTexture);
        GUI.color = Color.white;
    }

    public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        // Top
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
            // Move origin from bottom left to top left
            screenPosition1.y = Screen.height - screenPosition1.y;
            screenPosition2.y = Screen.height - screenPosition2.y;
            // Calculate corners
            var topLeft = Vector3.Min(screenPosition1, screenPosition2);
            var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
            // Create Rect
            return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    public static void DrawRect(Vector3 startPoint, Color rectColor, float borderThickness, Color borderColor, float rectOpacity)
    {
        Rect rect = GetScreenRect(startPoint, Input.mousePosition);
        rectColor.a = rectOpacity;
        DrawScreenRect(rect, rectColor);
        DrawScreenRectBorder(rect, borderThickness, borderColor);
    }

    public static Bounds GetViewportBounds(Camera cam, Vector3 p1, Vector3 p2)
    {

        Vector3 a = Camera.main.ScreenToViewportPoint(p1);
        Vector3 b = Camera.main.ScreenToViewportPoint(p2);

        Bounds bounds = new Bounds();

        Vector3 min = Vector3.Min(p1, p2);
        Vector3 max = Vector3.Max(p1, p2);

        bounds.SetMinMax(min, max);

        return bounds;
    }

    public static bool IsWithBounds(GameObject obj, Vector3 startPoint)
    {
        Camera camera = Camera.main;
        Bounds viewportBounds =
            GetViewportBounds(camera, startPoint, Input.mousePosition);

        return viewportBounds.Contains(
            camera.WorldToViewportPoint(obj.transform.position));
    }
}
