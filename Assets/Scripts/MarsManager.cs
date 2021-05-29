using System;
using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;
using static BreakInfinity.BigDouble;


public class MarsManager : MonoBehaviour
{
    public IdleManager juego;
    public Text textoMarshalls;

    public Canvas ventanaMejorasGrupo;

    public float[] marteMejoraMultiplicadorCoste;
    public BigDouble[] marteCosteBase;
    public BigDouble[] marteCosteMejora;
    public int[] marteNivelesMejora;

    private string[] nombresMejora;

    public Text[] textoMejora;
    public Image[] rellenoMejora;
    public Image[] rellenoMejoraSuave;

    private BigDouble marshallsAux;

    private void Start()
    {
        marteMejoraMultiplicadorCoste = new float[] {2, 5};
        marteCosteBase = new BigDouble[] {10, 100};
        marteCosteMejora = new BigDouble[2];
        marteNivelesMejora = new int[2];
        nombresMejora = new[] {"Poder Click + 0.01x", "Gana + 0.01% de tus Marshalls por segundo"};
    }


    public void Update()
    {
        var data = juego.data;

        ArrayManager();
        UI();

        if (marteNivelesMejora[1] > 0)
        {
            data.marshalls *= 1 + marteNivelesMejora[1] * 0.0001;
        }

        void UI()
        {
            if (!ventanaMejorasGrupo.gameObject.activeSelf) return;
            textoMarshalls.text = $"{Metodos.MetodoNotacion(data.marshalls, "F2")}";
            Metodos.NumeroSuave(ref marshallsAux, data.marshalls);

            for (var i = 0; i < 2; i++)
            {
                textoMejora[i].text =
                    $"({marteNivelesMejora[i]}) {nombresMejora}\nCoste: {Metodos.MetodoNotacion(marteCosteMejora[i], "F2")} Marshalls";
                Metodos.BigDoubleRellenar(data.marshalls, marteCosteMejora[i], ref rellenoMejora[i]);
                Metodos.BigDoubleRellenar(marshallsAux, marteCosteMejora[i], ref rellenoMejoraSuave[i]);
            }
        }
    }

    public void Click()
    {
        var data = juego.data;
        data.marshalls *= 1.01 + 0.01 * marteNivelesMejora[0];
    }

    public void ComprarMejora(int index)
    {
        if (juego.data.marshalls > marteCosteMejora[index])
        {
            juego.data.marshalls -= marteCosteMejora[index];
            marteNivelesMejora[index]++;
        }

        NonArrayManager();
    }


    public void CompraMax()
    {
        Metodos.CompraMax(ref juego.data.marshalls, marteCosteBase[0], marteMejoraMultiplicadorCoste[0],
            ref marteNivelesMejora[0]);
        Metodos.CompraMax(ref juego.data.marshalls, marteCosteBase[1], marteMejoraMultiplicadorCoste[1],
            ref marteNivelesMejora[1]);
        NonArrayManager();
    }

    public void EncenderPopUp(string id)
    {
        switch (id)
        {
            case "Mejoras":
                ventanaMejorasGrupo.gameObject.SetActive(!ventanaMejorasGrupo.gameObject.activeSelf);
                break;
        }
    }

    /*public void CambiaVentanas(string id)
    {
        DesactivarTodo();
        switch (id)
        {

            case "Mejoras":
                ventanaMejorasGrupo.gameObject.SetActive(true);
                break;
        }

        void DesactivarTodo()
        {
            //En progreso
        }
    }*/

    private void ArrayManager()
    {
        var data = juego.data;
        marteNivelesMejora[0] = data.marteNivelesMejora1;
        marteCosteMejora[0] = marteCosteBase[0] * Pow(marteNivelesMejora[0], marteNivelesMejora[0]);
        marteCosteMejora[1] = marteCosteBase[1] * Pow(marteNivelesMejora[1], marteNivelesMejora[1]);
    }

    private void NonArrayManager()
    {
        juego.data.marteNivelesMejora1 = marteNivelesMejora[0];
        juego.data.marteNivelesMejora2 = marteNivelesMejora[1];
    }
}