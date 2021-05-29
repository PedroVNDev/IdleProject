using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;
using static BreakInfinity.BigDouble;

public class PlanetManager : MonoBehaviour
{
    public IdleManager juego;
    public Canvas Tierra;
    public Canvas Marte;

    public Text textoTerrans;
    public Text textoMarshalls;
    public Text textoTMMejora;

    public BigDouble TMMejora => Log(Sqrt(juego.data.marshalls) + 1, 20) + 1;

    public void Update()
    {
        var data = juego.data;

        textoTerrans.text = $"{Metodos.MetodoNotacion(data.recursos, "F2")} Terrans";
        textoMarshalls.text = $"{Metodos.MetodoNotacion(data.marshalls, "F2")} Marshalls";
        textoTMMejora.text = $"{Metodos.MetodoNotacion(TMMejora, "F2")}x Mejora";
    }

    public void CambiaVentanas(string id)
    {
        DesactivarTodo();
        switch (id)
        {
            case "Tierra":
                Tierra.gameObject.SetActive(true);
                juego.ventanaPrincipalGrupo.gameObject.SetActive(true);
                break;
            case "Marte":
                Marte.gameObject.SetActive(true);
                break;

        }

        void DesactivarTodo()
        {
            Tierra.gameObject.SetActive(false);
            Marte.gameObject.SetActive(false);
            juego.ventanaPlanetasGrupo.gameObject.SetActive(false);
        }
    }
}