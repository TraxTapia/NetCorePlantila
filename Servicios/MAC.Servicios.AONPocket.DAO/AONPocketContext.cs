using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Diagnostics;
using EF=MAC.Servicios.Data.DAO.EF;
using Entidades=MAC.Servicios.AONPocket.Entidades;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MAC.Servicios.Data.DAO.EF;

namespace MAC.Servicios.AONPocket.DAO
{
	public class AONPocketContext : DbContext
	{
		private String _Constring=String.Empty;
		public AONPocketContext(DbContextOptions<AONPocketContext> options,String pConstring)
			: base(options)
		{
			_Constring = pConstring;
		}

		public AONPocketContext(String pConstring)
		{
			_Constring = pConstring;
		}

		public AONPocketContext()
		{

		}

		//En la api o servicio se pueden definir las opciones de configuración.
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(_Constring);
			var lf = new LoggerFactory();
			lf.AddProvider(new LoggerProvider());
			optionsBuilder.UseLoggerFactory(lf);
			//Personalización para el servicio
		}

		//Para trabajar con la tablas
		public DbSet<Entidades.DocumentacionEnvio> DocumentacionEnvio { get; set; }
		public DbSet<Entidades.DocumentacionEnvioDetalles> DocumentacionEnvioDetalles { get; set; }

		
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			//Personalización para el servicio
		}

		public override int SaveChanges()
		{
			var entidades = ChangeTracker.Entries();
			if (entidades != null)
			{
				foreach( var entidad in entidades.Where(c => c.State != EntityState.Unchanged))
				{
					if (entidad.Entity is Entidades.DocumentacionEnvio)
					{
						Auditar(entidad);
					}
				}
			}
			return base.SaveChanges();
		}

		private void Auditar(EntityEntry entidad)
		{

		}
	}
}
