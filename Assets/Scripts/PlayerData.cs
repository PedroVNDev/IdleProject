using System;
using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using UnityEngine;

[Serializable]
public class PlayerData
{
    // Recursos
    public BigDouble recursos;
    public BigDouble recursosTotales;
    public BigDouble recursosClickValor;

    //Clicks
    public int clickMejora1Nivel;
    public BigDouble clickMejora1Coste;

    public BigDouble clickMejora2Coste;
    public int clickMejora2Nivel;

    //Pasivos
    public BigDouble produccionMejora1Coste;
    public int produccionMejora1Nivel;

    public BigDouble produccionMejora2Coste;
    public BigDouble produccionMejora2Poder;
    public int produccionMejora2Nivel;

    //Prestigio
    public BigDouble diamantes;
    public BigDouble diamantesConseguidos;

    //Nivel Logros
    public float logroNivel1;
    public float logroNivel2;

    //Eventos
    public BigDouble tokensEvento;
    public float[] eventCooldown = new float[7];
    public int eventoActivoID;

    //Opciones
    public bool musicaFondo;

    #region Prestigio

    public int prestigioMNivel1;
    public int prestigioMNivel2;
    public int prestigioMNivel3;

    #endregion

    #region SuperNova

    public BigDouble astros;

    #endregion

    #region Automatizadores

    public int autoNivel1;
    public int autoNivel2;

    #endregion

    public PlayerData()
    {
        FullReset();
    }

    public void FullReset()
    {
        recursos = 0;
        recursosTotales = 0;
        recursosClickValor = 1;

        //Prestigio
        diamantes = 0;

        prestigioMNivel1 = 0;
        prestigioMNivel2 = 0;
        prestigioMNivel3 = 0;

        //Mejoras
        clickMejora1Nivel = 0;
        clickMejora1Coste = 10;

        clickMejora2Nivel = 0;
        clickMejora2Coste = 100;

        produccionMejora1Nivel = 0;
        produccionMejora1Coste = 25;

        produccionMejora2Nivel = 0;
        produccionMejora2Poder = 5;
        produccionMejora2Coste = 250;

        //Opciones
        musicaFondo = true;

        //Nivel Logros
        logroNivel1 = 0;
        logroNivel2 = 0;

        //Eventos
        tokensEvento = 0;
        for (int i = 0; i < eventCooldown.Length; i++)
        {
            eventCooldown[i] = 0;
        }

        eventoActivoID = 0;

        prestigioMNivel1 = 0;
        prestigioMNivel2 = 0;
        prestigioMNivel3 = 0;

        astros = 0;

        #region Automatizadores

        autoNivel1 = 0;
        autoNivel2 = 0;

        #endregion
    }
}