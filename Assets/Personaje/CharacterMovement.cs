using UnityEngine;
using UnityEngine.InputSystem;

public class Playermovement : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;

    private Rigidbody rb;
    private bool puedeSaltar;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Salto
        if (Keyboard.current.spaceKey.wasPressedThisFrame && puedeSaltar)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            puedeSaltar = false;
        }
    }

    void FixedUpdate()
    {
        float horizontal = 0f;
        float vertical = 0f;

        // WASD
        if (Keyboard.current.aKey.isPressed)
            horizontal = -1f;

        if (Keyboard.current.dKey.isPressed)
            horizontal = 1f;

        if (Keyboard.current.wKey.isPressed)
            vertical = 1f;

        if (Keyboard.current.sKey.isPressed)
            vertical = -1f;

        Vector3 movimiento = new Vector3(horizontal, 0f, vertical).normalized;

        Vector3 velocidadMovimiento = movimiento * velocidad;

        rb.linearVelocity = new Vector3(
            velocidadMovimiento.x,
            rb.linearVelocity.y,
            velocidadMovimiento.z
        );
    }

    private void OnCollisionEnter(Collision collision)
    {
        puedeSaltar = true;
    }
}