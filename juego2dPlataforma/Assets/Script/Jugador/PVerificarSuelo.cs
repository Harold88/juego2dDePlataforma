using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVerificarSuelo : MonoBehaviour
{
    /*** Variables ***/
    /*****************/
    private int layerSuelo;
    private int layerCaja;
    public bool estaSuelo;
    public bool estaCaja;
    /*** Cuando se Activa, Desactiva , Destruye ***/
    /**********************************************/

    /*** LLamada por fotograma ***/
    /****************************/
    private void Start()
    {
        layerSuelo = LayerMask.NameToLayer("Suelo");
        layerCaja = LayerMask.NameToLayer("Caja");
    }
    private void Update()
    {

    }
    /*** Colisiones ***/
    /*****************/
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == layerSuelo)
        {
            estaSuelo = true;
        }
        if (collision.gameObject.layer == layerCaja)
        {
            estaCaja = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == layerSuelo)
        {
            estaSuelo = false;
        }
        if (collision.gameObject.layer == layerCaja)
        {
            estaCaja  = false;
        }
    }
    /*** Metodo ***/
    /*************/

    /*** Input ***/
    /************/

    /*** Ayuda Visuales ***/
    /*********************/
}
