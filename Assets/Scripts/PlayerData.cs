using System;
using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public bool progresoOfflineActivado;

    #region Tierra

    #region Basicos

    // Terrans
    public BigDouble terrans;
    public BigDouble terransTotales;
    public BigDouble terransClickValor;

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
    public float logroNivel3;
    public float logroNivel4;

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
    public int autoNivel3;
    public int autoNivel4;

    public short autosEnabled;

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

    #region Opciones

    public bool musicaFondo;
    public short tipoNotacion;
    public short idiomaSeleccionado;

    #endregion


    public PlayerData()
    {
        FullReset();
    }

    public void FullReset()
    {
        progresoOfflineActivado = false;

        #region Tierra

        #region Basicos

        terrans = 0;
        terransTotales = 0;
        terransClickValor = 1;

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
        logroNivel3 = 0;
        logroNivel4 = 0;

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
        autoNivel3 = 0;
        autoNivel4 = 0;

        autosEnabled = 0;

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

        musicaFondo = true;
        tipoNotacion = 0;
        idiomaSeleccionado = 0;

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