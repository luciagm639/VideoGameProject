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
    /// Este método se encarga de identificar en qué sección se encuentra la posición pasada por parámetro.
    /// </summary>
    /// <param name="position"> El vector que representa la posición de la cual se quiere identificar la sección</param>
    /// <returns>El número que simboliza la sección en la que se encuentra, o -1 si no se ha encontrado la sección a la que pertenece</returns>
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
    /// Este método se encarga de partir en secciones una lista de puntos según el ángulo que forman con los puntos consecutivos.
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

        //Iniciamos una sección:

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
            //Comprobamos que el ángulo de giro sea el ángulo de giro de la sección + - 10º
            if (anguloGiroSeccion - 10 <= anguloActual && anguloGiroSeccion + 10 >= anguloActual)
            {
                //Si está dentro de la sección, añadimos el punto a los puntos de la sección y recalculamos el ángulo de giro:
                puntosSeccion.Add(puntoActual);

                if (puntosSeccion.Count >= 3)
                {
                    anguloGiroSeccion = Punto.CalcularAnguloMedio(puntosSeccion);
                }
            }
            else
            {
                //Si no pertenece a la sección, guardamos la sección anterior y creamos una nueva sección.
                secciones.Add(new Seccion(puntosSeccion, anguloGiroSeccion));

                puntosSeccion = new List<Punto>();
                puntosSeccion.Add(puntoActual);
                anguloGiroSeccion = anguloActual;
            }
        }
        //Guardamos el último punto (en principio lo guardo en la última sección)
        puntosSeccion.Add(puntos[puntosSize - 1]);
        //Guardamos la última sección
        secciones.Add(new Seccion(puntosSeccion, anguloGiroSeccion));

        return secciones;
    }


}


