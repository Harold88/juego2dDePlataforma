using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CajaVerificarSuelo : MonoBehaviour
{
    /*** Variables ***/
    /*****************/
    private int layerSuelo;
    public bool estaSuelo;
    /*** Cuando se Activa, Desactiva , Destruye ***/
    /**********************************************/

    /*** LLamada por fotograma ***/
    /****************************/
    private void Start()
    {
        layerSuelo = LayerMask.NameToLayer("Suelo");
    }
    private void Update()
    {

    }
    /*** Colisiones ***/
    /*****************/
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == layerSuelo)
        {
            estaSuelo = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == layerSuelo)
        {
            estaSuelo = false;
        }
    }
}
