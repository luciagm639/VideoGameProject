using java.io;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using weka.classifiers.trees;
using weka.core;
using weka.core.converters;

public class EntrenamientoCoche : MonoBehaviour
{
    M5P saberPredecirMagnitud;
    Instances casosEntrenamiento;
    private string ESTADO = "Sin conocimiento";
    float mejorMagnitud;
    public float valorMaximoMagnitud, pasoMagnitud;                                           //Es un ejemplo: Se asume que este valor es extremo para ese problema
    Rigidbody r;

    public Seccion seccionActual;

    private bool haTocadoElSuelo= false;

    void Start()
    {
        //texto = Canvas.FindObjectOfType<Text>();
        if (ESTADO == "Sin conocimiento") StartCoroutine("Entrenamiento");          //Lanza el proceso de entrenamiento                                          

    }

    public static Vector2 RotateVector(Vector2 vector, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        float x = vector.x * cos - vector.y * sin;
        float y = vector.x * sin + vector.y * cos;

        return new Vector2(x, y);
    }

    IEnumerator Entrenamiento()
    {
        casosEntrenamiento = new Instances(new FileReader("Entrenamiento/Experiencias.arff"));  //Lee fichero con las variables y experiencias

        if (casosEntrenamiento.numInstances() < 10)
            for (float magnitudFuerza = 1; magnitudFuerza <= valorMaximoMagnitud; magnitudFuerza = magnitudFuerza + pasoMagnitud)               //BUCLE de planificación de la fuerza FX durante el entrenamiento
            {
                transform.position = new Vector3(seccionActual.getInicioSeccion().getCoordenadaX(), transform.position.y, seccionActual.getInicioSeccion().getCoordenadaZ());

                if (haTocadoElSuelo) { haTocadoElSuelo = false; } //hago reset de la variable

                Rigidbody rb = GetComponent<Rigidbody>();

                Vector2 aDondeVoy = new Vector2(transform.forward.x, transform.forward.z); // Obtengo hacia dónde está mirando el coche
                aDondeVoy = RotateVector(aDondeVoy, seccionActual.getAnguloGiro()); // Giro el vector los grados que me indica la sección
                aDondeVoy *= magnitudFuerza; // Aplico la fuerza que se está probando

                rb.AddForce(new Vector3(aDondeVoy.x, 0, aDondeVoy.y), ForceMode.Force); //Add force con la fuerza que se está probando
                yield return new WaitUntil(() => (!seccionActual.perteneceASeccion(transform.position))|| haTocadoElSuelo);
                
                Instance casoAaprender = new Instance(casosEntrenamiento.numAttributes());
                casoAaprender.setDataset(casosEntrenamiento);                           //crea un registro de experiencia
                casoAaprender.setValue(0, magnitudFuerza);                              //guarda el dato de la fuerza utilizada
                if (haTocadoElSuelo)
                {
                    casoAaprender.setValue(1, 0); //Se ha caido
                }
                casoAaprender.setValue(1, 1);      //No se ha caido                
                casosEntrenamiento.add(casoAaprender);                                  //guarda el registro de experiencia             

            }                                                                           //FIN bucle de lanzamientos con diferentes de fuerzas
        //APRENDIZADE CONOCIMIENTO:  
        saberPredecirMagnitud = new M5P();                                            //crea un algoritmo de aprendizaje M5P (árboles de regresión)
        casosEntrenamiento.setClassIndex(0);                                        //la variable a aprender será la magnitud (id=0) dado que no se ha caido
        saberPredecirMagnitud.buildClassifier(casosEntrenamiento);                    //REALIZA EL APRENDIZAJE DE FX A PARTIR DE LAS EXPERIENCIAS

        File salida = new File("Entrenamiento/Finales_Experiencias.arff");
        if (!salida.exists())
            System.IO.File.Create(salida.getAbsoluteFile().toString()).Dispose();
        ArffSaver saver = new ArffSaver();
        saver.setInstances(casosEntrenamiento);
        saver.setFile(salida);
        saver.writeBatch();

        //EVALUACION DEL CONOCIMIENTO APRENDIDO: 
        print("intancias=" + casosEntrenamiento.numInstances());
        /*
        if (casosEntrenamiento.numInstances() >= 10)
        {
            Evaluation evaluador = new Evaluation(casosEntrenamiento);                   //...Opcional: si tien mas de 10 ejemplo, estima la posible precisión
            evaluador.crossValidateModel(saberPredecirFuerzaX, casosEntrenamiento, 10, new java.util.Random(1));
            print("El Error Absoluto Promedio durante el entrenamiento fue de " + evaluador.meanAbsoluteError().ToString("0.000000") + " N");
        }*/

        ESTADO = "Con conocimiento";

    }

    void FixedUpdate()                                                                    //Aplica conocimiento aprendido
    {
        if ((ESTADO == "Con conocimiento"))
        {
            Instance casoPrueba = new Instance(casosEntrenamiento.numAttributes());  //Crea un registro de experiencia durante el juego
            casoPrueba.setDataset(casosEntrenamiento);
            casoPrueba.setValue(1, 1);                               //indica que no se quiere salir de la carretera

            mejorMagnitud = (float)saberPredecirMagnitud.classifyInstance(casoPrueba);  //predice la magnitud dado que no se sale de la carretera

            Vector2 aDondeVoy = new Vector2(transform.forward.x, transform.forward.z); // Obtengo hacia dónde está mirando el coche
            aDondeVoy = RotateVector(aDondeVoy, seccionActual.getAnguloGiro()); // Giro el vector los grados que me indica la sección
            aDondeVoy *= mejorMagnitud; // Aplico la fuerza que me ha devuelto M5P

            r.AddForce(new Vector3(aDondeVoy.x, 0, aDondeVoy.y), ForceMode.Force);          //Se aplica la fuerza

            ESTADO = "FIN";

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Acciones a realizar cuando ocurre una colisión
        if(collision.gameObject.tag == "fueraDeLimites")
        {
            haTocadoElSuelo = true;
        }
    }
}
