using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tranpolin : MonoBehaviour
{
    private int layerJugador;
    [SerializeField] private float fuerzaImpulso;
    private void Start()
    {
        layerJugador = LayerMask.NameToLayer("Jugador");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if( collision.gameObject.layer == layerJugador)
        {
            collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up * fuerzaImpulso);
        }
    }
}
