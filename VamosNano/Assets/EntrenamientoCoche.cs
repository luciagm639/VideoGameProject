using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using weka.classifiers.trees;
using weka.classifiers.evaluation;
using weka.core;
using java.io;
using java.lang;
using java.util;
using weka.classifiers.functions;
using weka.classifiers;
using weka.core.converters;

public class EntrenamientoCoche : MonoBehaviour
{
    M5P saberPredecirFuerzaX;
    Instances casosEntrenamiento;
    Text texto;
    private string ESTADO = "Sin conocimiento";
    public GameObject pelota;
    GameObject InstanciaPelota, PuntoObjetivo;
    float distanciaObjetivo, mejorFuerzaX;
    public float valorMaximoFx, pasoFx;
    float Fy_calculada, valorMaximoFy = 18;                                                //Es un ejemplo: Se asume que este valor es extremo para ese problema
    Rigidbody r;

    private float valorMaximoFz, pasoFz;

    float valor_calculada_por_metodo_simple(float valorMaximoFy)                        //Calcula una �Fy v�lida� usando alg�n m�todo simple.
    {
        float minimoValor = 1f;
        /*Ejemplo:*/
        float valorFactible = (minimoValor + valorMaximoFy) / 2f;                 //Por ejemplo, la fuerza media entre el m�nimo y el m�ximo.
        return valorFactible;
    }

    void Start()
    {
        Fy_calculada = valor_calculada_por_metodo_simple(valorMaximoFy);             //Se va a aprender Fx, hay que seleccionar Fy factible
        //texto = Canvas.FindObjectOfType<Text>();
        if (ESTADO == "Sin conocimiento") StartCoroutine("Entrenamiento");          //Lanza el proceso de entrenamiento                                          

    }

    IEnumerator Entrenamiento()
    {
        casosEntrenamiento = new Instances(new FileReader("Entrenamiento/Experiencias.arff"));  //Lee fichero con las variables y experiencias

        //texto.text = "ENTRENAMIENTO: crea una tabla con las Fx utilizadas y distancias alcanzadas (Fy calculada=" + Fy_calculada.ToString("0.00") + " N)";
        //print("Datos de entrada= Fy=" + Fy_calculada + " Fx variables de 1 a " + valorMaximoFx + "  " + ((valorMaximoFx == 0 || Fy_calculada == 0) ? " ERROR: alguna fuerza es siempre 0" : ""));
        if (casosEntrenamiento.numInstances() < 10)
            for (float Fx = 1; Fx <= valorMaximoFx; Fx = Fx + pasoFx)               //BUCLE de planificaci�n de la fuerza FX durante el entrenamiento
            {
                for (float Fz = 1; Fz <= valorMaximoFz; Fz = Fz + pasoFz)
                {
                    Rigidbody rb = GetComponent<Rigidbody>();               //Crea una pelota f�sica
                    rb.AddForce(new Vector3(Fx, 0, Fz), ForceMode.Force);  //y la lanza con esa fuerza Fx  (Fy se escoge en el Start())
                    yield return new WaitUntil(() => (rb.transform.position.y < 0)); /*INSERTAR METODO QUE COMPRUEBA QUE PERTENECE A LA SECCION*/        //... y espera a que la pelota llegue al suelo

                    Instance casoAaprender = new Instance(casosEntrenamiento.numAttributes());
                    print("con fuerzas:   Fy_fijo=" + Fy_calculada + "  y  Fx=" + Fx + "  se alcanz� una distancia de " + rb.transform.position.x);
                    casoAaprender.setDataset(casosEntrenamiento);                           //crea un registro de experiencia
                    casoAaprender.setValue(0, Fx);                                          //guarda el dato de la fuerza utilizada
                    casoAaprender.setValue(1, rb.transform.position.x);                     //anota la distancia alcanzada
                    casosEntrenamiento.add(casoAaprender);                                  //guarda el registro de experiencia 
                    rb.isKinematic = true; rb.GetComponent<Collider>().isTrigger = true;    //...opcional: paraliza la pelota
                    Destroy(InstanciaPelota, 1f);                                           //...opcional: destruye la pelota en 1 seg para que ver donde cay�.            
                }
            }                                                                           //FIN bucle de lanzamientos con diferentes de fuerzas
        //APRENDIZADE CONOCIMIENTO:  
        saberPredecirFuerzaX = new M5P();                                            //crea un algoritmo de aprendizaje M5P (�rboles de regresi�n)
        casosEntrenamiento.setClassIndex(0);                                        //la variable a aprender ser� la fuerza Fx (id=0) dada la distancia
        saberPredecirFuerzaX.buildClassifier(casosEntrenamiento);                    //REALIZA EL APRENDIZAJE DE FX A PARTIR DE LAS EXPERIENCIAS

        File salida = new File("Entrenamiento/Finales_Experiencias.arff");
        if (!salida.exists())
            System.IO.File.Create(salida.getAbsoluteFile().toString()).Dispose();
        ArffSaver saver = new ArffSaver();
        saver.setInstances(casosEntrenamiento);
        saver.setFile(salida);
        saver.writeBatch();

        //EVALUACION DEL CONOCIMIENTO APRENDIDO: 
        print("intancias=" + casosEntrenamiento.numInstances());
        if (casosEntrenamiento.numInstances() >= 10)
        {
            Evaluation evaluador = new Evaluation(casosEntrenamiento);                   //...Opcional: si tien mas de 10 ejemplo, estima la posible precisi�n
            evaluador.crossValidateModel(saberPredecirFuerzaX, casosEntrenamiento, 10, new java.util.Random(1));
            print("El Error Absoluto Promedio durante el entrenamiento fue de " + evaluador.meanAbsoluteError().ToString("0.000000") + " N");
        }

        distanciaObjetivo = UnityEngine.Random.Range(1.0f, 15.0f);                          //Distancia de la Canasta (... Opcional: generada aleatoriamente)

        //SITUA UNA CANASTA                                                        
        PuntoObjetivo = GameObject.CreatePrimitive(PrimitiveType.Cylinder);           // ... opcional: muestra la canasta a la distancia propuesta
        PuntoObjetivo.transform.position = new Vector3(distanciaObjetivo, -1, 0);
        PuntoObjetivo.transform.localScale = new Vector3(1.1f, 1, 1.1f);
        PuntoObjetivo.GetComponent<Collider>().isTrigger = true;                      //...  opcional: hace que la canasta no sea f�sica 

        ESTADO = "Con conocimiento";

    }

