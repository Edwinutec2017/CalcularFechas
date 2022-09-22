using System;
using System.Collections.Generic;
using System.Text;

namespace CalendarioSeppNotificaciones.Core.Domain
{

    public class Description:Attribute
    {
        public string Name { get; set; }
        public bool Ignore { get; set; }

    }
}
