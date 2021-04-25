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

    public BigDouble diamantesAux;

    public Text textoDiamantes;
    public Text textoMejoraDiamantes;
    public Text textodiamantesConseguidos;

    public void EmpezarPrestigio()
    {
        costes = new BigDouble[3];
        niveles = new int[3];
        costeDescripcion = new[]
        {
            "Los Clicks son 50% mas eficientes", "La produccion por segundo mejora un 10%",
            "Los diamantes son 1.01x mas eficientes"
        };
    }

    public void Run()
    {
        var data = juego.data;

        ArrayManager();
        UI();
        
        data.diamantesConseguidos = 150 * Sqrt(data.recursos / 1e7);

        textoDiamantes.text = "Diamantes: " + Metodos.MetodoNotacion(Floor(data.diamantes), "F2");
        textoMejoraDiamantes.text = Metodos.MetodoNotacion(ValorTotalDiamantesMejora(), "F2") + "x Mejora";

        if (juego.ventanaPrincipalGrupo.gameObject.activeSelf)
        {
            textodiamantesConseguidos.text =
                "Prestigio:\n+" + Metodos.MetodoNotacion(Floor(data.diamantesConseguidos), "F0") + " Diamantes";
        }


        void UI()
        {
            if (!prestigio.gameObject.activeSelf) return;
            {
                for (var i = 0; i < textoCoste.Length; i++)
                {
                    textoCoste[i].text =
                        $"Nivel {niveles[i]}\n{costeDescripcion[i]}\nCoste: {Metodos.MetodoNotacion(costes[i], "F0")} Diamantes";
                    Metodos.NumeroSuave(ref diamantesAux, juego.data.diamantes);
                    Metodos.BigDoubleRellenar(juego.data.diamantes, costes[i], ref barrasCoste[i]);
                    Metodos.BigDoubleRellenar(diamantesAux, costes[i], ref barrasCosteSuave[i]);
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
            if (data.diamantes < costes[id]) return;
            data.diamantes -= costes[id];
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

        if (data.recursos > 1000)
        {
            data.recursos = 0;
            data.recursosClickValor = 1;
            data.clickMejora2Coste = 100;
            data.produccionMejora1Coste = 25;
            data.produccionMejora2Coste = 250;
            data.produccionMejora2Poder = 5;

            data.clickMejora1Nivel = 0;
            data.clickMejora2Nivel = 0;
            data.produccionMejora1Nivel = 0;
            data.produccionMejora2Nivel = 0;

            data.diamantes += data.diamantesConseguidos;
        }
    }

    public BigDouble ValorTotalDiamantesMejora()
    {
        var aux = juego.data.diamantes;
        aux *= 0.05 + niveles[2] * 0.01;
        aux *= superNova.astrosMejora;

        return aux + 1;
    }
}