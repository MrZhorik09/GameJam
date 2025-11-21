using UnityEngine;

public class ShipController2 : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotSpeed;
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
       rb.AddForce(transform.forward*speed*Input.GetAxis("Vertical"));
      // rb.AddForce(transform.right*speed*Input.GetAxis("Horizontal"));
       rb.AddTorque(Vector3.up*rotSpeed*Input.GetAxis("Horizontal"));
       rb.AddTorque(Vector3.forward*rotSpeed*Input.GetAxis("Horizontal"));
    }

}
