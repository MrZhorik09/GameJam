using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float runSpeed = 15f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;

    private Vector2 mv;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        mv.x = Input.GetAxis("Horizontal");
        mv.y = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
        }
            // Делаем Raycast вниз от позиции персонажа
            RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
        {



                // Получаем нормаль поверхности под ногами
                Vector3 groundNormal = hit.normal;

                // Формируем вектор движения в мировых координатах (X,Z)
                Vector3 inputDir = new Vector3(mv.x, 0, mv.y);

                // Проецируем inputDir на плоскость, заданную groundNormal, чтобы движение было вдоль поверхности
                Vector3 moveDir = Vector3.ProjectOnPlane(inputDir, groundNormal).normalized;

                // Расчёт силы для перемещения
                Vector3 force = moveDir * movementSpeed;

                // Прикладываем силу к Rigidbody
                rb.AddForce(force, ForceMode.Force);
           
        }
    }
}
