
using java.io;
using org.ietf.jgss;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using weka.classifiers;
using weka.classifiers.functions;
using weka.classifiers.trees;
using weka.core;
using weka.core.converters;

public class EntrenamientoCoche : MonoBehaviour
{
    MultilayerPerceptron saberPredecirMagnitud;

    Instances casosEntrenamiento;

    private string ESTADO = "Sin conocimiento";

    float mejorMagnitud;

    public float valorMaximoMagnitud, pasoMagnitud;                                           //Es un ejemplo: Se asume que este valor es extremo para ese problema

    Rigidbody rb;

    public Seccion seccionActual;

    private bool haTocadoElSuelo = false;

    private Circuito circuito;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (ESTADO == "Sin conocimiento") StartCoroutine("Entrenamiento");          //Lanza el proceso de entrenamiento                                          
    }

    public void setCircuito(Circuito secciones)
    {
        this.circuito = secciones;
    }

    IEnumerator Entrenamiento()
    {
        yield return new WaitUntil(() => (circuito != null && circuito.getSecciones()!= null && circuito.getSecciones().Count > 0));

        int numSeccion = 0;
        string nombreFichero;

        foreach (Seccion seccion in circuito.getSecciones())
        {
            numSeccion++;
            nombreFichero = "Assets/Entrenamiento/ExperienciasSeccion" + numSeccion + ".arff";

            if (!System.IO.File.Exists(nombreFichero))
            {
                crearFichero(numSeccion);
            }

            casosEntrenamiento = new Instances(new FileReader(nombreFichero));

            if (casosEntrenamiento.numInstances() < 10)
            {
                for (float magnitudFuerza = 1; magnitudFuerza <= valorMaximoMagnitud; magnitudFuerza += pasoMagnitud)
                {
                    transform.position = new Vector3(seccion.getInicioSeccion().getCoordenadaX(), transform.position.y, seccion.getInicioSeccion().getCoordenadaZ());
                    haTocadoElSuelo = false;

                    aplicarFuerza(seccion, magnitudFuerza);

                    yield return new WaitUntil(() => (!seccion.perteneceASeccion(transform.position)) || haTocadoElSuelo);

                    Instance casoAaprender = crearCasoAAprender(
                                                                seccion.getInicioSeccion().getCoordenadaX(),
                                                                seccion.getInicioSeccion().getCoordenadaZ(),
                                                                seccion.getAnguloGiro(),
                                                                magnitudFuerza,
                                                                haTocadoElSuelo);
                    casosEntrenamiento.add(casoAaprender);                                  //guarda el registro de experiencia             

                } //FIN bucle de lanzamientos con diferentes de fuerzas
            }
        }

        //APRENDIZADE CONOCIMIENTO:  
        saberPredecirMagnitud = new MultilayerPerceptron();
        casosEntrenamiento.setClassIndex(3);                                        //la variable a aprender será la magnitud (id=3) dado que no se ha caido
        saberPredecirMagnitud.buildClassifier(casosEntrenamiento);                    //REALIZA EL APRENDIZAJE DE FX A PARTIR DE LAS EXPERIENCIAS


        crearSalida(casosEntrenamiento);

        //EVALUACION DEL CONOCIMIENTO APRENDIDO: 
        print("intancias=" + casosEntrenamiento.numInstances());
        if (casosEntrenamiento.numInstances() >= 10)
        {
            Evaluation evaluador = new Evaluation(casosEntrenamiento);                   //...Opcional: si tien mas de 10 ejemplo, estima la posible precisión
            evaluador.crossValidateModel(saberPredecirMagnitud, casosEntrenamiento, 10, new java.util.Random(1));
            print("El Error Absoluto Promedio durante el entrenamiento fue de " + evaluador.meanAbsoluteError().ToString("0.000000") + " N");
        }

        ESTADO = "Con conocimiento";

    }

    private static Vector2 RotateVector(Vector2 vector, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        float x = vector.x * cos - vector.y * sin;
        float y = vector.x * sin + vector.y * cos;

        return new Vector2(x, y);
    }

    private void crearFichero(int numSeccion)
    {
        string rutaDelArchivo = "Assets/Entrenamiento/ExperienciasSeccion" + numSeccion + ".arff";

        // Utilizamos un bloque 'using' para asegurarnos de que el StreamWriter se cierre correctamente
        using (StreamWriter escritor = new StreamWriter(rutaDelArchivo))
        {
            // Escribir el contenido en el archivo
            escritor.WriteLine("@relation ConduccionSeccion" + numSeccion);

            escritor.WriteLine("@attribute posicionX numeric");
            escritor.WriteLine("@attribute posicionZ numeric");
            escritor.WriteLine("@attribute anguloGiro numeric");
            escritor.WriteLine("@attribute magnitudFuerza numeric");
            escritor.WriteLine("@attribute resultado { true, false }");
            escritor.WriteLine("@data");
        }
    }

    private void aplicarFuerza(Seccion seccion, float magnitudFuerza)
    {
        Vector2 aDondeVoy = new Vector2(transform.forward.x, transform.forward.z); // Obtengo hacia dónde está mirando el coche
        aDondeVoy = RotateVector(aDondeVoy, seccion.getAnguloGiro()); // Giro el vector los grados que me indica la sección
        aDondeVoy *= magnitudFuerza; // Aplico la fuerza que se está probando

        rb.AddForce(new Vector3(aDondeVoy.x, 0, aDondeVoy.y), ForceMode.Force); //Add force con la fuerza que se está probando
    }

    private Instance crearCasoAAprender(float posicionX, float posicionZ, float anguloGiro, float magnitudFuerza, bool resultado)
    {
        Instance casoAaprender = new Instance(casosEntrenamiento.numAttributes());
        casoAaprender.setDataset(casosEntrenamiento);                           //crea un registro de experiencia
        casoAaprender.setValue(0, posicionX);
        casoAaprender.setValue(1, posicionZ);
        casoAaprender.setValue(2, anguloGiro);
        casoAaprender.setValue(3, magnitudFuerza);
        casoAaprender.setValue(4, resultado ? "true" : "false");

        return casoAaprender;
    }

    private void crearSalida(Instances casosEntrenamiento)
    {
        java.io.File salida = new java.io.File("Entrenamiento/Finales_Experiencias.arff");

        if (!salida.exists())
            System.IO.File.Create(salida.getAbsoluteFile().toString()).Dispose();

        ArffSaver saver = new ArffSaver();
        saver.setInstances(casosEntrenamiento);
        saver.setFile(salida);
        saver.writeBatch();
    }

    void FixedUpdate()                                                                    //Aplica conocimiento aprendido
    {
        if ((ESTADO == "Con conocimiento"))
        {
            int indiceDeSeccion = circuito.descubrirSeccionActual(transform.position);
            if(indiceDeSeccion == -1)
            {
                throw new Exception("No se ha encontrado la sección");
            }

            string nombreFichero = "Assets/Entrenamiento/ExperienciasSeccion" + indiceDeSeccion + ".arff";
            casosEntrenamiento = new Instances(new FileReader(nombreFichero));

            Instance casoPrueba = new Instance(casosEntrenamiento.numAttributes());  //Crea un registro de experiencia durante el juego

            casoPrueba.setDataset(casosEntrenamiento);

            casoPrueba.setValue(4, "false");                               //indica que no se quiere salir de la carretera

            mejorMagnitud = (float)saberPredecirMagnitud.classifyInstance(casoPrueba);  //predice la magnitud dado que no se sale de la carretera

            aplicarFuerza(this.circuito.getSecciones()[indiceDeSeccion], mejorMagnitud);
            
            ESTADO = "FIN";
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Acciones a realizar cuando ocurre una colisión
        if (collision.gameObject.tag == "fueraDeLimites")
        {
            haTocadoElSuelo = true;
        }
    }
}
