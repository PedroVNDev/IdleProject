using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public IdleManager juego;

    public Text textoTipoNotacion;

    public void StartSettings()
    {
        ActualizarTextoNotacion();
    }

    private void ActualizarTextoNotacion()
    {
        var notacion = juego.data.tipoNotacion;

        switch (notacion)
        {
            case 0:
                textoTipoNotacion.text = "Notacion:\n Cientifica";
                break;
            case 1:
                textoTipoNotacion.text = "Notacion:\n Ingeniera";
                break;
            case 2:
                textoTipoNotacion.text = "Notacion:\n Letras";
                break;
        }
    }

    public void CambiarNotacion()
    {
        var notacion = juego.data.tipoNotacion;

        switch (notacion)
        {
            case 0:
                notacion = 1;
                break;
            case 1:
                notacion = 2;
                break;
            case 2:
                notacion = 0;
                break;
        }

        juego.data.tipoNotacion = notacion;
        ActualizarTextoNotacion();
        
    }
}