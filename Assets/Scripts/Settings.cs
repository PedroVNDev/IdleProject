using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public IdleManager juego;

    public Text textoTipoNotacion;

    public Image idiomaImagen;

    public Sprite banderaESP;
    public Sprite banderaEEUU;

    public void StartSettings()
    {
        ActualizarTextoNotacion();
        ActualizarIdioma();
    }

    private void ActualizarTextoNotacion()
    {
        var notacion = juego.data.tipoNotacion;

        if (juego.data.idiomaSeleccionado == 0)
        {
            switch (notacion)
            {
                case 0:
                    textoTipoNotacion.text = "Notacion:\n Cientifica";
                    break;
                case 1:
                    textoTipoNotacion.text = "Notacion:\n Ingeniera";
                    break;
                case 2:
                    textoTipoNotacion.text = "Notacion:\n Abreviada";
                    break;
            }
        }
        
        if (juego.data.idiomaSeleccionado == 1)
        {
            switch (notacion)
            {
                case 0:
                    textoTipoNotacion.text = "Notation:\n Scientific";
                    break;
                case 1:
                    textoTipoNotacion.text = "Notation:\n Ingeniering";
                    break;
                case 2:
                    textoTipoNotacion.text = "Notation:\n Word";
                    break;
            }
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
        Metodos.OpcionesNotacion = juego.data.tipoNotacion;
        ActualizarTextoNotacion();
    }

    private void ActualizarIdioma()
    {
        var idioma = juego.data.idiomaSeleccionado;

        switch (idioma)
        {
            case 0:
                idiomaImagen.sprite = banderaESP;
                break;
            case 1:
                idiomaImagen.sprite = banderaEEUU;
                break;
        }
    }

    public void CambiarIdioma()
    {
        var idioma = juego.data.idiomaSeleccionado;

        switch (idioma)
        {
            case 0:
                idioma = 1;
                break;
            case 1:
                idioma = 0;
                break;
        }

        juego.data.idiomaSeleccionado = idioma;
        ActualizarIdioma();
    }
}