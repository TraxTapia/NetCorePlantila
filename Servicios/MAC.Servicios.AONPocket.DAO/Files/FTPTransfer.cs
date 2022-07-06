using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace MAC.Utilidades
{
    public static class FTPTransfer
    {
        public class FTPFileTransfer
        {
            public String FtpLocalFile { get; set; }
            public String ServerDirectory { get; set; }
        }

        public static void DownloadFile(List<string> Path, string localDestinationFilePath, string userName, string password)
        {
            try
            {
                foreach (var item in Path)
                {
                    DownloadFileFtp(userName, password, item, localDestinationFilePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private static void DownloadFileFtp(string userName, string password, string ftpSourceFilePath, string localDestinationFilePath)
        {
            try
            {
                int bytesRead = 0;
                byte[] buffer = new byte[5048];
                FtpWebRequest request = CreateFtpWebRequest(ftpSourceFilePath, userName, password, true);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                Stream reader = request.GetResponse().GetResponseStream();
                FileStream fileStream = new FileStream(localDestinationFilePath + request.RequestUri.AbsolutePath, FileMode.Create);
                while (true)
                {
                    bytesRead = reader.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;
                    fileStream.Write(buffer, 0, bytesRead);
                }
                fileStream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UploadFile(String host, List<FTPFileTransfer> files, string userName, string password, int port)
        {
            try
            {
                foreach (FTPFileTransfer item in files)
                {
                    FileUploadSFTP(host,userName, password, item.ServerDirectory,item.FtpLocalFile,port);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public static void UploadFile(List<string> Path, string ftpDestinationFilePath, string userName, string password)
        {
            try
            {
                foreach (var item in Path)
                {
                    UploadFileFTP(userName, password, item, ftpDestinationFilePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static void UploadFileFTP(string userName, string password, string localSourceFilePath, string ftpDestinationFilePath)
        {
            try
            {
                FileStream stream = File.OpenRead(localSourceFilePath);
                string FileName = System.IO.Path.GetFileName(localSourceFilePath);
                byte[] buffer = new byte[stream.Length];

                stream.Read(buffer, 0, buffer.Length);

                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(ftpDestinationFilePath + FileName);

                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.Credentials = new NetworkCredential(userName, password);

                request.UsePassive = false;

                request.UseBinary = true;

                request.KeepAlive = true;

                Stream reqStream = request.GetRequestStream();

                reqStream.Write(buffer, 0, buffer.Length);

                stream.Close();

                reqStream.Flush();

                reqStream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static FtpWebRequest CreateFtpWebRequest(string ftpDirectoryPath, string userName, string password, bool keepAlive = false)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(ftpDirectoryPath));

                //Set proxy to null. Under current configuration if this option is not set then the proxy that is used will get an html response from the web content gateway (firewall monitoring system)
                request.Proxy = null;
                request.Timeout = 30000;
                request.UsePassive = false;
                request.UseBinary = true;
                request.KeepAlive = keepAlive;

                request.Credentials = new NetworkCredential(userName, password);

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static void DownloadSFTPFile(string host, List<string> Path, string localDestinationFilePath, string userName, string password)
        {
            try
            {
                foreach (var item in Path)
                {
                    downloadSFTPFile(host, item, localDestinationFilePath, userName, password);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void downloadSFTPFile(string host, string pathRemoteFile, string pathLocalFile, string username, string password)
        {
            using (SftpClient sftp = new SftpClient(host, username, password))
            {
                try
                {
                    sftp.Connect();

                    Console.WriteLine("Downloading {0}", pathRemoteFile);

                    using (Stream fileStream = File.OpenWrite(pathLocalFile))
                    {
                        sftp.DownloadFile(pathRemoteFile, fileStream);
                    }

                    sftp.Disconnect();
                }
                catch (Exception er)
                {
                    throw er;
                }
            }
        }

        // you could pass the host, port, usr, pass, and uploadFile as parameters
        public static void FileUploadSFTP(String host, String username, String password,String Carpeta, String uploadFile, int port)
        {
            try
            {
                 String archivoDescarga = String.Concat(Carpeta, Path.GetFileName(uploadFile));
                 using (var client = new SftpClient(host, port, username, password))
                 {
                    client.Connect();
                    if (client.IsConnected)
                    {
                        using (var fileStream = new FileStream(uploadFile, FileMode.Open))
                        {
                            client.BufferSize = 4 * 1024; // bypass Payload error large files
                            client.UploadFile(fileStream, archivoDescarga);
                        }
                    }
                }
            }
            catch (Exception er)
            {
                throw er;
            }
        }
    }
}