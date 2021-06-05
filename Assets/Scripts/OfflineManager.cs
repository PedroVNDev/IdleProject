using System;
using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;


public class OfflineManager : MonoBehaviour
{
    public IdleManager juego;

    public GameObject offlinePopUp;

    public Text textoTiempoFuera;
    public Text textoGananciasTierra;

    public void LoadProduccionOffline()
    {
        if (juego.data.progresoOfflineActivado)
        {
            //Offline 
            var auxOfflineTiempo = Convert.ToInt64(PlayerPrefs.GetString("OfflineTime"));
            DateTime tiempoPasado = DateTime.FromBinary(auxOfflineTiempo);
            var tiempoActual = DateTime.Now;
            var diferencia = tiempoActual.Subtract(tiempoPasado);
            var tiempoBruto = (float) diferencia.TotalSeconds;
            var tiempoOffline = tiempoBruto / 10;

            offlinePopUp.gameObject.SetActive(true);
            TimeSpan timer = TimeSpan.FromSeconds(tiempoBruto);
            
            if (juego.data.idiomaSeleccionado == 0) 
                textoTiempoFuera.text = $"Estuviste fuera por:\n<color=#00FF00>{timer:dd\\:hh\\:mm\\:ss}</color>";
            else if (juego.data.idiomaSeleccionado == 1)
                textoTiempoFuera.text = $"You were away for:\n<color=#00FF00>{timer:dd\\:hh\\:mm\\:ss}</color>";
            


            BigDouble gananciasTerrans = juego.ValorTotalTerransPorSegundo() * tiempoOffline;
            juego.data.terrans += gananciasTerrans;
            juego.data.terransTotales += gananciasTerrans;
            
            if (juego.data.idiomaSeleccionado == 0) 
                textoGananciasTierra.text = $"<color=#00aeff>Ganancias:\n + {Metodos.MetodoNotacion(gananciasTerrans, "F2")} Terrans</color>";
            else if (juego.data.idiomaSeleccionado == 1)
                textoGananciasTierra.text = $"<color=#00aeff>Gains:\n + {Metodos.MetodoNotacion(gananciasTerrans, "F2")} Terrans</color>";
        }
    }

    public void CerrarOfflinePopUp()
    {
        offlinePopUp.gameObject.SetActive(false);
    }
}