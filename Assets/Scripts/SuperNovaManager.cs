using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;
using static BreakInfinity.BigDouble;

public class SuperNovaManager : MonoBehaviour
{
    public IdleManager juego;

    public Text textoSuperNovas;
    public Text textoSuperNovasObtener;

    private BigDouble superNovasObtener => 150 * Sqrt(juego.data.diamantes / 1e4);
    public BigDouble superNovasMejora => juego.data.superNovas * 0.001 + 1;

    public void Run()
    {
        UI();

        void UI()
        {
            var data = juego.data;
            if (!juego.ventanaSuperNovaGrupo.gameObject.activeSelf) return;
            {
                textoSuperNovas.text = $"SuperNovas: {data.superNovas}";
                textoSuperNovasObtener.text = $"+{superNovasObtener} SuperNovas";
            }
        }
    }

    public void SuperNova()
    {
        var data = juego.data;

        data.superNovas += superNovasObtener;
        
        data.recursos = 0;
        data.recursosTotales = 0;
        data.recursosClickValor = 1;
        data.clickMejora1Nivel = 0;
        data.clickMejora2Nivel = 0;
        data.produccionMejora1Nivel = 0;
        data.produccionMejora2Nivel = 0;
        data.produccionMejora2Poder = 0;
        data.diamantes = 0;
        
        juego.ventanaSuperNovaGrupo.gameObject.SetActive(false);
        juego.ventanaPrincipalGrupo.gameObject.SetActive(true);
    }
}