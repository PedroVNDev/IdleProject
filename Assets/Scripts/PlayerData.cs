using System;
using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using UnityEngine;

[Serializable]
public class PlayerData
{
    #region Tierra

    #region Basicos

    // Recursos
    public BigDouble recursos;
    public BigDouble recursosTotales;
    public BigDouble recursosClickValor;

    #endregion

    #region Mejoras

    //Clicks
    public int clickMejora1Nivel;
    public int clickMejora2Nivel;

    public BigDouble clickMejora1Coste;
    public BigDouble clickMejora2Coste;

    //Pasivos
    public int produccionMejora1Nivel;
    public BigDouble produccionMejora2Poder;
    public int produccionMejora2Nivel;

    public BigDouble produccionMejora1Coste;
    public BigDouble produccionMejora2Coste;

    #endregion

    #region Prestigio

    //Prestigio
    public BigDouble bitcoins;
    public BigDouble bitcoinsConseguidos;

    #endregion

    #region Logros

    //Nivel Logros
    public float logroNivel1;
    public float logroNivel2;

    #endregion


    #region Opciones

    //Opciones
    public bool musicaFondo;

    #endregion


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

    #endregion

    #region Marte

    public BigDouble marshalls;

    #region MejorasMarte

    /*
    public BigDouble marteCosteMejora1;
    public BigDouble marteCosteMejora2;*/

    public int marteNivelesMejora1;
    public int marteNivelesMejora2;

    #endregion

    #endregion

    #region Eventos

    //Eventos
    public BigDouble tokensEvento;
    public float[] eventCooldown = new float[7];
    public int eventoActivoID;

    #endregion


    public PlayerData()
    {
        FullReset();
    }

    public void FullReset()
    {
        #region Tierra

        #region Basicos

        recursos = 0;
        recursosTotales = 0;
        recursosClickValor = 1;

        #endregion

        #region Prestigio

        //Prestigio
        bitcoins = 0;

        prestigioMNivel1 = 0;
        prestigioMNivel2 = 0;
        prestigioMNivel3 = 0;

        #endregion

        #region Mejoras

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

        #endregion


        #region Logros

        //Nivel Logros
        logroNivel1 = 0;
        logroNivel2 = 0;

        #endregion

        #region Eventos

        //Eventos
        tokensEvento = 0;
        for (int i = 0; i < eventCooldown.Length; i++)
        {
            eventCooldown[i] = 0;
        }

        eventoActivoID = 0;

        #endregion

        #region Astros

        astros = 0;

        #endregion

        #region Automatizadores

        autoNivel1 = 0;
        autoNivel2 = 0;

        #endregion

        #endregion

        #region Marte

        marshalls = 1;

        #region MarteMejoras

        marteNivelesMejora1 = 0;
        marteNivelesMejora2 = 0;

        #endregion

        #endregion

        #region Opciones

        //Opciones
        musicaFondo = true;

        #endregion

        #region Eventos

        //Eventos
        tokensEvento = 0;
        for (int i = 0; i < eventCooldown.Length; i++)
        {
            eventCooldown[i] = 0;
        }

        eventoActivoID = 0;

        #endregion
    }
}