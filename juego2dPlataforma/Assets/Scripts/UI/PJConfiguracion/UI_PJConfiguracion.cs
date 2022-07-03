using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_PJConfiguracion : MonoBehaviour
{
    /** Componentes **/
    // velocidad
    public TMP_InputField velocidad;
    // salto
    public TMP_InputField salto;
    // mejorar salto
    public Toggle mejorarSalto;
    int ms;
    // activar salto doble
    public Toggle saltoDoble;
    int sd;
    // activar dash solo horizontal
    public Toggle dashHorizontal;
    int dh;

    private PMovimiento move;

    private void Start()
    {
        if(FindObjectOfType <PMovimiento>() != null) { move = FindObjectOfType<PMovimiento>(); }
        velocidad.text = PlayerPrefs.GetFloat("velocidad", 0).ToString();
        salto.text = PlayerPrefs.GetFloat("salto", 0).ToString();
        mejorarSalto.isOn = move.mejorarSalto;
        saltoDoble.isOn = move.activarDobleSalto;
        dashHorizontal.isOn = move.dashHorizontal;

    }
    private void Update()
    {
        // configuraciones del jugador
        PlayerPrefs.SetFloat("velocidad", float.Parse(velocidad.text));

        PlayerPrefs.SetFloat("salto", float.Parse(salto.text));

        if (mejorarSalto.isOn) { ms = 1; } else { ms = 0; }
        PlayerPrefs.SetInt("mejorarSalto", ms);

        if (saltoDoble.isOn) { sd = 1; } else { sd = 0; }
        PlayerPrefs.SetInt("saltoDoble", sd);

        if (dashHorizontal.isOn) { dh = 1; } else { dh = 0; }
        PlayerPrefs.SetInt("dashHorizontal", dh);

        // validacion
    }
    
}
