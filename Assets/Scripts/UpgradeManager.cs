using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;
using static BreakInfinity.BigDouble;

public class UpgradeManager : MonoBehaviour
{
    public IdleManager juego;

    public GameObject[] clickMejora = new GameObject[2];
    public GameObject[] produccionMejora = new GameObject[2];

    //Textos
    public Text[] textoMejoraClick = new Text[2];
    public Text[] textoMejoraClickMax = new Text[2];
    public Text[] textoMejoraProduccion = new Text[2];
    public Text[] textoMejoraProduccionMax = new Text[2];

    //Barras de progreso
    public Image mejoraBarraClick1;
    public Image mejoraBarraClick1Suave;

    public BigDouble[] clickMejoraCoste;
    public BigDouble[] clickMejoraPoder;
    public BigDouble[] clickMejoraCosteBase;
    public BigDouble[] clickMejoraDesbloqueoCoste;
    public double[] clickMejoraCosteMultiplicadores;

    public BigDouble[] produccionMejoraCoste;
    public BigDouble[] produccionMejoraPoder;
    public BigDouble[] produccionMejoraCosteBase;
    public BigDouble[] produccionMejoraDesbloqueoCoste;
    public double[] produccionMejoraCosteMultiplicadores;

    public int[] clickMejoraNiveles;
    public int[] produccionMejoraNiveles;


    public void Start()
    {
        clickMejoraCoste = new BigDouble[2];
        clickMejoraPoder = new BigDouble[] {1, 5};
        clickMejoraCosteBase = new BigDouble[] {10, 100};
        clickMejoraCosteMultiplicadores = new double[] {1.07, 1.07};
        clickMejoraDesbloqueoCoste = new BigDouble[] {5, 50};

        produccionMejoraCoste = new BigDouble[2];
        produccionMejoraPoder = new BigDouble[] {1, 5};
        produccionMejoraCosteBase = new BigDouble[] {25, 250};
        produccionMejoraCosteMultiplicadores = new double[] {1.07, 1.07};
        produccionMejoraDesbloqueoCoste = new BigDouble[] {15, 125};

        clickMejoraNiveles = new int[2];
        produccionMejoraNiveles = new int[2];
    }

    public void EmpezarMejoras()
    {
        var data = juego.data;

        ArrayManager();

        clickMejoraCoste[0] = clickMejoraCosteBase[0] * Pow(clickMejoraCosteMultiplicadores[0], data.clickMejora1Nivel);
        clickMejoraCoste[1] = clickMejoraCosteBase[1] * Pow(clickMejoraCosteMultiplicadores[1], data.clickMejora2Nivel);
        produccionMejoraCoste[0] = produccionMejoraCosteBase[0] *
                                   Pow(produccionMejoraCosteMultiplicadores[0], data.produccionMejora1Nivel);
        produccionMejoraCoste[1] = produccionMejoraCosteBase[1] *
                                   Pow(produccionMejoraCosteMultiplicadores[0], data.produccionMejora2Nivel);
    }

    public void EmpezarMejorasUI()
    {
        var data = juego.data;

        for (var i = 0; i < 2; i++)
        {
            textoMejoraClick[i].text =
                $"Mejora Click {i + 1}\nCoste: {ObtenerMejoraCoste(i, clickMejoraCoste)} Terrans\nPoder: + {clickMejoraPoder[i]} Click\n Nivel: {ObtenerMejoraNivel(i, clickMejoraNiveles)}";

            textoMejoraClickMax[i].text = $"Compra Max ({CompraClickMaxContador(i)})";

            textoMejoraProduccion[i].text =
                $"Mejora Produccion {i + 1}\nCoste: {ObtenerMejoraCoste(i, produccionMejoraCoste)} Terrans\nPoder: + {Metodos.MetodoNotacion(juego.MejoraTotal() * Pow(1.1, juego.prestigio.niveles[1]), "F2")}\n Nivel: {ObtenerMejoraNivel(i, produccionMejoraNiveles)}";

            textoMejoraProduccionMax[i].text = $"Compra Max ({CompraProduccionMaxContador(i)})";

            clickMejora[i].gameObject.SetActive(data.terransTotales >= clickMejoraDesbloqueoCoste[i]);
            produccionMejora[i].gameObject.SetActive(data.terransTotales >= produccionMejoraDesbloqueoCoste[i]);
        }

        Metodos.BigDoubleRellenar(data.terrans, clickMejoraCoste[0], ref mejoraBarraClick1);
        Metodos.BigDoubleRellenar(juego.terransTemporal, clickMejoraCoste[0], ref mejoraBarraClick1Suave);

        string ObtenerMejoraCoste(int index, BigDouble[] mejora)
        {
            return Metodos.MetodoNotacion(mejora[index], "F2");
        }

        string ObtenerMejoraNivel(int index, int[] mejoraNivel)
        {
            return Metodos.MetodoNotacion(mejoraNivel[index], "F2");
        }
    }

