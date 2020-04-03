using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace reporteCuits
{
    static class Procesos
    {
        const String archivoConfiguracion = @"\config\reporteConfig.xml";
        public static List<String> LoteDeCarga()
        {
            List<String> listadoAux = new List<string>();
            try
            {
                Console.Write("\tVerificando directorio... ");
                Configuracion configuracion = new Configuracion(Directory.GetCurrentDirectory() + archivoConfiguracion);
                String rutaCUITs = configuracion.rutaCuits;
                DirectoryInfo directorio = new DirectoryInfo(rutaCUITs);
                foreach (var cuit in directorio.GetDirectories()) { listadoAux.Add(cuit.Name); };
                Console.WriteLine("Ok.");
                Console.WriteLine();
            }
            catch (Exception excepcion)
            {
                Console.WriteLine("\tAlgo salio mal !!!");
                Console.WriteLine();
                Console.WriteLine("\t" + excepcion.Message);
                Console.WriteLine();
            }
            return listadoAux;
        }

        public static String CrearReporte(List<String> auxList)
        {
            String nombreReporte = String.Empty;
            StreamWriter reporte = null;
            try
            {
                Console.Write("\tGenerando reporte... ");
                Configuracion configuracion = new Configuracion(Directory.GetCurrentDirectory() + archivoConfiguracion);
                int count = 0;
                DateTime fecha = DateTime.Now;
                nombreReporte = "Reporte-" + fecha.ToString("dd-MM-yyyy") + ".md";
                reporte = new StreamWriter(configuracion.rutaReporte + nombreReporte);
                reporte.WriteLine();
                reporte.WriteLine("# Informe de CUITs activos FistData");
                reporte.WriteLine("");
                reporte.WriteLine("Cuits activos con alta en el sistema: ");
                reporte.WriteLine("");
                reporte.WriteLine("*Fecha: " + fecha + "*");
                reporte.WriteLine("");
                reporte.WriteLine("~~~");
                auxList.ForEach(delegate (String cuit)
                {
                    if (count < 3)
                    {
                        reporte.Write(cuit + "\t");
                        count++;
                    }
                    else
                    {
                        reporte.WriteLine(cuit);
                        count = 0;
                    }
                });
                reporte.WriteLine("");
                reporte.WriteLine("~~~");
                reporte.WriteLine("");
                reporte.WriteLine("**Total de Cuits reportados: " + auxList.Count.ToString() + "**");
                reporte.WriteLine("");
                reporte.WriteLine("*Archivado: " + configuracion.rutaReporte + nombreReporte + "*");
                Console.WriteLine("Ok.");
                Console.WriteLine();
            }
            catch (Exception excepcion)
            {
                Console.WriteLine("Fallo.");
                Console.WriteLine();
                Console.WriteLine(excepcion.Message);
                Console.WriteLine();
            }
            finally
            {
                if (reporte != null)
                {
                    reporte.Close();
                }
            }
            return nombreReporte;
        }

        public static void EnviarReporte(String nombreReporte)
        {

            try
            {
                Console.Write("\tEnviando reporte... ");
                Configuracion configuracion = new Configuracion(Directory.GetCurrentDirectory() + archivoConfiguracion);
                FileInfo fileInfo = new FileInfo(configuracion.rutaReporte + nombreReporte);
                long fileLength = fileInfo.Length;
                FileStream fs = new FileStream(configuracion.rutaReporte + nombreReporte, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                byte[] reporteBinario = br.ReadBytes((int)fileLength);
                String str = Encoding.Default.GetString(reporteBinario);
                Dictionary<string, string> values = new Dictionary<string, string>
                {
                    { "token", configuracion.token },
                    { "channels", configuracion.canalSlack },
                    { "content", str },
                    { "file", configuracion.rutaReporte + nombreReporte },
                    { "initial_comment", "Reporte de CUITs activos." },
                    { "filename", nombreReporte }
                };
                HttpClient client = new HttpClient();
                var content = new FormUrlEncodedContent(values);
                var response = client.PostAsync(configuracion.urlAPI, content);
                Console.WriteLine("Ok.");
                Console.WriteLine();
            }
            catch (Exception excepcion)
            {
                Console.WriteLine("Fallo.");
                Console.WriteLine();
                Console.WriteLine(excepcion.Message);
                Console.WriteLine();
            }
        }
    }
}
