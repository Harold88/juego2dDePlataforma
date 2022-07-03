using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public float salto;
    public bool mejorarSalto=false;
    public float alSaltar;
    public float alCaer;
    [Header("doble salto")]
    public bool activarDobleSalto;
    private bool dobleSalto = false;
    [Header("reiniciar posicion")]
    private int layerMuerte;
    private Vector2 posicionInicio;
    [Header("Dash")]
    private int flechaX, flechaY;
    public bool dashHorizontal;
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
    [HideInInspector] public float aumentarSalto;
    public float aSalto;
    [Header("bajar plataforma")]
    private int layerPInteractivo;
    private bool activarBajarPlataforma=false;
    private GameObject bajarPlataforma = null;
    [Header("menu Ui para configuracion")]
    [HideInInspector] public UI_PJConfiguracion UIConfiguracion;
    [Header("Punto de Guia")]
    [SerializeField] private Transform puntoGuia;
    private float puntoX, puntoY;
    /*** Cuando se Activa, Desactiva , Destruye ***/
    /**********************************************/

    /*** LLamada por fotograma ***/
    /****************************/
    private void Start()
    {
        if(FindObjectOfType<UI_PJConfiguracion>() != null) { UIConfiguracion = FindObjectOfType<UI_PJConfiguracion>(); }
        rb = GetComponent<Rigidbody2D>();
        verificarSuelo = transform.GetChild(0).gameObject.GetComponent<PVerificarSuelo>();
        interaccion = GetComponent<PInteraccion>();
        posicionInicio = transform.position;
        layerMuerte = LayerMask.NameToLayer("Muerte");
        aSalto = aumentarSalto;
        layerPInteractivo = LayerMask.NameToLayer("Default");


        // validacion de numero
        velocidad = PlayerPrefs.GetFloat("velocidad", 0);
        salto = PlayerPrefs.GetFloat("salto", 0);
        if(PlayerPrefs.GetInt("mejorarSalto", 0)    > 0) { mejorarSalto = true; }      else { mejorarSalto = false; }
        if (PlayerPrefs.GetInt("saltoDoble", 0)     > 0) { activarDobleSalto = true; } else { activarDobleSalto = false; }
        if (PlayerPrefs.GetInt("dashHorizontal", 0) > 0) { dashHorizontal = true; }    else { dashHorizontal = false; }
    }
    private void Update()
    {
        // conseguir script
        if (FindObjectOfType<UI_PJConfiguracion>() != null) { UIConfiguracion = FindObjectOfType<UI_PJConfiguracion>(); }
        else { UIConfiguracion = null; }
        // configuracion de ui 
        ConfiguracionUIPersonaje();
        // movimiento

        if (!activarDash) {
            // x
            if (Input.GetKey("a") && !Input.GetKey("d")) { x = -1; }
            if (Input.GetKey("d") && !Input.GetKey("a")) { x =  1; }
            if(!Input.GetKey("a") && !Input.GetKey("d")) { x =  0; }
            // y
            if (Input.GetKey("s") && !Input.GetKey("w")) { y = -1; }
            if (Input.GetKey("w") && !Input.GetKey("s")) { y =  1; }
            if (!Input.GetKey("s") && !Input.GetKey("w")){ y =  0; }
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
        if (!activarDash)
        {
            // x
            if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) { flechaX = -1; }
            if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow)) { flechaX = 1; }
            if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) { flechaX = 0; }
            // y
            if (Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow)) { flechaY = -1; }
            if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow)) { flechaY = 1; }
            if (!Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow)) { flechaY = 0; }
        }
        ObtenerDireccionDash();
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow))
        {
            if (!activarDash && !volverUsarDash && !gameObject.GetComponent<PInteraccion>().activarSostener)
            {
                StartCoroutine(RetrasoDash());
            }
        }
        if (activarDash) { Dash(); }
        // verificacion si tiene plataforma para bajar
        BajarPlataforma();

        // punto de guia
        PuntoGuia();
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
            if (!dashHorizontal)
            {
                // x
                if ( dashDer && flechaY == 0 ||  dashDer && flechaX == 0 && flechaY == 0) { rb.velocity = Vector2.right * fuerzaDash; }
                if (!dashDer && flechaY == 0 || !dashDer && flechaX == 0 && flechaY == 0) { rb.velocity = Vector2.left  * fuerzaDash; }
                // y
                if ( dashUp && flechaX == 0 && flechaY != 0) { rb.velocity = Vector2.up   * (fuerzaDash * 0.80f); }
                if (!dashUp && flechaX == 0 && flechaY != 0) { rb.velocity = Vector2.down * (fuerzaDash * 0.80f); }
            }
            else
            {
                // x
                if (dashDer  ) { rb.velocity = Vector2.right * fuerzaDash; }
                if (!dashDer ) { rb.velocity = Vector2.left * fuerzaDash; }
            }
        }
        else
        {
            rb.velocity = new Vector2(dashX, dashY) * (fuerzaDash * 0.70f);
        }
    }
    public void ObtenerDireccionDash()
    {
        if (!activarDash)
        {
            if (flechaX > 0)
            {
                dashDer = true;               
            }
            if (flechaX < 0)
            {
                dashDer = false;
            }
            if (flechaY > 0)
            {
                dashUp = true;
            }
            if (flechaY < 0)
            {
                dashUp = false;
            }
            if (!dashHorizontal) { dashMove = (flechaX != 0 && flechaY != 0 ? true : false); } else { dashMove = false; }
            dashX = (flechaX > 0 ? 1 : -1);
            dashY = (flechaY > 0 ? 1 : -1);
        }
    }
    IEnumerator TiempoDash()
    {
        yield return new WaitForSeconds(tiempoDash);
        activarDash = false;
        rb.gravityScale = 1;
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

    IEnumerator RetrasoDash()
    {
        yield return new WaitForSeconds(0.08f);
        activarDash = true;
        volverUsarDash = true;
        StartCoroutine(TiempoDash());
    }
    public void PuntoGuia()
    {
        puntoX = Input.GetAxis("Horizontal");
        puntoY = Input.GetAxis("Vertical");
        puntoGuia.localPosition = new Vector3(puntoX,puntoY,0);
    }
    /*** Input ***/
    /************/

    /*** Ayuda Visuales ***/
    /*********************/

    /*** configuracion desde Ui ***/
    /*********************/
    public void ConfiguracionUIPersonaje()
    {
        if (UIConfiguracion != null)
        {
            if (UIConfiguracion.gameObject.activeInHierarchy)
            {
                velocidad = float.Parse(UIConfiguracion.velocidad.text);
                salto = float.Parse(UIConfiguracion.salto.text);
                mejorarSalto = UIConfiguracion.mejorarSalto.isOn;
                activarDobleSalto = UIConfiguracion.saltoDoble.isOn;
                dashHorizontal = UIConfiguracion.dashHorizontal.isOn;
            }
        }
    }

}
