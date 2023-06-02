using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject menuPrincipal;
    public GameObject menuEstadisticas;
    public GameObject menuOpciones;
    public GameObject menuCreditos;
    public GameObject menuControles;
    public GameObject menuSeleccionarModo;
    public GameObject menuEnDesarrollo;


    public GameObject nivelSonido;
    public GameObject nivelMusica;
    public GameObject nivelVoces;

    // Start is called before the first frame update
    void Start()
    {
        activarMenuPrincipal();
    }

    public void entrarAlJuego()
    {
        SceneManager.LoadScene("CircuitoLago");
    }

    public void salir()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void activarMenuPrincipal()
    {
        menuPrincipal.SetActive(true);
        menuEstadisticas.SetActive(false);
        menuOpciones.SetActive(false);
        menuCreditos.SetActive(false);
        menuControles.SetActive(false);
        menuSeleccionarModo.SetActive(false);
        menuEnDesarrollo.SetActive(false);
    }

    public void activarMenuEstadisticas()
    {
        menuPrincipal.SetActive(false);
        menuEstadisticas.SetActive(true);
        menuOpciones.SetActive(false);
        menuCreditos.SetActive(false);
        menuControles.SetActive(false);
        menuSeleccionarModo.SetActive(false);
        menuEnDesarrollo.SetActive(false);
    }

    public void activarMenuOpciones()
    {
        menuPrincipal.SetActive(false);
        menuEstadisticas.SetActive(false);
        menuOpciones.SetActive(true);
        menuCreditos.SetActive(false);
        menuControles.SetActive(false);
        menuSeleccionarModo.SetActive(false);
        menuEnDesarrollo.SetActive(false);
        iniciarConfiguracion();
    }

    private void iniciarConfiguracion()
    {
        nivelSonido.GetComponent<Slider>().value = CONFIGURACION.nivelSonido;
        nivelMusica.GetComponent<Slider>().value = CONFIGURACION.nivelMusica;
        nivelVoces.GetComponent<Slider>().value = CONFIGURACION.nivelVoces;
    }

    public void activarMenuCreditos()
    {
        menuPrincipal.SetActive(false);
        menuEstadisticas.SetActive(false);
        menuOpciones.SetActive(false);
        menuCreditos.SetActive(true);
        menuControles.SetActive(false);
        menuSeleccionarModo.SetActive(false);
        menuEnDesarrollo.SetActive(false);
    }

    public void activarMenuControles()
    {
        menuPrincipal.SetActive(false);
        menuEstadisticas.SetActive(false);
        menuOpciones.SetActive(false);
        menuCreditos.SetActive(false);
        menuControles.SetActive(true);
        menuSeleccionarModo.SetActive(false);
        menuEnDesarrollo.SetActive(false);
    }

    public void activarMenuSeleccionarModo()
    {
        menuPrincipal.SetActive(false);
        menuEstadisticas.SetActive(false);
        menuOpciones.SetActive(false);
        menuCreditos.SetActive(false);
        menuControles.SetActive(false);
        menuSeleccionarModo.SetActive(true);
        menuEnDesarrollo.SetActive(false);
    }

    public void activarMenuEnDesarrollo()
    {
        menuPrincipal.SetActive(false);
        menuEstadisticas.SetActive(false);
        menuOpciones.SetActive(false);
        menuCreditos.SetActive(false);
        menuControles.SetActive(false);
        menuSeleccionarModo.SetActive(false);
        menuEnDesarrollo.SetActive(true);
    }

    public void cambiarNivelSonido()
    {
        CONFIGURACION.nivelSonido = nivelSonido.GetComponent<Slider>().value;
    }

    public void cambiarNivelMusica()
    {
        CONFIGURACION.nivelMusica = nivelMusica.GetComponent<Slider>().value;
    }

    public void cambiarNivelVoces()
    {
        CONFIGURACION.nivelVoces = nivelVoces.GetComponent<Slider>().value;
    }
}
