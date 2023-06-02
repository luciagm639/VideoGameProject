using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshObjectScript : MonoBehaviour
{
    private Vector3[] puntosDetectados;
    public NavMeshAgent agente;

    public GameObject coche;
    private Coche cocheScript;

    // Start is called before the first frame update
    void Start()
    {
        if (agente == null) agente = this.gameObject.GetComponent<NavMeshAgent>();

        puntosDetectados = new Vector3[7];

        for (int i = 0; i < puntosDetectados.Length; i++)
        {
            puntosDetectados[i] = Vector3.negativeInfinity;
        }

        cocheScript = coche.GetComponent<Coche>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PuntosRecibidos())
        {
            Vector3 puntoMedio = CalcularPuntoMedio();
            agente.SetDestination(puntoMedio);
            SetPuntosDetectadosANull();
            cocheScript.NotificarRayos();
        }
    }

    public void SetPosicionArray(Vector3 v, int pos)
    {
        puntosDetectados[pos] = v;
    }

    public Vector3 GetPosicionArray(int pos)
    {
        return puntosDetectados[pos];
    }

    private bool PuntosRecibidos()
    {
        int i = 0;
        while (i < puntosDetectados.Length && !puntosDetectados[i].Equals(Vector3.negativeInfinity))
        {
            i++;
        }
        return i >= puntosDetectados.Length;
    }

    private void SetPuntosDetectadosANull()
    {
        for (int i = 0; i < puntosDetectados.Length; i++)
        {
            puntosDetectados[i] = Vector3.negativeInfinity;
        }
    }

    private Vector3 CalcularPuntoMedio()
    {
        float x = 0.0f, y = 0.0f, z = 0.0f;
        float numPuntos = 0;

        foreach (Vector3 v in puntosDetectados)
        {
            if (!v.Equals(Vector3.positiveInfinity))
            {
                x += v.x;
                y += v.y;
                z += v.z;
                numPuntos++;
            }
        }

        return new Vector3(x/numPuntos, y/numPuntos, z/numPuntos);
    }
}
