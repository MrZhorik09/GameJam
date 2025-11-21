using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GravityZone : MonoBehaviour
{
    public enum ZoneShape
    {
        Sphere,
        Cube,
        Cylinder
    }

    [SerializeField] protected float gravityStrength = 9.81f;
    [SerializeField] protected float maxDistance = 10f;
    [SerializeField] protected bool useDistanceFalloff = true;
    [SerializeField] protected ZoneShape shape = ZoneShape.Sphere;

    [SerializeField] protected bool enableOrbitalMotion = false;
    [SerializeField] protected float orbitalSpeedMultiplier = 1f;

    protected static List<GravityZone> gravityZones = new List<GravityZone>();

    protected virtual void OnEnable()
    {
        gravityZones.Add(this);
    }

    protected virtual void OnDisable()
    {
        gravityZones.Remove(this);
    }

    public virtual bool IsPointInZone(Vector3 point)
    {
        Vector3 localPoint = transform.InverseTransformPoint(point);

        switch (shape)
        {
            case ZoneShape.Sphere:
                return localPoint.magnitude <= maxDistance;
            case ZoneShape.Cube:
                return Mathf.Abs(localPoint.x) <= maxDistance &&
                       Mathf.Abs(localPoint.y) <= maxDistance &&
                       Mathf.Abs(localPoint.z) <= maxDistance;
            case ZoneShape.Cylinder:
                Vector2 horizontal = new Vector2(localPoint.x, localPoint.z);
                return horizontal.magnitude <= maxDistance && Mathf.Abs(localPoint.y) <= maxDistance;
            default:
                return false;
        }
    }

    public virtual float GetDistanceFactor(Vector3 point)
    {
        Vector3 localPoint = transform.InverseTransformPoint(point);

        switch (shape)
        {
            case ZoneShape.Sphere:
                return localPoint.magnitude / maxDistance;
            case ZoneShape.Cube:
                Vector3 absLocal = new Vector3(Mathf.Abs(localPoint.x), Mathf.Abs(localPoint.y), Mathf.Abs(localPoint.z));
                return Mathf.Max(absLocal.x, absLocal.y, absLocal.z) / maxDistance;
            case ZoneShape.Cylinder:
                Vector2 horizontal = new Vector2(localPoint.x, localPoint.z);
                return Mathf.Max(horizontal.magnitude, Mathf.Abs(localPoint.y)) / maxDistance;
            default:
                return 1f;
        }
    }

    public virtual void ApplyAllGravity(Rigidbody rb)
    {
        Vector3 totalGravity = CalculateGravity(rb);
        rb.AddForce(totalGravity, ForceMode.Acceleration);
    }

    protected virtual Vector3 CalculateGravity(Rigidbody rb)
    {
        Vector3 totalGravity = Vector3.zero;

        foreach (GravityZone zone in gravityZones)
        {
            if (zone.IsPointInZone(rb.position))
            {
                Vector3 gravityDirection = (zone.transform.position - rb.position).normalized;
                float gravityForce = zone.gravityStrength;

                if (zone.useDistanceFalloff)
                {
                    float distanceFactor = zone.GetDistanceFactor(rb.position);
                    float falloff = Mathf.Clamp01(1f - distanceFactor);
                    gravityForce *= falloff;
                }

                totalGravity += gravityDirection * gravityForce;

                if (zone.enableOrbitalMotion)
                {
                    zone.ApplyOrbitalMotion(rb);
                }
            }
        }

        return totalGravity;
    }

    protected virtual void ApplyOrbitalMotion(Rigidbody rb)
    {
        if (rb.linearVelocity.sqrMagnitude < 0.01f)
        {
            Vector3 toCenter = (transform.position - rb.position).normalized;
            float distance = Vector3.Distance(transform.position, rb.position);
            Vector3 orbitalDirection = Vector3.Cross(toCenter, Vector3.up).normalized;
            float orbitalSpeed = Mathf.Sqrt(Mathf.Abs(gravityStrength) / distance) * orbitalSpeedMultiplier;
            rb.linearVelocity = orbitalDirection * orbitalSpeed;
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = gravityStrength >= 0 ? Color.blue : Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;

        switch (shape)
        {
            case ZoneShape.Sphere:
                Gizmos.DrawWireSphere(Vector3.zero, maxDistance);
                break;
            case ZoneShape.Cube:
                Gizmos.DrawWireCube(Vector3.zero, Vector3.one * maxDistance * 2f);
                break;
            case ZoneShape.Cylinder:
                DrawWireCylinder(Vector3.zero, maxDistance, maxDistance * 2f);
                break;
        }
    }

    protected void DrawWireCylinder(Vector3 center, float radius, float height)
    {
        Vector3 top = center + Vector3.up * height / 2f;
        Vector3 bottom = center - Vector3.up * height / 2f;

        for (int i = 0; i <= 36; i++)
        {
            float angle = i * Mathf.PI / 18f;
            Vector3 topPoint = top + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            Vector3 bottomPoint = bottom + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

            if (i > 0)
            {
                Gizmos.DrawLine(
                    top + new Vector3(Mathf.Cos(angle - Mathf.PI / 18f) * radius, 0, Mathf.Sin(angle - Mathf.PI / 18f) * radius),
                    topPoint);
                Gizmos.DrawLine(
                    bottom + new Vector3(Mathf.Cos(angle - Mathf.PI / 18f) * radius, 0, Mathf.Sin(angle - Mathf.PI / 18f) * radius),
                    bottomPoint);
                Gizmos.DrawLine(topPoint, bottomPoint);
            }
        }
    }

    public void SetOrbitalMotion(bool enabled)
    {
        enableOrbitalMotion = enabled;
    }
}