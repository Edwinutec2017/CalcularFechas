using CalendarioSeppNotificaciones.Core.Domain;
using CalendarioSeppNotificaciones.Core.UseCase.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CalendarioSeppNotificaciones.Core.UseCase
{
    public class CalculoFechas : ICalculoFechas
    {
        #region ATRIBUTOS 
        private readonly IGenerarScrip _generarScrip;
        private int anio = 0;
        private  List<DiasFeriados> feriados;
        private readonly ILog _log;

        #endregion

        #region CONSTRUCTOR
        public CalculoFechas(IGenerarScrip generarScrip)
        {
            _generarScrip = generarScrip ?? throw new ArgumentException(nameof(generarScrip));
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        public  async Task<List<DiasCalculados>> CalculoDias(string[] fecha)
        {
            var calculados = new List<DiasCalculados>();
            feriados = new List<DiasFeriados>();
            var  anioScrip = anio;
            try
            {
                var mes = 2;
                foreach (var ff in fecha) 
                {
                    var fech = DateTime.Parse(ff);

                    feriados.Add(new DiasFeriados()
                    {
                        Dia = fech.Day,
                        Anio = fech.Year,
                        Mes = fech.Month
                    });
                }

                _log.Info("Iniciando calculo de fecha");

                for (var m=1;m<=12;m++) 
                {
                    if (mes.Equals(13)) 
                    {
                        mes = 1;
                        anio++;
                    }
                    CultureInfo culture = new CultureInfo("es-SV");
                    var fechaCalculada = new DiasCalculados();
                    var fechas = new DateTime(anio, mes, 1);
                    fechas = await DiasHabiles10(fechas);
                    fechaCalculada.DiaUltimoPago = fechas.ToString("yyyy-MM-dd",culture);
                    fechas = await DiasCalendario30(fechas);
                    fechaCalculada.DiaAcreditacion = fechas.ToString("yyyy-MM-dd",culture);
                    var fechaNotificacion = fechas;
                    fechas =await DiasHabiles2(fechas);
                    fechaCalculada.DiaAnulacion = fechas.ToString("yyyy-MM-dd", culture);
                    fechas = await DiasHabiles3(fechaNotificacion);
                    fechaCalculada.DiaNotificacionEmpleadores = fechas.ToString("yyyy-MM-dd", culture);
                    fechas = await DiasHabiles10(fechas.AddDays(1));
                    fechaCalculada.DiaRespuestaEmpleador = fechas.ToString("yyyy-MM-dd", culture);
                    fechas = await DiasHabiles2(fechas);
                    fechaCalculada.DiaConvercionDnp = fechas.ToString("yyyy-MM-dd", culture);
                    fechaCalculada.Periodo =await Mes(m);
                    fechaCalculada.Anio = anioScrip;
                    calculados.Add(fechaCalculada);
                    mes++;
                
                }
                _log.Info("Finalizacion en el calculo de las fechas ");
            }
            catch (Exception ex) 
            {
                _log.Error($"Ocurrio un error al generar las fechas {ex.StackTrace}");
            }

            return calculados;
        }

        public async Task <List<Base64File>> CalculoFechasSepp(FechasFeriado fechas)
        {
            var archivo = new List<Base64File>();
            try 
            {
                anio = int.Parse(fechas.Anio);
                archivo = await _generarScrip.GenerarScript(await CalculoDias(fechas.Fechas),int.Parse(fechas.Anio));
                


            } catch (Exception ex) 
            {
                _log.Error($"Ocurrio un error en CalculoFechasSepp {ex.StackTrace}");
            }


            return archivo;
        }

        public async Task<DateTime> DiasCalendario30(DateTime fecha)
        {
            return await Task.FromResult(fecha.AddDays(30));
        }

        public async Task<DateTime> DiasHabiles10(DateTime fecha)
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
            return await Task.FromResult(fecha);
        }

        public async Task<DateTime> DiasHabiles2(DateTime fecha)
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
            return await Task.FromResult(fecha);
        }

        public async Task<DateTime> DiasHabiles3(DateTime fecha)
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
            return await Task.FromResult(fecha);
        }

        public async Task<string> Mes(int mes)
        { 
            var result = string.Empty;
            switch (mes)
            {
                case 1: result = $"ENE-{anio}"; break;
                case 2: result = $"FEB-{anio}"; break;
                case 3: result = $"MAR-{anio}"; break;
                case 4: result = $"ABR-{anio}"; break;
                case 5: result = $"MAY-{anio}"; break;
                case 6: result = $"JUN-{anio}"; break;
                case 7: result = $"JUL-{anio}"; break;
                case 8: result = $"AGO-{anio}"; break;
                case 9: result = $"SEP-{anio}"; break;
                case 10: result = $"OCT-{anio}"; break;
                case 11: result = $"NOV-{anio}"; break;
                case 12: result = $"DIC-{anio - 1}"; break;
            }
            return await Task.FromResult(result);
        }
        #endregion

    }
}
