using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using static BreakInfinity.BigDouble;
using UnityEngine;
using UnityEngine.UI;

public class PrestigeManager : MonoBehaviour
{
    public IdleManager juego;
    public SuperNovaManager superNova;

    public Canvas prestigio;

    public Text[] textoCoste = new Text[3];
    public Image[] barrasCoste = new Image[3];
    public Image[] barrasCosteSuave = new Image[3];

    public string[] costeDescripcion;
    public BigDouble[] costes;
    public int[] niveles;

    private BigDouble coste1 => 5 * BigDouble.Pow(1.5, juego.data.prestigioMNivel1);
    private BigDouble coste2 => 10 * BigDouble.Pow(1.5, juego.data.prestigioMNivel2);
    private BigDouble coste3 => 100 * BigDouble.Pow(2.5, juego.data.prestigioMNivel3);

    public BigDouble bitcoinsAux;

    public Text textoBitcoins;
    public Text textoMejoraBitcoins;
    public Text textobitcoinsConseguidos;

    public void EmpezarPrestigio()
    {
        costes = new BigDouble[3];
        niveles = new int[3];
        costeDescripcion = new[]
        {
            "Los Clicks son 50% mas eficientes", "La produccion por segundo mejora un 10%",
            "Los Bitcoins son 1.01x mas eficientes"
        };
    }

    public void Run()
    {
        var data = juego.data;

        ArrayManager();
        UI();
        
        data.bitcoinsConseguidos = 150 * Sqrt(data.terrans / 1e7);

        textoBitcoins.text = "Bitcoins: " + Metodos.MetodoNotacion(Floor(data.bitcoins), "F2");
        textoMejoraBitcoins.text = Metodos.MetodoNotacion(ValorTotalBitcoinsMejora(), "F2") + "x Mejora";

        if (juego.ventanaPrincipalGrupo.gameObject.activeSelf)
        {
            textobitcoinsConseguidos.text =
                "Prestigio:\n+" + Metodos.MetodoNotacion(Floor(data.bitcoinsConseguidos), "F0") + " Bitcoins";
        }


        void UI()
        {
            if (!prestigio.gameObject.activeSelf) return;
            {
                for (var i = 0; i < textoCoste.Length; i++)
                {
                    textoCoste[i].text =
                        $"Nivel {niveles[i]}\n{costeDescripcion[i]}\nCoste: {Metodos.MetodoNotacion(costes[i], "F0")} Bitcoins";
                    Metodos.NumeroSuave(ref bitcoinsAux, juego.data.bitcoins);
                    Metodos.BigDoubleRellenar(juego.data.bitcoins, costes[i], ref barrasCoste[i]);
                    Metodos.BigDoubleRellenar(bitcoinsAux, costes[i], ref barrasCosteSuave[i]);
                }
            }
        }
    }

    public void ComprarMejora(int id)
    {
        var data = juego.data;

        switch (id)
        {
            case 0:
                Comprar(ref data.prestigioMNivel1);
                break;

            case 1:
                Comprar(ref data.prestigioMNivel2);
                break;

            case 2:
                Comprar(ref data.prestigioMNivel3);
                break;
        }

        void Comprar(ref int nivel)
        {
            if (data.bitcoins < costes[id]) return;
            data.bitcoins -= costes[id];
            nivel++;
        }
    }

    public void ArrayManager()
    {
        var data = juego.data;

        costes[0] = coste1;
        costes[1] = coste2;
        costes[2] = coste3;

        niveles[0] = data.prestigioMNivel1;
        niveles[1] = data.prestigioMNivel2;
        niveles[2] = data.prestigioMNivel3;
    }

    public void Prestigio()
    {
        var data = juego.data;

        if (data.terrans > 1000)
        {
            data.terrans = 0;
            data.terransClickValor = 1;
            data.clickMejora2Coste = 100;
            data.produccionMejora1Coste = 25;
            data.produccionMejora2Coste = 250;
            data.produccionMejora2Poder = 5;

            data.clickMejora1Nivel = 0;
            data.clickMejora2Nivel = 0;
            data.produccionMejora1Nivel = 0;
            data.produccionMejora2Nivel = 0;

            data.bitcoins += data.bitcoinsConseguidos;
        }
    }

    public BigDouble ValorTotalBitcoinsMejora()
    {
        var aux = juego.data.bitcoins;
        aux *= 0.05 + niveles[2] * 0.01;
        aux *= superNova.astrosMejora;

        return aux + 1;
    }
}