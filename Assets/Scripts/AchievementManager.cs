using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using static BreakInfinity.BigDouble;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{

    public IdleManager juego;
    private static string[] LogrosString => new string[] {"Recursos actuales", "Recursos totales"};
    private BigDouble[] Logros => new BigDouble[] {juego.data.terrans, juego.data.terransTotales};

    public GameObject logroVentana;
    public List<Logros> ListaLogros = new List<Logros>();

    private void Start()
    {
        foreach (var obj in logroVentana.GetComponentsInChildren<Logros>())
        {
            ListaLogros.Add(obj);
        }
    }
    
    public void IniciarLogros()
    {
        var data = juego.data;
        ActualizarLogros(LogrosString[0], Logros[0], ref data.logroNivel1, ref ListaLogros[0].barraProgreso,
            ref ListaLogros[0].titulo, ref ListaLogros[0].progreso);

        ActualizarLogros(LogrosString[1], Logros[1], ref data.logroNivel2, ref ListaLogros[1].barraProgreso,
            ref ListaLogros[1].titulo, ref ListaLogros[1].progreso);
    }

    private void ActualizarLogros(string nombre, BigDouble numero, ref float nivel, ref Image rellenar,
        ref Text titulo, ref Text progreso)
    {
        var capacidad = BigDouble.Pow(10, nivel);

        if (juego.ventanaLogrosGrupo.gameObject.activeSelf)
        {
            titulo.text = $"{nombre}\n({nivel})";
            progreso.text = $"{Metodos.MetodoNotacion(numero, "F2")} / {Metodos.MetodoNotacion(capacidad, "F2")}";

            Metodos.BigDoubleRellenar(numero, capacidad, ref rellenar);
        }

        if (numero < capacidad) return;
        BigDouble niveles = 0;

        if (numero / capacidad >= 1)
        {
            niveles = Floor(Log10(numero / capacidad)) + 1;
        }

        nivel += (float) niveles;
    }
}