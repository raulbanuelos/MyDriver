using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DesignLibrary_Tutorial.Models
{
    public class User
    {
        public int ID_USUARIO { get; set; }

        public string USUARIO { get; set; }

        public string PASSWORD { get; set; }

        public string NOMBRE { get; set; }

        public string APELLIDO_PATERNO { get; set; }

        public string APELLIDO_MATERNO { get; set; }

        public string FECHA_NACIMIENTO { get; set; }

        public bool IsRemembered { get; set; }

        public int IdNegocio { get; set; }

        public string FullName { get { return string.Format("{0} {1}", NOMBRE, APELLIDO_PATERNO); } }

        
        public Negocio Negocio { get; set; }

        public override int GetHashCode()
        {
            return ID_USUARIO;
        }
    }
}