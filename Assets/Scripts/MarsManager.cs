using System;
using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;
using static BreakInfinity.BigDouble;


public class MarsManager : MonoBehaviour
{
    public IdleManager juego;
    public Text textoMarshalls;
    public Text textoClickMarshalls;
    public Text textoMarshallsPorSegundo;
    public Text textoMarteImpuestos;

    public BigDouble exponenteImpuestos => Pow(Log10(juego.data.marshalls + 1) + 1, 0.01);
    public BigDouble exponenteImpuestosClick => Pow(Log10(juego.data.marshalls + 1) + 1, 0.75);
    public BigDouble marteImpuestos => juego.data.marshalls;

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
    private BigDouble marshallsPorSegundo => Pow(juego.data.marshalls * ((BigDouble) marteNivelesMejora[1] * 0.0001), 1 / exponenteImpuestos);

    private void Start()
    {
        marteMejoraMultiplicadorCoste = new [] {2.5f, 5};
        marteCosteBase = new BigDouble[] {2, 20};
        marteCosteMejora = new BigDouble[2];
        marteNivelesMejora = new int[2];
        nombresMejora = new[] {"Poder Click + 0.01x", "Gana + 0.01% de tus Marshalls por segundo"};
    }


    public void Update()
    {
        var data = juego.data;

        if (data.marshalls <= 0.95)
        {
            data.marshalls = 1;
        }

        ArrayManager();
        UI();

        if (marteNivelesMejora[1] > 0)
            data.marshalls += marshallsPorSegundo * Time.deltaTime;

        void UI()
        {
            if (ventanaMejorasGrupo.gameObject.activeSelf)
            {
                Metodos.NumeroSuave(ref marshallsAux, data.marshalls);

                for (var i = 0; i < 2; i++)
                {
                    textoMejora[i].text =
                        $"({marteNivelesMejora[i]}) {nombresMejora[i]}\nCoste: {Metodos.MetodoNotacion(marteCosteMejora[i], "F2")} Marshalls";
                    Metodos.BigDoubleRellenar(data.marshalls, marteCosteMejora[i], ref rellenoMejora[i]);
                    Metodos.BigDoubleRellenar(marshallsAux, marteCosteMejora[i], ref rellenoMejoraSuave[i]);
                }
            }

            if (!juego.planetas.Marte.gameObject.activeSelf) return;
            textoMarshalls.text = $"{Metodos.MetodoNotacion(data.marshalls, "F2")} Marshalls";
            textoClickMarshalls.text = $"Click\n {Pow(poderClick, 1 / exponenteImpuestosClick):F2}x Marshalls";
            textoMarshallsPorSegundo.text = $"{Metodos.MetodoNotacion(marshallsPorSegundo, "F2")} Marshalls/s";
            textoMarteImpuestos.text =
                $"Impuestos:\n{Metodos.MetodoNotacion(impuestosMultiplicadorPorSegundo(), "F2")}x menos Marhshalls por segundo\n{impuestosMultiplicadorClick():F2}x menos Marshalls por click";
        }

        BigDouble impuestosMultiplicadorPorSegundo()
        {
            if (marteNivelesMejora[1] == 0) return 1;

            return juego.data.marshalls * ((BigDouble) marteNivelesMejora[1] * 0.0001) / marshallsPorSegundo;
        }
        
        BigDouble impuestosMultiplicadorClick()
        {
            return poderClick / Pow(poderClick, 1 / exponenteImpuestosClick);
        }
    }

    public void Click()
    {
        var data = juego.data;
        data.marshalls *= Pow(poderClick, 1 / exponenteImpuestosClick);
    }

    public float poderClick => 1.01f + 0.01f * marteNivelesMejora[0];

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

    private void ArrayManager()
    {
        var data = juego.data;
        marteNivelesMejora[0] = data.marteNivelesMejora1;
        marteNivelesMejora[1] = data.marteNivelesMejora2;
        marteCosteMejora[0] =
            marteCosteBase[0] * Pow(marteMejoraMultiplicadorCoste[0], (BigDouble) marteNivelesMejora[0]);
        marteCosteMejora[1] =
            marteCosteBase[1] * Pow(marteMejoraMultiplicadorCoste[1], (BigDouble) marteNivelesMejora[1]);
    }

    private void NonArrayManager()
    {
        juego.data.marteNivelesMejora1 = marteNivelesMejora[0];
        juego.data.marteNivelesMejora2 = marteNivelesMejora[1];
    }
}