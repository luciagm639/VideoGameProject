using System.Collections.Generic;

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

    public void setInicioSeccion(Punto inicioSeccion) { this.inicioSeccion = inicioSeccion; }

    public void setPuntoMedioSeccion(Punto puntoMedioSeccion) { this.puntoMedioSeccion = puntoMedioSeccion; }

    public void setFinalSeccion(Punto finalSeccion) { this.finalSeccion = finalSeccion; }

    public void setAnguloGiro(float anguloGiro) { this.anguloGiro = anguloGiro; }

    public override string ToString()
    {
        string ret = "Sección = {"
            + " \n Punto inicial:  " + getInicioSeccion().ToString()
            + " \n Punto medio:    " + getPuntoMedioSeccion().ToString()
            + " \n Punto final:    " + getFinalSeccion().ToString()
            + " \n Ángulo de giro: " + getAnguloGiro() + " }";
        return ret;
    }
}
