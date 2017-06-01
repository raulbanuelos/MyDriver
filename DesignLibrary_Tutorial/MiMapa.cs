
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Plugin.Geolocator;
using DesignLibrary_Tutorial.Models;
using DesignLibrary_Tutorial.Services;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;

namespace DesignLibrary_Tutorial
{
    [Activity(Label = "MiMapa", MainLauncher = true)]
    public class MiMapa : AppCompatActivity, IOnMapReadyCallback
    {
        #region Attributes
        private GoogleMap mMap;
        private bool banGetPedidosAsignados;
        ApiService WebService;
        Connection cnn;
        User usuario;
        DrawerLayout drawerLayout; 
        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MiMapa);

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            // Init toolbar
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetTitle(Resource.String.app_name);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            // Attach item selected handler to navigation view
            var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            // Create ActionBarDrawerToggle button and add it to the toolbar
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
            drawerLayout.SetDrawerListener(drawerToggle);
            drawerToggle.SyncState();
            
            WebService = new ApiService();
            cnn = new Connection(this);

            GetUsuario();
            SetUpMap();
            // Create your application here
        }

        protected override void OnResume()
        {
            SupportActionBar.SetTitle(Resource.String.app_name);
            base.OnResume();
        }
        //define action for navigation menu selection
        void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.MenuItem.ItemId)
            {
                case (Resource.Id.nav_home):
                    Toast.MakeText(this, "Home selected!", ToastLength.Short).Show();
                    break;
                case (Resource.Id.nav_messages):
                    Toast.MakeText(this, "Message selected!", ToastLength.Short).Show();
                    break;
                case (Resource.Id.nav_friends):
                    // React on 'Friends' selection
                    break;
            }
            // Close drawer
            drawerLayout.CloseDrawers();
        }

        //add custom icon to tolbar
        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.action_menu, menu);
            if (menu != null)
            {
                menu.FindItem(Resource.Id.action_refresh).SetVisible(true);
                menu.FindItem(Resource.Id.action_attach).SetVisible(false);
            }
            return base.OnCreateOptionsMenu(menu);
        }
        //define action for tolbar icon press
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    //this.Activity.Finish();
                    return true;
                case Resource.Id.action_attach:
                    //FnAttachImage();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        //to avoid direct app exit on backpreesed and to show fragment from stack
        public override void OnBackPressed()
        {
            if (FragmentManager.BackStackEntryCount != 0)
            {
                FragmentManager.PopBackStack();// fragmentManager.popBackStack();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        private void GetUsuario()
        {
            usuario = new User();

            usuario = DataManager.GetUser(this);
        }

        private void SetUpMap()
        {
            if (mMap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
            }
        }

        public async void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
            UpdateLocation();
            mMap.MyLocationEnabled = true;
            banGetPedidosAsignados = true;
            GetPedidosAsignados();
        }

        private async void UpdateLocation()
        {
            RequestPixie r = new RequestPixie();
            while (true)
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                var location = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                r = await WebService.SetActualPosition(location.Latitude, location.Longitude, usuario.IdNegocio);
            }
        }

        private async void GetPedidosAsignados()
        {
            RequestPixie r = new RequestPixie();

            while (banGetPedidosAsignados)
            {
                r = await WebService.GetPedidosAsignados(usuario.IdNegocio);

                if (r.Code == 1)
                {
                    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);

                    alert.SetTitle("Nuevo Servicio");

                    alert.SetPositiveButton("Aceptar", async (senderAlert, args) =>
                    {
                        //Aceptar Pedido
                        RequestPixie rAceptar = new RequestPixie();
                        rAceptar = await WebService.SetAceptarPedido(usuario.IdNegocio, (int)r.Data);
                        if (rAceptar.Code == 1)
                        {
                            banGetPedidosAsignados = false;
                        }
                        
                    });

                    alert.SetNegativeButton("Cancelar", (senderAlert, args) => {
                        //perform your own task for this conditional button click
                        string a = "Cancelo";
                    });
                    //run the alert in UI thread to display in the screen
                    RunOnUiThread(() => {
                        banGetPedidosAsignados = false;
                        alert.Show();
                    });
                }
            }
        }
    }
}