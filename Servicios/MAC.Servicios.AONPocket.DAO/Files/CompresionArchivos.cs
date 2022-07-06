using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Compression;
using System.IO;

namespace MAC.Utilidades
{
    public class CompresionArchivos
    {
        private String _ruta = String.Empty;
        public String ruta { get => _ruta; set => _ruta= value; }

        public CompresionArchivos()
        {

        }

        public CompresionArchivos(String pRuta)
        {
            _ruta = pRuta;
        }

        public String ComprimirCarpeta(String nombreArchivo)
        {
            String result = String.Empty;
            try
            {
                ZipFile.CreateFromDirectory(_ruta, nombreArchivo);
            }
            catch (Exception ex)
            {
                result=ex.Message.ToString();
            }
            return result;
        }

        public  void AgregarArchivos(string zipPath, string[] files)
        {
            if (files == null || files.Length == 0)
            {
                return;
            }

            using (var zipArchive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    zipArchive.CreateEntryFromFile(fileInfo.FullName, fileInfo.Name);
                }
            }
        }

        public void AgregarArchivos(string zipPath, List<ArchivoZip> files)
        {
            if (files == null || files.Count == 0)
            {
                return;
            }

            using (var zipArchive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file.path);
                    zipArchive.CreateEntryFromFile(fileInfo.FullName, file.descripcion);
                }
            }
        }
    }

    public class ArchivoZip
    {
        public String path { get; set; }
        public String descripcion { get; set; }
    }
}
