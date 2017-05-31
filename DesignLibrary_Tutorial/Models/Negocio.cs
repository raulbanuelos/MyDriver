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
    public class Negocio
    {
        
        public int idNegocio { get; set; }

        public double Latitud { get; set; }

        public double Longitud { get; set; }

        public string Descripcion { get; set; }

        public string Titulo { get; set; }

        public string Horario { get; set; }

        public int idCategoria { get; set; }

        public string Telefono { get; set; }

        public double Distancia { get; set; }

        public int Estatus { get; set; }

        public bool IsActivo { get; set; }

        public double Calificacion { get; set; }
        
        public int ID_USUARIO { get; set; }

        public User usuario { get; set; }

        public override int GetHashCode()
        {
            return idNegocio;
        }

    }
}