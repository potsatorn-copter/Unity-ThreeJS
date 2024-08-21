using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
 
{   public Transform player; // Transform ของผู้เล่น
    public NavMeshAgent agent; // ตัวแทน NavMesh ของ Ghost
    public float chaseSpeed = 10.0f; // ความเร็วในการไล่ล่า
    public float chaseRange = 10.0f; // ระยะการไล่ล่า
    public float slowChaseSpeed = 5.0f; // ความเร็วในการไล่ล่าแบบช้า
    public float patrolSpeed = 2.0f; // ความเร็วในการลาดตระเวน
    public float viewRange = 15.0f; // ระยะการมองเห็น
    public float viewAngle = 90.0f; // มุมมอง
    public LayerMask visionObstructingLayer; // เลเยอร์ของวัตถุที่บดบัง FOV
    public float stopChaseDistance = 1.0f;
    private bool isPlayerInFOVPreviously;


    public Transform tracker; // Transform ของ Tracker

    private bool isChasing = false; // สถานะการไล่ล่า
    private bool isPlayerDetected = false; // สถานะการตรวจจับผู้เล่น
    public bool isOnPatrol;

    private void Start()
    {
        agent.speed = patrolSpeed;
        isOnPatrol = true;
    }

    private void Update()
    {
        // ตรวจจับผู้เล่น
        isPlayerDetected = IsPlayerInFOV();

        // ตัดสินใจว่าจะไล่ล่า Tracker หรือ Player
        if (isPlayerDetected)
        {
            isChasing = true;
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);

            // ตรวจสอบว่า Ghost อยู่ใกล้ Player มากๆ หรือไม่
            if (Vector3.Distance(transform.position, player.position) <= stopChaseDistance)
            {
                agent.isStopped = true;
            }
        }
        else if (!isChasing)
        {
            isChasing = false;
            agent.speed = slowChaseSpeed;
            agent.SetDestination(tracker.position);
            agent.isStopped = false;
        }

        // ตรวจสอบว่า Player ออกจากระยะที่กำหนดหรือไม่
        if (isChasing && !isPlayerDetected && Vector3.Distance(transform.position, player.position) > stopChaseDistance)
        {
            agent.isStopped = false;
        }
    }


    private bool IsPlayerInFOV()
    {
        // คำนวณทิศทางไปยังผู้เล่น
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // คำนวณมุมระหว่างทิศทางที่ Ghost มองอยู่กับทิศทางไปยังผู้เล่น
        float angleToPlayer = Vector3.Angle(directionToPlayer, transform.forward);

        // ตรวจสอบว่าผู้เล่นอยู่ในระยะการมองเห็นหรือไม่
        if (Vector3.Distance(transform.position, player.position) <= viewRange)
        {
            // ตรวจสอบว่าผู้เล่นอยู่ในมุมมองหรือไม่
            if (angleToPlayer <= viewAngle / 2)
            {
                // ตรวจสอบว่ามีวัตถุบดบัง FOV หรือไม่
                if (!Physics.Raycast(transform.position, directionToPlayer, viewRange, visionObstructingLayer))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        // วาดทิศทางที่ Ghost มองอยู่
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * viewRange);

        // วาดมุมมอง
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward) * viewRange);
        Gizmos.DrawLine(transform.position, transform.position + (Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward) * viewRange);
    }
}