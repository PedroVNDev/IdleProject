using UnityEngine.UI;
using UnityEngine;
using BreakInfinity;
using System;
using TMPro;
using static BreakInfinity.BigDouble;

[Serializable]
public class PlayerData
{
    // Recursos
    public BigDouble recursos;
    public BigDouble recursosClickValor;
    public BigDouble recursosPorSegundo;

    //Clicks
    public BigDouble clickMejora1Nivel;
    public BigDouble clickMejora1Coste;

    public BigDouble clickMejora2Coste;
    public BigDouble clickMejora2Nivel;

    //Pasivos
    public BigDouble ProduccionMejora1Coste;
    public BigDouble ProduccionMejora1Nivel;

    public BigDouble ProduccionMejora2Coste;
    public BigDouble ProduccionMejora2Poder;
    public BigDouble ProduccionMejora2Nivel;

    //Prestigio
    public BigDouble diamantes;
    public BigDouble diamantesMejora;
    public BigDouble diamantesConseguidos;

    public PlayerData()
    {
        FullReset();
    }

    public void FullReset()
    {
        recursos = 0;
        recursosClickValor = 1;

        diamantes = 0;

        clickMejora1Nivel = 0;
        clickMejora1Coste = 10;

        clickMejora2Nivel = 0;
        clickMejora2Coste = 100;

        ProduccionMejora1Nivel = 0;
        ProduccionMejora1Coste = 25;

        ProduccionMejora2Nivel = 0;
        ProduccionMejora2Poder = 5;
        ProduccionMejora2Coste = 250;
    }
}

public class IdleManager : MonoBehaviour
{
    public PlayerData data;

    //Textos
    public Text textoRecursos;
    public Text textoRecursosClick;
    public Text textoRecursosPorSegundo;
    public Text textoMejoraClick1;
    public Text textoMejoraClick2;
    public Text textoMejoraProduccion1;
    public Text textoMejoraProduccion2;

    public Text textoMejoraClick1Max;


    //Prestigio
    public Text textoDiamantes;
    public Text textoMejoraDiamantes;
    public Text textodiamantesConseguidos;


    //Barras de progreso
    public Image MejoraBarraClick1;

    //Cambia Ventanas
    public CanvasGroup ventanaPrincipalGrupo;
    public CanvasGroup ventanaMejorasGrupo;

    //Opciones
    public GameObject opciones;

    public TMP_InputField textFieldImportar;
    public TMP_InputField textFieldExportar;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        CambiadorDeCanvas(true, ventanaPrincipalGrupo);
        CambiadorDeCanvas(false, ventanaMejorasGrupo);

