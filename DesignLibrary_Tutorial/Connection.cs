using System;
using Android.Database.Sqlite;
using Android.Content;
using Android.Database;

namespace DesignLibrary_Tutorial
{
    public class Connection : SQLiteOpenHelper
    {
        #region Tabla Usuario
        public static string ID_USUARIO = "ID_USUARIO";
        public static string USUARIO = "USUARIO";
        public static string PASSWORD = "CONTRASENA";
        public static string NOMBRE = "NOMBRE";
        public static string APELLIDO_PATERNO = "APELLIDO_PATERNO";
        public static string APELLIDO_MATERNO = "APELLIDO_MATERNO";
        public static string FECHA_NACIMIENTO = "FECHA_NACIMIENTO";
        public static string IsRemembered = "IsRemembered";
        public static string IdNegocio = "IdNegocio"; 
        #endregion


        private static string DATABASE = "DB_PIXIE.db";
        private static string TABLE = "TBL_USUARIO";

        public Connection(Context context): base(context, DATABASE,null,1)
        {

        }

        public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL("DELETE FROM " + TABLE);
            db.ExecSQL("CREATE TABLE " + TABLE + " (" + ID_USUARIO + " INTEGER PRIMARY KEY," + USUARIO + " TEXT, " + PASSWORD + " TEXT," + NOMBRE + " TEXT," + APELLIDO_PATERNO + " TEXT," + APELLIDO_MATERNO + " TEXT," + FECHA_NACIMIENTO + " TEXT," + IsRemembered + " INTEGER," + IdNegocio + " INTEGER)");
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            db.ExecSQL("DROP TABLE IF EXISTS " + TABLE);
            OnCreate(db);
        }

        public void addDatos(string id_usuario, string usuario,string password,string nombre,string apellido_paterno, string apellido_materno, string fecha_nacimiento,int isrecordado, int idNegocio)
        {
            
            ContentValues valores = new ContentValues();

            valores.Put(ID_USUARIO, id_usuario);
            valores.Put(USUARIO, usuario);
            valores.Put(PASSWORD, password);
            valores.Put(NOMBRE, nombre);
            valores.Put(APELLIDO_PATERNO, apellido_paterno);
            valores.Put(APELLIDO_MATERNO, apellido_materno);
            valores.Put(FECHA_NACIMIENTO, fecha_nacimiento);
            valores.Put(IsRemembered, isrecordado);
            valores.Put(IdNegocio,idNegocio);


            this.WritableDatabase.Insert(TABLE, null, valores);

        }

        public ICursor getData()
        {
            string[] columnas = { ID_USUARIO, USUARIO,PASSWORD, NOMBRE ,APELLIDO_PATERNO, APELLIDO_MATERNO, FECHA_NACIMIENTO,IsRemembered,IdNegocio};
            ICursor c = this.ReadableDatabase.Query(TABLE, columnas, null, null, null, null, null);
            return c;
        }
    }
}