using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PRecorrer : MonoBehaviour
{
    /*** Variables ***/
    /*****************/
    [Header("variables")]
    [SerializeField] private List<Transform> pos;
    [SerializeField] private float velocidad;
    private int i;
    [Header("Tiempo de Espera")]
    [SerializeField] private float tiempoEspera;
    private bool siguientePlataforma=false;
    [Header("layer jugador")]
    private int layerJugador;
    /*** Cuando se Activa, Desactiva , Destruye ***/
    /**********************************************/

    /*** LLamada por fotograma ***/
    /****************************/
    private void Start()
    {
        layerJugador = LayerMask.NameToLayer("Jugador");
    }
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, pos[i].position, velocidad * Time.deltaTime);
        if (Vector2.Distance(transform.position, pos[i].position) < 0.2f && !siguientePlataforma)
        {
            StartCoroutine(SiguientePlataforma(tiempoEspera));
            siguientePlataforma = true;
        }
    }

    /*** Colisiones ***/
    /*****************/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == layerJugador)
        {
            collision.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layerJugador)
        {
            collision.transform.SetParent(null);
        }
    }
    /*** Metodo ***/
    /*************/
    IEnumerator SiguientePlataforma(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        i++;
        if (i > pos.Count-1) { i = 0; }
        siguientePlataforma = false;
    }
    /*** Input ***/
    /************/

    /*** Ayuda Visuales ***/
    /*********************/
}
