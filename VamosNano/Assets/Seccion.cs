using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seccion
{
    private Punto inicioSeccion;
    private Punto puntoMedioSeccion;
    private Punto finalSeccion;

    public Seccion(Punto inicioSeccion, Punto puntoMedioSeccion, Punto finalSeccion)
    {
        this.inicioSeccion = inicioSeccion;
        this.puntoMedioSeccion = puntoMedioSeccion;
        this.finalSeccion = finalSeccion;
    }

    public Punto getInicioSeccion() { return inicioSeccion; }

    public Punto getFinalSeccion() { return finalSeccion; }

    public Punto getPuntoMedioSeccion() { return puntoMedioSeccion; }

    public void setInicioSeccion(Punto inicioSeccion)
    {
        this.inicioSeccion = inicioSeccion;
    }

    public void setPuntoMedioSeccion (Punto puntoMedioSeccion)
    {
        this.puntoMedioSeccion = puntoMedioSeccion;
    }

    public void setFinalSeccion (Punto finalSeccion)
    {
        this.finalSeccion = finalSeccion;
    }
}
