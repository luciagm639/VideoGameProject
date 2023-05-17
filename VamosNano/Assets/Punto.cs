using System;
using System.Collections.Generic;

public class Punto
{
    private float coordenadaX;
    private float coordenadaZ;

    /// <summary>
    /// Constructor de Punto. Está representado en dos dimensiones, por lo que la coordenada 'Y' no se tiene en cuenta.
    /// </summary>
    /// <param name="coordenadaX">Coordenada en X</param>
    /// <param name="coordenadaZ">Coordenada en Z</param>
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

    /// <summary>
    /// Esta función calcula el ángulo entre tres puntos. Dicho ángulo es definido por los segmentos [punto1, punto2] y [punto2, punto3]
    /// </summary>
    /// <param name="punto1"> Primer punto </param>
    /// <param name="punto2"> Segundo punto </param>
    /// <param name="punto3"> Tercer punto</param>
    /// <returns> El float que representa el ángulo que forman los puntos</returns>
    public static float CalcularAngulo(Punto punto1, Punto punto2, Punto punto3)
    {
        float x1 = punto1.getCoordenadaX();
        float y1 = punto1.getCoordenadaZ();

        float x2 = punto2.getCoordenadaX();
        float y2 = punto2.getCoordenadaZ();

        float x3 = punto3.getCoordenadaX();
        float y3 = punto3.getCoordenadaZ();

        double a = Math.Sqrt(Math.Pow(x2 - x3, 2) + Math.Pow(y2 - y3, 2));
        double b = Math.Sqrt(Math.Pow(x1 - x3, 2) + Math.Pow(y1 - y3, 2));
        double c = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        double angulo;

        if (a == 0 || b == 0)
        {
            angulo = 0;
        }
        else
        {
            angulo = Math.Acos((a * a + b * b - c * c) / (2 * a * b));
        }

        return (float)((float)angulo * (180 / Math.PI));
    }

    /// <summary>
    /// Calcula el ángulo medio que forman los puntos de la lista de puntos pasadas como referencia.
    /// </summary>
    /// <param name="puntos"> La lista de puntos que se le pasa de referencia. Debe tener más de 3 elementos para que el ángulo se calcule bien.</param>
    /// <returns>El float que representa el ángulo medio de la lista, o 0 si la lista tiene menos de 3 elementos.</returns>
    /// <exception cref="ArgumentNullException"> Si la lista es nula.</exception>
    public static float CalcularAnguloMedio(List<Punto> puntos)
    {
        if (puntos == null)
        {
            throw new ArgumentNullException("No se puede calcular el ángulo medio de una lista nula");
        }
        if (puntos.Count < 3)
        {
            return 0;
        }
        int puntosSize = puntos.Count;
        float anguloGiro = 0;
        for (int i = 1; i < puntosSize - 1; i++)
        {
            anguloGiro += Punto.CalcularAngulo(puntos[i - 1], puntos[i], puntos[i + 1]);
        }
        return anguloGiro / (puntosSize - 2);
    }

    public override string ToString()
    {
        return "[" + getCoordenadaX() + " , " + getCoordenadaZ() + "]";
    }

}
