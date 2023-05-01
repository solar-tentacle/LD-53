using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class PortalGridElement : ObjectGridElement
{
    public PortalData Data;
    
    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // Draw Gizmos code here
        Handles.color = Color.blue;
        //Handles.ArrowHandleCap(0, transform.position, transform.rotation, 1.0f, EventType.Repaint);

        var dir2d = Data.Direction;
        var direction = new Vector3(dir2d.x, dir2d.y, 0);
            
        // Set the color of the Gizmos
        Gizmos.color = Color.white;

        // Draw the line representing the direction
        Gizmos.DrawLine(transform.position, transform.position + direction);

        // Calculate the position and rotation of the triangles
        Vector3 arrowEnd = transform.position + direction;
        Quaternion arrowRotation = Quaternion.LookRotation(direction);
        Vector3 right = arrowRotation * Vector3.right;
        Vector3 up = arrowRotation * Vector3.up;

        // Draw the first triangle
        Gizmos.DrawLine(arrowEnd, arrowEnd - right * 0.25f + up * 0.15f);
        Gizmos.DrawLine(arrowEnd, arrowEnd - right * 0.25f - up * 0.15f);
        Gizmos.DrawLine(arrowEnd - right * 0.25f + up * 0.15f, arrowEnd - right * 0.25f - up * 0.15f);

        // Draw the second triangle
        Gizmos.DrawLine(arrowEnd, arrowEnd + right * 0.25f + up * 0.15f);
        Gizmos.DrawLine(arrowEnd, arrowEnd + right * 0.25f - up * 0.15f);
        Gizmos.DrawLine(arrowEnd + right * 0.25f + up * 0.15f, arrowEnd + right * 0.25f - up * 0.15f);
    }
    #endif
}

[Serializable] public class PortalData
{
    public Vector2Int Direction = Vector2Int.up;
}