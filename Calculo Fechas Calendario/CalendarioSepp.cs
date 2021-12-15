using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculo_Fechas_Calendario
{
 public  class CalendarioSepp
    {

        private string dias_ultimo_pago = "";//10 Dias  Habiles
        private string dias_acreditacion = "";//30 Dias Calendario
        private string dias_anulacion = "";// 2 Dias Habiles
        private string dias_notificacion_empleadores = ""; // 3Dias Habiles
        private string dias_respuesa_empleadores = "";// 10 Dias Habiles 
        private string dias_convercion_dnp = "";// 2 Dias Habiles 
        private int anio = 2022;
        private List<Feriados> feriados;

        public CalendarioSepp(List<Feriados> _feriados) { feriados = _feriados; }


        private string Mes(int mes) 
        { var result="";
            switch (mes) 
            {
                case 1: result = $"ENE-{anio}";  break;
                case 2: result = $"FEB-{anio}"; break;
                case 3: result = $"MAR-{anio}"; break;
                case 4: result = $"ABR-{anio}"; break;
                case 5: result = $"MAY-{anio}"; break;
                case 6: result = $"JUN-{anio}"; break;
                case 7: result = $"JUL-{anio}"; break;
                case 8: result = $"AGO-{anio}"; break;
                case 9: result = $"SEP-{anio}"; break;
                case 10: result =$"OCT-{anio}"; break;
                case 11: result =$"NOV-{anio}"; break;
                case 12: result =$"DIC-{anio-1}"; break;
            }
            return result;
        }

        public List<String> CalculoDias()
        {
            List<string> resul= new List<string>();
            var mes = 2;
        
            for (var x=1;x<=12;x ++) 
            {
                if (mes.Equals(13)) { 
                    mes = 1;
                    anio = anio + 1;
                }
          
                var fecha= new DateTime(anio, mes, 1);
                /*calcular fecha ultima de pago*/
                fecha = DiasHabiles10(fecha);
                dias_ultimo_pago = fecha.ToString("yyyy-MM-dd");
                /*fecha de acreditacion */
                fecha = DiasCalendario30(fecha);
                dias_acreditacion= fecha.ToString("yyyy-MM-dd");
                /*fecha anulacionn*/
                var fechanoti = fecha;
                fecha = DiasHabiles2(fecha);
                dias_anulacion= fecha.ToString("yyyy-MM-dd");
                /*fecha notificacion*/
                fecha = DiasHabiles3(fechanoti);
                dias_notificacion_empleadores= fecha.ToString("yyyy-MM-dd");
                /*fecha respuesta del empleador */
                fecha = DiasHabiles10(fecha.AddDays(1));
                dias_respuesa_empleadores= fecha.ToString("yyyy-MM-dd");
                /*fecha convercion DNP*/
                fecha = DiasHabiles2(fecha);
                dias_convercion_dnp= fecha.ToString("yyyy-MM-dd");


                var fechacalculadas = $"Periodo={Mes(x)} , dias_ultimo_pago ={dias_ultimo_pago} ," +
                    $"dias_acreditacion={dias_acreditacion} ,dias_anulacion={dias_anulacion} , dias_notificacion_empleadores={dias_notificacion_empleadores} ," +
                    $"dias_respuesa_empleadores={dias_respuesa_empleadores} , dias_convercion_dnp={dias_convercion_dnp}";

                Console.WriteLine(fechacalculadas);
                resul.Add(fechacalculadas);

                mes++;
            }


            return resul;
        }

        /*10 dias habiles*/
        private DateTime DiasHabiles10(DateTime fecha) 
        {
            var diasPago = 1;
            while (diasPago <= 10)
            {
                var resulfecha = from a in feriados
                                 where a.Anio == fecha.Year
                                 where a.Mes == fecha.Month
                                 where a.Dia == fecha.Day
                                 select a;

                if (resulfecha.Count() == 0)
                {
                    if (((int)fecha.DayOfWeek) != 6 && (int)fecha.DayOfWeek != 0)
                    {

                        if (diasPago < 10)
                            fecha = fecha.AddDays(1);
                        diasPago++;

                    }
                    else { fecha = fecha.AddDays(1); }
                }
                else { fecha = fecha.AddDays(1); }





            }
            return fecha;
        }

        /*30 dias calendario*/
        private DateTime DiasCalendario30(DateTime fecha) 
        {
            return fecha.AddDays(30);
        }

        /*2 dias habiles*/
        private DateTime DiasHabiles2(DateTime fecha)
        {
            var diasPago = 0;
            
            while (diasPago <= 2)
            {

                var resulfecha = from a in feriados
                                 where a.Anio == fecha.Year
                                 where a.Mes == fecha.Month
                                 where a.Dia == fecha.Day
                                 select a;

                if (resulfecha.Count() == 0)
                {
                    if (((int)fecha.DayOfWeek) != 6 && (int)fecha.DayOfWeek != 0)
                    {
                        if (diasPago < 2)
                            fecha = fecha.AddDays(1);

                        diasPago++;
                    }
                    else { fecha = fecha.AddDays(1); }
                }
                else { fecha = fecha.AddDays(1); }
            }
            return fecha;
        }

        /*3 dias habiles*/
        private DateTime DiasHabiles3(DateTime fecha)
        {
            var diasPago = 0;
            while (diasPago <= 3)
            {
                var resulfecha = from a in feriados
                                 where a.Anio == fecha.Year
                                 where a.Mes == fecha.Month
                                 where a.Dia == fecha.Day
                                 select a;

                if (resulfecha.Count() == 0)
                {
                    if (((int)fecha.DayOfWeek) != 6 && (int)fecha.DayOfWeek != 0)
                    {

                        if (diasPago < 3)
                            fecha = fecha.AddDays(1);

                        diasPago++;
                    }
                    else { fecha = fecha.AddDays(1); }
                }
                else { fecha = fecha.AddDays(1); }


            }
            return fecha;
        }

    }
}
