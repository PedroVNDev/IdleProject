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

    public Text textoAstros;
    public Text textoAstrosMejora;
    public Text textoAstrosObtener;

    private BigDouble astrosObtener => 150 * Sqrt(juego.data.diamantes / 1e4);
    public BigDouble astrosMejora => juego.data.astros * 0.001 + 1;

    public void Run()
    {
        UI();

        void UI()
        {
            var data = juego.data;
            if (!juego.ventanaSuperNovaGrupo.gameObject.activeSelf) return;
            {
                textoAstros.text = $"Astros: {Metodos.MetodoNotacion(data.astros, "F2")}";
                textoAstrosObtener.text = $"+{Metodos.MetodoNotacion(astrosObtener, "F2")} Astros";
                textoAstrosMejora.text = $"Los Diamantes son {Metodos.MetodoNotacion(astrosMejora, "F2")} x Mas fuertes";
            }
        }
    }

    public void SuperNova()
    {
        var data = juego.data;

        data.astros += astrosObtener;

        data.recursos = 0;
        data.recursosClickValor = 1;

        data.clickMejora1Nivel = 0;
        data.clickMejora1Coste = 10;

        data.clickMejora2Nivel = 0;
        data.clickMejora2Coste = 100;

        data.produccionMejora1Nivel = 0;
        data.produccionMejora1Coste = 25;

        data.produccionMejora2Nivel = 0;
        data.produccionMejora2Coste = 250;
        data.produccionMejora2Poder = 5;
        
        data.diamantes = 0;

        data.prestigioMNivel1 = 0;
        data.prestigioMNivel2 = 0;
        data.prestigioMNivel3 = 0;

        juego.ventanaSuperNovaGrupo.gameObject.SetActive(false);
        juego.ventanaPrincipalGrupo.gameObject.SetActive(true);
    }
}