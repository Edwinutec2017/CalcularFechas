using CalendarioSeppNotificaciones.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CalendarioSeppNotificaciones.Core.UseCase.Interfaces
{
  public  interface IGenerarScrip
    {
        Task<List<Base64File>> GenerarScript(List<DiasCalculados> diasCalculados,int anio);
        Task<Base64File> FileSql(List<DiasCalculados> calendario);
        Task<Base64File> FileSqlRollback(int anio);

    }
}
