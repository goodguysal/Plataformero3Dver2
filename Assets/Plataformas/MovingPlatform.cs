using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 distancia = new Vector3(5f, 0f, 0f);
    public float velocidad = 2f;

    private Rigidbody rb;
    private Vector3 posicionInicial;
    private Vector3 posicionAnterior;

    public Vector3 MovimientoActual { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        posicionInicial = rb.position;
        posicionAnterior = rb.position;

        // La plataforma es controlada por el script,
        // pero sigue participando correctamente en la física.
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void FixedUpdate()
    {
        float movimiento = Mathf.PingPong(Time.time * velocidad, 1f);

        Vector3 nuevaPosicion = Vector3.Lerp(
            posicionInicial,
            posicionInicial + distancia,
            movimiento
        );

        // Calculamos cuánto se moverá esta vez
        MovimientoActual = nuevaPosicion - posicionAnterior;

        // Movemos el Rigidbody, no el Transform
        rb.MovePosition(nuevaPosicion);

        posicionAnterior = nuevaPosicion;
    }
}

