using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMovimiento : MonoBehaviour
{
    /*** Variables ***/
    /*****************/
    [Header("Script")]
    private PVerificarSuelo verificarSuelo;
    [Header("componentes")]
    private Rigidbody2D rb;
    [Header("Variables")]
    private float x, y;
    [SerializeField]private float velocidad;
    [SerializeField] private float salto;
    [SerializeField] private bool mejorarSalto=false;
    public float alSaltar;
    public float alCaer;
    [Header("doble salto")]
    public bool activarDobleSalto;
    private bool dobleSalto = false;

    /*** Cuando se Activa, Desactiva , Destruye ***/
    /**********************************************/

    /*** LLamada por fotograma ***/
    /****************************/
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        verificarSuelo = transform.GetChild(0).gameObject.GetComponent<PVerificarSuelo>();
    }
    private void Update()
    {
        x = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(x * velocidad, rb.velocity.y);
        // salto
        if (Input.GetKey("space"))
        {
            if (verificarSuelo.estaSuelo)
            {
                rb.velocity = new Vector2(rb.velocity.x, salto);
                dobleSalto = true;
            }
            else
            {
                if (activarDobleSalto && Input.GetKeyDown("space") && activarDobleSalto)
                {
                    rb.velocity = new Vector2(rb.velocity.x, salto);
                    dobleSalto = false;
                }
            }
        }
        else
        {
            //if (verificarSuelo) { activarDobleSalto = false; }
        }
        // graduar salto  mejorado
        if (mejorarSalto)
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
    }
    /*** Colisiones ***/
    /*****************/

    /*** Metodo ***/
    /*************/


    /*** Input ***/
    /************/

    /*** Ayuda Visuales ***/
    /*********************/
}
