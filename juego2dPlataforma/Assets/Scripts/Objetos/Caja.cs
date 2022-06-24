using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caja : MonoBehaviour
{
    /*** Variables ***/
    /*****************/
    [Header("Script")]
    private CajaVerificarSuelo cajaVerificarSuelo;
    private PVerificarSuelo verificarSuelo;
    [Header("Layer")]
    private int layerJugador;
    private int layerAgua;
    private int layerSuelo;
    private int layerMuerte;
    [Header("Verificacion de suelo")]
    private Rigidbody2D rb;
    public bool empujar=false;
    [Header("Posicion del objeto")]
    float xActual;
    float xActerior;
    public float posicion;
    [Header("Guardar posicion")]
    private Vector2 posicionInicio;

    /*** Cuando se Activa, Desactiva , Destruye ***/
    /**********************************************/
    private void OnEnable()
    {
        if(FindObjectOfType<PVerificarSuelo>() != null)
        {
            verificarSuelo = FindObjectOfType<PVerificarSuelo>();
        }
        else { verificarSuelo = null; }
    }
    /*** LLamada por fotograma ***/
    /****************************/
    private void Start()
    {
        cajaVerificarSuelo = transform.GetChild(0).gameObject.GetComponent<CajaVerificarSuelo>();
        layerJugador = LayerMask.NameToLayer("Jugador");
        layerAgua = LayerMask.NameToLayer("Agua");
        layerSuelo = LayerMask.NameToLayer("Suelo");
        layerMuerte = LayerMask.NameToLayer("Muerte");
        rb = GetComponent<Rigidbody2D>();
        posicionInicio = transform.position;
    }
    private void Update()
    {
        // buscar posicion
        jugadorSobreCaja();
        // empujar
        EstaSuelo();
    }
    /*** Colisiones ***/
    /*****************/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layerMuerte)
        {
            rb.velocity = Vector2.zero;
            transform.position = posicionInicio;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.layer == layerJugador)
        {
            if (verificarSuelo.estaCaja)
            {
                collision.transform.position = new Vector3(collision.transform.position.x + posicion, collision.transform.position.y, collision.transform.position.z);

            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layerJugador)
        {
            collision.transform.SetParent(null);
            empujar = false;
        }
    }
    /*** Metodo ***/
    /*************/
    public void jugadorSobreCaja()
    {
        xActual = transform.position.x;
        if( xActual != xActerior)
        {
            posicion = xActual - xActerior;
        }
        xActerior = xActual;
    }
    public void EstaSuelo()
    {
        if (cajaVerificarSuelo.estaSuelo && !empujar)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;
        }
        if (!cajaVerificarSuelo.estaSuelo)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        if (empujar)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
    /*** Input ***/
    /************/

    /*** Ayuda Visuales ***/
    /*********************/
}
