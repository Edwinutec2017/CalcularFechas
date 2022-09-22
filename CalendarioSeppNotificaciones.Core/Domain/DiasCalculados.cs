using System;
using System.Collections.Generic;
using System.Text;

namespace CalendarioSeppNotificaciones.Core.Domain
{
   public class DiasCalculados
    {
        [Description(Name = "SE_TIPO_CALENDARIO_ID", Ignore = false)]
        public int TipoCalendario { get { return 1; } }
        [Description(Name = "ANIO", Ignore = false)]
        public int Anio { get; set; }
        [Description(Name = "PERIODO_DEVENGUE", Ignore =false)]
        public string Periodo { get; set; }
        [Description(Name = "FECHA_ULTIMA_PAGO", Ignore = false)]
        public string DiaUltimoPago { get; set; }
        [Description(Name = "FECHA_PROCESO_ACREDITACION", Ignore = false)]
        public string DiaAcreditacion { get; set; }
        [Description(Name = "FECHA_ANULACION", Ignore = false)]
        public string DiaAnulacion { get; set; }
        [Description(Name = "FECHA_NOTIFICACION_EMPLEADORES", Ignore = false)]
        public string DiaNotificacionEmpleadores { get; set; }
        [Description(Name = "FECHA_RESPUESTA_EMPLEADOR", Ignore = false)]
        public string DiaRespuestaEmpleador { get; set; }
        [Description(Name = "FECHA_CONVERSION_DNP", Ignore = false)]
        public string DiaConvercionDnp { get; set; }

        [Description(Name = "ADICIONADO_POR", Ignore = false)]
        public string AdicionaPor { get { return "SYSTEM_USER"; } }


    }
}
