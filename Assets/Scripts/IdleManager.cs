using UnityEngine.UI;
using UnityEngine;
using BreakInfinity;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using static BreakInfinity.BigDouble;

[Serializable]
public class PlayerData
{
    // Recursos
    public BigDouble recursos;
    public BigDouble recursosTotales;
    public BigDouble recursosClickValor;
    public BigDouble recursosPorSegundo;

    //Clicks
    public BigDouble clickMejora1Nivel;
    public BigDouble clickMejora1Coste;

    public BigDouble clickMejora2Coste;
    public BigDouble clickMejora2Nivel;

    //Pasivos
    public BigDouble produccionMejora1Coste;
    public BigDouble produccionMejora1Nivel;

    public BigDouble produccionMejora2Coste;
    public BigDouble produccionMejora2Poder;
    public BigDouble produccionMejora2Nivel;

    //Prestigio
    public BigDouble diamantes;
    public BigDouble diamantesMejora;
    public BigDouble diamantesConseguidos;

    public BigDouble logroNivel1;
    public BigDouble logroNivel2;

    public PlayerData()
    {
        FullReset();
    }

    public void FullReset()
    {
        recursos = 0;
        recursosTotales = 0;
        recursosClickValor = 1;

        diamantes = 0;

        clickMejora1Nivel = 0;
        clickMejora1Coste = 10;

        clickMejora2Nivel = 0;
        clickMejora2Coste = 100;

        produccionMejora1Nivel = 0;
        produccionMejora1Coste = 25;

        produccionMejora2Nivel = 0;
        produccionMejora2Poder = 5;
        produccionMejora2Coste = 250;

        logroNivel1 = 0;
        logroNivel2 = 0;
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
    public Image mejoraBarraClick1;
    public Image mejoraBarraClick1Suave;

    public BigDouble recursosTemporal;

    //Cambia Ventanas
    public CanvasGroup ventanaPrincipalGrupo;
    public CanvasGroup ventanaMejorasGrupo;
    public CanvasGroup ventanaLogrosGrupo;

    //Opciones
    public GameObject opciones;

    public TMP_InputField textFieldImportar;
    public TMP_InputField textFieldExportar;

    public bool musicaFondo = true;
    public Image musicaFondoIcono;

    public GameObject logroVentana;
    public GameObject soundManager;

    public List<Logros> ListaLogros = new List<Logros>();

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        foreach (var obj in logroVentana.GetComponentsInChildren<Logros>())
        {
            ListaLogros.Add(obj);
        }

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

    // Update is called once per frame
    void Update()
    {
        IniciarLogros();

        //Barras de progreso
        NumeroSuave(ref recursosTemporal, data.recursos);
        BigDoubleRellenar(data.recursos, 10 * Pow(1.07, data.clickMejora1Nivel), mejoraBarraClick1);
        BigDoubleRellenar(recursosTemporal, 10 * Pow(1.07, data.clickMejora1Nivel), mejoraBarraClick1Suave);

        data.diamantesConseguidos = 150 * Sqrt(data.recursos / 1e7);
        data.diamantesMejora = data.diamantes * 0.05 + 1;

        textodiamantesConseguidos.text =
            "Prestigio:\n+" + MetodoNotacion(Floor(data.diamantesConseguidos), "F0") + " Diamantes";
        textoDiamantes.text = "Diamantes: " + MetodoNotacion(Floor(data.diamantes), "F0");
        textoMejoraDiamantes.text = MetodoNotacion(data.diamantesMejora, "F2") + "x Mejora";

        data.recursosPorSegundo =
            (data.produccionMejora1Nivel + (data.produccionMejora2Poder * data.produccionMejora2Nivel)) *
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
                                      MetodoNotacion(data.produccionMejora1Coste, "F0") +
                                      " recursos\nPoder + " + MetodoNotacion(data.diamantesMejora, "F0") +
                                      " Recursos/s\nNivel: " +
                                      data.produccionMejora1Nivel;

        textoMejoraProduccion2.text = "Produccion Mejora 2\nCoste: " +
                                      MetodoNotacion(data.produccionMejora2Coste, "F0") +
                                      " recursos\nPoder +" +
                                      MetodoNotacion((data.produccionMejora2Poder * data.diamantesMejora), "F0") +
                                      " Recursos/s\nNivel: " +
                                      data.produccionMejora2Nivel;

        data.recursos += data.recursosPorSegundo * Time.deltaTime;


        data.recursosTotales += data.recursosPorSegundo * Time.deltaTime;


        textoMejoraClick1Max.text = "Comprar todo (" + CompraClick1MaxContador() + ")";

        SaveSystem.SavePlayer(data);
    }

    private static string[] LogrosString => new string[] {"Recursos actuales", "Recursos totales"};
    private BigDouble[] Logros => new BigDouble[] {data.recursos, data.recursosTotales};

    private void IniciarLogros()
    {
        ActualizarLogros(LogrosString[0], Logros[0], ref data.logroNivel1, ref ListaLogros[0].barraProgreso,
            ref ListaLogros[0].titulo, ref ListaLogros[0].progreso);

        ActualizarLogros(LogrosString[1], Logros[1], ref data.logroNivel2, ref ListaLogros[1].barraProgreso,
            ref ListaLogros[1].titulo, ref ListaLogros[1].progreso);
    }

    private void ActualizarLogros(string nombre, BigDouble numero, ref BigDouble nivel, ref Image rellenar,
        ref Text titulo, ref Text progreso)
    {
        var capacidad = BigDouble.Pow(10, nivel);

        titulo.text = $"{nombre}\n({nivel})";
        progreso.text = $"{MetodoNotacion(numero, "F2")} / {MetodoNotacion(capacidad, "F2")}";

        BigDoubleRellenar(numero, capacidad, rellenar);

        if (numero < capacidad) return;
        BigDouble niveles = 0;

        if (numero / capacidad >= 1)
        {
            niveles = Floor(Log10(numero / capacidad)) + 1;
        }

        nivel += niveles;
    }

    public void BigDoubleRellenar(BigDouble x, BigDouble y, Image rellenar)
    {
        float z;
        var a = x / y;
        if (a < 0.001)
        {
            z = 0;
        }
        else if (a > 10)
        {
            z = 1;
        }
        else
        {
            z = (float) a.ToDouble();
            rellenar.fillAmount = z;
        }
    }

    public void NumeroSuave(ref BigDouble suave, BigDouble actual)
    {
        if (suave > actual & actual == 0)
        {
            suave -= (suave - actual) / 10 + 0.1 * Time.deltaTime;
        }
        else if (Floor(suave) < actual)
        {
            suave += (actual - suave) / 10 + 0.1 * Time.deltaTime;
        }
        else if (Floor(suave) > actual)
        {
            suave -= (suave - actual) / 10 + 0.1 * Time.deltaTime;
        }
        else
        {
            suave = actual;
        }
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

    public void Click()
    {
        data.recursos += data.recursosClickValor;
        data.recursosTotales += data.recursosClickValor;
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
                if (data.recursos >= data.produccionMejora1Coste)
                {
                    data.produccionMejora1Nivel++;
                    data.recursos -= data.produccionMejora1Coste;
                    data.produccionMejora1Coste *= 1.07;
                }

                break;

            case "M2":
                if (data.recursos >= data.produccionMejora2Coste)
                {
                    data.produccionMejora2Nivel++;
                    data.recursos -= data.produccionMejora2Coste;
                    data.produccionMejora2Coste *= 1.07;
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
                CambiadorDeCanvas(false, ventanaLogrosGrupo);
                break;

            case "Principal":
                CambiadorDeCanvas(true, ventanaPrincipalGrupo);
                CambiadorDeCanvas(false, ventanaMejorasGrupo);
                CambiadorDeCanvas(false, ventanaLogrosGrupo);
                break;

            case "Logros":
                CambiadorDeCanvas(false, ventanaPrincipalGrupo);
                CambiadorDeCanvas(false, ventanaMejorasGrupo);
                CambiadorDeCanvas(true, ventanaLogrosGrupo);
                break;
        }
    }

    public void IrAOpciones()
    {
        opciones.gameObject.SetActive(true);
    }

    public void AtrasOpciones()
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

    public void MusicaFondo()
    {
        if (musicaFondo)
        {
            soundManager.GetComponent<AudioSource>().enabled = false;
            musicaFondo = false;
            musicaFondoIcono.color = Color.red;
        }
        else if (!musicaFondo)
        {
            soundManager.GetComponent<AudioSource>().enabled = true;
            musicaFondo = true;
            musicaFondoIcono.color = Color.green;
        }
    }

    public void FullReset()
    {
        data.FullReset();
    }
}