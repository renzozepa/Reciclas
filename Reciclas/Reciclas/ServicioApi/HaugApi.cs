using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using Reciclas.Models;
using System.Threading;
using System.IO;
using System.Net.Http.Headers;

namespace Reciclas.ServicioApi
{
    public class HaugApi
    {
        public static HaugApi Metodo = new HaugApi();
        public const string urlapiUsuarioxToken = "http://ti.haug.com.pe/WAReciclas/api/USUARIOs";
        public const string urlapiHorario = "http://ti.haug.com.pe/waReciclas/Api/HORARIOs";
        public const string urlapiRecojo = "http://ti.haug.com.pe/waReciclas/Api/RECOJOes";

        public async Task<List<UsuarioApi>> GetUsuarioxToken(string token)
        {
            List<UsuarioApi> LstTask = new List<UsuarioApi>();
            try
            {
                HttpClient client = new HttpClient
                {
                    MaxResponseContentBufferSize = 256000
                };
                var uri = new Uri(urlapiUsuarioxToken + "?token=" + token);
                var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    LstTask = JsonConvert.DeserializeObject<List<UsuarioApi>>(content);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return LstTask;
        }
        public async Task<List<HorarioApi>> GetHorarioDisponible(string zipcode)
        {
            List<HorarioApi> LstTask = new List<HorarioApi>();
            try
            {
                HttpClient client = new HttpClient
                {
                    MaxResponseContentBufferSize = 256000
                };
                var uri = new Uri(urlapiHorario + "?zipcode=" + zipcode);
                var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    LstTask = JsonConvert.DeserializeObject<List<HorarioApi>>(content);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return LstTask;
        }
        public async Task<List<HorarioApi>> GetIdHorarioDisponible(string descripcion)
        {
            List<HorarioApi> LstTask = new List<HorarioApi>();
            try
            {
                HttpClient client = new HttpClient
                {
                    MaxResponseContentBufferSize = 256000
                };
                var uri = new Uri(urlapiHorario + "?descripcion=" + descripcion);
                var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    LstTask = JsonConvert.DeserializeObject<List<HorarioApi>>(content);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return LstTask;
        }

        public async Task PostJsonHttpClient(string nombre,string usuario,string clave,string direccion,string latitud,string longitud,
            string perfil,string token,DateTime fecha_reg,int alta,DateTime fecha_alt,string celular, string zipcode)
        {
            try
            {
                UsuarioApi user = new UsuarioApi
                {
                    ID = 0,
                    NOMBRE = nombre,
                    USUARIO1 = usuario,
                    CLAVE = clave,
                    DIRECCION = direccion,
                    LATITUD = latitud,
                    LONGITUD = longitud,
                    ID_PERFIL = Convert.ToInt32(perfil),
                    TOKEN = token,
                    FECHA_REGISTRO = fecha_reg.Date,
                    ALTA = alta,
                    FECHA_ALTA = DateTime.Now.Date,
                    CELULAR = celular,
                    ZIPCODE = zipcode
                };

                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(user);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await httpClient.PostAsync(urlapiUsuarioxToken, httpContent).ConfigureAwait(false);
                string respuesta = response.RequestMessage.ToString();
                string status_code = response.StatusCode.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task PostJsonHttpClientRecojo(string descripcion, int horario, string direccion, string token_recojo, string token_usuario)
        {
            try
            {
                RecojoApi var_recojo = new RecojoApi
                {
                    ID = 0,
                    DESCRIPCION = descripcion,
                    ID_HORARIO = horario,
                    DIRECCION = direccion,
                    TOKEN_RECOJO = token_recojo,
                    TOKEN_USUARIO = token_usuario,
                    FECHA_TRANSACCION = DateTime.Now.Date,
                    ID_ESTADO = 1
                };

                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(var_recojo);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await httpClient.PostAsync(urlapiRecojo, httpContent).ConfigureAwait(false);
                string respuesta = response.RequestMessage.ToString();
                string status_code = response.StatusCode.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