    public void CompraClickMejora(int index)
    {
        var data = juego.data;

        if (data.terrans >= clickMejoraCoste[index])
        {
            clickMejoraNiveles[index]++;
            data.terrans -= clickMejoraCoste[index];
            data.terransClickValor += clickMejoraPoder[index];
        }

        NoArrayManager();
    }

    public void CompraProduccionMejora(int index)
    {
        var data = juego.data;

        if (data.terrans >= produccionMejoraCoste[index])
        {
            produccionMejoraNiveles[index]++;
            data.terrans -= produccionMejoraCoste[index];
        }

        NoArrayManager();
    }

    //Mejoras
    public void CompraClickMax(int index)
    {
        var data = juego.data;

        var b = clickMejoraCosteBase[index];
        var c = data.terrans;
        var r = clickMejoraCosteMultiplicadores[index];
        var k = clickMejoraNiveles[index];
        var n = Floor(Log(c * (r - 1) / (b * Pow(r, k)) + 1, r));

        var coste = b * (Pow(r, k) * (Pow(r, n) - 1) / (r - 1));

        if (data.terrans >= coste)
        {
            clickMejoraNiveles[index] += (int) n;
            data.terrans -= coste;
            data.terransClickValor += n * clickMejoraPoder[index];
        }

        NoArrayManager();
    }

    public BigDouble CompraClickMaxContador(int index)
    {
        var data = juego.data;

        var b = clickMejoraCosteBase[index];
        var c = data.terrans;
        var r = clickMejoraCosteMultiplicadores[index];
        var k = clickMejoraNiveles[index];
        var n = Floor(Log(c * (r - 1) / (b * Pow(r, k)) + 1, r));

        return n;
    }

    public void CompraProduccionMax(int index)
    {
        var data = juego.data;

        var b = produccionMejoraCosteBase[index];
        var c = data.terrans;
        var r = produccionMejoraCosteMultiplicadores[index];
        var k = produccionMejoraNiveles[index];
        var n = Floor(Log(c * (r - 1) / (b * Pow(r, k)) + 1, r));

        var coste = b * (Pow(r, k) * (Pow(r, n) - 1) / (r - 1));
        Metodos.BigDoubleRellenar(data.terrans, clickMejoraCoste[0], ref mejoraBarraClick1);


        if (data.terrans >= coste)
        {
            produccionMejoraNiveles[index] += (int) n;
            data.terrans -= coste;
        }

        NoArrayManager();
    }

    public BigDouble CompraProduccionMaxContador(int index)
    {
        var data = juego.data;

        var b = produccionMejoraCosteBase[index];
        var c = data.terrans;
        var r = produccionMejoraCosteMultiplicadores[index];
        var k = produccionMejoraNiveles[index];
        var n = Floor(Log(c * (r - 1) / (b * Pow(r, k)) + 1, r));

        return n;
    }

    private void ArrayManager()
    {
        var data = juego.data;

        clickMejoraNiveles[0] = data.clickMejora1Nivel;
        clickMejoraNiveles[1] = data.clickMejora2Nivel;
        produccionMejoraNiveles[0] = data.produccionMejora1Nivel;
        produccionMejoraNiveles[1] = data.produccionMejora2Nivel;
    }

    private void NoArrayManager()
    {
        var data = juego.data;

        data.clickMejora1Nivel = clickMejoraNiveles[0];
        data.clickMejora2Nivel = clickMejoraNiveles[1];
        data.produccionMejora1Nivel = produccionMejoraNiveles[0];
        data.produccionMejora2Nivel = produccionMejoraNiveles[1];
    }
}