using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PInteraccion : MonoBehaviour
{
    /*** Variables ***/
    /*****************/
    private PMovimiento move;
    [SerializeField] private float distanciaDelRayCast;
    [SerializeField] private LayerMask mascara;
    private int layerPSostener;
    private int direccionX;
    [HideInInspector] public bool activarSostener;
    [Header("img resistencia")]
    [SerializeField] private Image imgResistencia;
    [HideInInspector]public bool activarSalto = false;
    [Header("tiempo para recuperarse")]
    [SerializeField] private float tiempoRecuperarse;
    private float tiempoParaRecuperar;
    [Header("layer caja")]
    private int layerCaja;

    /*** Cuando se Activa, Desactiva , Destruye ***/
    /**********************************************/

    /*** LLamada por fotograma ***/
    /****************************/
    private void Start()
    {
        move = GetComponent<PMovimiento>();
        layerPSostener = LayerMask.NameToLayer("PSostener");
        layerCaja = LayerMask.NameToLayer("Caja");
    }
    private void Update()
    {
        // direccion de x
        DireccionX();
        // sostener en la pared
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(direccionX, 0), distanciaDelRayCast,mascara) ;
        Debug.DrawRay(transform.position, new Vector3(direccionX*distanciaDelRayCast, 0, 0), Color.blue, 0.1f);
        if(hit.collider != null && !Input.GetKey("space"))
        {
            // interar con pared
            if (hit.collider.gameObject.layer == layerPSostener && imgResistencia.fillAmount > 0.001f)
            {
                move.activarAumentarSalto = true;
                activarSostener = true;
                imgResistencia.fillAmount -= 0.005f;
                move.rb.velocity = new Vector2(move.rb.velocity.x, 0); move.rb.gravityScale = 0;
                if (!activarSalto) { move.verificarSuelo.estaSuelo = true;activarSalto = true; }
                tiempoParaRecuperar = tiempoRecuperarse;
            }
            else
            {
                activarSostener = false;
                move.rb.gravityScale = 1;
            }
            // interar con caja
            if (hit.collider.gameObject.layer == layerCaja)
            {
                if (Input.GetKey("k"))
                {
                    hit.collider.gameObject.GetComponent<Caja>().empujar = true;
                    move.velocidad = 2;
                }
                else
                {
                    hit.collider.gameObject.GetComponent<Caja>().empujar = false;
                    move.velocidad = 2;
                }
            }
            else
            {
                move.velocidad = move.vel;
            }
        }
        else
        {
            activarSostener = false;
            move.rb.gravityScale = 1;
            activarSalto = false;
            move.velocidad = move.vel;
            if (move.activarAumentarSalto) { StartCoroutine(AumentarSalto()); move.activarAumentarSalto = false; }
        }
        // recuperar resistencia
        if (tiempoParaRecuperar<0 && imgResistencia.fillAmount != 1)
        {
            imgResistencia.fillAmount += 0.001f;
        }
        if (tiempoParaRecuperar > 0)
        {
            tiempoParaRecuperar -= Time.deltaTime;
        }
    }
    /*** Colisiones ***/
    /*****************/
    public void DireccionX()
    {
        if(Input.GetKey("d")) { direccionX = 1; }
        if(Input.GetKey("a")) { direccionX = -1; }
        if(!Input.GetKey("d") && !Input.GetKey("a")) { direccionX = 0; }
    }
    /*** Metodo ***/
    /*************/
    IEnumerator AumentarSalto()
    {
        move.aumentarSalto = move.aSalto;
        yield return new WaitForSeconds(0.5f);
        move.aumentarSalto = 0;
    }
    /*** Input ***/
    /************/

    /*** Ayuda Visuales ***/
    /*********************/
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position, new Vector3(move.x, 0,0));
    }
}
