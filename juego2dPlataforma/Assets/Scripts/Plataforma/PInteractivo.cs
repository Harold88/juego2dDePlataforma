using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PInteractivo : MonoBehaviour
{
    private PCambioLayer cambio;
    private BoxCollider2D colision;
    private GameObject jugador;
    private int layerJugador;
    Vector3 posJugador;
    [SerializeField] private float ajustarAltura;
    private void Start()
    {
        colision = transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
        layerJugador = LayerMask.NameToLayer("Jugador");
        cambio = transform.GetChild(0).gameObject.GetComponent<PCambioLayer>();
    }
    private void Update()
    {
        if (jugador != null)
        {
            if (!cambio.activarBajar)
            {
                GiroDelEfecctor();
            }
            else
            {
                colision.isTrigger = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == layerJugador)
        {
            jugador = collision.gameObject;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == layerJugador)
        {
            jugador = null;

        }
    }

    /** Metodos **/
    public void GiroDelEfecctor()
    {
        posJugador = new Vector3(jugador.transform.position.x, jugador.transform.position.y - jugador.GetComponent<CapsuleCollider2D>().size.y / 2);
        colision.isTrigger = (posJugador.y < (colision.transform.position.y + ajustarAltura) ? true : false);
    }

    private void OnDrawGizmos()
    {
        if (jugador != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(colision.transform.position + new Vector3(0, ajustarAltura,0), posJugador);
        }
    }
}
