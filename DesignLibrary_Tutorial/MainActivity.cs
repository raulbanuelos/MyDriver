using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBar = Android.Support.V7.App.ActionBar;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.App;
using System.Collections.Generic;
using Java.Lang;
using DesignLibrary_Tutorial.Fragments;
using DesignLibrary_Tutorial.Models;
using DesignLibrary_Tutorial.Services;
using Newtonsoft.Json;
using Android.Database;

namespace DesignLibrary_Tutorial
{
    [Activity(Label = "DesignLibrary_Tutorial", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/Theme.DesignDemo")]
    public class MainActivity : AppCompatActivity
    {
        //private DrawerLayout mDrawerLayout;

        #region Controles
        EditText txtUsuario;
        EditText txtPassword;
        Button btnLogin;
        Switch ckbRecordar;
        
        #endregion


        #region Servicios
        ApiService WebService;
        Connection cnn;
        #endregion
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            txtUsuario = FindViewById<EditText>(Resource.Id.txtUsuario);
            txtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            ckbRecordar = FindViewById<Switch>(Resource.Id.ckbRecordar);
            

            WebService = new ApiService();
            cnn = new Connection(this);

            User user = DataManager.GetUser(this);
            if (user.IsRemembered)
            {
                var miMapa = new Intent(this, typeof(MiMapa));
                StartActivity(miMapa);
            }

            btnLogin.Click += BtnLogin_Click;
            //SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolBar);
            //SetSupportActionBar(toolBar);

            //SupportActionBar ab = SupportActionBar;
            //ab.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            //ab.SetDisplayHomeAsUpEnabled(true);

            //mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            //NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            //if (navigationView != null)
            //{
            //    SetUpDrawerContent(navigationView);
            //}

            //TabLayout tabs = FindViewById<TabLayout>(Resource.Id.tabs);

            //ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);

            //SetUpViewPager(viewPager);

            //tabs.SetupWithViewPager(viewPager);

            //FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);

            //fab.Click += (o, e) =>
            //{
            //    View anchor = o as View;

            //    Snackbar.Make(anchor, "Yay Snackbar!!", Snackbar.LengthLong)
            //            .SetAction("Action", v =>
            //            {
            //                //Do something here
            //                Intent intent = new Intent(fab.Context, typeof(BottomSheetActivity));
            //                StartActivity(intent);
            //            })
            //            .Show();
            //};
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text == string.Empty)
            {
                //txtMensaje.Text = "Ingrese un usuario";
                return;
            }

            if (txtPassword.Text == string.Empty)
            {
                //txtMensaje.Text = "Ingrese una contraseña";
                txtPassword.Text = string.Empty;
                return;
            }

            RequestPixie result = await WebService.Login(txtUsuario.Text, txtPassword.Text);

            if (result.IsSuccess && result.Code == 1)
            {
                //txtMensaje.Text = result.Message;

                var res = result.Data.ToString().Substring(2, result.Data.ToString().Length - 4);

                User user = JsonConvert.DeserializeObject<User>(res);

                user.IsRemembered = ckbRecordar.Checked;

                user.PASSWORD = txtPassword.Text;

                cnn.addDatos(user.ID_USUARIO.ToString(), user.USUARIO, user.PASSWORD, user.NOMBRE,user.APELLIDO_PATERNO,user.APELLIDO_MATERNO,user.FECHA_NACIMIENTO,Convert.ToInt32(user.IsRemembered),user.Negocio.idNegocio);
                
                var miMapa = new Intent(this, typeof(MiMapa));
                StartActivity(miMapa);
            }
            else {
                //txtMensaje.Text = result.Message;
            }
        }

        //private void SetUpViewPager(ViewPager viewPager)
        //{
        //    TabAdapter adapter = new TabAdapter(SupportFragmentManager);
        //    adapter.A/*ddFragment(new Fragment1(), "Fragment 1");
        //    adapter.AddFragment(new Fragment2(), "Fragment 2");
        //    adapter.AddFragment(new Fragment3(), "Fragment 3");

        //    viewPager.Adapter = adapter;
        //}

        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    switch (item.ItemId)
        //    {
        //        case Android.Resource.Id.Home:
        //            mDrawerLayout.OpenDrawer((int)GravityFlags.Left);
        //            return true;

        //        default:
        //            return base.OnOptionsItemSelected(item);                    
        //    }
        //}

        //private void SetUpDrawerContent(NavigationView navigationView)
        //{
        //    navigationView.NavigationItemSelected += (object sender, NavigationView.NavigationItemSelectedEventArgs e) =>
        //    {
        //        e.MenuItem.SetChecked(true);
        //        mDrawerLayout.CloseDrawers();
        //    };
        //}

        //public class TabAdapter : FragmentPagerAdapter
        //{
        //    public List<SupportFragment> Fragments { get; set; }
        //    public List<string> FragmentNames { get; set; }

        //    public TabAdapter (SupportFragmentManager sfm) : base (sfm)
        //    {
        //        Fragments = new List<SupportFragment>();
        //        FragmentNames = new List<string>();
        //    }

        //    public void AddFragment(SupportFragment fragment, string name)
        //    {
        //        Fragments.Add(fragment);
        //        FragmentNames.Add(name);
        //    }

        //    public override int Count
        //    {
        //        get
        //        {
        //            return Fragments.Count;
        //        }
        //    }

        //    public override SupportFragment GetItem(int position)
        //    {
        //        return Fragments[position];
        //    }

        //    public override ICharSequence GetPageTitleFormatted(int position)
        //    {
        //        return new Java.Lang.String(FragmentNames[position]);
        //    }
        //}
    }
}

