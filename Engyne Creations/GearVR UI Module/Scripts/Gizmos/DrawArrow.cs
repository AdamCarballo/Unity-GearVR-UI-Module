/*
 * DrawArrow.cs
 * Similar to Gizmos.DrawRay but drawing arrows instead.
 * Original by: https://forum.unity3d.com/threads/debug-drawarrow.85980/
 * 
 * by Adam Carballo under GPLv3 license.
 * https://github.com/AdamEC/Unity-GearVR-UI-Module
 */

using UnityEngine;

public static class DrawArrow {
    public static void GearVR(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f) {

        direction = (direction - pos).normalized * Vector2.Distance(pos, direction);
        Gizmos.color = color;
        Gizmos.DrawRay(pos, direction);

        DrawArrowEnd(pos, direction, Color.black, arrowHeadLength, arrowHeadAngle);
    }

    private static void DrawArrowEnd(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f) {

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * Vector3.back;
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * Vector3.back;
        Vector3 up = Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.back;
        Vector3 down = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.back;

        Gizmos.color = color;
        Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
        Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        Gizmos.DrawRay(pos + direction, up * arrowHeadLength);
        Gizmos.DrawRay(pos + direction, down * arrowHeadLength);
    }
}