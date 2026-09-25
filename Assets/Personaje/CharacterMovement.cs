using UnityEngine;
using UnityEngine.InputSystem;

public class Playermovement : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;

    private Rigidbody rb;
    private bool puedeSaltar;
    private MovingPlatform plataformaActual;

    // Checkpoint actual
    private Vector3 posicionCheckpoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // El primer checkpoint es la posición inicial del Player
        posicionCheckpoint = transform.position;
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
        // Poder saltar al tocar el suelo
        puedeSaltar = true;

        // Si toca un piso mortal
        if (collision.gameObject.CompareTag("Piso"))
        {
            Morir();
        }

    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            plataformaActual = collision.gameObject.GetComponent<MovingPlatform>();

            if (plataformaActual != null)
            {
                rb.MovePosition(rb.position + plataformaActual.MovimientoActual);
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            if (plataformaActual == collision.gameObject.GetComponent<MovingPlatform>())
            {
                plataformaActual = null;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Si toca un checkpoint
        if (other.CompareTag("Checkpoint"))
        {
            posicionCheckpoint = other.transform.position;

            Debug.Log("Checkpoint actualizado");
        }
    }

    private void Morir()
    {
        Debug.Log("Player murió");

        // Detener el movimiento
        rb.linearVelocity = Vector3.zero;

        // Volver al último checkpoint
        transform.position = posicionCheckpoint;
    }
}