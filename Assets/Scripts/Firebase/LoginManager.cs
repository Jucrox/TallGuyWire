using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{

    [Header("URL Firebase")]
    [Tooltip("Link realtime database from firebase")]
    public string urlBaseFirebase;



    [Header("InputFields Login")]
    public TMP_InputField userEmail;
    public TMP_InputField userPassword;

    private string refactorEmail;
    private string fechas;

    public TMP_Text advicesLogin;
    //private string advice;



    // Start is called before the first frame update
    void Start()
    {
        urlBaseFirebase = UrlFirebase.inst.url;
        //colorBlock.normalColor = new Color(255f, 61f, 61f, 255f);
        if (urlBaseFirebase == "")
        {
            Debug.LogError("urlBaseFirebase cannot be empty");
        }
        else if (!urlBaseFirebase.EndsWith("/"))
        {
            urlBaseFirebase = urlBaseFirebase + "/";
            ////Debug.log(urlBaseFirebase);
        }
    }

    public void verificarCredenciales()
    {
        string email = userEmail.text;
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(email);

        advicesLogin.text = "";

        if (userEmail.text == "")
        {
            //Debug.Log("El correo esta vacio");
            advicesLogin.text = "Llene todos los campos";
            //El correo no puede estar vacio
            //ErrorUser("Correo no valido");
        }
        else if (userPassword.text == "")
        {
            //Debug.Log("la contrasena esta vacia");
            advicesLogin.text = "Llene todos los campos";
            //La contraseña no puede estar vacia
            //ErrorPassword("Contraseña incorrecta");
        }
        else
        {
            if (match.Success)
            {
                refactorEmail = Regex.Replace(userEmail.text, @"[^0-9a-zA-Z]+", "_");
                refactorEmail = refactorEmail.ToLower();
                //Debug.log("Correo Bien");
                //spinner.SetActive(true);
                //textBtn.SetActive(false);
                VerificarSiExisteCorreo();
            }
            else
            {
                Debug.Log("Correo Mal");
                //El correo debe contener @ y punto(.) 
                //ErrorUser("Correo no valido");
            }
        }
    }


    public void VerificarSiExisteCorreo()
    {

        string urlNombreUsuario = urlBaseFirebase + "DatosUsuarios/" + refactorEmail + "/.json";

        RestClient.Get<User>(urlNombreUsuario)
                    .Then(firstUser =>
                    {
                        //if (firstUser.metodo == "Office")
                        //{
                        //    Debug.Log("Cuenta creada con pasaporte virtual, inicia sesion con esta");
                        //    //ErrorUser("Cuenta registrada con pasaporte virtual");
                        //}
                        //else
                        //{
                        VerificarContrasena(firstUser.contrasena);
                        //}
                        //Debug.log("El correo de usuario ya existe");

                        //Debug.log(firstUser.contrasena);
                        //PlayerPrefs.SetString("Identidad", firstUser.identidad);
                        PlayerPrefs.SetString("Email", firstUser.email);
                        //PlayerPrefs.SetString("Name", firstUser.nombre);
                        //spinner.SetActive(false);
                        //textBtn.SetActive(true);

                        //Debug.log(firstUser.perfil);

                    }).Catch(err =>
                    {
                        //Debug.Log("El correo de usuario no existe");
                        advicesLogin.text = "Credenciales incorrectas";
                        //El correo del usuario no existe
                        //spinner.SetActive(false);
                        //textBtn.SetActive(true);
                        //ErrorUser("Correo no valido");
                    });
    }

    public void FechasInicioSesion()
    {

        string urlFechas = urlBaseFirebase + "FechasIngreso/" + refactorEmail + "/.json";

        RestClient.Get<Fecha>(urlFechas).Then(firstUser =>
        {
            fechas = firstUser.fechasDeLogin + "," + DateTime.Now.ToString("MM/dd/yyyy-HH:mm:ss");

            GuardarFechasInicioSesion();
        }).Catch(err =>
        {
            fechas = DateTime.Now.ToString("MM/dd/yyyy-HH:mm:ss");

            GuardarFechasInicioSesion();
        });
    }
    public void GuardarFechasInicioSesion()
    {
        string urlNombreUsuario = urlBaseFirebase + "FechasIngreso/" + refactorEmail + "/.json";
        RestClient.Put<Fecha>(urlNombreUsuario, new Fecha(fechas))
                    .Then(firstUser =>
                    {
                        //StartCoroutine(CargarEscena());
                        //llamar comunicacion vivox
                        StartCoroutine(CargarEscena());

                    }).Catch(err => {
                        //Debug.log("No se pudo guardar");
                        //ErrorUser("No se pudo iniciar sesion");
                        advicesLogin.text = "No se pudo iniciar sesion";
                    });
    }
    public void VerificarContrasena(string contrasena)
    {
        if (contrasena == userPassword.text)
        {
            //Debug.log("Contraseña Correcta");
            //ConexionSatisfactoria();
            advicesLogin.text = "Conectando";
            FechasInicioSesion();
            //spinner.SetActive(false);
            //textBtn.SetActive(true);

        }
        else
        {
            Debug.Log("Contrasena Incorrecta");
            advicesLogin.text = "Credenciales incorrectas";
            //ErrorPassword("Contraseña incorrecta");
            //spinner.SetActive(false);
            //textBtn.SetActive(true);
        }
    }

    IEnumerator CargarEscena()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("SceneExperience");
    }
}
