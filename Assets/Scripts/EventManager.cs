using System;
using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using static BreakInfinity.BigDouble;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public IdleManager juego;
    public Text TextoTokensEvento;

    public BigDouble tokensEventoMejora => (juego.data.tokensEvento / 100) + 1;

    public GameObject recompensaEventoPopUp;
    public Text textoEventoRecompensa;
    public Text textoPingusSegundo;

    public GameObject[] eventos = new GameObject[7];
    public GameObject[] eventosDesbloqueados = new GameObject[7];
    public Text[] textoRecompensa = new Text[7];
    public Text[] textoPingus = new Text[7];
    public Text[] textoClick = new Text[7];
    public Text[] textoCoste = new Text[7];
    public Text[] textoMejora = new Text[7];
    public Text[] textoEmpezar = new Text[7];

    public BigDouble[] recompensa = new BigDouble[7];
    public BigDouble[] pingus = new BigDouble[7];

    public BigDouble[] costes = new BigDouble[7];
    public int[] niveles = new int[7];

    public string DiaDelaSemana()
    {
        DateTime tiempo = DateTime.Now;

        //Para hacer debug
        //var tiempodebug = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1);

        return tiempo.DayOfWeek.ToString();
    }

    public string diaAnteriorComprobar;

    public void StartEventos()
    {
        diaAnteriorComprobar = DiaDelaSemana();

        if (juego.data.eventoActivoID != 0)
        {
            juego.data.eventoActivoID = 0;
            juego.data.eventCooldown = new float[7];
        }
    }

    public void Update()
    {
        var data = juego.data;

        TextoTokensEvento.text =
            $"Tokens Evento: {Metodos.MetodoNotacion(data.tokensEvento, "F2")} ({Metodos.MetodoNotacion(tokensEventoMejora, "F2")}x)";

        recompensa[0] = Log10(pingus[0] + 1);
        recompensa[1] = Log10(pingus[1] / 5 + 1);

        for (int i = 0; i < 2; i++)
        {
            costes[i] = 10 * Pow(1.15, niveles[i]);
        }

        if (diaAnteriorComprobar != DiaDelaSemana())
        {
            data.eventoActivoID = 0;
            //2 Son los dias que tenemos (Lunes y Martes)
            for (int i = 0; i < 7; i++)
            {
                data.eventCooldown[i] = 0;
            }
        }

        switch (DiaDelaSemana())
        {
            case "Monday":
                if (juego.ventanaEventosGrupo.gameObject.activeSelf)
                {
                    IniciarEventoUI(0);
                }

                EmpezarEvento(0);
                break;

            case "Tuesday":
                if (juego.ventanaEventosGrupo.gameObject.activeSelf)
                {
                    IniciarEventoUI(0);
                }

                EmpezarEvento(0);
                break;

            case "Wednesday":
                if (juego.ventanaEventosGrupo.gameObject.activeSelf)
                {
                    IniciarEventoUI(0);
                }

                EmpezarEvento(0);
                break;

            case "Thursday":
                if (juego.ventanaEventosGrupo.gameObject.activeSelf)
                {
                    IniciarEventoUI(0);
                }

                EmpezarEvento(0);
                break;

            case "Friday":
                if (juego.ventanaEventosGrupo.gameObject.activeSelf)
                {
                    IniciarEventoUI(0);
                }

                EmpezarEvento(0);
                break;

            case "Saturday":
                if (juego.ventanaEventosGrupo.gameObject.activeSelf)
                {
                    IniciarEventoUI(1);
                }

                EmpezarEvento(1);
                break;

            case "Sunday":
                if (juego.ventanaEventosGrupo.gameObject.activeSelf)
                {
                    IniciarEventoUI(1);
                }

                EmpezarEvento(1);
                break;
        }

        if (data.eventoActivoID == 0 & juego.data.eventCooldown[DiaDeHoy()] > 0)
        {
            juego.data.eventCooldown[DiaDeHoy()] -= Time.deltaTime;
        }
        else if (data.eventoActivoID != 0 & juego.data.eventCooldown[DiaDeHoy()] > 0)
        {
            juego.data.eventCooldown[DiaDeHoy()] -= Time.deltaTime;
        }
        else if (data.eventoActivoID != 0 & juego.data.eventCooldown[DiaDeHoy()] <= 0)
        {
            EventoCompletado(DiaDeHoy());
        }

        diaAnteriorComprobar = DiaDelaSemana();
    }

    public int DiaDeHoy()
    {
        switch (DiaDelaSemana())
        {
            case "Monday": return 0;
            case "Tuesday": return 0;
            case "Wednesday": return 0;
            case "Thursday": return 0;
            case "Friday": return 0;
            case "Saturday": return 1;
            case "Sunday": return 1;
        }

        return 0;
    }

    public void Click(int id)
    {
        switch (id)
        {
            case 0:
                pingus[id] += 1 + niveles[id] * juego.MejoraTotal();
                break;

            case 1:
                pingus[id] += 1 * juego.MejoraTotal();
                break;
        }
    }

    public void Comprar(int id)
    {
        if (pingus[id] < costes[id]) return;
        {
            pingus[id] -= costes[id];
            niveles[id]++;
        }
    }

    public void ActivarEvento(int id)
    {
        var id2 = id - 1;
        var data = juego.data;
        var ahora = DateTime.Now;

        //Empezar
        if (data.eventoActivoID == 0 & data.eventCooldown[id2] <= 0 & !(ahora.Hour == 23 & ahora.Minute >= 55))
        {
            data.eventoActivoID = id;
            data.eventCooldown[id2] = 300;

            pingus[id2] = 0;
            niveles[id2] = 0;
        }

        else if (ahora.Hour == 23 & ahora.Minute >= 55 & data.eventoActivoID == 0) ;

        else if (data.eventCooldown[id2] > 0 & data.eventoActivoID == 0) ;

        else
        {
            EventoCompletado(id2);
        }
    }

    private void EventoCompletado(int id)
    {
        var data = juego.data;

        data.tokensEvento += recompensa[id];
        textoEventoRecompensa.text = $"+{Metodos.MetodoNotacion(recompensa[id], "F2")} Tokens Evento";

        pingus[id] = 0;
        niveles[id] = 0;
        //data.tokensEvento += recompensa[id];
        data.eventoActivoID = 0;
        data.eventCooldown[id] = 7200;

        recompensaEventoPopUp.gameObject.SetActive(true);
    }

    public void CerrarRecompensaEvento()
    {
        recompensaEventoPopUp.gameObject.SetActive(false);
    }

    private void IniciarEventoUI(int id)
    {
        var data = juego.data;

        for (var i = 0; i < 2; i++)
        {
            if (i == id)
            {
                eventos[id].gameObject.SetActive(true);
            }
            else
            {
                eventos[i].gameObject.SetActive(false);
            }
        }

        var tiempo = TimeSpan.FromSeconds(data.eventCooldown[id]);

        if (data.eventoActivoID == 0)
        {
            textoEmpezar[id].text = data.eventCooldown[id] > 0
                ? tiempo.ToString(@"hh\:mm\:ss")
                : "Empezar Evento";
        }
        else
        {
            textoEmpezar[id].text = $"Salir del Evento: {(tiempo.ToString(@"hh\:mm\:ss"))}";
        }

        if (data.eventoActivoID != id + 1) return;
        {
            eventosDesbloqueados[id].gameObject.SetActive(true);

            textoRecompensa[id].text = $"+{Metodos.MetodoNotacion(recompensa[id], "F2")} Tokens Evento";
            textoPingus[id].text = $"{Metodos.MetodoNotacion(pingus[id], "F2")} Pingus";
            textoCoste[id].text = $"Coste: {Metodos.MetodoNotacion(costes[id], "F2")} ({niveles[id]})";

            if (id == 0)
            {
                textoClick[id].text =
                    $"Crear {Metodos.MetodoNotacion(niveles[id] + 1 * juego.MejoraTotal(), "F2")}\nPingus";
                textoMejora[id].text =
                    $"Mejora Click 1\nGanar +{Metodos.MetodoNotacion(1 * juego.MejoraTotal(), "F2")} Pingus";
            }
            else if (id == 1)
            {
                textoMejora[id].text =
                    $"Mejora Producción 1\nGanar +{Metodos.MetodoNotacion(1 * juego.MejoraTotal(), "F2")} Pingu/s";
                textoClick[id].text = $"Crear {Metodos.MetodoNotacion(1 * juego.MejoraTotal(), "F2")}\nPingus";
                textoPingusSegundo.text =
                    $"Pingus/s {Metodos.MetodoNotacion(niveles[id] * juego.MejoraTotal(), "F2")}";
            }
        }
    }

    private void EmpezarEvento(int id)
    {
        switch (id)
        {
            case 1:
                pingus[id] += niveles[id] * Time.deltaTime * juego.MejoraTotal();
                break;
        }
    }
}