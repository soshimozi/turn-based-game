using UnityEngine;

public class SteeringBehavior : MonoBehaviour
{
    public float slowDownRadius = 2f; // The radius within which we start slowing down
    public float stopRadius = 0.05f; // The radius within which we consider ourselves arrived
    //public float timeToTarget = 0.1f; // A tweak factor to adjust how quickly the agent adjusts its speed
    //public float maxSpeed = 6f;

    public bool Arrive(Vector3 targetPosition, out Vector3 force)
    {
        var toTarget = targetPosition - transform.position;
        var distance = toTarget.magnitude;

        force = Vector3.zero;
        // Check if we are within the stop radius
        if (distance < stopRadius)
        {
            return true;  // Essentially means we've arrived, so no steering force
            //return Vector3.zero; 
        }

        // Calculate the reduced speed if we're within the slowdown radius
        var targetSpeed = 1.0f;
        if (distance < slowDownRadius)
        {
            targetSpeed *= (distance / slowDownRadius);
        }

        // Calculate the desired velocity
        force = toTarget.normalized * targetSpeed;
        return false;
    }

}
