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

    //Clicks
    public int clickMejora1Nivel;
    public BigDouble clickMejora1Coste;

    public BigDouble clickMejora2Coste;
    public int clickMejora2Nivel;

    //Pasivos
    public BigDouble produccionMejora1Coste;
    public int produccionMejora1Nivel;

    public BigDouble produccionMejora2Coste;
    public BigDouble produccionMejora2Poder;
    public int produccionMejora2Nivel;

    //Prestigio
    public BigDouble diamantes;
    public BigDouble diamantesConseguidos;

    //Nivel Logros
    public float logroNivel1;
    public float logroNivel2;

    //Eventos
    public BigDouble tokensEvento;
    public float[] eventCooldown = new float[7];
    public int eventoActivoID;

    #region Prestigio

    public int prestigioMNivel1;
    public int prestigioMNivel2;
    public int prestigioMNivel3;

    #endregion

    #region SuperNova

    public BigDouble astros;

    #endregion

    public PlayerData()
    {
        FullReset();
    }

    public void FullReset()
    {
        recursos = 0;
        recursosTotales = 0;
        recursosClickValor = 1;

        //Prestigio
        diamantes = 0;

        prestigioMNivel1 = 0;
        prestigioMNivel2 = 0;
        prestigioMNivel3 = 0;

        //Mejoras
        clickMejora1Nivel = 0;
        clickMejora1Coste = 10;

        clickMejora2Nivel = 0;
        clickMejora2Coste = 100;

        produccionMejora1Nivel = 0;
        produccionMejora1Coste = 25;

        produccionMejora2Nivel = 0;
        produccionMejora2Poder = 5;
        produccionMejora2Coste = 250;

        //Nivel Logros
        logroNivel1 = 0;
        logroNivel2 = 0;

        //Eventos
        tokensEvento = 0;
        for (int i = 0; i < eventCooldown.Length; i++)
        {
            eventCooldown[i] = 0;
        }

        eventoActivoID = 0;

        prestigioMNivel1 = 0;
        prestigioMNivel2 = 0;
        prestigioMNivel3 = 0;

        astros = 0;
    }
}

public class IdleManager : MonoBehaviour
{
    public PlayerData data;
    public EventManager eventos;
    public PrestigeManager prestigio;
    public SuperNovaManager superNova;

    public GameObject clickMejora1;
    public GameObject clickMejora2;
    public GameObject produccionMejora1;
    public GameObject produccionMejora2;

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
    public Canvas ventanacabeceraGrupo;
    public Canvas ventanaPrincipalGrupo;
    public Canvas ventanaMejorasGrupo;
    public Canvas ventanaLogrosGrupo;
    public Canvas ventanaEventosGrupo;
    public Canvas ventanaSuperNovaGrupo;
    public Canvas ventanaOpcionesGrupo;


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

        ventanaPrincipalGrupo.gameObject.SetActive(true);
        ventanaMejorasGrupo.gameObject.SetActive(false);
        prestigio.prestigio.gameObject.SetActive(false);

        eventos.StartEventos();
        prestigio.EmpezarPrestigio();

