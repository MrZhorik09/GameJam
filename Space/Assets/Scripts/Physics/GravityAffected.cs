using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityAffected : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Disable standard gravity usage if we are using custom gravity
        rb.useGravity = false;
    }

    private void FixedUpdate()
    {
        // Efficiently apply gravity from all zones
        GravityZone.ApplyGravity(rb);
    }
}