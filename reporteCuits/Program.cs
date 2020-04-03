using System;
using System.Collections.Generic;
using System.Threading;

namespace reporteCuits
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine();
                Console.WriteLine(" ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine("   Iniciando reporte de Cuits...");
                Console.WriteLine(" ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine();

                // Creo un arreglo vacio para almacenar los Cuits.
                List<String> listadoCuits = new List<string>();

                // Leer el directorio en busca de cuits y lo almaceno en una arreglo.
                listadoCuits = Procesos.LoteDeCarga();

                // crear informe con los cuits detectados.
                String nombreReporte;
                nombreReporte = Procesos.CrearReporte(listadoCuits);

                // Envio en informe con los cuits detectados.
                Procesos.EnviarReporte(nombreReporte);

                Console.WriteLine(" ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine("   Proceso finalizado.");
                Console.WriteLine(" ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine();

                Thread.Sleep(20000);

            }
            catch (Exception excepcion)
            {
                Console.WriteLine();
                Console.WriteLine("\tAlgo salio mal !!!");
                Console.WriteLine();
                Console.WriteLine("\t" + excepcion.Message);
                Console.WriteLine();
                Thread.Sleep(6000);
            }
        }
    }
}
