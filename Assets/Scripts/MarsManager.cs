using System;
using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;
using static BreakInfinity.BigDouble;


public class MarsManager : MonoBehaviour
{
    public IdleManager juego;
    public Text textoMarshalls;

    public void Update()
    {
        var data = juego.data;
        textoMarshalls.text = $"{Metodos.MetodoNotacion(data.marshalls, "F2")}";
    }

    public void Click()
    {
        var data = juego.data;
        data.marshalls *= 1.01;
        
    }
}