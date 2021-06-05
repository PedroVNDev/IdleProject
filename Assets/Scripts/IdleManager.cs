using UnityEngine.UI;
using UnityEngine;
using BreakInfinity;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using static BreakInfinity.BigDouble;


public class IdleManager : MonoBehaviour
{
    public PlayerData data;
    public EventManager eventos;
    public PrestigeManager prestigio;
    public SuperNovaManager superNova;
    public AutomatorManager automatizador;
    public AchievementManager logros;
    public UpgradeManager mejoras;
    public PlanetManager planetas;
    public OfflineManager offline;
    public Settings settingsManager;

    public Text textoTerrans;
    public Text textoTerransClick;
    public Text textoTerransPorSegundo;

    public BigDouble terransTemporal;

    //Cambia Ventanas
    public Canvas ventanacabeceraGrupo;
    public Canvas ventanaPrincipalGrupo;
    public Canvas ventanaMejorasGrupo;
    public Canvas ventanaLogrosGrupo;
    public Canvas ventanaEventosGrupo;
    public Canvas ventanaSuperNovaGrupo;
    public Canvas ventanaOpcionesGrupo;
    public Canvas ventanaAutomaticoGrupo;
    public Canvas ventanaPlanetasGrupo;
    public Canvas ventanaTierraGrupo;
    public Canvas ventanaMenuJuegoGrupo;
    public Canvas ventanaMenuUsuarioGrupo;


    //Opciones
    public GameObject opciones;

    public TMP_InputField textFieldImportar;
    public TMP_InputField textFieldExportar;

    public Image musicaFondoIcono;
    public GameObject soundManager;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        ventanaPrincipalGrupo.gameObject.SetActive(true);
        ventanaMejorasGrupo.gameObject.SetActive(false);
        prestigio.prestigio.gameObject.SetActive(false);

        SaveSystem.LoadPlayer(ref data);
        eventos.StartEventos();
        prestigio.EmpezarPrestigio();
        automatizador.EmpezarAutomatizadores();
        ValorTotalTerransPorSegundo();
        offline.LoadProduccionOffline();
        settingsManager.StartSettings();
        Metodos.OpcionesNotacion = data.tipoNotacion;
        
    }

    // Update is called once per frame
    void Update()
    {
        logros.IniciarLogros();
        prestigio.Run();
        superNova.Run();
        automatizador.Run();

        if (ventanaMejorasGrupo.gameObject.activeSelf)
        {
            mejoras.EmpezarMejorasUI();
        }

        mejoras.EmpezarMejoras();

        if (data.musicaFondo)
        {
            soundManager.gameObject.SetActive(true);
            musicaFondoIcono.color = Color.green;
        }
        else if (!data.musicaFondo)
        {
            soundManager.gameObject.SetActive(false);
            musicaFondoIcono.color = Color.red;
        }

        //Barras de progreso
        Metodos.NumeroSuave(ref terransTemporal, data.terrans);


        if (ventanaPrincipalGrupo.gameObject.activeSelf)
        {
            textoTerransClick.text = "Click \n" +
                                      Metodos.MetodoNotacion((ValorClickTotal()), "F0") +
                                      " Terrans";
        }

        textoTerrans.text = "Terrans: " + Metodos.MetodoNotacion(data.terrans, "F0");


        textoTerransPorSegundo.text = " Terrans/s " + Metodos.MetodoNotacion(ValorTotalTerransPorSegundo(), "F0");


        string ComprarTodoFormato(BigDouble x)
        {
            return $"Comprar Todo ({x})";
        }

        //Update de terrans
        data.terrans += ValorTotalTerransPorSegundo() * Time.deltaTime;
        data.terransTotales += ValorTotalTerransPorSegundo() * Time.deltaTime;


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

    //Prestigio 
    public BigDouble MejoraTotal()
    {
        BigDouble aux = prestigio.ValorTotalBitcoinsMejora();
        aux *= eventos.tokensEventoMejora;
        return aux;
    }


    public BigDouble ValorTotalTerransPorSegundo()
    {
        BigDouble aux = 0;

        aux += data.produccionMejora1Nivel;
        aux += data.produccionMejora2Poder * data.produccionMejora2Nivel;
        aux *= MejoraTotal();
        aux *= eventos.tokensEventoMejora;
        aux *= Pow(1.1, prestigio.niveles[1]);
        aux *= planetas.TMMejora;

        return aux;
    }

    private BigDouble ValorClickTotal()
    {
        var aux = data.terransClickValor;

        aux *= eventos.tokensEventoMejora;
        aux *= MejoraTotal();
        aux *= Pow(1.5, prestigio.niveles[0]);

        return aux;
    }

    public void Click()
    {
        data.terrans += ValorClickTotal();
        data.terransTotales += ValorClickTotal();
    }

    public void PopUps(string id)
    {
        switch (id)
        {
            case "MenuJuego":
                ventanaMenuJuegoGrupo.gameObject.SetActive(!ventanaMenuJuegoGrupo.gameObject.activeSelf);
                break;
            
            case "MenuUsuario":
                ventanaMenuUsuarioGrupo.gameObject.SetActive(!ventanaMenuUsuarioGrupo.gameObject.activeSelf);
                break;
        }
    }

    public void CambiaVentanas(string id)
    {
        DesactivarTodo();
        switch (id)
        {
            case "VentanaTierra":
                ventanaTierraGrupo.gameObject.SetActive(true);
                break;

            case "Principal":
                ventanaPrincipalGrupo.gameObject.SetActive(true);
                break;

            case "Cabecera":
                ventanacabeceraGrupo.gameObject.SetActive(true);
                break;

            case "Mejoras":
                ventanaMejorasGrupo.gameObject.SetActive(true);
                break;

            case "Logros":
                ventanaLogrosGrupo.gameObject.SetActive(true);
                break;

            case "Opciones":
                ventanaTierraGrupo.gameObject.SetActive(false);
                ventanaOpcionesGrupo.gameObject.SetActive(true);
                break;

            case "VolverOpciones":
                ventanaOpcionesGrupo.gameObject.SetActive(false);
                ventanaTierraGrupo.gameObject.SetActive(true);
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

            case "Automatico":
                ventanaAutomaticoGrupo.gameObject.SetActive(true);
                break;

            case "Planetas":
                ventanaPlanetasGrupo.gameObject.SetActive(true);
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
            ventanaAutomaticoGrupo.gameObject.SetActive(false);
            ventanaPlanetasGrupo.gameObject.SetActive(false);
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
        if (data.musicaFondo)
        {
            soundManager.gameObject.SetActive(false);
            data.musicaFondo = false;
            musicaFondoIcono.color = Color.red;
        }
        else if (!data.musicaFondo)
        {
            soundManager.gameObject.SetActive(true);
            data.musicaFondo = true;
            musicaFondoIcono.color = Color.green;
        }
    }

    public void FullReset()
    {
        data.FullReset();
        CambiaVentanas("VentanaTierra");
    }
}