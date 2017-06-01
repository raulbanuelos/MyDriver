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
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace DesignLibrary_Tutorial.Services
{
    public class ApiService
    {
        public async Task<RequestPixie> Login(string email, string password)
        {
            try
            {
                var url = "http://74.208.227.248/PixieAPI/api/Negocio/" + email + "/" + password;
                var webrequest = (HttpWebRequest)System.Net.WebRequest.Create(url);
                using (var response = await webrequest.GetResponseAsync())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var result = reader.ReadToEnd();
                    var t = result.Substring(1, result.Length - 2);
                    RequestPixie obj = JsonConvert.DeserializeObject<RequestPixie>(t);
                    return obj;
                }
            }
            catch (Exception ex)
            {
                return new RequestPixie
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Code = 3,
                    Data = null
                };
            }
        }

        public async Task<RequestPixie> SetActualPosition(double latitud, double longitud, int idNegocio)
        {
            try
            {
                var url = "http://74.208.227.248/PixieAPI/api/Negocio/ActualizaPosicion/" + Convert.ToString(latitud) + "/" + Convert.ToString(longitud) + "/" + Convert.ToString(idNegocio);
                var webrequest = (HttpWebRequest)System.Net.WebRequest.Create(url);
                using (var response = await webrequest.GetResponseAsync())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var result = reader.ReadToEnd();
                    RequestPixie obj = JsonConvert.DeserializeObject<RequestPixie>(result.Substring(1, result.Length - 2));
                    return new RequestPixie
                    {
                        IsSuccess = true,
                        Message = "Posición actualizada",
                        Data = obj
                    };
                }
            }
            catch (Exception ex)
            {
                return new RequestPixie
                {
                    IsSuccess = false,
                    Message = ex.Message,

                };
            }
        }

        public async Task<RequestPixie> GetPedidosAsignados(int idNegocio)
        {
            try
            {
                var url = "http://74.208.227.248/PixieAPI/api/Negocio/VerificarPedidosAsignados/" + idNegocio;
                var webrequest1 = (HttpWebRequest)System.Net.WebRequest.Create(url);
                using (var response1 = await webrequest1.GetResponseAsync())
                using (var reader1 = new StreamReader(response1.GetResponseStream()))
                {
                    var result1 = reader1.ReadToEnd();
                    RequestPixie obj1 = JsonConvert.DeserializeObject<RequestPixie>(result1.Substring(1, result1.Length - 2));
                    return new RequestPixie
                    {
                        IsSuccess = true,
                        Message = obj1.Message,
                        Data = obj1,
                        Code = obj1.Code
                        
                    };
                }
            }
            catch (Exception er)
            {
                return new RequestPixie
                {
                    IsSuccess = false,
                    Message = er.Message,

                };
            }
        }

        public async Task<RequestPixie> SetAceptarPedido(int idNegocio, int idPedido)
        {
            try
            {
                var url = "http://74.208.227.248/PixieAPI/api/Negocio/AceptarPedido/" + idNegocio + "/" + idPedido;
                var webrequest = (HttpWebRequest)System.Net.WebRequest.Create(url);
                using (var response = await webrequest.GetResponseAsync())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var result = reader.ReadToEnd();
                    RequestPixie obj = JsonConvert.DeserializeObject<RequestPixie>(result.Substring(1, result.Length - 2));
                    return new RequestPixie
                    {
                        IsSuccess = true,
                        Message = obj.Message,
                        Data = obj
                    };
                }
            }
            catch (Exception er)
            {
                return new RequestPixie
                {
                    IsSuccess = false,
                    Message = er.Message,

                };
            }
        }

        public async Task<RequestPixie> SetInciarViaje(int idNegocio, int idPedido)
        {
            try
            {
                var url = "http://74.208.227.248/PixieAPI/api/Negocio/IniciarServicio/" + idNegocio + "/" + idPedido;
                var webrequest = (HttpWebRequest)System.Net.WebRequest.Create(url);
                using (var response = await webrequest.GetResponseAsync())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var result = reader.ReadToEnd();
                    RequestPixie obj = JsonConvert.DeserializeObject<RequestPixie>(result.Substring(1, result.Length - 2));
                    return new RequestPixie
                    {
                        IsSuccess = true,
                        Message = obj.Message,
                        Data = obj
                    };
                }
            }
            catch (Exception er)
            {
                return new RequestPixie
                {
                    IsSuccess = false,
                    Message = er.Message,

                };
            }
        }

    }
}