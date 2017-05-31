﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Org.Xml.Sax;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using DesignLibrary_Tutorial.Models;
using DesignLibrary_Tutorial.Services;
using Android.Database;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Support.V7.App;

namespace DesignLibrary_Tutorial
{
    [Activity(Label = "MiMapa")]
    public class MiMapa : AppCompatActivity, IOnMapReadyCallback
    {
        private GoogleMap mMap;

        ApiService WebService;
        Connection cnn;
        User usuario;

        DrawerLayout drawerLayout;

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

            //load default home screen
            var ft = FragmentManager.BeginTransaction();
            ft.AddToBackStack(null);
            ft.Add(Resource.Id.HomeFrameLayout, new HomeFragment());
            ft.Commit();
            // drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);





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
    }
}