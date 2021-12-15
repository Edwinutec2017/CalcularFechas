using System;
using System.Collections.Generic;

namespace Calculo_Fechas_Calendario
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Feriados> feriados = new List<Feriados>()
            {
            new Feriados(){Anio=2022,Mes=4,Dia=14 },
            new Feriados(){Anio=2022,Mes=4,Dia=15 },
            new Feriados(){Anio=2022,Mes=5,Dia=10 },
            new Feriados(){Anio=2022,Mes=6,Dia=17 },
            new Feriados(){Anio=2022,Mes=8,Dia=3 },
            new Feriados(){Anio=2022,Mes=8,Dia=4 },
            new Feriados(){Anio=2022,Mes=8,Dia=5 },
            new Feriados(){Anio=2022,Mes=9,Dia=15 },
            new Feriados(){Anio=2022,Mes=11,Dia=2 }
            };

            CalendarioSepp calendarioSepp = new CalendarioSepp(feriados);

            var resul = calendarioSepp.CalculoDias();

        }
    }
}
