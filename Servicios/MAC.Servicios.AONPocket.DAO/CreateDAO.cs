using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades=MAC.Servicios.AONPocket.Entidades;
using MAC.Servicios.Data.DAO.EF;
using Microsoft.EntityFrameworkCore;
using EF = MAC.Servicios.Data.DAO.EF;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MAC.Servicios.AONPocket.DAO
{
 
    public class CreateDAO
    {
        private String _constring = String.Empty;
        private String _rutaLogs = String.Empty;
        public String ConParams { get => _constring; set => _constring = value; }
        public CreateDAO()
        {

        }
        public CreateDAO(String pConstring,String prutaLogs)
        {
            _constring = pConstring;
            _rutaLogs = prutaLogs;
        }

        private AONPocketContext GetContext()
        {
            return new AONPocketContext(_constring);
        }

        public DAOCRUDStandar<Entidades.DocumentacionEnvio> GeneraDAODocumentacionEnvio(String UserId)
        {
            return new DAOCRUDStandar<Entidades.DocumentacionEnvio>(UserId, GetContext());
        }

        public DAOCRUDStandar<Entidades.DocumentacionEnvioDetalles> GeneraDAODocumentacionEnvioDetalles(String UserId)
        {
            return new DAOCRUDStandar<Entidades.DocumentacionEnvioDetalles>(UserId, GetContext());
        }

    }
}
