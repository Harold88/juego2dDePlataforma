using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PImpulso : MonoBehaviour
{
    /** Variables **/
    [SerializeField] private List<Transform> pos;
    private int i;
    [SerializeField] private float velocidad;
    private float Vel;
    private int layerJugador;

    /** LLamado por Fotograma **/
    private void Start()
    {
        Vel = velocidad;
        layerJugador = LayerMask.NameToLayer("Jugador");
    }
    private void Update()
    {
        Movimiento();
    }
    /** Colisiones **/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layerJugador) { i = 1; collision.collider.transform.SetParent(transform);}
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layerJugador) { collision.collider.transform.SetParent(null); }
    }


    /** Metodos **/
    public void Movimiento()
    {
        transform.position = Vector2.MoveTowards(transform.position, pos[i].position, velocidad * Time.deltaTime);
        velocidad = (i == 0 ? Vel / 4 : Vel);
        if (Vector2.Distance(transform.position, pos[1].position) < 0.1f) { i = 0; }
    }
}
