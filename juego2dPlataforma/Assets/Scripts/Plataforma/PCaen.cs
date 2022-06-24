using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCaen : MonoBehaviour
{
    private int layerJugador;
    [SerializeField] private float tiempoCaida;
    private Rigidbody2D rb2D;
    private BoxCollider2D colision;

    private void Start()
    {
        layerJugador = LayerMask.NameToLayer("Jugador");
        rb2D = GetComponent<Rigidbody2D>();
        colision = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == layerJugador)
        {
            StartCoroutine("SeCae");
        }
    }
    IEnumerator SeCae()
    {
        Vector2 Inicio = transform.position;
        yield return new WaitForSeconds(tiempoCaida);
        rb2D.bodyType = RigidbodyType2D.Dynamic;
        rb2D.gravityScale = 1;
        colision.isTrigger = true;
        yield return new WaitForSeconds(3);
        rb2D.velocity = Vector2.zero;
        rb2D.bodyType = RigidbodyType2D.Kinematic;
        transform.position = Inicio;
        colision.isTrigger = false;




    }
}
