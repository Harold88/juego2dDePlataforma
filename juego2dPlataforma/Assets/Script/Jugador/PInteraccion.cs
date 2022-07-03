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
    private int direccionY;
    [HideInInspector] public bool activarSostener;
    [Header("img resistencia")]
    [SerializeField] private Image imgResistencia;
    [HideInInspector]public bool activarSalto = false;
    public float tiempoEstamina;
    [Header("tiempo para recuperarse")]
    [SerializeField] private float tiempoRecuperarse;
    private float tiempoParaRecuperar;
    [Header("layer caja")]
    private int layerCaja;
    [Header("desabilitar llamado")]
    private bool desabilitar;
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
        DireccionXY();
        // sostener en la pared
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(direccionX, direccionY), distanciaDelRayCast,mascara) ;
        Debug.DrawRay(transform.position, new Vector3(direccionX*distanciaDelRayCast, direccionY * distanciaDelRayCast, 0), Color.blue, 0.1f);
        if(hit.collider != null && !Input.GetKey("space"))
        {
            desabilitar = true;
            // interar con pared
            if (hit.collider.gameObject.layer == layerPSostener && imgResistencia.fillAmount > 0.001f)
            {
                move.activarAumentarSalto = true;
                activarSostener = true;
                imgResistencia.fillAmount -= 0.003f;
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
                //move.velocidad = PlayerPrefs.GetFloat("velocidad", 0);
                print("1");
            }
        }
        else
        {
            activarSostener = false;
            move.rb.gravityScale = 1;
            activarSalto = false;
            if (move.activarAumentarSalto) { StartCoroutine(AumentarSalto()); move.activarAumentarSalto = false; }
            if (desabilitar)
            {
                move.velocidad = PlayerPrefs.GetFloat("velocidad", 0);
                desabilitar = false;
            }
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
    public void DireccionXY()
    {
        if(Input.GetKey(KeyCode.LeftArrow)) {  direccionX = -1; }
        if(Input.GetKey(KeyCode.RightArrow)) { direccionX =  1; }
        if(!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) { direccionX = 0; }

        if(Input.GetKey(KeyCode.UpArrow))   {  direccionY =  1; }
        if (Input.GetKey(KeyCode.DownArrow)) { direccionY = -1; }
        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow)) { direccionY = 0; }


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
