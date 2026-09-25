using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 distancia = new Vector3(5f, 0f, 0f);
    public float velocidad = 2f;

    private Vector3 posicionInicial;
    private Vector3 posicionAnterior;

    public Vector3 MovimientoActual { get; private set; }

    void Start()
    {
        posicionInicial = transform.position;
        posicionAnterior = transform.position;
    }

    void FixedUpdate()
    {
        float movimiento = Mathf.PingPong(Time.time * velocidad, 1f);

        Vector3 nuevaPosicion = Vector3.Lerp(
            posicionInicial,
            posicionInicial + distancia,
            movimiento
        );

        transform.position = nuevaPosicion;

        MovimientoActual = nuevaPosicion - posicionAnterior;

        posicionAnterior = nuevaPosicion;
    }
}
