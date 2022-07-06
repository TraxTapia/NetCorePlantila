using MAC.Servicios.Data.DAO.EF;
using MAC.Servicios.AONPocket.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entidades = MAC.Servicios.AONPocket.Entidades;
using AutoMapper;
using MAC.Utilidades;
using MAC.Servicios.ClienteRestNC;
using System.Threading.Tasks;
using UniversalModeloNegocio.Mensajeria;
using MACServiceGenerico.Respuesta;
using MAC.Servicios.Modelos;
using MAC.AONPocket.Models;
using UniversalModeloNegocio.AONPocket;
using MAC.Servicios.AONPocket.Modelos;
using System.Reflection;
using System.IO;
using System.Xml;
using HtmlAgilityPack;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading;
using UniversalModeloNegocio.RespuestaServ;
using MAC.Servicios.Mensajeria.Lib;
using Seguridad=Utilidades;
namespace MAC.Servicios.AONPocket.Negocio
{
    public class NegocioAONPocket
    {
        private const byte envioAON = 1;
        private const byte envioMail = 2;

        private String _UserId = String.Empty;
        //Crear Mapeos
        IMapper mapper = new Mapeado().config.CreateMapper();
        private CreateDAO _DAO = new CreateDAO();
        public CreateDAO DAO { get => _DAO; set => _DAO=value; }

        String _urlMensajeria = String.Empty;
        public String urlMensajeria { get=> _urlMensajeria; set => _urlMensajeria=value; }

        String _accionMail = String.Empty;
        public String accionMail { get => _accionMail; set => _accionMail = value; }

        public String URLFTP { get; set; }
        public String PuertoFTP { get; set; }
        public String UsuarioFTP { get; set; }
        public String PassFTP { get; set; }
        public String CarpetaFTP { get; set; }
        public Boolean ModoPasivo { get; set; }
        public String ModoEjecucion { get; set; }

        public ConfigLayout configLayout { get; set; }

        private int _columnaAfiliado = 0;
        public int columnaAfiliado { get => _columnaAfiliado; set => _columnaAfiliado = value; }

        private int _columnaCorreo = 0;
        public int columnaCorreo { get => _columnaCorreo; set => _columnaCorreo = value; }

        private int _columnaNombre = 0;
        public int columnaNombre { get => _columnaNombre; set => _columnaNombre = value; }

        private int _columnaPoliza = 0;
        public int columnaPoliza { get => _columnaPoliza; set => _columnaPoliza = value; }


        // Variables de Correo
        private String usuariosec = String.Empty;
        private String _correoSalida = String.Empty;
        private String _alias = String.Empty;
        private Boolean _SecureSSL = false;
        private String _servicioSMTP = String.Empty;
        private int _puertoSMTP = 0;
        private String _usuario = String.Empty;
        private String _password = String.Empty;
        private string from = String.Empty;
        private String Token = String.Empty;
        private String _archivoPlantilla=String.Empty;
        public String archivoPlantilla { get => _archivoPlantilla; set => _archivoPlantilla = value; }
        public String plantilla { get; set; }

        private String _rutaPlantillas = String.Empty;
        public String rutaPlantillas { get => _rutaPlantillas; set => _rutaPlantillas = value; }

        public NegocioAONPocket()
        {

        }

        public NegocioAONPocket(String pusuario)
        {
            _UserId = pusuario;
        }

        public NegocioAONPocket(String pusuario,CreateDAO pDAO)
        {
            _UserId = pusuario;
            _DAO = pDAO;
        }

        public NegocioAONPocket(String pUsuarioSec, String pcorreoSalida, String palias, Boolean pSecureSSL, String pservicioSMTP, int ppuertoSMTP, String pusuario, String ppassword, String pRutaPlantillas = "")
        {
            setMailConfig(pUsuarioSec, pcorreoSalida, palias, pSecureSSL, pservicioSMTP, ppuertoSMTP, pusuario, ppassword, pRutaPlantillas);
        }

        public void setMailConfig (String pUsuarioSec, String pcorreoSalida, String palias, Boolean pSecureSSL, String pservicioSMTP, int ppuertoSMTP, String pusuario, String ppassword,String pRutaPlantillas="")
        {
            usuariosec = pUsuarioSec;
            _correoSalida = Seguridad.Encriptacion.Desencriptar(Convert.FromBase64String(pcorreoSalida), usuariosec);
            _alias = Seguridad.Encriptacion.Desencriptar(Convert.FromBase64String(palias), usuariosec);
            _SecureSSL = pSecureSSL;
            _servicioSMTP = Seguridad.Encriptacion.Desencriptar(Convert.FromBase64String(pservicioSMTP), usuariosec);
            _puertoSMTP = ppuertoSMTP;
            _usuario = Seguridad.Encriptacion.Desencriptar(Convert.FromBase64String(pusuario), usuariosec);
            _password = Seguridad.Encriptacion.Desencriptar(Convert.FromBase64String(ppassword), usuariosec);
            if (!String.IsNullOrEmpty(pRutaPlantillas))
            {
                _rutaPlantillas = pRutaPlantillas;
                _archivoPlantilla = String.Concat(_rutaPlantillas, "MACPlatillaGeneral.html");
            }
        }

