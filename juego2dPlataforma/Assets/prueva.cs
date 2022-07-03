using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prueva : MonoBehaviour
{
    private int layerSuelo;
    public bool estaSuelo;
    private void Start()
    {
        layerSuelo = LayerMask.NameToLayer("Suelo");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == layerSuelo)
        {
            estaSuelo = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layerSuelo)
        {
            estaSuelo = false;
        }
    }
}
