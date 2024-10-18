using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaypointMover_AV : MonoBehaviour
{
    // stores reference to the waypoint system this object will use.
    [SerializeField] private Waypoints waypoints;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float distanceThreshold = 0.1f;

    // references for lane change on distance
    [SerializeField] private GameObject otherObject;
    [SerializeField] public float triggerDistance = 15f;
    public float distance;

    // the current waypoint target that the object is moving towards
    private Transform currentWaypoint;

    // variables for lane change
    [SerializeField] private float offsetValue = 4f;
    private bool offsetApplied = false;

    // variables for exiting lanechange
    private Vector3 initialPosition;
    private bool measuringDistance = false;
    private float totalDistanceTravelled = 0f;
    public float exitDistance = 50f; // Distance threshold to exit the if statement
    bool lanechange_amount = false;

    // Start is called before the first frame update
    void Start()
    {
        // set initial position to the first waypoint
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.position = currentWaypoint.position;

        // set the next waypoint target
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        UpdateLookAt();
        //transform.LookAt(currentWaypoint);
    }


    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, otherObject.transform.position);

        // Check distance for performing lanechange
        if (distance <= triggerDistance && measuringDistance == false && lanechange_amount == false)
        {
            Debug.Log("LaneChange TIME");
            offsetApplied = !offsetApplied;
            lanechange_amount = true;
            moveSpeed = 10f;
            initialPosition = transform.position; // Record the initial position
            measuringDistance = true; // Start measuring distance
            UpdateLookAt();
        }

        if (measuringDistance == true)
        {
            totalDistanceTravelled = Vector3.Distance(initialPosition, transform.position);
            Debug.Log(totalDistanceTravelled);
        }
       
        if (totalDistanceTravelled >= exitDistance && measuringDistance == true)
        {
            Debug.Log("End LaneChange");
            measuringDistance = false; // Exit the if statement
            offsetApplied = !offsetApplied;
            moveSpeed = 5f;
            UpdateLookAt();
        }

        // Move the object towards the current waypoint (with or without offset)
        Vector3 targetPosition = GetTargetPosition();
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if the object has reached the current waypoint (or offsetted position)
        if (Vector3.Distance(transform.position, targetPosition) < distanceThreshold)
        {
            // Move to the next waypoint
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
            UpdateLookAt();
        }
    }


    // Returns the current waypoint position, applying the offset if enabled
    private Vector3 GetTargetPosition()
    {
        Vector3 targetPosition = currentWaypoint.position;
        if (offsetApplied)
        {
            targetPosition.x += offsetValue;  // Apply the X offset
        }
        return targetPosition;
    }


    // Updates the object's LookAt direction based on the current waypoint (with or without offset)
    private void UpdateLookAt()
    {
        Vector3 lookAtTarget = currentWaypoint.position;
        if (offsetApplied)
        {
            lookAtTarget.x += offsetValue;  // Apply the X offset for LookAt
        }
        transform.LookAt(lookAtTarget);
    }


    void TriggerEvent()
    {
        Debug.Log("Distance warning!");
    }
}
