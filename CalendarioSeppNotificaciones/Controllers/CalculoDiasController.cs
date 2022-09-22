using CalendarioSeppNotificaciones.Core.Domain;
using CalendarioSeppNotificaciones.Core.UseCase.Interfaces;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CalendarioSeppNotificaciones.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CalculoDiasController : ControllerBase
    {
        #region ATRIBUTOS
        private readonly ICalculoFechas _calculoFechas;
        private readonly ILog _log;
        #endregion

        #region CONSTRUCTOR
        public CalculoDiasController(ICalculoFechas calculoFechas) 
        {
            _calculoFechas = calculoFechas ?? throw new ArgumentNullException(nameof(calculoFechas));
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        }
        #endregion

        #region CONTROLLER
        [HttpPost]
        public async Task<List<Base64File>> CalculoFechas([FromBody] FechasFeriado fechas)
        {

            try
            {
                if (fechas.Anio != null && fechas.Fechas.Length > 0)
                {
                  return  await _calculoFechas.CalculoFechasSepp(fechas);
                }
            }
            catch (Exception ex)
            {
                _log.Error($"Ocurrio un erro {ex.StackTrace}");
            }

            return new List<Base64File>();
        }
        #endregion
    }
}
