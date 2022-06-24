using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCambioLayer : MonoBehaviour
{
    public float tiempoCambioLayer;
    private float layerJugador;
    public bool activarBajar = false;
    private bool restaurarPlataforma;
    // Start is called before the first frame update
    void Start()
    {
        layerJugador = LayerMask.NameToLayer("Jugador");
    }

    // Update is called once per frame
    void Update()
    {
        // restaurar plataforma
        Restaurar();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layerJugador && gameObject.layer == 0)
        {
            StartCoroutine(CambioLayer());
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layerJugador)
        {
            gameObject.layer = 0;
        }
    }
    IEnumerator CambioLayer()
    {
        yield return new WaitForSeconds(tiempoCambioLayer);
        gameObject.layer = 6;
    }
    IEnumerator RestaurarPlataforma()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.layer = 0;
        activarBajar = false;
        restaurarPlataforma = false;
    }
    public void Restaurar()
    {
        if (!restaurarPlataforma && activarBajar)
        {
            StartCoroutine(RestaurarPlataforma());
            restaurarPlataforma = true;
        }
    }
}
