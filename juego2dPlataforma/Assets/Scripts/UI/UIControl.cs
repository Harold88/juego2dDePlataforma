using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour
{
    /*** Variables ***/
    /*****************/
    [Header("Boton Opciones jugador")]
    [SerializeField] private GameObject PJConfiguracion;
    /*** Cuando se Activa, Desactiva , Destruye ***/
    /**********************************************/

    /*** LLamada por fotograma ***/
    /****************************/
    private void Start()
    {

    }
    private void Update()
    {
        if(Input.GetKeyDown("escape") || Input.GetKeyDown("enter"))
        {
            if (PJConfiguracion.activeInHierarchy)
            {
                PJConfiguracion.SetActive(false);
            }
            else
            {
                PJConfiguracion.SetActive(true);
            }
        }
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
