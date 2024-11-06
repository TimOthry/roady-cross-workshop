using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private bool isMoving;
    private Vector3 origPos, targetPos, playerOrigPos, playerTargerPos;
    private float timeToMove = 0.1f;
    private float dist = 1.6f; //Adjust this to match your grid size
    public LayerMask obstacleLayer; //Layer for obstacles
    public LayerMask logLayer; // Layer for logs
    private ObjMovement currentLog = null; // Reference to the ObjMovement script on the log

    // TASK 5: When a key is released, the player moves in the direction of the key pressed.
    // I have done the forward key for you, and also include CanMove into this method to ensure
    // you do not go through obstacles 
    void Update() {
        // Prevents multiple coroutines to occur at the same time
        if(isMoving) {
            return;
        }
        // Check for key releases and trigger movement
        if (Input.GetKeyUp(KeyCode.W)) {
            StartCoroutine(MovePlayer(new Vector3(0, 0, dist)));
        }

         // If player is on a log, move with the log using its manual speed
        if (currentLog != null) {
            // Adjust movement based on the log's direction
            Vector3 logMovementDirection = currentLog.transform.right; // Assume logs move along their local X-axis
            transform.position += logMovementDirection * -currentLog.speed * Time.deltaTime;
        }
    }

    // TASK 4: Use RayCastHit to check if there is a obstacle (collider) in front of the player
    // if false, then obstacle is in the way and the player cannot move
    // if true, then player is allowed to move forward
    private bool CanMove(Vector3 direction) {
        // Perform a raycast in the desired direction to detect obstacles
        return true;
    }
    
    private IEnumerator MovePlayer(Vector3 direction) {
        isMoving = true;

        float elapsedTime = 0;
        origPos = transform.position;
        targetPos = origPos + direction;
        playerOrigPos = transform.Find("default").position;
        playerTargerPos = playerOrigPos + new Vector3(0 , 0.5f, 0) + direction;

        // Allows coroutine to run in the next frame
        while(elapsedTime < timeToMove) {
            float lerpFactor = elapsedTime / timeToMove;

            transform.position = Vector3.Lerp(origPos, targetPos, lerpFactor);
            transform.Find("default").position = Vector3.Lerp(playerOrigPos, playerTargerPos, lerpFactor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Snap position to the nearest 1.6f on x and z axes
        if (currentLog == null) {
            transform.position = SnapToGrid(targetPos);
            transform.Find("default").position = SnapToGrid(playerTargerPos);
        }

        // Check if player is on a log after moving
        DetectLogUnderneath();
        
        isMoving = false;
    }

    private Vector3 SnapToGrid(Vector3 position) {
        // Round the x and z coordinates to the nearest multiple of 1.6
        float snappedX = Mathf.Round(position.x / 1.6f) * 1.6f;
        float snappedZ = Mathf.Round(position.z / 1.6f) * 1.6f;
        
        // Return the new position, y remains the same
        return new Vector3(snappedX, position.y, snappedZ);
    }

    private void DetectLogUnderneath() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f, logLayer)) {
            ObjMovement log = hit.collider.GetComponent<ObjMovement>();

            if (log != null) {
                SnapLog(log);
                currentLog = log; // Store the log's ObjMovement reference
            }
        } else {
            currentLog = null; // If no log is detected, set currentLog to null
        }
    }

    private void SnapLog(ObjMovement log) {
        Vector3 logPosition = log.transform.position;
        Vector3 snappedPosition = new Vector3(logPosition.x, transform.position.y, transform.position.z); // Adjust only X
        transform.position = snappedPosition;
    }
}
