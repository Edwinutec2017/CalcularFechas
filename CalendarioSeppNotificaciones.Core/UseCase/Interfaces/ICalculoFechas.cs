using CalendarioSeppNotificaciones.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CalendarioSeppNotificaciones.Core.UseCase.Interfaces
{
  public  interface ICalculoFechas
    {
        Task<List<Base64File>> CalculoFechasSepp(FechasFeriado fechas);
        Task<string> Mes(int mes);
        Task< List<DiasCalculados>> CalculoDias(string[] fecha);
        Task<DateTime> DiasHabiles10(DateTime fecha);
        Task< DateTime> DiasCalendario30(DateTime fecha);
        Task<DateTime> DiasHabiles2(DateTime fecha);
        Task<DateTime> DiasHabiles3(DateTime fecha);
    }
}
