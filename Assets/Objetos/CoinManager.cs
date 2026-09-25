using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int monedas = 0;
    public int maxMonedas = 4;

    public TextMeshProUGUI textoMonedas;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ActualizarTexto();
    }

    public void RecolectarMoneda()
    {
        if (monedas < maxMonedas)
        {
            monedas++;
            ActualizarTexto();
        }
    }

    private void ActualizarTexto()
    {
        textoMonedas.text = "ORO: " + monedas + " / " + maxMonedas;
    }
}
