using System.Collections.Generic;

public class Seccion
{
    private Punto inicioSeccion;
    private Punto puntoMedioSeccion;
    private Punto finalSeccion;

    private float anguloGiro;

    /// <summary>
    /// Constructor de una secci�n.
    /// </summary>
    /// <param name="inicioSeccion">Punto que representa el inicio de secci�n</param>
    /// <param name="puntoMedioSeccion">Punto que representa el punto medio de la secci�n</param>
    /// <param name="finalSeccion">Punto que representa el final de la secci�n</param>
    /// <param name="anguloGiro">�ngulo de giro de la secci�n</param>
    public Seccion(Punto inicioSeccion, Punto puntoMedioSeccion, Punto finalSeccion, float anguloGiro)
    {
        this.inicioSeccion = inicioSeccion;
        this.puntoMedioSeccion = puntoMedioSeccion;
        this.finalSeccion = finalSeccion;
        this.anguloGiro = anguloGiro;
    }

    /// <summary>
    /// Constructor de una secci�n.
    /// </summary>
    /// <param name="puntos">Lista de puntos. El primer elemento ser� el inicio de la secci�n, el �ltimo elemento ser� el final de la secci�n,
    /// el elemento de la mitad ser� el punto medio de la secci�n.</param>
    /// <param name="anguloGiro">El �ngulo de la secci�n.</param>
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

    public void setInicioSeccion(Punto inicioSeccion) { this.inicioSeccion = inicioSeccion; }

    public void setPuntoMedioSeccion(Punto puntoMedioSeccion) { this.puntoMedioSeccion = puntoMedioSeccion; }

    public void setFinalSeccion(Punto finalSeccion) { this.finalSeccion = finalSeccion; }

    public void setAnguloGiro(float anguloGiro) { this.anguloGiro = anguloGiro; }

    public override string ToString()
    {
        string ret = "Secci�n = {"
            + " \n Punto inicial:  " + getInicioSeccion().ToString()
            + " \n Punto medio:    " + getPuntoMedioSeccion().ToString()
            + " \n Punto final:    " + getFinalSeccion().ToString()
            + " \n �ngulo de giro: " + getAnguloGiro() + " }";
        return ret;
    }
}
