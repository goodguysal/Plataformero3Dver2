using UnityEngine;

public class MovingPlatformVertical : MonoBehaviour
{
    public Vector3 distancia = new Vector3(0f, 5f, 0f);
    public float velocidad = 2f;

    private Rigidbody rb;
    private Vector3 posicionInicial;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        posicionInicial = rb.position;
    }

    void FixedUpdate()
    {
        float movimiento = Mathf.PingPong(
            Time.time * velocidad,
            1f
        );

        Vector3 nuevaPosicion = Vector3.Lerp(
            posicionInicial,
            posicionInicial + distancia,
            movimiento
        );

        rb.MovePosition(nuevaPosicion);
    }
}