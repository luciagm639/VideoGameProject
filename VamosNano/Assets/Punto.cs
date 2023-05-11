using com.sun.tools.javac.util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punto
{
    float coordenadaX;
    float coordenadaZ;

    public Punto(float coordenadaX, float coordenadaZ)
    {
        this.coordenadaX = coordenadaX;
        this.coordenadaZ = coordenadaZ;
    }

    public float getCoordenadaX()
    {
        return coordenadaX;
    }

    public float getCoordenadaZ()
    {
        return coordenadaZ;
    }

    public void setCoordenadaX(float coordenadaX)
    {
        this.coordenadaX = coordenadaX;
    }

    public void setCoordenadaZ(float coordenadaZ)
    {
        this.coordenadaZ = coordenadaZ;
    }

    public static double CalcularAngulo(Punto punto1, Punto punto2, Punto punto3)
    {
        double x1 = punto1.getCoordenadaX();
        double y1 = punto1.getCoordenadaZ();

        double x2 = punto2.getCoordenadaX();
        double y2 = punto2.getCoordenadaZ();

        double x3 = punto3.getCoordenadaX();
        double y3 = punto3.getCoordenadaZ();

        double a = Math.Sqrt(Math.Pow(x2 - x3, 2) + Math.Pow(y2 - y3, 2));
        double b = Math.Sqrt(Math.Pow(x1 - x3, 2) + Math.Pow(y1 - y3, 2));
        double c = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        double angulo = Math.Acos((a * a + b * b - c * c) / (2 * a * b));
        return angulo * (180 / Math.PI);
    }

}
