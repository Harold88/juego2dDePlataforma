using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAnimacion : MonoBehaviour
{
    /*** Variables ***/
    /*****************/
    [Header("Script")]
    private PMovimiento move;
    [Header("animacion")]
    private Animator anim;
    private SpriteRenderer sprite;
    /*** Cuando se Activa, Desactiva , Destruye ***/
    /**********************************************/

    /*** LLamada por fotograma ***/
    /****************************/
    private void Start()
    {
        move = GetComponent<PMovimiento>();
        anim = gameObject.transform.GetChild(2).gameObject.GetComponent<Animator>();
        sprite = gameObject.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        // volvertar sprite
        VolverSprite();
    }
    /*** Colisiones ***/
    /*****************/

    /*** Metodo ***/
    /*************/
    public void VolverSprite()
    {
        if(move.x > 0)
        {
            sprite.flipX = false;
        }
        if(move.x < 0)
        {
            sprite.flipX = true ;
        }
    }
    /*** Input ***/
    /************/

    /*** Ayuda Visuales ***/
    /*********************/
}
