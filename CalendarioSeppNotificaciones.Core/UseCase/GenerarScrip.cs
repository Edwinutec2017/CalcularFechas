using CalendarioSeppNotificaciones.Core.Domain;
using CalendarioSeppNotificaciones.Core.UseCase.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace CalendarioSeppNotificaciones.Core.UseCase
{
    public class GenerarScrip : IGenerarScrip
    {
        private readonly ILog _log;
        public GenerarScrip() 
        {
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        public async Task<Base64File> FileSql(List<DiasCalculados> calendario)
        {
            Base64File base64File = new Base64File();
            string[] textoscrip = new string[10] ;
            try
            {
                TypeInfo typeInfo = typeof(DiasCalculados).GetTypeInfo();
                int count = 0;
                foreach (var type in typeInfo.DeclaredProperties) {

                    var atribute = type.GetCustomAttributes( true);

                    textoscrip[count] = ((Description)atribute[0]).Name;
                    count++;
                }


                if (calendario != null && calendario.Count > 0)
                {
                    string file = "scrip.txt";
                    FileStream fileStream = new FileStream(file, FileMode.CreateNew);

                    StreamWriter write = new StreamWriter(fileStream);

                    write.WriteLine("USE ServicioEmpresas;");
                    write.WriteLine("");
                    write.WriteLine("GO");
                    write.WriteLine("");
                    write.WriteLine("-- INSERT");
                    
                    foreach (var fechas in calendario)
                    {
                        write.WriteLine(" ");
                        write.WriteLine($"INSERT INTO [dbo].[SE_TBL_CALENDARIZACION] ");
                        write.WriteLine($"([{textoscrip[0]}],[{textoscrip[1]}],[{textoscrip[2]}],[{textoscrip[3]}],[{textoscrip[4]}],[{textoscrip[5]}],[{textoscrip[6]}],[{textoscrip[7]}],[{textoscrip[8]}],[{textoscrip[9]}])");
                        write.WriteLine($"VALUES ({fechas.TipoCalendario},{fechas.Anio},'{fechas.Periodo}','{fechas.DiaUltimoPago}','{fechas.DiaAcreditacion}','{fechas.DiaAnulacion}','{fechas.DiaNotificacionEmpleadores}','{fechas.DiaRespuestaEmpleador}','{fechas.DiaConvercionDnp}',{fechas.AdicionaPor})");
                        write.WriteLine(" ");
                        write.WriteLine("GO");
                    }

                    write.Close();
                    fileStream.Close();

                    base64File.Base64 = Convert.ToBase64String(File.ReadAllBytes(fileStream.Name));
                    base64File.Name = $"V{DateTime.Now:ddMMyyyy}_SQL_AUTOSERVICIOEMPLEADORES_INSERT_CALENDARIO2022SEP.SQL";

                    if (base64File.Base64 != null && base64File.Base64.Length > 0) 
                    {
                        File.Delete(fileStream.Name);
                    
                    }
                    _log.Info("Finaliacion en la creacion y base64 del archivo");
                }
            }
            catch (Exception ex)
            {
                _log.Error($"Ocurrio un error al momento de crear  el archivo.!!  {ex.StackTrace}");
            }

            return await Task.FromResult(base64File);


        }


        public async Task<Base64File> FileSqlRollback(int anio)
        {
            Base64File base64File = new Base64File();
            try
            {
                if (anio > 2020)
                {
                    _log.Info("Creacion del Rollback");
                    string file = "rollback.txt";
                    FileStream fileStream = new FileStream(file, FileMode.CreateNew);

                    StreamWriter write = new StreamWriter(fileStream);

                    write.WriteLine("USE ServicioEmpresas;");
                    write.WriteLine("");
                    write.WriteLine("GO");
                    write.WriteLine("");
                    write.WriteLine($"DELETE FROM SE_TBL_CALENDARIZACION WHERE ANIO={anio};");

                    write.Close();
                    fileStream.Close();

                    base64File.Base64 = Convert.ToBase64String(File.ReadAllBytes(fileStream.Name));
                    base64File.Name = $"U{DateTime.Now.ToString("ddMMyyyy")}_SQL_AUTOSERVICIOEMPLEADORES_INSERT_CALENDARIO2022SEP_ROLLBACK.SQL";

                    if (base64File.Base64 != null && base64File.Base64.Length > 0)
                    {
                        File.Delete(fileStream.Name);

                    }
                    _log.Info("Finaliacion en la creacion y base64 del archivo Rollback");
                }
                else 
                {
                    _log.ErrorFormat("Erro en el formato del año");
                }
            }
            catch (Exception ex) 
            {
                _log.Error($"Ocurrio un erro al crear el Rollback {ex.StackTrace}");
            }

            return await Task.FromResult(base64File) ;
        }

        public async Task<List<Base64File>> GenerarScript(List<DiasCalculados> diasCalculados, int anio)
        {
        return new List<Base64File>() { await FileSql(diasCalculados), await FileSqlRollback(anio) };
         
        }
    }
}
