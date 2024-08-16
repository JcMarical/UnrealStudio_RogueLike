using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 贝赛尔曲线计算脚本，用于模拟抛物线子弹
/// </summary>
public class BezierCurve : MonoBehaviour
{
    [Tooltip("落地时间")] public float time;
    [Tooltip("抛物高度")][Range(0, 1)] public float height;

    public Vector3 startPosition;  //开始位置
    public Vector3 targetPosition; //目标位置（需在实例化后赋值）
    public Vector3 middlePosition; //中间辅助位置

    private Vector3 pointA; //辅助点A
    private Vector3 pointB; //辅助点B
    private float percent;

    private float speedA;
    private float speedB;
    private float speedPercent;

    private void FixedUpdate()
    {
        pointA = Vector3.MoveTowards(pointA, middlePosition, speedA * Time.fixedDeltaTime);
        pointB = Vector3.MoveTowards(pointB, targetPosition, speedB * Time.fixedDeltaTime);
        percent = Mathf.MoveTowards(percent, 1, speedPercent * Time.fixedDeltaTime);
        transform.position = Vector3.Lerp(pointA, pointB, percent);
    }

    /// <summary>
    /// 计算贝塞尔曲线，在实例化赋值后调用
    /// </summary>
    public void CalculateCurve()
    {
        startPosition = transform.position;
        middlePosition = new Vector3((startPosition.x + targetPosition.x) / 2, Mathf.Max(startPosition.y, targetPosition.y) + Vector3.Distance(startPosition, targetPosition) * height, 0);
        pointA = startPosition;
        pointB = middlePosition;
        percent = 0;
        speedA = Vector3.Distance(startPosition, middlePosition) / time;
        speedB = Vector3.Distance(targetPosition, middlePosition) / time;
        speedPercent = 1 / time;
    }
}