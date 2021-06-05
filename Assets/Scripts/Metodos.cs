using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;
using static BreakInfinity.BigDouble;

public class Metodos : MonoBehaviour
{
    public static int OpcionesNotacion;

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
        if (x <= 1000) return x.ToString(y);
        switch (OpcionesNotacion)
        {
            case 0:
            {
                var exponente = Floor(Log10(Abs(x)));
                var mantissa = x / Pow(10, exponente);
                return mantissa.ToString("F2") + "e" + exponente;
            }

            case 1:
            {
                var exponente = 3 * Floor(Floor(Log10(x)) / 3);
                var mantissa = x / Pow(10, exponente);
                return mantissa.ToString("F2") + "e" + exponente;
            }

            case 2:
            {
                var exponente = Floor(Log10(x));
                var tercerExponente = 3 * Floor(exponente / 3);
                var mantissa = x / Pow(10, tercerExponente);

                if (x <= 1000) return x.ToString("F2");
                if (x >= 1e93) return mantissa.ToString("F2") + "e" + tercerExponente;
                return mantissa.ToString("F2") + prefijos[(int)tercerExponente];
            }
        }

        return "";
    }

    private static readonly Dictionary<int, string> prefijos = new Dictionary<int, string>
    {
        {3, " K"},
        {6, " M"},
        {9, " B"},
        {12, " T"},
        {15, " QA"},
        {18, " QI"},
        {21, " S"},
        {24, " Sp"},
        {27, " O"},
        {30, " N"},
        {33, " D"},
        {36, " UD"},
        {39, " DD"},
        {42, " TD"},
        {45, " QAD"},
        {48, " QID"},
        {51, " SD"},
        {54, " SPD"},
        {57, " OD"},
        {60, " ND"},
        {63, " V"},
        {66, " UV"},
        {69, " DV"},
        {72, " TV"},
        {75, " QAV"},
        {78, " QIV"},
        {81, " SV"},
        {84, " SPV"},
        {87, " OV"},
        {90, " NV"}
    };

    public static void CompraMax(ref BigDouble c, BigDouble b, BigDouble r, ref int k)
    {
        var n = Floor(Log(c * (r - 1) / (b * Pow(r, (BigDouble) k)) + 1, r));

        var coste = b * (Pow(r, (BigDouble) k) * (Pow(r, n) - 1) / (r - 1));

        if (c >= coste)
        {
            k += (int) n;
            c -= coste;
        }
    }
}