using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;
using static BreakInfinity.BigDouble;

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

    public static string MetodoNotacion(BigDouble x, string y)
    {
        if (x > 1000)
        {
            var exponente = Floor(Log10(Abs(x)));
            var mantissa = x / Pow(10, exponente);
            return mantissa.ToString("F2") + "e" + exponente;
        }

        return x.ToString(y);
    }
}