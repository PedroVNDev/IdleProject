using System;
using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public IdleManager juego;
    public Text TextoTokensEvento;

    public BigDouble tokensEventoMejora => juego.data.tokensEvento / 100;

    public GameObject recompensaEventoPopUp;

    public GameObject[] eventos = new GameObject[7];
    public GameObject[] eventosDesbloqueados = new GameObject[7];
    public Text[] textoRecompensa = new Text[7];
    public Text[] textoPingus = new Text[7];
    public Text[] textoCoste = new Text[7];
    public Text[] textoEmpezar = new Text[7];

    public BigDouble[] recompensa = new BigDouble[7];
    public BigDouble[] pingus = new BigDouble[7];

    public BigDouble[] costes = new BigDouble[7];
    public int[] niveles = new int[7]; 

    public bool eventoActivo;
    public int eventoActivoID;

    public string DiaDelaSemana()
    {
        DateTime tiempo = DateTime.Now;

        return tiempo.DayOfWeek.ToString();
    }

    public string diaAnteriorComprobar;

    public void Start()
    {
        eventoActivo = false;
        diaAnteriorComprobar = DiaDelaSemana();
    }

    public void Update()
    {
        var data = juego.data;

        TextoTokensEvento.text =
            $"Tokens Evento: {juego.MetodoNotacion(data.tokensEvento, "F2")} ({juego.MetodoNotacion(tokensEventoMejora, "F2")})";

        recompensa[0] = BigDouble.Log10(pingus[0] + 1);
        recompensa[1] = BigDouble.Log10(pingus[1] / 5 + 1);

        for (int i = 0; i < 2; i++)
        {
            costes[i] = 10 * BigDouble.Pow(1.15, niveles[i]);
        }

        if (diaAnteriorComprobar != DiaDelaSemana() & eventoActivo)
        {
            eventoActivoID = 0;
            //2 Son los dias que tenemos (Lunes y Martes)
            for (int i = 0; i < 2; i++)
            {
                data.eventCooldown[i] = 0;
            }
        }

        switch (DiaDelaSemana())
        {
            case "Monday":
                IniciarEventoUI(1);
                break;

            case "Tuesday":
                IniciarEventoUI(2);
                break;
        }

        diaAnteriorComprobar = DiaDelaSemana();
    }

    private void IniciarEventoUI(int id)
    {

        var data = juego.data;
        
        for (int i = 0; i < 2; i++)
        {
            eventos[i].gameObject.SetActive(false);
        }

        eventos[id].gameObject.SetActive(true);

        if (eventoActivoID == id)
        {
            eventosDesbloqueados[id].gameObject.SetActive(true);

            textoRecompensa[id].text = $"+{recompensa[id]} Tokens Evento";
            textoPingus[id].text = $"{pingus[id]} Pingus";
            textoCoste[id].text = $"Coste: {costes[id]}";

            if (eventoActivoID == 0)
            {
                var tiempo = TimeSpan.FromSeconds(data.eventCooldown[id]);
                textoEmpezar[id].text = (eventoActivoID == 0 & data.eventCooldown[id] > 0) ? tiempo.ToString("hh/:mm/:ss") : "Empezar Evento";
            }
            
        }
    }
}