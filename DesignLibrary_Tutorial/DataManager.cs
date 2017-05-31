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
using DesignLibrary_Tutorial.Models;
using Android.Support.V7.App;
using Android.Database;

namespace DesignLibrary_Tutorial
{
    public static class DataManager
    {
        public static User GetUser(Activity activity)
        {
            Connection cnn = new Connection(activity);

            User user = new User();

            ICursor cursor =  cnn.getData();
            if (cursor.MoveToFirst())
            {
                do
                {
                    user.ID_USUARIO = cursor.GetInt(0);
                    user.USUARIO = cursor.GetString(1);
                    user.PASSWORD = cursor.GetString(2);
                    user.NOMBRE = cursor.GetString(3);
                    user.APELLIDO_PATERNO = cursor.GetString(4);
                    user.APELLIDO_MATERNO = cursor.GetString(5);
                    user.FECHA_NACIMIENTO = cursor.GetString(6);
                    user.IsRemembered = cursor.GetInt(7) == 1 ? true : false;
                    user.IdNegocio = cursor.GetInt(8);
                } while (cursor.MoveToNext());
            }
            return user;
        }
    }
}