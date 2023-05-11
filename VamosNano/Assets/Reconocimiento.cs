using com.sun.tools.@internal.ws.wsdl.document;
using java.io;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Reconocimiento : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print(Punto.CalcularAngulo(new Punto(0,0), new Punto (0,1), new Punto(1,0)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void partirEnSecciones(List<Punto> puntos)
    {
        int puntosSize = puntos.Count;

        float anguloGiro = 0.0f;


        for (int i = 0; i < puntosSize; i++)
        {

        }
    }


}


