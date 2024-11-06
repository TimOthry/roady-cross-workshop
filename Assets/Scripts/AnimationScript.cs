using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private bool isRotating;
    private Quaternion origRot, targetRot;
    private float timeToRotate = 0.1f;

    // TASK 3: Keep checking for when a key is pressed down and then call StartCoroutine to rotate
    // the player based on the key pressed (WASD)
    void Update()
    {
        // Prevent multiple coroutines from occurring at the same time
        if(isRotating) {
            return;
        }
        // When a key is press down, the player rotates
        
    }

    private IEnumerator RotatePlayer(Vector3 rotationAngles) {
        isRotating = true;

        float elapsedTime = 0;
        origRot = transform.rotation;
        targetRot = Quaternion.Euler(rotationAngles); // Convert rotation angles to a Quaternion

        // Allows coroutine to run in the next frame
        while(elapsedTime < timeToRotate) {
            transform.rotation = Quaternion.Lerp(origRot, targetRot, elapsedTime / timeToRotate);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRot;
        isRotating = false;
    }


}
