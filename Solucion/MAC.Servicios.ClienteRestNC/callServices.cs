using MACServiceGenerico.Peticion;
using MACServiceGenerico.Respuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAC.Servicios.ClienteRestNC
{
    
    public static class CallServices<T>
    {
        //PUT o POST
        public static async Task<RespuestaData<T>> CallServiceAsync<T2>(T2 Params, String apiUrl, String accion, String tipoLlamado)
        {
            ClienteRest<RespuestaData<T>> clientePOST = new ClienteRest<RespuestaData<T>>();

            RespuestaData<T> serviceResult = new RespuestaData<T>();
            if (tipoLlamado == "POST")
            {
                serviceResult = await clientePOST.LLamarServicioPostGeneral<T2>(apiUrl, accion, Params);
            }
            
            return serviceResult;
        }

        public static async Task<RespuestaData<T>> CallServiceCompletoAsync<T2>(T2 Clase, Boolean Transaccionalidad, String Token , String apiUrl, String accion, String tipoLlamado)
        {
            RespuestaData<T> serviceResult = new RespuestaData<T>();

            try
            {
                //Armar ClasePeticion con los datos de ENTRADA
                ClasePeticion<T2> Params = new ClasePeticion<T2>();
                Params.Clase = Clase;
                Params.Transaccionalidad = Transaccionalidad;
                Params.Token = Token;            

                ClienteRest<RespuestaData<T>> clientePOST = new ClienteRest<RespuestaData<T>>();

                switch (tipoLlamado)
                {
                    case "POST":
                        serviceResult = await clientePOST.LLamarServicioPostGeneral<ClasePeticion<T2>>(apiUrl, accion, Params);
                        break;
                    case "PUT":
                        serviceResult = await clientePOST.LLamarServicioPutGeneral<ClasePeticion<T2>>(apiUrl, accion, Params);
                        break;
                }
                
            }
            catch (Exception ex )
                {
                    serviceResult.Respuesta.result = 0;
                    serviceResult.Respuesta.mensaje = "Algo fallo " + ex.Message.ToString();
                }

            return serviceResult;
        }

    }

}