using com.sun.tools.@internal.ws.wsdl.document;
using java.io;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Reconocimiento : MonoBehaviour
{

    private bool girado = false;
    // Start is called before the first frame update
    void Start()
    {
        List<Punto> puntos = new List<Punto>();
        puntos.Add(new Punto(0, 0));
        puntos.Add(new Punto(0, 0.64f));
        puntos.Add(new Punto(0, 1.3f));
        puntos.Add(new Punto(0, 2));
        puntos.Add(new Punto(0.68f, 2.66f));
        puntos.Add(new Punto(1.64f, 3));
        puntos.Add(new Punto(2.82f, 3.14f));
        puntos.Add(new Punto(4, 3));
        puntos.Add(new Punto(4.86f, 2.52f));
        puntos.Add(new Punto(5.52f, 1.98f));
        puntos.Add(new Punto(6.18f, 1.5f));
        puntos.Add(new Punto(7f, 1f));
        puntos.Add(new Punto(7.62f, 1));
        puntos.Add(new Punto(7.33f, -0.95f));
        puntos.Add(new Punto(6.23f, -1.15f));
        puntos.Add(new Punto(5.77f, -0.41f));
        puntos.Add(new Punto(5.11f, 0.47f));
        puntos.Add(new Punto(4.04f, 1.19f));
        puntos.Add(new Punto(3f, 2f));
        puntos.Add(new Punto(2.12f, 2.15f));
        puntos.Add(new Punto(1.63f, 1.49f));
        puntos.Add(new Punto(2.13f, 0.85f));
        puntos.Add(new Punto(2.19f, 0f));
        puntos.Add(new Punto(2.25f, -0.95f));
        List<Seccion> list = partirEnSecciones(puntos);
        int size = list.Count;
        for (int i = 0; i < size; i++)
        {
            print(list[i].ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(400,0,-1000);

    }

    List<Seccion> partirEnSecciones(List<Punto> puntos)
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


