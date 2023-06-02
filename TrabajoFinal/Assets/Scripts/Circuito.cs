using System;
using System.Collections.Generic;
using UnityEngine;

public class Circuito
{
    private List<Seccion> secciones;

    public Circuito(List<Punto> puntos)
    {
        secciones = partirEnSecciones(puntos);
    }

    public List<Seccion> getSecciones()
    {
        return secciones;
    }

    /// <summary>
    /// Este m�todo se encarga de identificar en qu� secci�n se encuentra la posici�n pasada por par�metro.
    /// </summary>
    /// <param name="position"> El vector que representa la posici�n de la cual se quiere identificar la secci�n</param>
    /// <returns>El n�mero que simboliza la secci�n en la que se encuentra, o -1 si no se ha encontrado la secci�n a la que pertenece</returns>
    public int descubrirSeccionActual(Vector3 position)
    {
        Vector2 punto = new Vector2(position.x, position.z);
        int i = 0;
        int found = -1;

        foreach (Seccion seccion in secciones)
        {
            if (seccion.perteneceASeccion(punto))
            {
                found = i;
                break;
            }
            i++;
        }

        return found;
    }

    /// <summary>
    /// Este m�todo se encarga de partir en secciones una lista de puntos seg�n el �ngulo que forman con los puntos consecutivos.
    /// </summary>
    /// <param name="puntos">La lista de puntos a partir</param>
    /// <returns>Lista de secciones</returns>
    private List<Seccion> partirEnSecciones(List<Punto> puntos)
    {
        List<Seccion> secciones = new List<Seccion>();

        int puntosSize = puntos.Count;

        Punto puntoAnterior;
        Punto puntoActual;
        Punto puntoSiguiente;

        //Iniciamos una secci�n:

        List<Punto> puntosSeccion = new List<Punto>();
        puntosSeccion.Add(puntos[0]);
        puntosSeccion.Add(puntos[1]);
        float anguloGiroSeccion = Punto.CalcularAngulo(puntos[0], puntos[1], puntos[2]);

        for (int i = 2; i < puntosSize - 1; i++)
        {
            puntoAnterior = puntos[i - 1];
            puntoActual = puntos[i];
            puntoSiguiente = puntos[i + 1];

            float anguloActual = Punto.CalcularAngulo(puntoAnterior, puntoActual, puntoSiguiente);
            //print(anguloActual);
            //Comprobamos que el �ngulo de giro sea el �ngulo de giro de la secci�n + - 10�
            if (anguloGiroSeccion - 10 <= anguloActual && anguloGiroSeccion + 10 >= anguloActual)
            {
                //Si est� dentro de la secci�n, a�adimos el punto a los puntos de la secci�n y recalculamos el �ngulo de giro:
                puntosSeccion.Add(puntoActual);

                if (puntosSeccion.Count >= 3)
                {
                    anguloGiroSeccion = Punto.CalcularAnguloMedio(puntosSeccion);
                }
            }
            else
            {
                //Si no pertenece a la secci�n, guardamos la secci�n anterior y creamos una nueva secci�n.
                secciones.Add(new Seccion(puntosSeccion, anguloGiroSeccion));

                puntosSeccion = new List<Punto>();
                puntosSeccion.Add(puntoActual);
                anguloGiroSeccion = anguloActual;
            }
        }
        //Guardamos el �ltimo punto (en principio lo guardo en la �ltima secci�n)
        puntosSeccion.Add(puntos[puntosSize - 1]);
        //Guardamos la �ltima secci�n
        secciones.Add(new Seccion(puntosSeccion, anguloGiroSeccion));

        return secciones;
    }


}


