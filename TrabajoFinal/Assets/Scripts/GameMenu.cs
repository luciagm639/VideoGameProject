using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public GameObject menuPausa;
    public GameObject menuOpciones;
    public GameObject menuControles;
    public GameObject camaraPrimeraPersona;
    public GameObject camaraTerceraPersona;

    public GameObject nivelSonido;
    public GameObject nivelMusica;
    public GameObject nivelVoces;


    public bool enTerceraPersona = false;

    void Start()
    {
        activarJuego();
    }

    public void activarMenuPausa()
    {
        menuPausa.SetActive(true);
        menuOpciones.SetActive(false);
        menuControles.SetActive(false);
        camaraPrimeraPersona.SetActive(false);
        camaraTerceraPersona.SetActive(false);
    }

    public void activarJuego()
    {
        Time.timeScale = 0;
        menuPausa.SetActive(false);
        menuOpciones.SetActive(false);
        menuControles.SetActive(false);
        camaraPrimeraPersona.SetActive(!enTerceraPersona);
        camaraTerceraPersona.SetActive(enTerceraPersona);
    }

    public void activarMenuOpciones()
    {
        menuPausa.SetActive(false);
        menuOpciones.SetActive(true);
        menuControles.SetActive(false);
        camaraPrimeraPersona.SetActive(false);
        camaraTerceraPersona.SetActive(false);

        iniciarConfiguracion();
    }

    private void iniciarConfiguracion()
    {
        nivelSonido.GetComponent<Slider>().value = CONFIGURACION.nivelSonido;
        nivelMusica.GetComponent<Slider>().value = CONFIGURACION.nivelMusica;
        nivelVoces.GetComponent<Slider>().value = CONFIGURACION.nivelVoces;
    }

    public void activarMenuControles()
    {
        menuPausa.SetActive(false);
        menuOpciones.SetActive(false);
        menuControles.SetActive(true);
        camaraPrimeraPersona.SetActive(false);
        camaraTerceraPersona.SetActive(false);
    }

    public void salirAlMenuPrincipal()
    {
        SceneManager.LoadScene("INICIO");
    }

    public void salirDelJuego()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            if(!menuPausa.active && !menuOpciones.active && !menuControles.active){
                activarMenuPausa();
            }
            else
            {
                activarJuego();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            enTerceraPersona = false;

            if (!menuPausa.active && !menuOpciones.active && !menuControles.active)
            {
                camaraPrimeraPersona.SetActive(!enTerceraPersona);
                camaraTerceraPersona.SetActive(enTerceraPersona);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            enTerceraPersona = true;

            if (!menuPausa.active && !menuOpciones.active && !menuControles.active)
            {
                camaraPrimeraPersona.SetActive(!enTerceraPersona);
                camaraTerceraPersona.SetActive(enTerceraPersona);
            }
        }

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
