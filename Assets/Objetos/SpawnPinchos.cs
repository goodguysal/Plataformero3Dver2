using UnityEngine;

public class SpawnPinchos : MonoBehaviour
{
    public GameObject plataformaPinchos;
    public Transform puntoSpawn;

    private bool yaAparecio = false;
    private GameObject plataformaActual;

    private void OnTriggerEnter(Collider other)
    {
        if (yaAparecio)
            return;

        if (other.CompareTag("Player"))
        {
            plataformaActual = Instantiate(
                plataformaPinchos,
                puntoSpawn.position,
                puntoSpawn.rotation
            );

            yaAparecio = true;
        }
    }

    public void ResetearSpawn()
    {
        
        if (plataformaActual != null)
        {
            Destroy(plataformaActual);
            plataformaActual = null;
        }

        
        yaAparecio = false;
    }
}
