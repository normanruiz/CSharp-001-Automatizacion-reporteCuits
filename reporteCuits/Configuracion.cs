using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace reporteCuits
{
    public class Configuracion
    {

        public String rutaCuits { get; set; }

        public String rutaReporte { get; set; }

        public String token { get; set; }

        public String canalSlack { get; set; }

        public String urlAPI { get; set; }

        public XDocument archivoConfiguracion;

        public Configuracion(String rutaConfiguracion)
        {
            StringBuilder result = new StringBuilder();
            archivoConfiguracion = XDocument.Load(rutaConfiguracion);
            var configuraciones = from configuracion in archivoConfiguracion.Descendants("configuracion")
                                  select new
                                  {
                                      Header = configuracion.Attribute("value").Value,
                                      Children = configuracion.Descendants("parametro")
                                  };

            foreach (var configuracion in configuraciones)
            {
                foreach (var parametro in configuracion.Children)
                {
                    if (configuracion.Header == "rutaCuits")
                    {
                        rutaCuits = parametro.Attribute("value").Value;
                    }
                    if (configuracion.Header == "rutaReporte")
                    {
                        rutaReporte = parametro.Attribute("value").Value;
                    }
                    if (configuracion.Header == "urlAPI")
                    {
                        urlAPI = parametro.Attribute("value").Value;
                    }
                    if (configuracion.Header == "token")
                    {
                        token = parametro.Attribute("value").Value;
                    }
                    if (configuracion.Header == "canalSlack")
                    {
                        canalSlack = parametro.Attribute("value").Value;
                    }

                }
            }
        }
    }
}
