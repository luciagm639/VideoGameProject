using System.Collections.Generic;
using UnityEngine;

public class Seccion
{
    private Punto inicioSeccion;
    private Punto puntoMedioSeccion;
    private Punto finalSeccion;

    private float anguloGiro;

    /// <summary>
    /// Constructor de una sección.
    /// </summary>
    /// <param name="inicioSeccion">Punto que representa el inicio de sección</param>
    /// <param name="puntoMedioSeccion">Punto que representa el punto medio de la sección</param>
    /// <param name="finalSeccion">Punto que representa el final de la sección</param>
    /// <param name="anguloGiro">Ángulo de giro de la sección</param>
    public Seccion(Punto inicioSeccion, Punto puntoMedioSeccion, Punto finalSeccion, float anguloGiro)
    {
        this.inicioSeccion = inicioSeccion;
        this.puntoMedioSeccion = puntoMedioSeccion;
        this.finalSeccion = finalSeccion;
        this.anguloGiro = anguloGiro;
    }

    /// <summary>
    /// Constructor de una sección.
    /// </summary>
    /// <param name="puntos">Lista de puntos. El primer elemento será el inicio de la sección, el último elemento será el final de la sección,
    /// el elemento de la mitad será el punto medio de la sección.</param>
    /// <param name="anguloGiro">El ángulo de la sección.</param>
    public Seccion(List<Punto> puntos, float anguloGiro)
    {
        int puntosSize = puntos.Count;
        this.inicioSeccion = puntos[0];
        this.finalSeccion = puntos[puntosSize-1];
        int middlePuntos = puntosSize / 2;
        this.puntoMedioSeccion = puntos[middlePuntos];

        this.anguloGiro = anguloGiro;
    }

    public Punto getInicioSeccion() { return inicioSeccion; }

    public Punto getFinalSeccion() { return finalSeccion; }

    public Punto getPuntoMedioSeccion() { return puntoMedioSeccion; }

    public float getAnguloGiro() { return anguloGiro; }

    public override string ToString()
    {
        string ret = "Sección = {"
            + " \n Punto inicial:  " + getInicioSeccion().ToString()
            + " \n Punto medio:    " + getPuntoMedioSeccion().ToString()
            + " \n Punto final:    " + getFinalSeccion().ToString()
            + " \n Ángulo de giro: " + getAnguloGiro() + " }";
        return ret;
    }

    public bool perteneceASeccion(Vector2 punto)
    {
        Vector2 inicioSeccionV = new Vector2(inicioSeccion.getCoordenadaX(), inicioSeccion.getCoordenadaZ());
        Vector2 puntoMedioSeccionV = new Vector2(puntoMedioSeccion.getCoordenadaX(), puntoMedioSeccion.getCoordenadaZ());
        Vector2 finalSeccionV = new Vector2(finalSeccion.getCoordenadaX(), finalSeccion.getCoordenadaZ());

        Vector2 parallelEnd1;
        Vector2 parallelEnd2;

        GetParallelLines(inicioSeccionV, puntoMedioSeccionV, out parallelEnd1, out parallelEnd2);

        if(IsPointInParallelSection(punto, inicioSeccionV, parallelEnd1, puntoMedioSeccionV, parallelEnd2))
        {
            return true;
        }else
        {
            GetParallelLines(puntoMedioSeccionV, finalSeccionV, out parallelEnd1, out parallelEnd2);

            if (IsPointInParallelSection(punto, puntoMedioSeccionV, parallelEnd1, finalSeccionV, parallelEnd2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public static void GetParallelLines(Vector2 point1, Vector2 point2, out Vector2 parallelEnd1, out Vector2 parallelEnd2)
    {
        Vector2 offset = (point2 - point1);
        Vector2 lineDirection = new Vector2(-offset.y, offset.x).normalized;

        parallelEnd1 = point1 + lineDirection;
        parallelEnd2 = point2 + lineDirection;
    }


    public static bool IsPointInParallelSection(Vector2 point, Vector2 rectStart1, Vector2 rectEnd1, Vector2 rectStart2, Vector2 rectEnd2)
    {

        float crossProduct1 = (point.x - rectStart1.x) * (rectEnd1.y - rectStart1.y) - (point.y - rectStart1.y) * (rectEnd1.x - rectStart1.x);
        float crossProduct2 = (point.x - rectStart2.x) * (rectEnd2.y - rectStart2.y) - (point.y - rectStart2.y) * (rectEnd2.x - rectStart2.x);

        return (crossProduct1 >= 0 && crossProduct2 <= 0) || (crossProduct1 <= 0 && crossProduct2 >= 0);
    }

}
