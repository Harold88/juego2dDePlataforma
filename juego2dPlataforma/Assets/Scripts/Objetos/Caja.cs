using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caja : MonoBehaviour
{
    /*** Variables ***/
    /*****************/
    [Header("Script")]
    private PVerificarSuelo verificarSuelo;
    [Header("Layer")]
    private int layerJugador;
    private int layerAgua;
    private int layerSuelo;
    private int layerMuerte;
    [Header("Verificacion de suelo")]
    private Rigidbody2D rb;
    public bool empujar=false;
    public bool estaSuelo=false;
    public bool estaAgua = false;
    [Header("Posicion del objeto")]
    float xActual;
    float xActerior;
    public float posicion;
    private Vector3 posicionDeLaCaja;
    [Header("Guardar posicion")]
    private Vector2 posicionInicio;
    [Header("Control de rotacion")]
    [SerializeField] private GameObject objetoRayo;
    [SerializeField] private List<Transform> posRayo;
    [SerializeField] private float distanciaDelRayCast;
    [SerializeField] private LayerMask mascara;
    public int rayo1;
    public int rayo2;
    public int rayo3;
    private int cantidadDeRayos;
    private bool habilitarRotacion = false;
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
        // verificacion de suelo si rota
        ActivacionDeRotacion();
        // esta en agua
        VolverASuPosicionEnAgua();
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
        if(collision.gameObject.layer == layerJugador )
        {
            if (verificarSuelo.estaCaja && estaAgua)
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == layerAgua)
        {
            estaAgua = true;
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), 0.5f);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == layerAgua)
        {
            estaAgua = false;
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
        if(cantidadDeRayos > 0) { estaSuelo = true; } else { estaSuelo = false; }
        if (estaSuelo && !empujar && cantidadDeRayos > 1)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        if(estaSuelo && cantidadDeRayos < 2) { rb.bodyType = RigidbodyType2D.Dynamic; }
        if (!estaSuelo)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        if(empujar && estaSuelo)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
    public void ActivacionDeRotacion()
    {
        // rayo 1
        RaycastHit2D hit1 = Physics2D.Raycast(posRayo[0].position, Vector2.down, distanciaDelRayCast, mascara);
        //Debug.DrawLine(transform.position, posRayo[0].position, Color.red, 0.1f);
        if (hit1.collider != null)
        {
            if (hit1.collider.gameObject.layer == layerSuelo)
            {
                rayo1 = 1;
            }
            else { rayo1 = 0; }
        }
        else { rayo1 = 0; }

        // rayo 2
        RaycastHit2D hit2 = Physics2D.Raycast(posRayo[1].position,Vector2.down, distanciaDelRayCast, mascara);
        //Debug.DrawLine(posRayo[1].position, Vector2.down*distanciaDelRayCast, Color.yellow, 0.1f);
        if (hit2.collider != null)
        {
            print(hit2.collider.gameObject.name);
            if (hit2.collider.gameObject.layer == layerSuelo)
            {
                rayo2 = 1;
            }
            else { rayo2 = 0; }
        }
        else { rayo2 = 0; }

        // rayo 3
        RaycastHit2D hit3 = Physics2D.Raycast(posRayo[2].position, Vector2.down, distanciaDelRayCast, mascara);
        //Debug.DrawLine(transform.position, posRayo[2].position , Color.blue, 0.1f);
        if (hit3.collider != null)
        {
            if (hit3.collider.gameObject.layer == layerSuelo)
            {
                rayo3 = 1;
            }
            else { rayo3 = 0; }
        }
        else { rayo3 = 0; }

        //  rayos dirigidos en la misma posicion sin importar el giro del padre
        objetoRayo.transform.rotation = Quaternion.Euler(Vector3.zero);
        // habilitacion de la rotacion z
        cantidadDeRayos = rayo1 + rayo2 + rayo3;
        if( cantidadDeRayos > 1 && !habilitarRotacion) { habilitarRotacion = false; StartCoroutine(HabilitarRotacion()); }
        else { rb.constraints = RigidbodyConstraints2D.None; }
    }
    IEnumerator HabilitarRotacion()
    {
        yield return new WaitForSeconds(0.5f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    public void VolverASuPosicionEnAgua()
    {
        if (estaAgua)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), 0.015f);
        }
    }
    /*** Input ***/
    /************/

    /*** Ayuda Visuales ***/
    /*********************/
    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(posRayo[0].position, posRayo[0].position - new Vector3(0, distanciaDelRayCast, 0));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(posRayo[1].position, posRayo[1].position - new Vector3(0, distanciaDelRayCast, 0));
         Gizmos.color = Color.blue;
        Gizmos.DrawLine(posRayo[2].position, posRayo[2].position - new Vector3(0, distanciaDelRayCast, 0));
    }*/
}
