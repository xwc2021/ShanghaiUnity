﻿using UnityEngine;
using System.Collections;
using UnityEditor;

public class MahjongMapDrawer
{
    static Color SetValueColor = Color.red;
    static Color EvenPointColor = Color.blue;
    static Color OddPointColor = Color.green;
    static Color ClickPointColor = Color.red;
    static Color HitPointColor = Color.yellow;
    static float DebugHitR = 0.05f;


    [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
    static void DrawGizmoFor(MahjongMap target, GizmoType gizmoType)
    {
        DrawGrid(target);
        DrawMap(target);

        //畫Ray
        Gizmos.color = ClickPointColor;
        var clickPointOnRay = target.GetClickPointOnRay();
        Gizmos.DrawSphere(clickPointOnRay, DebugHitR);
        Gizmos.DrawLine(clickPointOnRay, clickPointOnRay - target.GetClickNormalDir() * 10);

        //畫HitPoint
        Gizmos.color = HitPointColor;
        Gizmos.DrawSphere(target.GetHitPoint(), DebugHitR);
    }

    static void DrawGrid(MahjongMap target)
    {
        var X = target.GetX(); var Y = target.GetY();
        var nowHeight = target.GetNowFlowerHeight();
        var original = target.transform.position + nowHeight;

        //畫直線
        var yLine = Y * MahjongMap.yUnit * Vector3.forward;
        var offset = MahjongMap.xUnit * Vector3.right;
        var from = original;
        for (var x = 1; x <= X + 1; ++x)
        {
            Gizmos.DrawLine(from, from + yLine);
            from = from + offset;
        }

        //畫橫線
        offset = MahjongMap.yUnit * Vector3.forward;
        var xLine = X * MahjongMap.xUnit * Vector3.right; ;
        from = original;
        for (var y = 1; y <= Y + 1; ++y)
        {
            Gizmos.DrawLine(from, from + xLine);
            from = from + offset;
        }
    }

    static void DrawMap(MahjongMap target)
    {
        var nowHeight = target.GetNowFlowerHeight();
        var original = target.transform.position + nowHeight;

        //畫點
        var offsetX = 0.5f * Vector3.right * MahjongMap.xUnit;
        var offsetY = 0.5f * Vector3.forward * MahjongMap.yUnit;
        var offsetXY = offsetX + offsetY;
        
        var nowFloorIndex = target.GetNowFloorIndex();
        if (!target.IsValidatedFloorIndex(nowFloorIndex))
            return;

        var CountY = target.CountY(); var CountX = target.CountX();
        for (var y = 0; y < CountY; ++y)
        {
            var from = original + offsetXY + offsetY * y;
            for (var x = 0; x < CountX; ++x)
            {
                ChoseColor(target, nowFloorIndex,y, x);
                Gizmos.DrawSphere(from, MapNode.HitRangeRadius);
                from = from + offsetX;
            }
        }
    }

    static void ChoseColor(MahjongMap target,int nowFloorIndex, int y, int x) {
        if (target.IsSetValue(nowFloorIndex, y, x))
            Gizmos.color = SetValueColor;
        else if (x % 2 == 0 && y % 2 == 0)
            Gizmos.color = EvenPointColor;
        else
            Gizmos.color = OddPointColor;
    }
}