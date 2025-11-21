using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAffected : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        GravityZone gravityZone = GetComponentInParent<GravityZone>() ?? FindObjectOfType<GravityZone>();
        if (gravityZone != null)
        {
            gravityZone.ApplyAllGravity(rb);
        }
    }
}