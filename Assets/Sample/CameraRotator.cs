using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public float angleA = 0f; // First rotation angle
    public float angleB = 90f; // Second rotation angle
    public float rotationDuration = 2f; // Time to complete one rotation from A to B or B to A

    private float timer = 0f;
    private bool rotatingToB = true;

    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Calculate the fraction of the duration that has passed
        float fraction = timer / rotationDuration;

        // Calculate the new angle based on the fraction
        float newAngle;
        if (rotatingToB)
        {
            newAngle = Mathf.Lerp(angleA, angleB, fraction);
        }
        else
        {
            newAngle = Mathf.Lerp(angleB, angleA, fraction);
        }

        // Apply the rotation to the camera
        transform.rotation = Quaternion.Euler(0f, newAngle, 0f);

        // Check if the rotation duration has been exceeded
        if (timer >= rotationDuration)
        {
            // Switch the direction of rotation
            rotatingToB = !rotatingToB;
            // Reset the timer
            timer = 0f;
        }
    }
}
