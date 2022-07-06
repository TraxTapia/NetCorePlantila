using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MAC.Servicios.Data.DAO.EF;
using UniversalModeloNegocio.AONPocket;

namespace MAC.Servicios.AONPocket.Negocio
{
    public class Mapeado
    {
        private MapperConfiguration _config;
        public MapperConfiguration config { get => _config; set => _config = value; }

        public Mapeado()
        {
			_config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Entidades.DocumentacionEnvio, VMDocumentacionEnvio>()
                .ForMember(x => x.OT, x => x.MapFrom(y => y.OT))
                .ForMember(x => x.Archivo, x => x.MapFrom(y => y.Archivo))
                .ForMember(x => x.FechaEnvio, x => x.MapFrom(y => y.FechaEnvio))
                .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.UsuCambio, x => x.MapFrom(y => y.UsuCambio))
                .ForMember(x => x.FechaCambio, x => x.MapFrom(y => y.FechaCambio));

                cfg.CreateMap<VMDocumentacionEnvio, Entidades.DocumentacionEnvio>()
                .ForMember(x => x.OT, x => x.MapFrom(y => y.OT))
                .ForMember(x => x.Archivo, x => x.MapFrom(y => y.Archivo))
                .ForMember(x => x.FechaEnvio, x => x.MapFrom(y => y.FechaEnvio))
                .ForMember(x => x.FechaAlta, x => x.Ignore())
                .ForMember(x => x.FechaCambio, x => x.Ignore())
                .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.UsuAlta, x => x.Ignore())
                .ForMember(x => x.UsuCambio, x => x.Ignore());
    
                cfg.CreateMap<VMDocumentacionEnvioDetalles, Entidades.DocumentacionEnvioDetalles>()
                .ForMember(x => x.apellido_Materno, x => x.MapFrom(y => y.ApellidoMaterno))
                .ForMember(x => x.apellido_Paterno, x => x.MapFrom(y => y.ApellidoPaterno))
                .ForMember(x => x.archivo_Final, x => x.MapFrom(y => y.ArchivoFinal))
                .ForMember(x => x.archivo_Original, x => x.MapFrom(y => y.ArchivoOriginal))
                .ForMember(x => x.certificado, x => x.MapFrom(y => y.Certificado))
                .ForMember(x => x.clave_Plan, x => x.MapFrom(y => y.ClavePlan))
                .ForMember(x => x.plan_dsc, x => x.MapFrom(y => y.Plan))
                .ForMember(x => x.contrato, x => x.MapFrom(y => y.Contrato))
                .ForMember(x => x.email, x => x.MapFrom(y => y.Email))
                .ForMember(x => x.email_Agente, x => x.MapFrom(y => y.EmailAgente))
                .ForMember(x => x.email_Ejecutivo, x => x.MapFrom(y => y.EmailEjecutivo))
                .ForMember(x => x.email_Promotor, x => x.MapFrom(y => y.EmailPromotor))
                .ForMember(x => x.envio, x => x.MapFrom(y => y.Envio))
                .ForMember(x => x.num_Solicitud, x => x.MapFrom(y => y.NumSolicitud))
                .ForMember(x => x.fecha_Envio, x => x.MapFrom(y => y.FechaEnvio))
                .ForMember(x => x.envioxeMail, x => x.MapFrom(y => y.EnvioxEmail))
                .ForMember(x => x.envioxftp, x => x.MapFrom(y => y.EnvioxFTP))
                .ForMember(x => x.iCodAfiliado, x => x.MapFrom(y => y.ICodAfiliado))
                .ForMember(x => x.nombre, x => x.MapFrom(y => y.Nombre))
                .ForMember(x => x.parentesco, x => x.MapFrom(y => y.Parentesco))
                .ForMember(x => x.poliza, x => x.MapFrom(y => y.Poliza))
                .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.FechaAlta, x => x.Ignore())
                .ForMember(x => x.FechaCambio, x => x.Ignore())
                .ForMember(x => x.UsuAlta, x => x.Ignore())
                .ForMember(x => x.UsuCambio, x => x.Ignore());

                cfg.CreateMap<Entidades.DocumentacionEnvioDetalles, VMDocumentacionEnvioDetalles>()
                .ForMember(x => x.ApellidoMaterno, x => x.MapFrom(y => y.apellido_Materno))
                .ForMember(x => x.ApellidoPaterno, x => x.MapFrom(y => y.apellido_Paterno))
                .ForMember(x => x.ArchivoFinal, x => x.MapFrom(y => y.archivo_Final))
                .ForMember(x => x.ArchivoOriginal, x => x.MapFrom(y => y.archivo_Original))
                .ForMember(x => x.Certificado, x => x.MapFrom(y => y.certificado))
                .ForMember(x => x.Plan, x => x.MapFrom(y => y.plan_dsc))
                .ForMember(x => x.Contrato, x => x.MapFrom(y => y.contrato))
                .ForMember(x => x.Email, x => x.MapFrom(y => y.email))
                .ForMember(x => x.EmailAgente, x => x.MapFrom(y => y.email_Agente))
                .ForMember(x => x.EmailEjecutivo, x => x.MapFrom(y => y.email_Ejecutivo))
                .ForMember(x => x.EmailPromotor, x => x.MapFrom(y => y.email_Promotor))
                .ForMember(x => x.Envio, x => x.MapFrom(y => y.envio))
                .ForMember(x => x.NumSolicitud, x => x.MapFrom(y => y.num_Solicitud))
                .ForMember(x => x.FechaEnvio, x => x.MapFrom(y => y.fecha_Envio))
                .ForMember(x => x.EnvioxEmail, x => x.MapFrom(y => y.envioxeMail))
                .ForMember(x => x.EnvioxFTP, x => x.MapFrom(y => y.envioxftp))
                .ForMember(x => x.ICodAfiliado, x => x.MapFrom(y => y.iCodAfiliado))
                .ForMember(x => x.Nombre, x => x.MapFrom(y => y.nombre))
                .ForMember(x => x.Parentesco, x => x.MapFrom(y => y.parentesco))
                .ForMember(x => x.Poliza, x => x.MapFrom(y => y.poliza))
                .ForMember(x => x.Id, x => x.MapFrom(y => y.Id));
            });
		}
    }
}
