using UnityEngine;
using UnityEngine.UI;

public class PlanetManager : MonoBehaviour
{
    public IdleManager juego;
    public Canvas Tierra;
    public Canvas Marte;

    public Text textoTerrans;
    public Text textoMarshalls;
    public Text textoTMMejora;

    public void Update()
    {
        var data = juego.data;

        textoTerrans.text = $"{Metodos.MetodoNotacion(data.recursos, "F2")} Terrans";
        textoMarshalls.text = $"{Metodos.MetodoNotacion(data.marshalls, "F2")} Marshalls";
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