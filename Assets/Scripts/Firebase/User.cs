using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string email;
    public string contrasena;
    public string fecha;

    public User()
    {

    }
    public User(string email, string contrasena, string fecha)
    {
        this.email = email;
        this.contrasena = contrasena;
        this.fecha = fecha;
    }

}