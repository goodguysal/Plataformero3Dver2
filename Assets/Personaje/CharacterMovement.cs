using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Playermovement : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;

    private Rigidbody rb;
    private bool puedeSaltar;
    private MovingPlatform plataformaActual;

    // Checkpoint actual
    private Vector3 posicionCheckpoint;

    // =========================
    // DASH
    // =========================

    public float fuerzaDash = 15f;
    public float duracionDash = 0.25f;

    private GameObject bolaObjetivo;
    private bool haciendoDash;
    private float tiempoDash;

    // Texto "E"
    public TextMeshProUGUI textoDash;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // El primer checkpoint es la posición inicial del Player
        posicionCheckpoint = transform.position;

        // Ocultar la E al comenzar
        if (textoDash != null)
        {
            textoDash.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // =========================
        // SALTO
        // =========================

        if (Keyboard.current.spaceKey.wasPressedThisFrame && puedeSaltar)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            puedeSaltar = false;
        }

        // =========================
        // BUSCAR BOLA
        // =========================

        BuscarBolaDash();

        // =========================
        // ACTIVAR DASH
        // =========================

        if (Keyboard.current.eKey.wasPressedThisFrame && bolaObjetivo != null && !haciendoDash)
        {
            IniciarDash();
        }
    }

    void FixedUpdate()
    {
        // Si estamos haciendo dash
        if (haciendoDash)
        {
            tiempoDash -= Time.fixedDeltaTime;

            if (tiempoDash <= 0f)
            {
                haciendoDash = false;
            }

            return;
        }

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

    // =====================================================
    // BUSCAR LA BOLA MÁS CERCANA
    // =====================================================

    private void BuscarBolaDash()
    {
        GameObject nuevaBola = null;
        float distanciaMasCercana = Mathf.Infinity;

        // Busca todos los colliders cercanos
        Collider[] objetosCercanos = Physics.OverlapSphere(
            transform.position,
            5f
        );

        foreach (Collider objeto in objetosCercanos)
        {
            if (objeto.CompareTag("BolaDash"))
            {
                float distancia = Vector3.Distance(
                    transform.position,
                    objeto.transform.position
                );

                if (distancia < distanciaMasCercana)
                {
                    distanciaMasCercana = distancia;
                    nuevaBola = objeto.gameObject;
                }
            }
        }

        bolaObjetivo = nuevaBola;

        // Mostrar u ocultar la E
        if (textoDash != null)
        {
            textoDash.gameObject.SetActive(bolaObjetivo != null);
        }
    }

    // =====================================================
    // INICIAR DASH
    // =====================================================

    private void IniciarDash()
    {
        if (bolaObjetivo == null)
            return;

        Vector3 direccion = (
            bolaObjetivo.transform.position - transform.position
        ).normalized;

        // Impulso hacia la bola
        rb.linearVelocity = direccion * fuerzaDash;

        haciendoDash = true;
        tiempoDash = duracionDash;

        // Ocultar la E durante el dash
        if (textoDash != null)
        {
            textoDash.gameObject.SetActive(false);
        }
    }

    // =====================================================
    // COLISIONES
    // =====================================================

    private void OnCollisionEnter(Collision collision)
    {
        // Poder saltar al tocar el suelo
        puedeSaltar = true;

        // Si toca un piso mortal
        if (collision.gameObject.CompareTag("Piso"))
        {
            Morir();
        } }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            plataformaActual = collision.gameObject.GetComponent<MovingPlatform>();

            if (plataformaActual != null)
            {
                rb.MovePosition(
                    rb.position + plataformaActual.MovimientoActual
                );
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            if (plataformaActual ==
                collision.gameObject.GetComponent<MovingPlatform>())
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

    // =====================================================
    // MORIR
    // =====================================================

    private void Morir()
    {
        Debug.Log("Player murió");

        rb.linearVelocity = Vector3.zero;
        transform.position = posicionCheckpoint;
    }
}