        public List<String> Test(Boolean alta = true)
        {
            List<String> result = new List<String>();
            try
            {
                result.Add("In");
                //DAOCRUDGenerico<Entidades.AONPocketGen> repo = _DAO.GeneraDAOAONPocketGen();
                //Entidades.AONPocketGen AONPocket = mapper.Map<VMAONPocket, Entidades.AONPocketGen>(vmAONPocket);
                //if (alta)
                //{
                //    AONPocket.Id = 0;
                //    AONPocket = repo.AgregarIdentity(AONPocket);
                //}
                //else
                //{
                //    repo.Actualizar(AONPocket);
                //}
                //repo.Dispose();

                //vmAONPocket = Registro(vmAONPocket);
                //QueryParameters<Entidades.AONPocket> filtro = new QueryParameters<Entidades.AONPocket>();
                //filtro.where = x => x.Activo == true;
                //result = Consultar(filtro);
                List<String> archivos= new List<String>();
                String archivo = "C:\\Temp\\AONPocket\\LayoutAon.xlsx";
                archivos.Add(archivo);
                //Object[,] Data = LeerLayoutEnvio(archivo);
                //archivo = "C:\\Temp\\AONPocket\\LayoutAon.zip";
                //ZipArchivos(archivo,archivos);
                //String sendFile = await EnviarCorreoZip(archivo,"Envio doctos","gerardo.ruiz@mediaccess.com.mx","test xxxxx","Póliza xxx");

                //FTPTransfer.UploadFileToFtp(this.URLFTP, archivo, this.UsuarioFTP,this.PassFTP);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private String EnviarCorreoZip(String ruta,String archivo, String asunto,String correo,String Mensaje, String asuntoDetalle)
        {
            String result = String.Empty;
            try
            {
                ClienteRest<RespuestaSimple> cliente = new ClienteRest<RespuestaSimple>();
                RespuestaSimple respuesta = new RespuestaSimple();
                VMMailMessageExtend vMMail = new VMMailMessageExtend();
                vMMail.Files = new List<String>();
                vMMail.ruta = ruta;
                vMMail.Files.Add(archivo);
                vMMail.asunto = asunto;
                vMMail.to = correo;
                vMMail.mensaje = Mensaje;
                vMMail.asuntoDetalle = asuntoDetalle;
                vMMail.Template = "MACPlantillaMembreciaSalud.html";
                vMMail.accion = String.Empty;
                Respuesta respMail= SendMailDocumentacion(vMMail);
               if (respMail != null)
               {
                    respuesta.result = (short)respMail.Status;
                    respuesta.mensaje = respMail.Mensaje;
               }
               if (respuesta.result != 1)
               {
                    result = respuesta.mensaje;
               }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private void ZipArchivos(String NombreZip,List<String> archivos)
        {
           try
            {
                new CompresionArchivos().AgregarArchivos(NombreZip, archivos.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Object[,] buscarLayout(int Id,String archivo, Boolean conEncabezado = true)
        {
            Object[,] data = null;
            try
            {
                //Todo: Buscar el Layout
                String archivoOT = String.Concat(this.configLayout.RutaDescarga,Id.ToString(),"\\",archivo);
                String extension = Path.GetExtension(archivoOT);
                if (extension.ToLower().Equals(".xls"))
                {
                    //Archivo en formato html.
                    byte[] bytes = File.ReadAllBytes(archivoOT);
                    data = GetDataHtml(bytes, conEncabezado);
                }
                else
                {
                    data = LeerLayoutEnvio(archivoOT, conEncabezado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        Object[,] GetDataHtml(byte[] data, Boolean conEncabezado=false)
        {
            Object[,] result = null;
            try
            {
                string utfString = Encoding.UTF8.GetString(data, 0, data.Length);
                int posTabla = utfString.IndexOf("<table");
                if (posTabla < 0) { posTabla=utfString.IndexOf("<TABLE"); }
                String tabla = utfString.Substring(posTabla);
                int posCierre = tabla.IndexOf("</table>");
                if (posCierre < 0) { posCierre=utfString.IndexOf("</TABLE>"); }
                posCierre += 8;
                tabla = tabla.Substring(0, posCierre);
                //Corregir atributos
                tabla = tabla.Replace(((char)34).ToString(), "'");
                tabla = tabla.Replace("border = 1", "border = '1'");
                tabla = tabla.Replace("align=center", "align='center'");
                tabla = tabla.Replace("class=texto", "class='texto'");
                tabla = tabla.Replace("<font color='#FFFFFF'>", "");
                tabla = tabla.Replace("</font>", "");

                HtmlDocument html = new HtmlDocument();
                html.LoadHtml(tabla);
                HtmlNodeCollection tableNode = html.DocumentNode.SelectNodes("//table");

               if (tableNode != null && tableNode.Count > 0)
                {
                    foreach (HtmlNode table in tableNode)
                    {
                        ///This is the table.
                        HtmlNodeCollection rows = table.SelectNodes("tr");
                        if (rows!=null && rows.Count > 0)
                        {
                            int cols = rows[0].SelectNodes("th|td").Count;
                            //Se agrega el numero de registro al arreglo
                            cols += 1;
                            
                            int regs = table.SelectNodes("tr").Count;
                            int regsArray = regs;
                            regsArray -= conEncabezado ? 0 : 1;
                            result = new Object[cols, regsArray];
                            int curreg = 0;
                            int curCol = 0;
                            if (conEncabezado)
                            {
                                result[0, 0] = "Id";
                                curCol = 0;
                                foreach (HtmlNode cell in rows[0].SelectNodes("th"))
                                {
                                    curCol += 1;
                                    result[curCol, 0] = cell.InnerText.Trim();
                                }
                            }
                            int regArray = 0;
                            for (curreg=1; curreg < regs; curreg++)
                            {
                                HtmlNode row = rows[curreg];
                                regArray = curreg;
                                regArray -= conEncabezado ? 0 : 1;
                                result[0, regArray] = curreg;
                                curCol = 0;
                                foreach (HtmlNode cell in row.SelectNodes("td"))
                                {
                                    curCol += 1;
                                    result[curCol, regArray] = cell.InnerText.Trim();
                                }
                            }
                        }
                    }
                }
             }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<List<String>> EnvioCorreos(Envio envio,String correoNotificacion)
        {
            List<String> result = new List<String>();
            try
            {
                String raiz = String.Concat(configLayout.RutaDescarga, envio.Id, "\\", "Mails", "\\");
                List<VMDocumentacionEnvioDetalles> ListaEnvios = detalleEnvio(envio.Id);
                String sendFile = String.Empty;
                String archivo = String.Empty;
                String correo = String.Empty;
                String nombre = String.Empty;
                String poliza = String.Empty;
                List<String> notificados = new List<String>();
                List<String> correosEnviados = new List<String>();
                List<Entidades.DocumentacionEnvioDetalles> documentacionEnviodetalle = mapper.Map<List<VMDocumentacionEnvioDetalles>, List<Entidades.DocumentacionEnvioDetalles>>(ListaEnvios);
                List<CorreoPoliza> correosPoliza = GetCorreosPoliza(documentacionEnviodetalle);
                String fileName = String.Empty;
                foreach (CorreoPoliza correoPoliza in correosPoliza)
                {
                    correo = correoPoliza.Email;
                    if (!String.IsNullOrEmpty(correo))
                    {
                        fileName = correoPoliza.Poliza;
                        archivo = String.Concat(fileName, ".zip");
                        if (File.Exists(String.Concat(raiz, archivo)))
                        {
                            Thread.Sleep(3000);
                            sendFile = EnviarCorreoZip(raiz, archivo, String.Concat("Envío de documentación - Póliza ", correoPoliza.Poliza), correo, "Documentación de la póliza " + poliza, "Documentación de la póliza " + poliza);
                            if (!String.IsNullOrEmpty(sendFile))
                            {
                                result.Add("Error de envío de correo: Póliza " + correoPoliza.Poliza);
                            }
                            if(!String.IsNullOrEmpty(correoNotificacion))
                            {
                                if (!notificados.Contains(correoPoliza.Poliza))
                                {
                                    notificados.Add(correoPoliza.Poliza);
                                    Thread.Sleep(3000);
                                    sendFile = EnviarCorreoZip(raiz, archivo, String.Concat("Envío de documentación - Póliza ", correoPoliza.Poliza), correoNotificacion, "Documentación de la póliza " + poliza, "Documentación de la póliza " + poliza);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex){
                throw ex;
            }
            return result;
        }

        public List<UniversalModeloNegocio.Generales.Arbol> ArmadoArchivos(Envio envio,ref List<String> Errores)
        {
            List<UniversalModeloNegocio.Generales.Arbol> result = new List<UniversalModeloNegocio.Generales.Arbol>();
            Errores = new List<String>();
            String folderDoc = String.Empty;
            try
            {
                DAOCRUDStandar<Entidades.DocumentacionEnvio> repoDoc = _DAO.GeneraDAODocumentacionEnvio(_UserId);
                QueryParameters<Entidades.DocumentacionEnvio> parametrosdoc = new QueryParameters<Entidades.DocumentacionEnvio>();
                parametrosdoc.where = x => x.Id == envio.Id;
                Entidades.DocumentacionEnvio OTEnvio = repoDoc.EncontrarPor(parametrosdoc).FirstOrDefault();
                String OT = OTEnvio.OT;
                DAOCRUDStandar<Entidades.DocumentacionEnvioDetalles> repo = _DAO.GeneraDAODocumentacionEnvioDetalles(_UserId);
                QueryParameters<Entidades.DocumentacionEnvioDetalles> parametros = new QueryParameters<Entidades.DocumentacionEnvioDetalles>();
                parametros.where = x => x.envio == envio.Id;
                parametros.orderBy = x => x.Id;
                List<Entidades.DocumentacionEnvioDetalles> Detallearchivos = repo.EncontrarPor(parametros).ToList();
                
                String raiz = String.Concat(configLayout.RutaDescarga,envio.Id,"\\");
                folderDoc = raiz;
                raiz += envio.tipoEnvio == envioMail ? "Mails\\" : "FTP\\";
                if (! Directory.Exists(raiz))
                {
                    Directory.CreateDirectory(raiz);
                }
                UniversalModeloNegocio.Generales.Arbol carpetaArchivo = new UniversalModeloNegocio.Generales.Arbol();
                Directory.EnumerateFiles(raiz, "*.*").ToList().ForEach(x => File.Delete(x));
                String archivoOrigen = String.Empty;
                String archivoDestino = String.Empty;
                List<String> archivosEnvio = new List<String>();
                if (envio.tipoEnvio == envioAON)
                {
                    carpetaArchivo = new UniversalModeloNegocio.Generales.Arbol() { IdPadre="01-FTP", Id= "01-FTP", Descripcion1="FTP",Descripcion2=String.Empty,Descripcion3=String.Empty  };
                    result.Add(carpetaArchivo);
                    carpetaArchivo = new UniversalModeloNegocio.Generales.Arbol() { IdPadre = "01-FTP", Id = "02-CERT", Descripcion1 = "Certificados", Descripcion2 = String.Empty, Descripcion3 = String.Empty };
                    result.Add(carpetaArchivo);
                    carpetaArchivo = new UniversalModeloNegocio.Generales.Arbol() { IdPadre = "01-FTP", Id = "03-TAR", Descripcion1 = "Tarjetas", Descripcion2 = String.Empty, Descripcion3 = String.Empty };
                    result.Add(carpetaArchivo);
                    carpetaArchivo = new UniversalModeloNegocio.Generales.Arbol() { IdPadre = "01-FTP", Id = "04-POL", Descripcion1 = "Polizas", Descripcion2 = String.Empty, Descripcion3 = String.Empty };
                    result.Add(carpetaArchivo);
                    carpetaArchivo = new UniversalModeloNegocio.Generales.Arbol() { IdPadre = "04-POL", Id = "04-POL-01", Descripcion1 = String.Concat(OT,".zip"), Descripcion2 = String.Empty, Descripcion3 = String.Empty };
                    result.Add(carpetaArchivo);
                }
                else
                {
                    carpetaArchivo = new UniversalModeloNegocio.Generales.Arbol() { IdPadre = "Mails", Id = "Mails", Descripcion1 = "Mails", Descripcion2 = String.Empty, Descripcion3 = String.Empty };
                    result.Add(carpetaArchivo);
                }
                List<ArchivoZip> archivoZips = new List<ArchivoZip>();
                CompresionArchivos compresion = new CompresionArchivos();
                String idZip = String.Empty;
                String idAnterior = "NULO";
                if (envio.tipoEnvio == envioMail)
                {
                    Detallearchivos = Detallearchivos.OrderBy(x=>x.poliza).ThenBy(y=> y.iCodAfiliado).ThenBy(z => z.parentesco).ToList();
                }
                String fileName = String.Empty;
                String rutaCondiciones = String.Empty;
                List<PlanAgrupacion> ListaPlanes = GetPlanes();
                Boolean existeDocumento = false;
                // por correo se revisa en cuatro direcciones
                String FileZip = String.Empty;
                foreach (Entidades.DocumentacionEnvioDetalles doc in Detallearchivos)
                {
                    try
                    {
                        //Envío a AON solo se ejecuta una vez
                        if (envio.tipoEnvio == envioAON)
                        {
                            archivoOrigen = String.Concat(doc.poliza, "_", doc.iCodAfiliado, ".pdf");
                            archivoDestino = String.Concat(OT, "_", doc.iCodAfiliado, "_", "TARJ_TARJETA.pdf");
                            fileName = String.Concat(folderDoc, archivoOrigen);
                            if (File.Exists(fileName))
                            {
                                File.Copy(String.Concat(folderDoc, archivoOrigen), String.Concat(raiz, archivoDestino));
                                carpetaArchivo = new UniversalModeloNegocio.Generales.Arbol() { IdPadre = "03-TAR", Id = archivoDestino, Descripcion1 = archivoDestino, Descripcion2 = String.Empty, Descripcion3 = String.Empty };
                                result.Add(carpetaArchivo);

                                carpetaArchivo = new UniversalModeloNegocio.Generales.Arbol() { IdPadre = "04-POL-01", Id = "04-POL-ZIP-01-" + archivoDestino, Descripcion1 = archivoDestino, Descripcion2 = String.Empty, Descripcion3 = String.Empty };
                                result.Add(carpetaArchivo);

                                archivosEnvio.Add(String.Concat(raiz, archivoDestino));
                            }
                            else
                            {
                                Errores.Add(String.Concat("No existe credencial del afiliado ", doc.iCodAfiliado));
                            }

                            archivoOrigen = String.Concat(doc.num_Solicitud, "_", doc.certificado, "_Certificado", ".pdf");
                            archivoDestino = String.Concat(OT, "_", doc.certificado, "_", "CERT_CERTIFICADO.pdf");

                            fileName = String.Concat(folderDoc, archivoOrigen);
                            if (File.Exists(fileName))
                            {
                                if (!File.Exists(String.Concat(raiz, archivoDestino)))
                                {
                                    File.Copy(String.Concat(folderDoc, archivoOrigen), String.Concat(raiz, archivoDestino));
                                    carpetaArchivo = new UniversalModeloNegocio.Generales.Arbol() { IdPadre = "02-CERT", Id = archivoDestino, Descripcion1 = archivoDestino, Descripcion2 = String.Empty, Descripcion3 = String.Empty };
                                    result.Add(carpetaArchivo);

                                    carpetaArchivo = new UniversalModeloNegocio.Generales.Arbol() { IdPadre = "04-POL-01", Id = "04-POL-ZIP-02-" + archivoDestino, Descripcion1 = archivoDestino, Descripcion2 = String.Empty, Descripcion3 = String.Empty };
                                    result.Add(carpetaArchivo);

                                    archivosEnvio.Add(String.Concat(raiz, archivoDestino));
                                }
                            }
                            else
                            {
                                Errores.Add(String.Concat("No existe certificado del afiliado ", doc.iCodAfiliado));
                            }
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(doc.poliza))
                            {
                                idZip = doc.poliza;
                                if (result.FindIndex(x => x.Id.Equals(idZip)) < 0)
                                {
                                    if (idAnterior!="NULO" && archivoZips.Count() > 0)
                                    {
                                        AgregaCondiciones(idAnterior, rutaCondiciones, ref archivoZips, ref result, ref Errores);
                                        compresion.AgregarArchivos(String.Concat(raiz, idAnterior, ".zip"), archivoZips);
                                    }
                                    idAnterior = idZip;
                                    archivoZips = new List<ArchivoZip>();
                                    carpetaArchivo = new UniversalModeloNegocio.Generales.Arbol() { IdPadre = "Mails", Id = idZip, Descripcion1 = String.Concat(idZip, ".zip"), Descripcion2 = String.Empty, Descripcion3 = String.Empty };
                                    result.Add(carpetaArchivo);
                                }
                                else
                                {
                                    carpetaArchivo = result.Where(x => x.Id.Equals(idZip)).FirstOrDefault();
                                }
                                rutaCondiciones = getRutaCondiciones(doc.clave_Plan, ListaPlanes);
                                existeDocumento = AgregarArchivo(folderDoc, String.Concat(doc.poliza, "_", doc.iCodAfiliado), String.Concat(doc.poliza, "_", doc.iCodAfiliado), String.Empty, idZip, ref result, ref archivoZips);
                                if (!existeDocumento)
                                {
                                    Errores.Add(String.Concat("No existe credencial del afiliado ", doc.iCodAfiliado));
                                }
                                existeDocumento = AgregarArchivo(folderDoc, String.Concat(doc.num_Solicitud, "_", doc.certificado), String.Concat(doc.num_Solicitud, "_", doc.certificado), String.Empty, idZip, ref result, ref archivoZips);
                                if (!existeDocumento)
                                {
                                    Errores.Add(String.Concat("No existe certificado del afiliado ", doc.iCodAfiliado));
                                }
                                AgregarArchivo(folderDoc, doc.num_Solicitud, doc.num_Solicitud, "_Caratula", idZip, ref result, ref archivoZips);
                                AgregarArchivo(folderDoc, doc.num_Solicitud, doc.num_Solicitud, "_Carta_Bienvenida", idZip, ref result, ref archivoZips);
                                AgregarArchivo(folderDoc, doc.num_Solicitud, doc.num_Solicitud, "_Endoso_Exclusión_de_Padecimientos", idZip, ref result, ref archivoZips);
                                AgregarArchivo(folderDoc, doc.num_Solicitud, doc.num_Solicitud, "_Extra_Prima", idZip, ref result, ref archivoZips);
                                AgregarArchivo(folderDoc, doc.num_Solicitud, doc.num_Solicitud, "_Aviso_Cobro", idZip, ref result, ref archivoZips);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Errores.Add(ex.Message.ToString());
                    }
                }
                if (envio.tipoEnvio == envioMail)
                {
                    if (archivoZips.Count() > 0)
                    {
                        AgregaCondiciones(idZip, rutaCondiciones, ref archivoZips, ref result, ref Errores);
                        FileZip = String.Concat(raiz, idZip, ".zip");
                        if(!File.Exists(FileZip))
                        {
                            compresion.AgregarArchivos(String.Concat(raiz, idZip, ".zip"), archivoZips);
                        }
                    }
                    // LLenar arbol de Envíos.
                    List<CorreoPoliza> correosPoliza=GetCorreosPoliza(Detallearchivos);
                    foreach(CorreoPoliza correopoliza in correosPoliza)
                    {
                        carpetaArchivo = new UniversalModeloNegocio.Generales.Arbol() { IdPadre = correopoliza.Poliza, Id = String.Concat(correopoliza.Poliza, correopoliza.Email), Descripcion1 = correopoliza.Email, Descripcion2 = String.Empty, Descripcion3 = String.Empty };
                        result.Add(carpetaArchivo);
                    }
                }
                if (archivosEnvio.Count() > 0)
                {
                    if (envio.tipoEnvio == envioAON)
                    {
                        compresion.AgregarArchivos(String.Concat(raiz,OT, ".zip"), archivosEnvio.ToArray());
                    }
                }
                repo.Dispose();
            }
            catch (Exception ex)
            {
                Errores.Add(ex.Message.ToString());
            }
            if (Errores.Count() > 0)
            {
                List<RegistroError> registroErrores = new List<RegistroError>();
                for(int numError=0;numError<Errores.Count(); numError++)
                {
                    registroErrores.Add(new RegistroError() { Registro=numError+1, Descripcion = Errores[numError]});
                }
                String identificador = String.Concat("Errores_AON", ".xlsx");
                MAC.Utilidades.Excel.WriteExcelOpenXML<RegistroError> excelUtils = new MAC.Utilidades.Excel.WriteExcelOpenXML<RegistroError>(String.Concat(folderDoc, identificador), "dd/MM/yyyy");
                String archivoErrores=excelUtils.WriteDataB64(registroErrores);
                Errores.Clear();
                Errores.Add(archivoErrores);
            }
            return result;
        }

        private List<CorreoPoliza> GetCorreosPoliza(List<Entidades.DocumentacionEnvioDetalles> Detallearchivos)
        {
            List<CorreoPoliza> correosPoliza = new List<CorreoPoliza>();
            try
            {
                correosPoliza = Detallearchivos.Where(x => x.email.Length > 0).GroupBy(x => new { x.poliza, x.email }, (key, group) => new CorreoPoliza
                {
                    Poliza = key.poliza,
                    Email = key.email
                }).ToList();
                correosPoliza.AddRange(Detallearchivos.Where(x => x.email_Agente.Length > 0).GroupBy(x => new { x.poliza, x.email_Agente }, (key, group) => new CorreoPoliza
                {
                    Poliza = key.poliza,
                    Email = key.email_Agente
                }).ToList());
                correosPoliza.AddRange(Detallearchivos.Where(x => x.email_Promotor.Length > 0).GroupBy(x => new { x.poliza, x.email_Promotor }, (key, group) => new CorreoPoliza
                {
                    Poliza = key.poliza,
                    Email = key.email_Promotor
                }).ToList());
                correosPoliza.AddRange(Detallearchivos.Where(x => x.email_Ejecutivo.Length > 0).GroupBy(x => new { x.poliza, x.email_Ejecutivo }, (key, group) => new CorreoPoliza
                {
                    Poliza = key.poliza,
                    Email = key.email_Ejecutivo
                }).ToList());
                correosPoliza = correosPoliza.Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return correosPoliza;
        }

        private String getEmail(int columna, Entidades.DocumentacionEnvioDetalles doc)
        {
            String mail = String.Empty;
            switch (columna)
            {
                case 1:
                    mail = doc.email;
                    break;
                case 2:
                    mail = doc.email_Agente;
                    break;
                case 3:
                    mail = doc.email_Promotor;
                    break;
                case 4:
                    mail = doc.email_Ejecutivo;
                    break;
                default:
                    mail = doc.email;
                    break;
            }
            return mail;
        }

        private String getRutaCondiciones(String plan,List<PlanAgrupacion> ListaPlanes)
        {
            String ruta = String.Empty;
            PlanAgrupacion planSel = ListaPlanes.Where(x=> x.ClavePlan.Equals(plan)).FirstOrDefault();
            if (planSel != null)
            {
                ruta = planSel.AgrupacionCG;
            }
            return ruta;
        }

        private void AgregaCondiciones(String idPadre,String rutaDocumentacion,ref List<ArchivoZip> archivoZips,ref List<UniversalModeloNegocio.Generales.Arbol> result, ref List<string> Errores)
        {
            try
            {
                // si no existe poner cualquier ruta que no exista
                rutaDocumentacion = String.IsNullOrEmpty(rutaDocumentacion) ? "NEXXXX999" : rutaDocumentacion;
                string filepath = Path.Combine(configLayout.RutaDescarga, rutaDocumentacion);
                if (Directory.Exists(filepath))
                {
                    DirectoryInfo d = new DirectoryInfo(filepath);
                    String fileEnvio = String.Empty;
                    String FileAddZip = String.Empty;
                    foreach (var file in d.GetFiles("*.*"))
                    {
                        fileEnvio = file.Name;
                        FileAddZip = file.FullName;
                        if (result.FindIndex(x => x.IdPadre.Equals(idPadre) && x.Id.Equals(fileEnvio) ) < 0)
                        {
                            UniversalModeloNegocio.Generales.Arbol carpetaArchivo = new UniversalModeloNegocio.Generales.Arbol() { IdPadre = idPadre, Id = fileEnvio, Descripcion1 = String.Concat("Condiciones Generales ", fileEnvio), Descripcion2 = String.Empty, Descripcion3 = String.Empty };
                            result.Add(carpetaArchivo);
                        }
                        if (archivoZips.FindIndex(x=> x.path.Equals(FileAddZip) && x.descripcion.Equals(fileEnvio)) < 0)
                        {
                            archivoZips.Add(new ArchivoZip() { path = FileAddZip, descripcion = fileEnvio });
                        }
                    }
                }
                else
                {
                    Errores.Add(String.Concat("No se encontro el archivo de condiciones generales ", idPadre));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Boolean AgregarArchivo(String folderDoc,String idArchivo, String idArchivoDestino, String complementoArchivo, String idPadre, ref List<UniversalModeloNegocio.Generales.Arbol> result, ref List<ArchivoZip> archivoZips)
        {
            String archivoOrigen = String.Concat(idArchivo, complementoArchivo, "*.pdf");
            List<String> archivosEnc =Directory.EnumerateFiles(folderDoc, archivoOrigen).ToList();
            Boolean encontrado = false;
            if (archivosEnc.Count()>0)
            {
                encontrado = true;
                archivoOrigen = Path.GetFileName(archivosEnc[0]);
                String archivoDestino = String.Concat(idArchivoDestino, complementoArchivo, ".pdf");
                if (result.FindIndex(x => x.Id.Equals(archivoDestino) && x.IdPadre.Equals(idPadre))<0)
                {
                    UniversalModeloNegocio.Generales.Arbol carpetaArchivo = new UniversalModeloNegocio.Generales.Arbol() { IdPadre = idPadre, Id = archivoDestino, Descripcion1 = archivoDestino, Descripcion2 = String.Empty, Descripcion3 = String.Empty };
                    result.Add(carpetaArchivo);
                    archivoZips.Add(new ArchivoZip() { path = String.Concat(folderDoc, archivoOrigen), descripcion = archivoDestino });
                }
            }
            return encontrado;
        }

        public List<String> EnvioFTP(Envio envio)
        {
            List<String> result = new List<String>();
            try
            {
                String sendFile = String.Empty;
                String raiz = String.Concat(configLayout.RutaDescarga, envio.Id, "\\","FTP","\\");
                if (!Directory.Exists(raiz))
                {
                    throw new Exception("No se genero la información de envío.");
                }
                else
                {
                    List<FTPTransfer.FTPFileTransfer> ftpFiles = new List<FTPTransfer.FTPFileTransfer>();
                    DirectoryInfo di = new DirectoryInfo(raiz);
                    FileInfo[] files = di.GetFiles("*.*");
                    foreach(FileInfo f in files)
                    {
                        if (Path.GetExtension(f.FullName).ToLower().Equals(".zip"))
                        {
                            ftpFiles.Add(new FTPTransfer.FTPFileTransfer() { FtpLocalFile= f.FullName, ServerDirectory= String.Concat(this.CarpetaFTP, "POLIZAS/") });
                        }
                        else
                        {
                            if (f.FullName.IndexOf("TARJ_TARJETA") > 0)
                            {
                               ftpFiles.Add(new FTPTransfer.FTPFileTransfer() { FtpLocalFile = f.FullName, ServerDirectory = String.Concat(this.CarpetaFTP, "TARJETAS/") });
                            }
                            else
                            {
                                ftpFiles.Add(new FTPTransfer.FTPFileTransfer() { FtpLocalFile = f.FullName, ServerDirectory = String.Concat(this.CarpetaFTP, "CERTIFICADOS/") });
                            }
                        }
                    }
                    FTPTransfer.UploadFile(URLFTP,ftpFiles, this.UsuarioFTP, this.PassFTP, int.Parse(this.PuertoFTP));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public TablaGenerica buscarLayoutTabla(int Id, Boolean conEncabezado = true)
        {
            TablaGenerica data = new TablaGenerica();
            try
            {
                //Todo: Buscar el Layout
                VMDocumentacionEnvio envio = GetInf(Id);
                Object[,] dataObj = buscarLayout(Id, envio.Archivo, conEncabezado);
                
                if (dataObj != null)
                {
                    TablaGenerica.fila fila = new TablaGenerica.fila();
                    int rows = dataObj.GetLength(1);
                    int cols = dataObj.GetLength(0);
                    for (int r = 0; r < rows; r++)
                    {
                        fila = new TablaGenerica.fila();
                        for (int c = 0; c < cols; c++)
                        {
                            fila.columnas.Add(new TablaGenerica.columna() { valor= dataObj[c, r] });
                        }
                        if (r == 0 && conEncabezado)
                        {
                            data.encabezado = fila;
                        }
                        else
                        {
                            data.filas.Add(fila);
                        }
                    }
                    if (conEncabezado)
                    {
                        data.encabezado.columnas[0].valor = "Id";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public TablaGenerica datosLayout(int Id)
        {
            TablaGenerica data = new TablaGenerica();
            try
            {
                data = buscarLayoutTabla(Id);
                List<VMDocumentacionEnvioDetalles> detalle = detalleEnvio(Id);
                if (detalle.Count() > 0)
                {
                    for (int ifila = 0; ifila < data.filas.Count(); ifila++)
                    {
                        data.filas[ifila].columnas[0].valor = detalle[ifila].Id;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        private Object[,] LeerLayoutEnvio(String archivo,Boolean conEncabezado=true)
        {
            Object[,] data=null;
            try
            {
                data = new ReadExcelOpenXML(archivo).LeerHoja(conEncabezado);
                if (data != null)
                {
                    int cols = data.GetLength(0);
                    int rows = data.GetLength(1);
                    Object[,] newdata = new Object[cols+1,rows];
                    for(int r=0; r < rows; r++)
                    {
                        newdata[0, r] = r + 1;
                        //if (r == 0)
                        //{
                        //    newdata[0, r] = "Id";
                        //}
                        //else
                        //{
                        //    newdata[0, r] = (r + 1);
                        //}
                        for (int c = 0; c < cols; c++)
                        {
                            newdata[c+1, r] = data[c,r];
                        }
                    }
                    data = newdata;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public VMDocumentacionEnvio Registro(VMDocumentacionEnvio vMDocumentacionEnvio, Boolean alta = true)
        {
            try
            {
                DAOCRUDStandar<Entidades.DocumentacionEnvio> repo = _DAO.GeneraDAODocumentacionEnvio(_UserId);
                Entidades.DocumentacionEnvio documentacionEnvio = mapper.Map<VMDocumentacionEnvio, Entidades.DocumentacionEnvio>(vMDocumentacionEnvio);
                if (alta)
                {
                    documentacionEnvio.Id = 0;
                    documentacionEnvio = repo.AgregarIdentity(documentacionEnvio);
                    vMDocumentacionEnvio.Id = documentacionEnvio.Id;
                }
                else
                {
                    repo.Actualizar(documentacionEnvio);
                }
                repo.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vMDocumentacionEnvio;
        }

        public VMDocumentacionEnvioDetalles RegistroDetalle(VMDocumentacionEnvioDetalles vMDocumentacionEnvioDetalles, Boolean alta = true)
        {
            try
            {
                DAOCRUDStandar<Entidades.DocumentacionEnvioDetalles> repo = _DAO.GeneraDAODocumentacionEnvioDetalles(_UserId);
                Entidades.DocumentacionEnvioDetalles documentacionEnviodetalle = mapper.Map<VMDocumentacionEnvioDetalles, Entidades.DocumentacionEnvioDetalles>(vMDocumentacionEnvioDetalles);
                if (alta)
                {
                    documentacionEnviodetalle.Id = 0;
                    documentacionEnviodetalle = repo.AgregarIdentity(documentacionEnviodetalle);
                    vMDocumentacionEnvioDetalles.Id = documentacionEnviodetalle.Id;
                }
                else
                {
                    repo.Actualizar(documentacionEnviodetalle);
                }
                repo.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vMDocumentacionEnvioDetalles;
        }

        public List<String> ObtenerDetalle(VMDocumentacionEnvio vMDocumentacionEnvio)
        {
            List<String> results = new List<String>();
            try{
                TablaGenerica detalleOT = buscarLayoutTabla(vMDocumentacionEnvio.Id, false);
                List<VMDocumentacionEnvioDetalles> detalles = new List<VMDocumentacionEnvioDetalles>();
                VMDocumentacionEnvioDetalles detalle = new VMDocumentacionEnvioDetalles();
                foreach (TablaGenerica.fila fila in detalleOT.filas)
                {
                    detalle = new VMDocumentacionEnvioDetalles();
                    detalle.Envio = vMDocumentacionEnvio.Id;
                    detalle.ArchivoOriginal = String.Empty;
                    detalle.ArchivoFinal = String.Empty;
                    foreach (ConfigLayout.Columna columna in this.configLayout.Columnas)
                    {
                        SetData(detalle, fila.columnas[columna.Indice].valor, columna.Destino);
                    }
                    detalles.Add(detalle);
                }
                RegistroDetalle(detalles);
            }
            catch (Exception ex)
            {
                results.Add(ex.Message.ToString());
            }
            return results;
        }

        private String GetData(Object target, object propertyValue)
        {
            Type type = target.GetType();
            PropertyInfo prop = type.GetProperty("propertyName");
            return prop.GetValue(target).ToString();
        }

        private void SetData(Object target, object propertyValue,String propertyName)
        {
            Type type = target.GetType();

            PropertyInfo prop = type.GetProperty(propertyName);
            
            if (prop.PropertyType == typeof(string) || prop.PropertyType == typeof(String))
            {
                if (propertyValue == null) { propertyValue = String.Empty; }
                prop.SetValue(target, propertyValue.ToString(), null);
            }
            else if (prop.PropertyType == typeof(int))
            {
                if (propertyValue == null) { propertyValue = 0; }
                prop.SetValue(target, int.Parse(propertyValue.ToString()), null);
            }
            else if (prop.PropertyType == typeof(bool))
            {
                if (propertyValue == null) { propertyValue = false; }
                prop.SetValue(target, (bool)propertyValue, null);
            }
            else if (prop.PropertyType == typeof(bool))
            {
                if (propertyValue == null) { propertyValue = false; }
                prop.SetValue(target, (Boolean)propertyValue, null);
            }
            else if (prop.PropertyType == typeof(DateTime))
            {
                if (propertyValue != null)
                {
                    prop.SetValue(target, (DateTime)propertyValue, null);
                }
            }
        }

        public void RegistroDetalle(List<VMDocumentacionEnvioDetalles> ListvMDocumentacionEnvioDetalles, Boolean alta = true)
        {
            try
            {
                DAOCRUDStandar<Entidades.DocumentacionEnvioDetalles> repo = _DAO.GeneraDAODocumentacionEnvioDetalles(_UserId);
                foreach(VMDocumentacionEnvioDetalles vMDocumentacionEnvioDetalles in ListvMDocumentacionEnvioDetalles)
                {
                    Entidades.DocumentacionEnvioDetalles documentacionEnviodetalle = mapper.Map<VMDocumentacionEnvioDetalles, Entidades.DocumentacionEnvioDetalles>(vMDocumentacionEnvioDetalles);
                    if (alta)
                    {
                        documentacionEnviodetalle.Id = 0;
                        repo.AgregarPending(documentacionEnviodetalle);
                    }
                    else
                    {
                        repo.ActualizarPending(documentacionEnviodetalle);
                    }
                }
                repo.Save();
                repo.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Eliminar(QueryParameters<Entidades.DocumentacionEnvio> dbFiltro)
        {
            String result = String.Empty;
            try
            {
                DAOCRUDStandar<Entidades.DocumentacionEnvio> repo = _DAO.GeneraDAODocumentacionEnvio(_UserId);
                repo.Eliminar(dbFiltro);
                repo.Dispose();
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return result;
        }

        public VMDocumentacionEnvio GetInf(int Id)
        {
            VMDocumentacionEnvio vMDocumentacionEnvio = new VMDocumentacionEnvio();
            try
            {
                DAOCRUDStandar<Entidades.DocumentacionEnvio> repo = _DAO.GeneraDAODocumentacionEnvio(_UserId);
                QueryParameters<Entidades.DocumentacionEnvio> filtro = new QueryParameters<Entidades.DocumentacionEnvio>();
                filtro.where = x => x.Id == Id;
                Entidades.DocumentacionEnvio documentacionEnvio = repo.EncontrarPor(filtro).FirstOrDefault();
                vMDocumentacionEnvio= mapper.Map<Entidades.DocumentacionEnvio,VMDocumentacionEnvio >(documentacionEnvio);
                repo.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vMDocumentacionEnvio;
        }

        public List<VMDocumentacionEnvio> Consultar(QueryParameters<Entidades.DocumentacionEnvio> dbFiltro)
        {
            List<VMDocumentacionEnvio> result = new List<VMDocumentacionEnvio>();
            try
            {
                DAOCRUDStandar<Entidades.DocumentacionEnvio> repo = _DAO.GeneraDAODocumentacionEnvio(_UserId);
                List<Entidades.DocumentacionEnvio> consulta = repo.EncontrarPor(dbFiltro).ToList();
                repo.Dispose();
                result = mapper.Map<List<Entidades.DocumentacionEnvio>, List<VMDocumentacionEnvio>>(consulta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private List<VMDocumentacionEnvioDetalles> detalleEnvio(int Id)
        {
            List<VMDocumentacionEnvioDetalles> result = new List<VMDocumentacionEnvioDetalles>();
            try
            {
                QueryParameters<Entidades.DocumentacionEnvioDetalles> dbFiltro = new QueryParameters<Entidades.DocumentacionEnvioDetalles>();
                dbFiltro.where = x => x.envio == Id;
                result = ConsultarDetalle(dbFiltro);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public List<VMDocumentacionEnvioDetalles> ConsultarDetalle(QueryParameters<Entidades.DocumentacionEnvioDetalles> dbFiltro)
        {
            List<VMDocumentacionEnvioDetalles> result = new List<VMDocumentacionEnvioDetalles>();
            try
            {
                DAOCRUDStandar<Entidades.DocumentacionEnvioDetalles> repo = _DAO.GeneraDAODocumentacionEnvioDetalles(_UserId);
                List<Entidades.DocumentacionEnvioDetalles> consulta = repo.EncontrarPor(dbFiltro).ToList();
                repo.Dispose();
                result = mapper.Map<List<Entidades.DocumentacionEnvioDetalles>, List<VMDocumentacionEnvioDetalles>>(consulta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public List<PlanAgrupacion> GetPlanes()
        {
            try
            {
                String stringConnection = _DAO.ConParams;
                List<PlanAgrupacion> list = new List<PlanAgrupacion>();
                using (SqlConnection conn = new SqlConnection(stringConnection))
                {
                    DataSet dataset = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand("SELECT DISTINCT DescripcionSicas,AgrupacionCG,ClavePlanSicas " +
                                                           "FROM [dbo].[PlanProducto] " +
                                                           "GROUP BY DescripcionSicas, AgrupacionCG,ClavePlanSicas", conn);
                    adapter.SelectCommand.CommandType = CommandType.Text;

                    adapter.Fill(dataset);
                    foreach (DataRow row in dataset.Tables[0].Rows)
                    {
                        list.Add(new PlanAgrupacion
                        {
                            ClavePlan = row["ClavePlanSicas"].ToString(),
                            DescripcionSicas = String.Concat(row["AgrupacionCG"].ToString(),"-",row["DescripcionSicas"].ToString()),
                            AgrupacionCG = row["AgrupacionCG"].ToString()
                        });
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<PlanCondicionesGenerales> GetPlanesCondiciones()
        {
            List<PlanCondicionesGenerales> list = new List<PlanCondicionesGenerales>();
            try
            {
                list = GetPlanesAgrupacion();
                string filepath = String.Empty;
                for(int c=0;c<list.Count(); c++)
                {
                    filepath = Path.Combine(configLayout.RutaDescarga, list[c].AgrupacionCG);
                    if (Directory.Exists(filepath))
                    {
                        DirectoryInfo d = new DirectoryInfo(filepath);
                        foreach (var file in d.GetFiles("*.*"))
                        {
                            list[c].CondicionesGenerales = true;
                            break;
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<PlanCondicionesGenerales> GetPlanesAgrupacion()
        {
            try
            {
                String stringConnection = _DAO.ConParams;
                List<PlanCondicionesGenerales> list = new List<PlanCondicionesGenerales>();
                using (SqlConnection conn = new SqlConnection(stringConnection))
                {
                    DataSet dataset = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand("SELECT AgrupacionCG " +
                                                           "FROM [dbo].[PlanProducto] " +
                                                           "GROUP BY AgrupacionCG", conn);
                    adapter.SelectCommand.CommandType = CommandType.Text;

                    adapter.Fill(dataset);
                    foreach (DataRow row in dataset.Tables[0].Rows)
                    {
                        list.Add(new PlanCondicionesGenerales
                        {
                            DescripcionSicas = row["AgrupacionCG"].ToString(),
                            AgrupacionCG = row["AgrupacionCG"].ToString(),
                        });
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<PlanProducto> AddPlanes(List<PlanProducto> planProducto)
        {
            try
            {
                List<PlanProducto> list = new List<PlanProducto>();
                String stringConnection = _DAO.ConParams;
                using (SqlConnection conn = new SqlConnection(stringConnection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand("", conn);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Respuesta SendMailDocumentacion(VMMailMessageExtend mail)
        {
            Respuesta respuesta = new Respuesta();
            respuesta.Status = 0;
            try
            {
                _archivoPlantilla = _archivoPlantilla.Replace("MACPlatillaGeneral.html", mail.Template);
                this.plantilla = Getplantilla();
                respuesta = SendMail(mail.to,mail.asunto,mail.mensaje,mail.Files,mail.ruta,mail.asuntoDetalle,mail.accion);
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message.ToString();
            }
            return respuesta;
        }

        private String Getplantilla()
        {
            String textoPlantilla = String.Empty;
            textoPlantilla = System.IO.File.ReadAllText(_archivoPlantilla);
            return textoPlantilla;
        }

        /// <summary>
		/// Envía correos con o sin archivos añadidos que esten previamente en el servidor.
		/// usa una plantilla de base.
		/// </summary>
		/// <param name="to"></param>
		/// <param name="asunto"></param>
		/// <param name="mensaje"></param>
		/// <param name="Files"></param>
		/// <param name="ruta"></param>
		/// <param name="asuntoDetalle"></param>
		/// <param name="accion"></param>
		/// <returns>En caso de error regresa el texto de la excepción.</returns>
		public Respuesta SendMail(String to, String asunto, String mensaje, List<String> Files, String ruta, String asuntoDetalle, String accion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //Se inicializa con error
                respuesta.Status = 2;
                respuesta.Mensaje = String.Empty;
                EMailMessage eMailMessage = new EMailMessage(_correoSalida, _alias, _SecureSSL, _servicioSMTP, _puertoSMTP, _usuario, _password, ruta);
                mensaje = mensajePlantilla(asuntoDetalle, mensaje, accion);
                respuesta.Mensaje = eMailMessage.SendMail(to, asunto, mensaje, Files);
                if (String.IsNullOrEmpty(respuesta.Mensaje))
                {
                    respuesta.Status = 1;
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        private String mensajePlantilla(String asuntoDetalle, String detalle, String accion)
        {
            String textoPlantilla = this.plantilla;
            textoPlantilla = textoPlantilla.Replace("@asuntodetalle", asuntoDetalle);
            textoPlantilla = textoPlantilla.Replace("@detalle", detalle);
            if (String.IsNullOrEmpty(accion))
            {
                accion = "<a href='#' style='color: #F9FAE5; text-decoration: none; font-size: 16px; -webkit-border-radius: 5px; -moz-border-radius: 5px; border-radius: 5px; display: block; padding: 1px 8px;'>Confirmar de Enterado</a>";
            }
            textoPlantilla = textoPlantilla.Replace("@accion", accion);
            return textoPlantilla;
        }
    }
}
