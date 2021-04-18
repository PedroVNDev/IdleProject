using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;
using static BreakInfinity.BigDouble;

public class AutomatorManager : MonoBehaviour
{
    public IdleManager juego;

    public Text[] textoCoste = new Text[2];
    public Image[] barrasCoste = new Image[2];
    public Image[] barrasCosteSuave = new Image[2];

    public string[] costeDescripcion;
    public BigDouble[] costes;
    public int[] niveles;
    public int[] nivelesLimite;
    public float[] intervalos;
    public float[] timer;

    private BigDouble coste1 => 1e4 * BigDouble.Pow(1.5, juego.data.autoNivel1);
    private BigDouble coste2 => 1e5 * BigDouble.Pow(1.5, juego.data.autoNivel2);

    public BigDouble diamantesAux;


    public void EmpezarAutomatizadores()
    {
        costes = new BigDouble[2];
        niveles = new int[2];
        nivelesLimite = new[] {21, 21};
        intervalos = new float[2];
        timer = new float[2];
        costeDescripcion = new[]
        {
            "Click Mejora 1\n AutoComprador", "Produccion Mejora 1 AutoComprador"
        };
    }

    public void Run()
    {
        ArrayManager();
        UI();
        RunAuto();

        void UI()
        {
            if (!juego.ventanaAutomaticoGrupo.gameObject.activeSelf) return;
            {
                for (var i = 0; i < textoCoste.Length; i++)
                {
                    textoCoste[i].text =
                        $"{costeDescripcion[i]}\nCoste: {juego.MetodoNotacion(costes[i], "F0")} Recursos\nIntervalo: {(niveles[i] >= nivelesLimite[i] ? "Instantaneo" : intervalos[i].ToString("F1"))}";
                    Metodos.BigDoubleRellenar(juego.data.recursos, costes[i], ref barrasCoste[i]);
                    Metodos.BigDoubleRellenar(juego.recursosTemporal, costes[i], ref barrasCosteSuave[i]);
                }
            }
        }
    }

    void RunAuto()
    {
        Auto(0, "C1");
        Auto(0, "M1");

        void Auto(int id, string nombre)
        {
            if (niveles[id] > 0)
            {
                if (niveles[id] != nivelesLimite[id])
                {
                    timer[id] += Time.deltaTime;
                    if (timer[id] >= intervalos[id])
                    {
                        juego.CompraMejora("C1");
                        timer[0] = 0;
                    }
                }
            }
            else
            {
                switch (nombre)
                {
                    case "C1":
                        if (juego.CompraClick1MaxContador() != 0)
                        {
                            //juego.CompraMejora("C1Max");
                        }

                        break;

                    case "M1":
                        if (juego.CompraClick1MaxContador() != 0)
                        {
                            //juego.CompraMejora("M1Max");
                        }

                        break;
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
                Comprar(ref data.autoNivel1);
                break;

            case 1:
                Comprar(ref data.autoNivel2);
                break;
        }

        void Comprar(ref int nivel)
        {
            if (!(data.recursos >= costes[id] & nivel < nivelesLimite[id])) return;
            {
                data.recursos -= costes[id];
                nivel++;
            }
        }
    }

    public void ArrayManager()
    {
        var data = juego.data;

        costes[0] = coste1;
        costes[1] = coste2;

        niveles[0] = data.autoNivel1;
        niveles[1] = data.autoNivel2;

        if (data.autoNivel1 > 0)
        {
            intervalos[0] = 10 - (data.autoNivel1 - 1) * 0.5f;
        }

        if (data.autoNivel2 > 0)
        {
            intervalos[1] = 10 - (data.autoNivel2 - 1) * 0.5f;
        }
    }
}