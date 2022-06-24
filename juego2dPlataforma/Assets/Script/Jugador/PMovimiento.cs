using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PMovimiento : MonoBehaviour
{
    /*** Variables ***/
    /*****************/
    [Header("Script")]
    [HideInInspector] public PVerificarSuelo verificarSuelo;
    private PInteraccion interaccion;
    [Header("componentes")]
    [HideInInspector] public Rigidbody2D rb;
    [Header("Variables")]
    [HideInInspector] public float x, y;
     public float velocidad;
    [HideInInspector] public float vel;
    public float salto;
    [SerializeField] private bool mejorarSalto=false;
    public float alSaltar;
    public float alCaer;
    [Header("doble salto")]
    public bool activarDobleSalto;
    private bool dobleSalto = false;
    [Header("reiniciar posicion")]
    private int layerMuerte;
    private Vector2 posicionInicio;
    [Header("Dash")]
    [SerializeField] private float fuerzaDash;
    public bool activarDash=false;
    private bool volverUsarDash;
    [SerializeField] private float tiempoDash;
    [SerializeField] private float tiempoVolverDash;
    [HideInInspector] public int dashX, dashY;
    private bool dashDer;
    private bool dashUp;
    private bool dashMove;
    [Header("Aumento de Salto")]
    public bool activarAumentarSalto;
    public float aumentarSalto;
    [HideInInspector] public float aSalto;
    [Header("bajar plataforma")]
    private int layerPInteractivo;
    private bool activarBajarPlataforma=false;
    private GameObject bajarPlataforma = null;
    /*** Cuando se Activa, Desactiva , Destruye ***/
    /**********************************************/

    /*** LLamada por fotograma ***/
    /****************************/
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        verificarSuelo = transform.GetChild(0).gameObject.GetComponent<PVerificarSuelo>();
        interaccion = GetComponent<PInteraccion>();
        vel = velocidad;
        posicionInicio = transform.position;
        layerMuerte = LayerMask.NameToLayer("Muerte");
        aSalto = aumentarSalto;
        layerPInteractivo = LayerMask.NameToLayer("Default");
    }
    private void Update()
    {
        // movimiento
        
        if (!activarDash) {
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(x * velocidad, rb.velocity.y);
        }
        // salto
        if (Input.GetKey("space") && !activarDash )
        {
            if (verificarSuelo.estaSuelo && !activarBajarPlataforma || verificarSuelo.estaCaja && !activarBajarPlataforma)
            {
                rb.velocity = new Vector2(rb.velocity.x, salto + aumentarSalto);
                dobleSalto = true;
                verificarSuelo.estaSuelo = false;
            }
            else
            {
                if (activarDobleSalto && Input.GetKeyDown("space") && dobleSalto && !activarBajarPlataforma)
                {
                    rb.velocity = new Vector2(rb.velocity.x, salto);
                    dobleSalto = false;
                }
            }
            if(bajarPlataforma != null)
            {
                if (Input.GetKeyDown("space") && y < 0)
                {
                    bajarPlataforma.GetComponent<PCambioLayer>().activarBajar = true;
                    StartCoroutine(TiempoBajarPlataforma());
                }
            }
        }
        else
        {
            //if (verificarSuelo) { activarDobleSalto = false; }
        }
        // graduar salto  mejorado
        if (mejorarSalto && !interaccion.activarSostener)
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * alSaltar * Time.deltaTime;
            }
            if(rb.velocity.y > 0 && !Input.GetKey("space"))
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * alCaer * Time.deltaTime;
            }    
        }
        // dash
        ObtenerDireccionDash();
        if (Input.GetKeyDown("j") && !activarDash && !volverUsarDash)
        {
            activarDash = true;
            volverUsarDash = true;
            StartCoroutine(TiempoDash());
        }
        if (activarDash) { Dash(); }
        // verificacion si tiene plataforma para bajar
        BajarPlataforma();
    }
    /*** Colisiones ***/
    /*****************/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layerMuerte)
        {
            transform.position = posicionInicio;
        }
        if (collision.gameObject.layer == layerPInteractivo && collision.gameObject.name == "PInteractiva")
        {
            bajarPlataforma = collision.gameObject;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if ( collision.gameObject.name == "PInteractiva")
        {
            bajarPlataforma = null;
        }
    }
    /*** Metodo ***/
    /*************/
    public void Dash()
    {
        rb.gravityScale = 0;
        if (!dashMove)
        {
            // x
            if ( dashDer && y == 0 ||  dashDer && x == 0 && y == 0){ rb.velocity = Vector2.right * fuerzaDash; }
            if (!dashDer && y == 0 || !dashDer && x == 0 && y == 0){ rb.velocity = Vector2.left  * fuerzaDash; }
            // y
            if( dashUp && x == 0 && y != 0) { rb.velocity = Vector2.up   * (fuerzaDash * 0.80f); }
            if(!dashUp && x == 0 && y != 0) { rb.velocity = Vector2.down * (fuerzaDash * 0.80f); }
        }
        else
        {
            rb.velocity = new Vector2(dashX, dashY)*(fuerzaDash*0.70f);
        }
    }
    public void ObtenerDireccionDash()
    {
        if (!activarDash)
        {
            if (x > 0)
            {
                dashDer = true;               
            }
            if (x < 0)
            {
                dashDer = false;
            }
            if (y > 0)
            {
                dashUp = true;
            }
            if (y < 0)
            {
                dashUp = false;
            }
            dashMove = (x != 0 && y != 0 ? true : false);
            dashX = (x > 0 ? 1 : -1);
            dashY = (y > 0 ? 1 : -1);
        }
    }
    IEnumerator TiempoDash()
    {
        yield return new WaitForSeconds(tiempoDash);
        rb.gravityScale = 1;
        activarDash = false;
        yield return new WaitForSeconds(tiempoVolverDash);
        volverUsarDash = false;

    }
    IEnumerator TiempoBajarPlataforma()
    {
        yield return new WaitForSeconds(0.5f);
        bajarPlataforma = null;
    }
    public void BajarPlataforma()
    {
        if(bajarPlataforma != null && y < 0)
        {
            activarBajarPlataforma = true;
        }
        else { activarBajarPlataforma = false; }
    }
    /*** Input ***/
    /************/

    /*** Ayuda Visuales ***/
    /*********************/
}