    void FixedUpdate()                                                                    //Aplica conocimiento aprendido para lanzar a la canasta propuesta
    {
        if ((ESTADO == "Con conocimiento") && (distanciaObjetivo > 0))
        {
            Instance casoPrueba = new Instance(casosEntrenamiento.numAttributes());  //Crea un registro de experiencia durante el juego
            casoPrueba.setDataset(casosEntrenamiento);
            casoPrueba.setValue(1, distanciaObjetivo);                               //le pone el dato de la distancia a alcanzar

            mejorFuerzaX = (float)saberPredecirFuerzaX.classifyInstance(casoPrueba);  //predice la fuerza dada la distancia utilizando el algoritmo M5P
            print("Durante el juego, se observ� Y=" + distanciaObjetivo + ". El NPC calcula la fuerza X =" + mejorFuerzaX);

            InstanciaPelota = Instantiate(pelota) as GameObject;                      //Utiliza la pelota f�sica del juego (si no existe la crea)
            r = InstanciaPelota.GetComponent<Rigidbody>();
            r.AddForce(new Vector3(mejorFuerzaX, Fy_calculada, 0), ForceMode.Impulse);          //y porfin la la lanza en el videojuego con la fuerza encontrara
            print("Se lanz� una pelota con fuerzas:   Fy_fijo = " + Fy_calculada + "  y  Fx =" + mejorFuerzaX);
            ESTADO = "Acci�n realizada";

        }
        if (ESTADO == "Acci�n realizada")
        {
            texto.text = "Para una canasta a " + distanciaObjetivo.ToString("0.000") + " m, la fuerza Fx a utilizar ser� de " + mejorFuerzaX.ToString("0.000") + "N  (Fy calculada=" + Fy_calculada.ToString("0.00") + " N)";
            if (r.transform.position.y < 0)                                            //cuando la pelota cae por debajo de 0 m
            {                                                                          //escribe la distancia en x alcanzada
                print("La canasta est� a una distancia de " + distanciaObjetivo + " m");
                print("La pelota lanzada lleg� a " + r.transform.position.x + ". El error fue de " + (r.transform.position.x - distanciaObjetivo).ToString("0.000000") + " m");
                r.isKinematic = true;
                ESTADO = "FIN";
            }
        }
    }
}