        SaveSystem.LoadPlayer(ref data);
    }

    public void CambiadorDeCanvas(bool x, CanvasGroup y)
    {
        if (x)
        {
            y.alpha = 1;
            y.interactable = true;
            y.blocksRaycasts = true;
            return;
        }

        y.alpha = 0;
        y.interactable = false;
        y.blocksRaycasts = false;
    }

    //Carga con PlayerPrefs
    /*
    public void Cargar()
    {
        recursos = Parse(PlayerPrefs.GetString("recursos", "0"));
        recursosClickValor = Parse(PlayerPrefs.GetString("recursosClickValor", "1"));
        clickMejora2Coste = Parse(PlayerPrefs.GetString("clickMejora2Coste", "100"));
        ProduccionMejora1Coste = Parse(PlayerPrefs.GetString("ProduccionMejora1Coste", "25"));
        ProduccionMejora2Coste = Parse(PlayerPrefs.GetString("ProduccionMejora2Coste", "250"));
        ProduccionMejora2Poder = Parse(PlayerPrefs.GetString("ProduccionMejora2Poder", "5"));

        diamantes = Parse(PlayerPrefs.GetString("diamantes", "0"));

        clickMejora1Nivel = Parse(PlayerPrefs.GetString("clickMejora1Nivel", "0"));
        clickMejora2Nivel = Parse(PlayerPrefs.GetString("clickMejora2Nivel", "0"));
        ProduccionMejora1Nivel = Parse(PlayerPrefs.GetString("ProduccionMejora1Nivel", "0"));
        ProduccionMejora2Nivel = Parse(PlayerPrefs.GetString("ProduccionMejora2Nivel", "0"));
    }

    //Guardado con PlayerPrefs
    public void Guardar()
    {
        PlayerPrefs.SetString("recursos", recursos.ToString());
        PlayerPrefs.SetString("recursosClickValor", recursosClickValor.ToString());
        PlayerPrefs.SetString("clickMejora2Coste", clickMejora2Coste.ToString());
        PlayerPrefs.SetString("ProduccionMejora1Coste", ProduccionMejora1Coste.ToString());
        PlayerPrefs.SetString("ProduccionMejora2Coste", ProduccionMejora2Coste.ToString());
        PlayerPrefs.SetString("ProduccionMejora2Poder", ProduccionMejora2Poder.ToString());

        PlayerPrefs.SetString("diamantes", diamantes.ToString());

        PlayerPrefs.SetString("clickMejora1Nivel", clickMejora1Nivel.ToString());
        PlayerPrefs.SetString("clickMejora2Nivel", clickMejora2Nivel.ToString());
        PlayerPrefs.SetString("ProduccionMejora1Nivel", ProduccionMejora1Nivel.ToString());
        PlayerPrefs.SetString("ProduccionMejora2Nivel", ProduccionMejora2Nivel.ToString());
    }*/

    // Update is called once per frame
    void Update()
    {
        data.diamantesConseguidos = 150 * Sqrt(data.recursos / 1e7);
        data.diamantesMejora = data.diamantes * 0.05 + 1;

        textodiamantesConseguidos.text =
            "Prestigio:\n+" + MetodoNotacion(Floor(data.diamantesConseguidos), "F0") + " Diamantes";
        textoDiamantes.text = "Diamantes: " + MetodoNotacion(Floor(data.diamantes), "F0");
        textoMejoraDiamantes.text = MetodoNotacion(data.diamantesMejora, "F2") + "x Mejora";

        data.recursosPorSegundo =
            (data.ProduccionMejora1Nivel + (data.ProduccionMejora2Poder * data.ProduccionMejora2Nivel)) *
            data.diamantesMejora;


        textoRecursosClick.text = "Click \n" + MetodoNotacion(data.recursosClickValor, "F0") + " Recursos";
        textoRecursos.text = "Recursos: " + MetodoNotacion(data.recursos, "F0");


        textoRecursosPorSegundo.text = " Recursos/s " + MetodoNotacion(data.recursosPorSegundo, "F0");

        var clickMejora1Coste = 10 * Pow(1.07, data.clickMejora1Nivel);

        //Texto de mejoras

        textoMejoraClick1.text = "Click Mejora 1\nCoste: " + MetodoNotacion(clickMejora1Coste, "F0") +
                                 " recursos\nPoder +1 Click\nNivel: " +
                                 data.clickMejora1Nivel;

        textoMejoraClick2.text = "Click Mejora 2\nCoste: " + MetodoNotacion(data.clickMejora2Coste, "F0") +
                                 " recursos\nPoder +5 Click\nNivel: " +
                                 data.clickMejora2Nivel;

        textoMejoraProduccion1.text = "Produccion Mejora 1\nCoste: " +
                                      MetodoNotacion(data.ProduccionMejora1Coste, "F0") +
                                      " recursos\nPoder + " + MetodoNotacion(data.diamantesMejora, "F0") +
                                      " Recursos/s\nNivel: " +
                                      data.ProduccionMejora1Nivel;

        textoMejoraProduccion2.text = "Produccion Mejora 2\nCoste: " +
                                      MetodoNotacion(data.ProduccionMejora2Coste, "F0") +
                                      " recursos\nPoder +" +
                                      MetodoNotacion((data.ProduccionMejora2Poder * data.diamantesMejora), "F0") +
                                      " Recursos/s\nNivel: " +
                                      data.ProduccionMejora2Nivel;

        data.recursos += data.recursosPorSegundo * Time.deltaTime;

        //Barras de progreso
        if (data.recursos / clickMejora1Coste < 0.01)
        {
            MejoraBarraClick1.fillAmount = 0;
        }
        else if (data.recursos / clickMejora1Coste > 10)
        {
            MejoraBarraClick1.fillAmount = 1;
        }
        else
        {
            MejoraBarraClick1.fillAmount = (float) (data.recursos / clickMejora1Coste).ToDouble();
        }

        textoMejoraClick1Max.text = "Comprar todo (" + CompraClick1MaxContador() + ")";

        SaveSystem.SavePlayer(data);
    }

    public string MetodoNotacion(BigDouble x, string y)
    {
        if (x > 1000)
        {
            var exponente = Floor(Log10(Abs(x)));
            var mantissa = x / Pow(10, exponente);
            return mantissa.ToString("F2") + "e" + exponente;
        }

        return x.ToString(y);
    }

    //Prestigio reset
    public void Prestigio()
    {
        if (data.recursos > 1000)
        {
            data.recursos = 0;
            data.recursosClickValor = 1;
            data.clickMejora2Coste = 100;
            data.ProduccionMejora1Coste = 25;
            data.ProduccionMejora2Coste = 250;
            data.ProduccionMejora2Poder = 5;

            data.clickMejora1Nivel = 0;
            data.clickMejora2Nivel = 0;
            data.ProduccionMejora1Nivel = 0;
            data.ProduccionMejora2Nivel = 0;

            data.diamantes += data.diamantesConseguidos;
        }
    }

    public void Click()
    {
        data.recursos += data.recursosClickValor;
    }

    //Mejoras
    public BigDouble CompraClick1MaxContador()
    {
        var b = 10;
        var c = data.recursos;
        var r = 1.07;
        var k = data.clickMejora1Nivel;
        var n = Floor(Log(c * (r - 1) / (b * Pow(r, k)) + 1, r));

        return n;
    }

    public void CompraMejora(string mejoraID)
    {
        switch (mejoraID)
        {
            case "C1":
                var coste1 = 10 * Pow(1.07, data.clickMejora1Nivel);

                if (data.recursos >= coste1)
                {
                    data.clickMejora1Nivel++;
                    data.recursos -= coste1;
                    data.recursosClickValor++;
                }

                break;

            case "C1Max":
                var b = 10;
                var c = data.recursos;
                var r = 1.07;
                var k = data.clickMejora1Nivel;
                var n = Floor(Log(c * (r - 1) / (b * Pow(r, k)) + 1, r));

                var coste2 = b * (Pow(r, k) * (Pow(r, n) - 1) / (r - 1));

                if (data.recursos >= coste2)
                {
                    data.clickMejora1Nivel += n;
                    data.recursos -= coste2;
                    data.recursosClickValor += n;
                }

                break;

            case "C2":
                if (data.recursos >= data.clickMejora2Coste)
                {
                    data.clickMejora2Nivel++;
                    data.recursos -= data.clickMejora2Coste;
                    data.clickMejora2Coste *= 1.09;
                    data.recursosClickValor += 5;
                }

                break;

            case "M1":
                if (data.recursos >= data.ProduccionMejora1Coste)
                {
                    data.ProduccionMejora1Nivel++;
                    data.recursos -= data.ProduccionMejora1Coste;
                    data.ProduccionMejora1Coste *= 1.07;
                }

                break;

            case "M2":
                if (data.recursos >= data.ProduccionMejora2Coste)
                {
                    data.ProduccionMejora2Nivel++;
                    data.recursos -= data.ProduccionMejora2Coste;
                    data.ProduccionMejora2Coste *= 1.07;
                }

                break;

            default:
                Debug.Log("Mejora sin asignacion");
                break;
        }
    }

    public void CambiaVentanas(string id)
    {
        switch (id)
        {
            case "Mejoras":
                CambiadorDeCanvas(false, ventanaPrincipalGrupo);
                CambiadorDeCanvas(true, ventanaMejorasGrupo);
                break;

            case "Principal":
                CambiadorDeCanvas(true, ventanaPrincipalGrupo);
                CambiadorDeCanvas(false, ventanaMejorasGrupo);
                break;
        }
    }

    public void irAOpciones()
    {
        opciones.gameObject.SetActive(true);
    }

    public void atrasOpciones()
    {
        opciones.gameObject.SetActive(false);
    }

    public void BotonGuardar()
    {
        SaveSystem.SavePlayer(data);
        Debug.Log("Guardado Manual");
    }

    public void BorrarCampos()
    {

        textFieldExportar.text = "";
        textFieldImportar.text = "";
        Debug.Log("Borrar Campos");
    }

    public void FullReset()
    {
        data.FullReset();
    }
}