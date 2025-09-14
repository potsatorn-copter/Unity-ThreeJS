using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public Transform[] waypoints;
    
    [Space(20)]
    [SerializeField] private Transform tracker;
    [SerializeField] private float trackerMoveSpeed = 1f;
    [SerializeField] private float trackerRotationSpeed = 1f;
    [SerializeField] private float trackerWaitOffset = 3f;
    [SerializeField] private float TrackerReachOffset = 1f;

    private int _currentWaypointIndex;
    private bool _isWaypointActive;

    void Start()
    {
        CheckActive();
    }

    private void CheckActive()
    {
        if (waypoints != null)
        {
            _isWaypointActive = true;
        }
        else
        {
            Debug.LogError($"NO WAYPOINTS DETECTED");
        }
    }

    void LateUpdate()
    {
        if (_isWaypointActive)
        {
            MoveTracker();
        }
    }

    private void MoveTracker()
    {
        if (Vector3.Distance(tracker.position, waypoints[_currentWaypointIndex].position) < TrackerReachOffset)
        {
            _currentWaypointIndex++;
            if (_currentWaypointIndex == waypoints.Length)
            {
                _currentWaypointIndex = 0; 
            }
        }

        tracker.LookAt(waypoints[_currentWaypointIndex].position);
        tracker.Translate(0, 0, trackerMoveSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        if (_isWaypointActive)
        {
            Gizmos.color = Color.green;
            foreach (var eachPoint in waypoints)
            {
                Gizmos.DrawSphere(eachPoint.position, 0.5f);
            }

            Gizmos.color = Color.yellow;
            for (var i = 0; i < waypoints.Length; i++)
            {
                var nextWaypointIndex = i + 1 >= waypoints.Length ? 0 : i + 1;
                Gizmos.DrawLine(waypoints[i].position, waypoints[nextWaypointIndex].position);
            }
        }
    }
}