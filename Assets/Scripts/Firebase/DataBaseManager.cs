using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DataBaseManager : MonoBehaviour
{
    [Header("URL Firebase")]
    [Tooltip("Link realtime database from firebase")]
    private string urlBaseFirebase;

    [Header("RegisterInputs")]
    public TMP_InputField userEmail;
    public TMP_InputField userPassword;
    public TMP_InputField repeatPassword;

    [Header("Windows")]
    public GameObject registerPanel;
    public GameObject loginPanel;

    private string nombreUsuarioSin = "";
    private string refactorEmail;

    public TMP_Text advicesRegister;

    void Start()
    {
        urlBaseFirebase = UrlFirebase.inst.url;
        if (urlBaseFirebase == "")
        {
            Debug.LogError("urlBaseFirebase cannot be empty");
        }
        else if (!urlBaseFirebase.EndsWith("/"))
        {
            urlBaseFirebase = urlBaseFirebase + "/";
            //Debug.Log(urlBaseFirebase);
        }

    }

    public void verificarCredenciales()
    {
        string email = userEmail.text;
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(email);
        advicesRegister.text = "";

        if (userEmail.text == "")
        {
            //Debug.Log("correo vacio");
            advicesRegister.text = "Ingresa un correo valido";
            //ErrorEmail("Ingresa un correo valido");

        }
        else
        if (userPassword.text == "")
        {
            //Debug.Log("contrasena vacio");
            advicesRegister.text = "Asigna una contraseña a la cuenta";
            //ErrorPassword("Asigna una contraseña a la cuenta");

        }
        else
        if (repeatPassword.text == "")
        {
            //Debug.Log("repetir contrasena vacio");
            advicesRegister.text = "Contraseña no coincide";
            //ErrorPassword("Contraseña no coincide");
            //ErrorRepeatPassword("Contraseña no coincide");

        }
        else
        if (userPassword.text != repeatPassword.text)
        {
            //Debug.Log("contrasenas diferentes");
            advicesRegister.text = "Contraseña no coincide";
            //ErrorPassword("Contraseña no coincide");
            //ErrorRepeatPassword("Contraseña no coincide");

        }
        else
        {
            if (match.Success)
            {
                //Debug.log("correo bien ");
                //Activar //spinner
                //spinner.SetActive(true);
                //textBtn.SetActive(false);
                refactorEmail = Regex.Replace(userEmail.text, @"[^0-9a-zA-Z]+", "_");
                refactorEmail = refactorEmail.ToLower();
                //nombreUsuarioSin = userName.text.ToLower();
                EmailVerify();

            }
            else
            {
                //Debug.Log(" correo mal");
                advicesRegister.text = "El correo debe contener @ y punto(.) ";
                //ErrorEmail("El correo debe contener @ y punto(.) ");


            }
        }
    }

    public void GuardarNuevoUsuarioCorreo()
    {


        string urlNombreUsuario = urlBaseFirebase + "DatosUsuarios/" + refactorEmail + "/.json";
        User email = new User(userEmail.text, userPassword.text,DateTime.Now.ToString("MM/dd/yyyy : HH:mm:ss"));
        RestClient.Put<User>(urlNombreUsuario, email)
                    .Then(firstUser =>
                    {
                        CreacionUsuarioSatisfactoria();
                        //Debug.log("Se guardo");
                        //GuardarNuevoUsuarioNombre();
                    }).Catch(err =>
                    {
                        Debug.Log("No se pudo guardar");
                        advicesRegister.text = "No se ha podido crear tu usuario";
                        //ErrorUserName("No se ha podido crear tu usuario");
                        //ErrorEmail("No se ha podido crear tu usuario");
                    });


    }
    public void EmailVerify()
    {
        string urlNombreUsuario = urlBaseFirebase + "DatosUsuarios/" + refactorEmail + "/.json";
        RestClient.Get<User>(urlNombreUsuario)
            .Then(firstUser =>
            {
                //Debug.Log("El correo de usuario ya existe");
                advicesRegister.text = "Correo vinculado a cuenta ya existente";
                //ErrorEmail("Correo vinculado a cuenta ya existente");
                //apagar //spinner
                //spinner.SetActive(false);
                //textBtn.SetActive(true);
            }).Catch(error =>
            {
                //Debug.log("El correo de usuario no existe");
                GuardarNuevoUsuarioCorreo();
            });
    }

    public void CreacionUsuarioSatisfactoria()
    {
        //apagarTextError();
        advicesRegister.text = "Creación de usuario satisfactoria";
        //TextSuccess.text = "Creación de usuario satisfactoria";
        //apagar //spinner
        StartCoroutine(RegistroSatisfactorio());
    }

    IEnumerator RegistroSatisfactorio()
    {
        yield return new WaitForSeconds(1.5f);
        registerPanel.SetActive(false);
        loginPanel.SetActive(true);
        ResetInputs();
    }

    public void ResetInputs()
    {
        userEmail.text = "";
        userPassword.text = "";
        repeatPassword.text = "";
        //TextError.text = "";
        //TextSuccess.text = "";
    }
}
