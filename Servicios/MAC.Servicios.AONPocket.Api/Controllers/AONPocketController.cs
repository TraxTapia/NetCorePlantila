using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAC.Servicios.Data.DAO.EF;
using MAC.Servicios.AONPocket.Api.App;
using MAC.Servicios.AONPocket.DAO;
using MAC.Servicios.AONPocket.Negocio;
using MACServiceGenerico.Peticion;
using MACServiceGenerico.Respuesta;
using Microsoft.AspNetCore.Mvc;
using MCConfig=Microsoft.Extensions.Configuration;
using MAC.Servicios.Modelos;
using MAC.AONPocket.Models;
using UniversalModeloNegocio.AONPocket;
using MAC.Servicios.AONPocket.Modelos;
using Microsoft.Extensions.Options;
using MAC.Utilidades;
using UniversalModeloNegocio.RespuestaServ;
using UniversalModeloNegocio.Mensajeria;

namespace MAC.Servicios.AONPocket.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AONPocketController : ControllerBase
    {

        private String _usuarioId=String.Empty;
        private const int ok = 1;
        private CreateDAO _DAO = new CreateDAO();
        private RespuestaSimple respuestaSimpleIni = new RespuestaSimple();
        private String MensajeriaApiURL = String.Empty;
        private String MensajeriaMailTemplate = String.Empty;
        private String URLFtp = String.Empty;
        private String PuertoFtp = String.Empty;
        private String UsuarioFTP = String.Empty;
        private String PasswordFTP = String.Empty;
        private String CarpetaFTP = String.Empty;
        private Boolean ModoPasivo = false;
        private String ModoEjecucion = String.Empty;
        private String rutaDescargas = String.Empty;
        private ConfigLayout configLayout = new ConfigLayout();
        private String RutaDescarga = String.Empty;
        public AONPocketController(IOptions<ConfigLayout> pconfigLayout)
        {
            configLayout = pconfigLayout.Value;
            respuestaSimpleIni.mensaje = String.Empty;
            respuestaSimpleIni.result = 0;
            MCConfig.IConfiguration setting = ConfigHelper.GetConfig();

            MCConfig.IConfiguration dbsetting = setting.GetSection("ConnectionStrings");

            String appConstring = dbsetting["ApiConnection"];

            MCConfig.IConfiguration EFsetting = setting.GetSection("Logs");
            String pathlog = EFsetting["EFPath"];
            _DAO = new CreateDAO(appConstring, pathlog);

            MCConfig.IConfiguration serviciossetting = setting.GetSection("Servicios");
            MensajeriaApiURL = serviciossetting["MensajeriaApiURL"];
            MensajeriaMailTemplate = serviciossetting["MensajeriaMailTemplate"];

            MCConfig.IConfiguration ftpsetting = setting.GetSection("FTP");
            URLFtp = ftpsetting["URL"];
            PuertoFtp = ftpsetting["Puerto"];
            UsuarioFTP = ftpsetting["Usuario"];
            PasswordFTP = ftpsetting["Password"];
            CarpetaFTP = ftpsetting["Carpeta"];
            ModoPasivo = ftpsetting["ModoPasivo"].Equals("1") ? true : false;
            ModoEjecucion = ftpsetting["ModoEjecucion"];

            MCConfig.IConfiguration Layoutsetting = setting.GetSection("Layout");
            rutaDescargas = Layoutsetting["RutaDescarga"];
        }

        [Route("api/Info")]
        [HttpGet]
        public RespuestaSimple Info()
        {
            return new RespuestaSimple() { result = 1, mensaje = "API MAC AON" };
        }

        private RespuestaSimple validarToken(String token)
        {
            RespuestaSimple result = new RespuestaSimple();
            result.result = ok;
            result.mensaje = "Test";
            //TODO: Validar Token y regresar usuario.
            try
            {

            }
            catch (Exception ex)
            {
                result = new RespuestaSimple() { result = 0, mensaje = ex.Message.ToString() };
            }
            return result;
        }

        [Route("api/Test")]
        [HttpGet]
        public RespuestaData<List<String>> Test(String peticion)
        {
            RespuestaData<List<String>> oRespuesta = new RespuestaData<List<String>>();
            oRespuesta.Respuesta = respuestaSimpleIni;
            _usuarioId = "Test";
            NegocioAONPocket onegocio = new NegocioAONPocket(_usuarioId, _DAO);
            try
            {
                onegocio.accionMail = MensajeriaMailTemplate;
                onegocio.URLFTP = URLFtp;
                onegocio.PassFTP = PasswordFTP;
                onegocio.urlMensajeria = MensajeriaApiURL;
                onegocio.UsuarioFTP = UsuarioFTP;
                onegocio.CarpetaFTP = CarpetaFTP;
                onegocio.ModoPasivo = ModoPasivo;
                onegocio.ModoEjecucion = ModoEjecucion;
                onegocio.configLayout = configLayout;
                oRespuesta.Datos = onegocio.Test();
            }
            catch (Exception ex)
            {
                oRespuesta.Respuesta = new RespuestaSimple() { result = 0, mensaje = ex.Message.ToString() };
            }
            return oRespuesta;
        }

        [Route("api/LeerLayout")]
        [HttpPost]
        [Produces("application/json")]
        public RespuestaData<TablaGenerica> LeerLayout(ClasePeticion<int> peticion)
        {
            RespuestaData<TablaGenerica> oRespuesta = new RespuestaData<TablaGenerica>();
            try
            {
                oRespuesta.Respuesta = validarToken(peticion.Token);
                if (oRespuesta.Respuesta.result == ok)
                {
                    _usuarioId = oRespuesta.Respuesta.mensaje;
                    NegocioAONPocket onegocio = new NegocioAONPocket(_usuarioId, _DAO);
                    onegocio.configLayout = configLayout;
                    oRespuesta.Datos = onegocio.datosLayout(peticion.Clase);
                    oRespuesta.Respuesta = new RespuestaSimple() { result = 1, mensaje = String.Empty };
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Respuesta = new RespuestaSimple() { result = 0, mensaje = ex.Message.ToString() };
            }
            return oRespuesta;
        }

        [Route("api/EnviarCorreos")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<RespuestaData<List<String>>> EnviarCorreos(ClasePeticion<Envio> peticion)
        {
            RespuestaData<List<String>> oRespuesta = new RespuestaData<List<String>>();
            try
            {
                oRespuesta.Respuesta = validarToken(peticion.Token);
                if (oRespuesta.Respuesta.result == ok)
                {
                    _usuarioId = oRespuesta.Respuesta.mensaje;
                    NegocioAONPocket onegocio = new NegocioAONPocket(_usuarioId, _DAO);
                    List<String> errores = new List<String>();
                    onegocio.urlMensajeria = MensajeriaApiURL;
                    onegocio.accionMail = MensajeriaMailTemplate;
                    onegocio.configLayout = configLayout;
                    MCConfig.IConfiguration setting = ConfigHelper.GetConfig();
                    MCConfig.IConfiguration mailsetting = setting.GetSection("CorreoNotificacion");
                    String correoNotificacion = mailsetting["Direccion"];
                    mailsetting = setting.GetSection("MailSettings");
                    Boolean isSSL = mailsetting["SecureSSL"].Equals("1") ? true : false;
                    onegocio.setMailConfig(mailsetting["usuariosec"], mailsetting["correoSalida"],
                        mailsetting["alias"], isSSL, mailsetting["servicioSMTP"], int.Parse(mailsetting["puertoSMTP"]),
                        mailsetting["usuarioMail"], mailsetting["passMail"], mailsetting["rutaPlantillas"]);
                    errores = await onegocio.EnvioCorreos(peticion.Clase, correoNotificacion);
                    oRespuesta.Respuesta.result = errores.Count() > 0 ? (short)2 : (short)ok;
                    oRespuesta.Datos = errores;
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Respuesta = new RespuestaSimple() { result = 0, mensaje = ex.Message.ToString() };
            }
            return oRespuesta;
        }

        [Route("api/EnviarAON")]
        [HttpPost]
        [Produces("application/json")]
        public RespuestaData<List<String>> EnviarAON(ClasePeticion<Envio> peticion)
        {
            RespuestaData<List<String>> oRespuesta = new RespuestaData<List<String>>();
            try
            {
                oRespuesta.Respuesta = validarToken(peticion.Token);
                if (oRespuesta.Respuesta.result == ok)
                {
                    _usuarioId = oRespuesta.Respuesta.mensaje;
                    NegocioAONPocket onegocio = new NegocioAONPocket(_usuarioId, _DAO);
                    List<String> errores = new List<String>();
                    onegocio.URLFTP = URLFtp;
                    onegocio.PassFTP = PasswordFTP;
                    onegocio.UsuarioFTP = UsuarioFTP;
                    onegocio.CarpetaFTP = CarpetaFTP;
                    onegocio.ModoPasivo = ModoPasivo;
                    onegocio.ModoEjecucion = ModoEjecucion;
                    onegocio.PuertoFTP = PuertoFtp;
                    onegocio.configLayout = configLayout;
                    errores = onegocio.EnvioFTP(peticion.Clase);
                    oRespuesta.Respuesta.result = errores.Count() > 0 ? (short)2 : (short)ok;
                    oRespuesta.Datos = errores;
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Respuesta = new RespuestaSimple() { result = 0, mensaje = ex.Message.ToString() };
            }
            return oRespuesta;
        }

        [Route("api/EnviarAONTest")]
        [HttpGet]
        [Produces("application/json")]
        public RespuestaData<String> EnviarAONTest()
        {
            RespuestaData<String> oRespuesta = new RespuestaData<String>();
            try
            {
                oRespuesta.Respuesta = new RespuestaSimple() { result=1,mensaje=String.Empty };
                if (oRespuesta.Respuesta.result == ok)
                {
                    _usuarioId = oRespuesta.Respuesta.mensaje;
                    List<FTPTransfer.FTPFileTransfer> ftpFiles = new List<FTPTransfer.FTPFileTransfer>();
                    oRespuesta.Respuesta.result = 1;
                    ftpFiles.Add(new FTPTransfer.FTPFileTransfer() { FtpLocalFile = "C:\\Temp\\AONPocket\\Test_Envio.txt", ServerDirectory = String.Concat(CarpetaFTP,"CERTIFICADOS/TARJETAS/POLIZAS/") });
                    FTPTransfer.UploadFile(URLFtp,ftpFiles, UsuarioFTP,PasswordFTP, int.Parse(PuertoFtp));
                    oRespuesta.Datos = "OK";
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Respuesta = new RespuestaSimple() { result = 0, mensaje = ex.Message.ToString() };
            }
            return oRespuesta;
        }

        [Route("api/ArmadoArchivos")]
        [HttpPost]
        [Produces("application/json")]
        public RespuestaData<List<UniversalModeloNegocio.Generales.Arbol>> ArmadoArchivos(ClasePeticion<Envio> peticion)
        {
            RespuestaData<List<UniversalModeloNegocio.Generales.Arbol>> oRespuesta = new RespuestaData<List<UniversalModeloNegocio.Generales.Arbol>>();
            try
            {
                oRespuesta.Respuesta = validarToken(peticion.Token);
                if (oRespuesta.Respuesta.result == ok)
                {
                    _usuarioId = oRespuesta.Respuesta.mensaje;
                    NegocioAONPocket onegocio = new NegocioAONPocket(_usuarioId, _DAO);
                    onegocio.configLayout = configLayout;
                    List<UniversalModeloNegocio.Generales.Arbol> archivos = new List<UniversalModeloNegocio.Generales.Arbol>();
                    List<String> Errores = new List<String>();
                    archivos = onegocio.ArmadoArchivos(peticion.Clase, ref Errores);
                    if (Errores.Count > 0)
                    {
                        oRespuesta.Respuesta = new RespuestaSimple() { result = 0, mensaje = String.Join(',',Errores) };
                    }
                    else
                    {
                        oRespuesta.Respuesta.result = (short)ok;
                    }
                    
                    oRespuesta.Datos = archivos;
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Respuesta = new RespuestaSimple() { result = 0, mensaje = ex.Message.ToString() };
            }
            return oRespuesta;
        }

        [Route("api/Registro")]
        [HttpPost]
        public RespuestaData<VMDocumentacionEnvio> Registro(ClasePeticion<VMDocumentacionEnvio> peticion)
        {
            RespuestaData<VMDocumentacionEnvio> oRespuesta = new RespuestaData<VMDocumentacionEnvio>();
            oRespuesta.Respuesta = validarToken(peticion.Token);
            if (oRespuesta.Respuesta.result == ok)
            {
                _usuarioId = oRespuesta.Respuesta.mensaje;
                NegocioAONPocket onegocio = new NegocioAONPocket(_usuarioId, _DAO);
                try
                {
                    VMDocumentacionEnvio vMDocumentacionEnvio = peticion.Clase;
                    vMDocumentacionEnvio = onegocio.Registro(vMDocumentacionEnvio);
                    oRespuesta.Datos = vMDocumentacionEnvio;
                    oRespuesta.Respuesta.result = 1;
                }
                catch (Exception ex)
                {
                    oRespuesta.Respuesta = new RespuestaSimple() { result = 0, mensaje = ex.Message.ToString() };
                }
            }
            return oRespuesta;
        }

        [Route("api/RegistroDetalle")]
        [HttpPost]
        public RespuestaData<List<String>> RegistroDetalle(ClasePeticion<VMDocumentacionEnvio> peticion)
        {
            RespuestaData<List<String>> oRespuesta = new RespuestaData<List<String>>();
            oRespuesta.Respuesta = validarToken(peticion.Token);
            if (oRespuesta.Respuesta.result == ok)
            {
                _usuarioId = oRespuesta.Respuesta.mensaje;
                NegocioAONPocket onegocio = new NegocioAONPocket(_usuarioId, _DAO);
                try
                {
                    VMDocumentacionEnvio vMDocumentacionEnvio = peticion.Clase;
                    onegocio.configLayout = configLayout;
                    List<String> results = onegocio.ObtenerDetalle(vMDocumentacionEnvio);
                    oRespuesta.Datos = results;
                    oRespuesta.Respuesta.result = results.Count()>0 ? (short)0 :  (short)1;
                }
                catch (Exception ex)
                {
                    oRespuesta.Respuesta = new RespuestaSimple() { result = 0, mensaje = ex.Message.ToString() };
                }
            }
            return oRespuesta;
        }


        [Route("api/Edicion")]
        [HttpPost]
        public RespuestaData<VMDocumentacionEnvio> Edicion(ClasePeticion<VMDocumentacionEnvio> peticion)
        {
            RespuestaData<VMDocumentacionEnvio> oRespuesta = new RespuestaData<VMDocumentacionEnvio>();
            oRespuesta.Respuesta = validarToken(peticion.Token);
            if (oRespuesta.Respuesta.result == ok)
            {
                _usuarioId = oRespuesta.Respuesta.mensaje;
                NegocioAONPocket onegocio = new NegocioAONPocket(_usuarioId, _DAO);
                try
                {
                    VMDocumentacionEnvio vMDocumentacionEnvio = peticion.Clase;
                    vMDocumentacionEnvio = onegocio.Registro(vMDocumentacionEnvio, false);
                    oRespuesta.Datos = vMDocumentacionEnvio;
                    oRespuesta.Respuesta.result = 1;
                }
                catch (Exception ex)
                {
                    oRespuesta.Respuesta = new RespuestaSimple() { result = 0, mensaje = ex.Message.ToString() };
                }
            }
            return oRespuesta;
        }

        [Route("api/Eliminacion")]
        [HttpPost]
        public RespuestaSimple Eliminacion(ClasePeticion<QueryParameters<Entidades.DocumentacionEnvio>> peticion)
        {
            RespuestaSimple oRespuesta = new RespuestaSimple();
            oRespuesta = validarToken(peticion.Token);
            if (oRespuesta.result == ok)
            {
                _usuarioId = oRespuesta.mensaje;
                NegocioAONPocket onegocio = new NegocioAONPocket(_usuarioId, _DAO);
                try
                {
                    oRespuesta.mensaje = onegocio.Eliminar(peticion.Clase);
                    oRespuesta.result = String.IsNullOrEmpty(oRespuesta.mensaje) ? (short)1 : (short)0;
                }
                catch (Exception ex)
                {
                    oRespuesta.result = 0;
                    oRespuesta.mensaje = ex.Message.ToString();
                }
            }
            return oRespuesta;
        }

        [Route("api/GetInf")]
        [HttpPost]
        public RespuestaData<VMDocumentacionEnvio> GetInf(ClasePeticion<int> peticion)
        {
            RespuestaData<VMDocumentacionEnvio> oRespuesta = new RespuestaData<VMDocumentacionEnvio>();
            oRespuesta.Respuesta = validarToken(peticion.Token);
            if (oRespuesta.Respuesta.result == ok)
            {
                _usuarioId = oRespuesta.Respuesta.mensaje;
                NegocioAONPocket onegocio = new NegocioAONPocket(_usuarioId, _DAO);
                try
                {
                    int Id = peticion.Clase;
                    oRespuesta.Datos = onegocio.GetInf(Id);
                    oRespuesta.Respuesta.result = 1;
                }
                catch (Exception ex)
                {
                    oRespuesta.Respuesta = new RespuestaSimple() { result = 0, mensaje = ex.Message.ToString() };
                }
            }
            return oRespuesta;
        }

        [Route("api/GetList")]
        [HttpPost]
        public RespuestaData<List<VMDocumentacionEnvio>> GetList(ClasePeticion<Boolean> peticion)
        {
            RespuestaData<List<VMDocumentacionEnvio>> oRespuesta = new RespuestaData<List<VMDocumentacionEnvio>>();
            oRespuesta.Respuesta = validarToken(peticion.Token);
            if (oRespuesta.Respuesta.result == ok)
            {
                _usuarioId = oRespuesta.Respuesta.mensaje;
                try
                {
                    NegocioAONPocket onegocio = new NegocioAONPocket(_usuarioId, _DAO);
                    QueryParameters<Entidades.DocumentacionEnvio> filtro = new QueryParameters<Entidades.DocumentacionEnvio>();
                    Boolean activos = peticion.Clase;
                    filtro.where = x => x.Id > 0;
                    List<VMDocumentacionEnvio> consulta = onegocio.Consultar(filtro);
                    oRespuesta.Datos = consulta;
                    oRespuesta.Respuesta.result = 1;
                }
                catch (Exception ex)
                {
                    oRespuesta.Respuesta = new RespuestaSimple() { result = 0, mensaje = ex.Message.ToString() };
                }
            }
            return oRespuesta;
        }

        [Route("api/Consultar")]
        [HttpPost]
        public RespuestaData<List<VMDocumentacionEnvio>> Consultar(ClasePeticion<VMDocumentacionEnvio> peticion)
        {
            RespuestaData<List<VMDocumentacionEnvio>> oRespuesta = new RespuestaData<List<VMDocumentacionEnvio>>();
            oRespuesta.Respuesta = validarToken(peticion.Token);
            if (oRespuesta.Respuesta.result == ok)
            {
                _usuarioId = oRespuesta.Respuesta.mensaje;
                NegocioAONPocket onegocio = new NegocioAONPocket(_usuarioId, _DAO);
                try
                {
                    QueryParameters<Entidades.DocumentacionEnvio> filtro = new QueryParameters<Entidades.DocumentacionEnvio>();
                    List<VMDocumentacionEnvio> consulta = onegocio.Consultar(filtro);
                    oRespuesta.Datos = consulta;
                    oRespuesta.Respuesta.result = 1;
                }
                catch (Exception ex)
                {
                    oRespuesta.Respuesta = new RespuestaSimple() { result = 0, mensaje = ex.Message.ToString() };
                }
            }
            return oRespuesta;
        }

        [Route("api/ConsultarFechaEnvio")]
        [HttpPost]
        public RespuestaData<List<VMDocumentacionEnvio>> ConsultarFechaEnvio(ClasePeticion<DateTime?> peticion)
        {
            RespuestaData<List<VMDocumentacionEnvio>> oRespuesta = new RespuestaData<List<VMDocumentacionEnvio>>();
            oRespuesta.Respuesta = validarToken(peticion.Token);
            if (oRespuesta.Respuesta.result == ok)
            {
                _usuarioId = oRespuesta.Respuesta.mensaje;
                NegocioAONPocket onegocio = new NegocioAONPocket(_usuarioId, _DAO);
                try
                {
                    QueryParameters<Entidades.DocumentacionEnvio> filtro = new QueryParameters<Entidades.DocumentacionEnvio>();
                    DateTime? Fecha = peticion.Clase;
                    filtro.where = x => x.FechaEnvio == Fecha;
                    List<VMDocumentacionEnvio> consulta = onegocio.Consultar(filtro);
                    oRespuesta.Datos = consulta;
                    oRespuesta.Respuesta.result = 1;
                }
                catch (Exception ex)
                {
                    oRespuesta.Respuesta = new RespuestaSimple() { result = 0, mensaje = ex.Message.ToString() };
                }
            }
            return oRespuesta;
        }

        [Route("api/ConsultarFechas")]
        [HttpPost]
        public RespuestaData<List<VMDocumentacionEnvio>> ConsultarFechas(ClasePeticion<FiltroFechas> peticion)
        {
            RespuestaData<List<VMDocumentacionEnvio>> oRespuesta = new RespuestaData<List<VMDocumentacionEnvio>>();
            oRespuesta.Respuesta = validarToken(peticion.Token);
            if (oRespuesta.Respuesta.result == ok)
            {
                _usuarioId = oRespuesta.Respuesta.mensaje;
                NegocioAONPocket onegocio = new NegocioAONPocket(_usuarioId, _DAO);
                try
                {
                    QueryParameters<Entidades.DocumentacionEnvio> filtro = new QueryParameters<Entidades.DocumentacionEnvio>();
                    FiltroFechas Fechas = peticion.Clase;
                    filtro.where = x => x.FechaEnvio >= Fechas.fechaInicial && x.FechaEnvio<=Fechas.fechaFinal;
                    List<VMDocumentacionEnvio> consulta = onegocio.Consultar(filtro);
                    oRespuesta.Datos = consulta;
                    oRespuesta.Respuesta.result = 1;
                }
                catch (Exception ex)
                {
                    oRespuesta.Respuesta = new RespuestaSimple() { result = 0, mensaje = ex.Message.ToString() };
                }
            }
            return oRespuesta;
        }

        [Route("api/DetalleEnvio")]
        [HttpPost]
        public RespuestaData<List<VMDocumentacionEnvioDetalles>> DetalleEnvio(ClasePeticion<int> peticion)
        {
            RespuestaData<List<VMDocumentacionEnvioDetalles>> oRespuesta = new RespuestaData<List<VMDocumentacionEnvioDetalles>>();
            oRespuesta.Respuesta = validarToken(peticion.Token);
            if (oRespuesta.Respuesta.result == ok)
            {
                _usuarioId = oRespuesta.Respuesta.mensaje;
                NegocioAONPocket onegocio = new NegocioAONPocket(_usuarioId, _DAO);
                try
                {
                    QueryParameters<Entidades.DocumentacionEnvioDetalles> filtro = new QueryParameters<Entidades.DocumentacionEnvioDetalles>();
                    int idEnvio = peticion.Clase;
                    filtro.where = x => x.envio == idEnvio;
                    List<VMDocumentacionEnvioDetalles> consulta = onegocio.ConsultarDetalle(filtro);
                    oRespuesta.Datos = consulta;
                    oRespuesta.Respuesta.result = 1;
                }
                catch (Exception ex)
                {
                    oRespuesta.Respuesta = new RespuestaSimple() { result = 0, mensaje = ex.Message.ToString() };
                }
            }
            return oRespuesta;
        }

        [Route("api/GetPlanAgrupacion")]
        [HttpPost]
        public RespuestaData<List<PlanCondicionesGenerales>> GetPlanAgrupacion(ClasePeticion<String> peticion)
        {
            RespuestaData<List<PlanCondicionesGenerales>> respuestaData = new RespuestaData<List<PlanCondicionesGenerales>>
            {
                Datos = new List<PlanCondicionesGenerales>(),
                Respuesta = new RespuestaSimple()
            };
            try
            {
                NegocioAONPocket onegocio = new NegocioAONPocket(_usuarioId, _DAO);
                onegocio.configLayout = configLayout;
                respuestaData.Datos = onegocio.GetPlanesCondiciones();
                respuestaData.Respuesta.mensaje = "Exitoso";
                respuestaData.Respuesta.result = 1;
            }
            catch (Exception ex)
            {
                respuestaData.Respuesta = new RespuestaSimple()
                {
                    result = 0,
                    mensaje = ex.Message.ToString()
                };
            }
            return respuestaData;
        }
        [Route("api/Addplan")]
        [HttpPost]
        public RespuestaData<List<PlanProducto>> AddPlanes(ClasePeticion<List<PlanProducto>> listPlanProducto)
        {
            RespuestaData<List<PlanProducto>> respuestaData = new RespuestaData<List<PlanProducto>>
            {
                Datos = new List<PlanProducto>(),
                Respuesta = new RespuestaSimple()
            };
            try
            {
                NegocioAONPocket onegocio = new NegocioAONPocket(_usuarioId, _DAO);
                respuestaData.Datos = onegocio.AddPlanes(listPlanProducto.Clase);
                respuestaData.Respuesta.mensaje = "Exitoso";
                respuestaData.Respuesta.result = 1;
            }
            catch (Exception ex)
            {
                respuestaData.Respuesta = new RespuestaSimple()
                {
                    result = 0,
                    mensaje = ex.Message.ToString()
                };
            }
            return respuestaData;
        }

        [Route("api/Info")]
        [HttpGet]
        public Respuesta SendMailTemplateCustom(VMMailMessageExtend mail)
        {
            Respuesta respuesta = new Respuesta();
            respuesta.Status = 0;
            try
            {
                NegocioAONPocket onegocio = new NegocioAONPocket(_usuarioId, _DAO);
                respuesta.Status = 1;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message.ToString();
            }
            return respuesta;
        }

    }
}