        SaveSystem.LoadPlayer(ref data);
    }

    // Update is called once per frame
    void Update()
    {
        IniciarLogros();
        prestigio.Run();
        superNova.Run();

        //Barras de progreso
        Metodos.NumeroSuave(ref recursosTemporal, data.recursos);
        Metodos.BigDoubleRellenar(data.recursos, 10 * Pow(1.07, data.clickMejora1Nivel), ref mejoraBarraClick1);
        Metodos.BigDoubleRellenar(recursosTemporal, 10 * Pow(1.07, data.clickMejora1Nivel), ref mejoraBarraClick1Suave);

        data.diamantesConseguidos = 150 * Sqrt(data.recursos / 1e7);


        if (ventanaPrincipalGrupo.gameObject.activeSelf)
        {
            textodiamantesConseguidos.text =
                "Prestigio:\n+" + MetodoNotacion(Floor(data.diamantesConseguidos), "F0") + " Diamantes";

            textoRecursosClick.text = "Click \n" +
                                      MetodoNotacion((ValorClickTotal()), "F0") +
                                      " Recursos";
        }


        textoDiamantes.text = "Diamantes: " + MetodoNotacion(Floor(data.diamantes), "F0");
        textoMejoraDiamantes.text = MetodoNotacion(ValorTotalDiamantesMejora(), "F2") + "x Mejora";


        textoRecursos.text = "Recursos: " + MetodoNotacion(data.recursos, "F0");


        textoRecursosPorSegundo.text = " Recursos/s " + MetodoNotacion(ValorTotalRecursosPorSegundo(), "F0");


        //Si hay autocompradores esto va fuera
        if (ventanaMejorasGrupo.gameObject.activeSelf)
        {
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
                                          " recursos\nPoder + " +
                                          MetodoNotacion((MejoraTotal() * Pow(1.1, prestigio.niveles[1])), "F0") +
                                          " Recursos/s\nNivel: " +
                                          data.produccionMejora1Nivel;

            textoMejoraProduccion2.text = "Produccion Mejora 2\nCoste: " +
                                          MetodoNotacion(data.produccionMejora2Coste, "F0") +
                                          " recursos\nPoder +" +
                                          MetodoNotacion(
                                              (data.produccionMejora2Poder * MejoraTotal() *
                                               Pow(1.1, prestigio.niveles[1])), "F0") +
                                          " Recursos/s\nNivel: " +
                                          data.produccionMejora2Nivel;

            //Comprar Max
            textoMejoraClick1Max.text = ComprarTodoFormato(CompraClick1MaxContador());

            if (data.recursosTotales >= 10)
                clickMejora1.SetActive(true);
            else
            {
                clickMejora1.SetActive(false);
            }

            if (data.recursosTotales >= 100)
                clickMejora2.SetActive(true);
            else
            {
                clickMejora2.SetActive(false);
            }

            if (data.recursosTotales >= 25)
                produccionMejora1.SetActive(true);
            else
            {
                produccionMejora1.SetActive(false);
            }

            if (data.recursosTotales >= 250)
                produccionMejora2.SetActive(true);
            else
            {
                produccionMejora2.SetActive(false);
            }
        }

        string ComprarTodoFormato(BigDouble x)
        {
            return $"Comprar Todo ({x})";
        }

        //Update de recursos
        data.recursos += ValorTotalRecursosPorSegundo() * Time.deltaTime;
        data.recursosTotales += ValorTotalRecursosPorSegundo() * Time.deltaTime;


        //Guardado Automatico
        contadorGuardado += Time.deltaTime;

        if (!(contadorGuardado >= 15)) return;
        {
            SaveSystem.SavePlayer(data);
            contadorGuardado = 0;
            Debug.Log("Guardado Automatico");
        }
    }

    public float contadorGuardado;

    private static string[] LogrosString => new string[] {"Recursos actuales", "Recursos totales"};
    private BigDouble[] Logros => new BigDouble[] {data.recursos, data.recursosTotales};

    private void IniciarLogros()
    {
        ActualizarLogros(LogrosString[0], Logros[0], ref data.logroNivel1, ref ListaLogros[0].barraProgreso,
            ref ListaLogros[0].titulo, ref ListaLogros[0].progreso);

        ActualizarLogros(LogrosString[1], Logros[1], ref data.logroNivel2, ref ListaLogros[1].barraProgreso,
            ref ListaLogros[1].titulo, ref ListaLogros[1].progreso);
    }

    private void ActualizarLogros(string nombre, BigDouble numero, ref float nivel, ref Image rellenar,
        ref Text titulo, ref Text progreso)
    {
        var capacidad = BigDouble.Pow(10, nivel);

        if (ventanaLogrosGrupo.gameObject.activeSelf)
        {
            titulo.text = $"{nombre}\n({nivel})";
            progreso.text = $"{MetodoNotacion(numero, "F2")} / {MetodoNotacion(capacidad, "F2")}";

            Metodos.BigDoubleRellenar(numero, capacidad, ref rellenar);
        }

        if (numero < capacidad) return;
        BigDouble niveles = 0;

        if (numero / capacidad >= 1)
        {
            niveles = Floor(Log10(numero / capacidad)) + 1;
        }

        nivel += (float) niveles;
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

    private BigDouble MejoraTotal()
    {
        BigDouble aux = ValorTotalDiamantesMejora();
        aux *= eventos.tokensEventoMejora;
        return aux;
    }

    private BigDouble ValorTotalDiamantesMejora()
    {
        var aux = data.diamantes;
        aux *= 0.05 + prestigio.niveles[2] * 0.01;
        aux *= superNova.astrosMejora;

        return aux + 1;
    }

    private BigDouble ValorTotalRecursosPorSegundo()
    {
        BigDouble aux = 0;

        aux += data.produccionMejora1Nivel;
        aux += data.produccionMejora2Poder * data.produccionMejora2Nivel;
        aux *= MejoraTotal();
        aux *= eventos.tokensEventoMejora;
        aux *= Pow(1.1, prestigio.niveles[1]);

        return aux;
    }

    private BigDouble ValorClickTotal()
    {
        var aux = data.recursosClickValor;

        aux *= eventos.tokensEventoMejora;
        aux *= MejoraTotal();
        aux *= Pow(1.5, prestigio.niveles[0]);

        return aux;
    }

    public void Click()
    {
        data.recursos += ValorClickTotal();
        data.recursosTotales += ValorClickTotal();
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
                    data.clickMejora1Nivel += (int) n;
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
        DesactivarTodo();
        switch (id)
        {
            case "Cabecera":
                ventanacabeceraGrupo.gameObject.SetActive(true);
                break;

            case "Mejoras":
                ventanaMejorasGrupo.gameObject.SetActive(true);
                break;

            case "Principal":
                ventanaPrincipalGrupo.gameObject.SetActive(true);
                break;

            case "Logros":
                ventanaLogrosGrupo.gameObject.SetActive(true);
                break;

            case "Opciones":
                ventanacabeceraGrupo.gameObject.SetActive(false);
                ventanaOpcionesGrupo.gameObject.SetActive(true);
                break;

            case "VolverOpciones":
                ventanaOpcionesGrupo.gameObject.SetActive(false);
                ventanacabeceraGrupo.gameObject.SetActive(true);
                ventanaPrincipalGrupo.gameObject.SetActive(true);
                break;

            case "Eventos":
                ventanaEventosGrupo.gameObject.SetActive(true);
                break;

            case "PrestigioMejoras":
                prestigio.prestigio.gameObject.SetActive(true);
                break;

            case "SuperNova":
                ventanaSuperNovaGrupo.gameObject.SetActive(true);
                break;
        }

        void DesactivarTodo()
        {
            ventanaPrincipalGrupo.gameObject.SetActive(false);
            ventanaMejorasGrupo.gameObject.SetActive(false);
            ventanaLogrosGrupo.gameObject.SetActive(false);
            ventanaEventosGrupo.gameObject.SetActive(false);
            prestigio.prestigio.gameObject.SetActive(false);
            ventanaSuperNovaGrupo.gameObject.SetActive(false);
            ventanaOpcionesGrupo.gameObject.SetActive(false);
        }
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
        CambiaVentanas("Principal");
    }
}

public class Metodos : MonoBehaviour
{
    public static void CambiadorDeCanvas(bool x, CanvasGroup y)
    {
        y.alpha = x ? 1 : 0;
        y.interactable = x;
        y.blocksRaycasts = x;
    }

    public static void BigDoubleRellenar(BigDouble x, BigDouble y, ref Image rellenar)
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

    public static void NumeroSuave(ref BigDouble suave, BigDouble actual)
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
}