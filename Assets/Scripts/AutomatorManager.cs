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
    public UpgradeManager mejoras;

    public Text textoAutoEnabled;

    public Text[] textoCoste = new Text[4];
    public Image[] barrasCoste = new Image[4];
    public Image[] barrasCosteSuave = new Image[4];

    public string[] costeDescripcion;
    public BigDouble[] costes;
    public int[] niveles;
    public int[] nivelesLimite;

    private BigDouble coste1 => 1e4 * BigDouble.Pow(1.5, juego.data.autoNivel1);
    private BigDouble coste2 => 1e5 * BigDouble.Pow(1.5, juego.data.autoNivel2);


    public void EmpezarAutomatizadores()
    {
        costes = new BigDouble[2];
        niveles = new int[2];
        nivelesLimite = new[] {1, 1};
        costeDescripcion = new[]
        {
            "Click \nAutoComprador", "Produccion \nAutoComprador"
        };
    }

    public void Run()
    {
        ArrayManager();
        UI();
        ActualizarTextoAutoActivado();
        if (juego.data.autosEnabled == 1)
        {
            RunAuto();
        }

        void UI()
        {
            if (!juego.ventanaAutomaticoGrupo.gameObject.activeSelf) return;
            {
                for (var i = 0; i < textoCoste.Length; i++)
                {
                    if (niveles[i] == 1)
                    {
                        textoCoste[i].text = $"Comprado";
                    }
                    else
                    {
                        textoCoste[i].text =
                            $"{costeDescripcion[i]}\nCoste: {Metodos.MetodoNotacion(costes[i], "F0")}";
                    }
                }
            }
        }
    }

    void RunAuto()
    {
        CAuto(0, 0);
        MAuto(1, 0);

        void CAuto(int id, int index)
        {
            if (niveles[id] <= 0) return;
            if (juego.data.terrans < juego.mejoras.clickMejoraCoste[index]) return;
            mejoras.CompraClickMax(index);
        }

        void MAuto(int id, int index)
        {
            if (niveles[id] <= 0) return;
            if (juego.data.terrans < juego.mejoras.produccionMejoraCoste[index]) return;
            mejoras.CompraProduccionMax(index);
        }
        
        CAuto2(0, 1);
        MAuto2(1, 1);

        void CAuto2(int id, int index)
        {
            if (niveles[id] <= 0) return;
            if (juego.data.terrans < juego.mejoras.clickMejoraCoste[index]) return;
            mejoras.CompraClickMax(index);
        }

        void MAuto2(int id, int index)
        {
            if (niveles[id] <= 0) return;
            if (juego.data.terrans < juego.mejoras.produccionMejoraCoste[index]) return;
            mejoras.CompraProduccionMax(index);
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
            if (!(data.terrans >= costes[id] & nivel < nivelesLimite[id])) return;
            data.terrans -= costes[id];
            nivel++;
        }
    }

    public void ArrayManager()
    {
        var data = juego.data;

        costes[0] = coste1;
        costes[1] = coste2;

        niveles[0] = data.autoNivel1;
        niveles[1] = data.autoNivel2;
    }

    public void ActivarAutos()
    {
        var autosActivados = juego.data.autosEnabled;

        switch (autosActivados)
        {
            case 0:
                autosActivados = 1;
                break;
            case 1:
                autosActivados = 0;
                break;
        }

        juego.data.autosEnabled = autosActivados;
        ActualizarTextoAutoActivado();
    }

    private void ActualizarTextoAutoActivado()
    {
        var autosActivados = juego.data.autosEnabled;

        switch (autosActivados)
        {
            case 0:
                textoAutoEnabled.text = $"Desactivados";
                break;
            case 1:
                textoAutoEnabled.text = $"Activados";
                break;
        }
    }
}