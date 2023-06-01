using System;
using System.Collections.Generic;

namespace Tiendav3.Models
{
    public partial class Factura
    {
        public int NumeroFactura { get; set; }
        public int? ClienteCedula { get; set; }
        public double ValorTotal { get; set; }
        public DateTime Fecha { get; set; }
        public int? ProductoCodigo { get; set; }
        public int Cantidad { get; set; }
        public double Valor { get; set; }

        public virtual Cliente? ClienteCedulaNavigation { get; set; }
        public virtual Producto? ProductoCodigoNavigation { get; set; }
        public void CalcularValorTotal()
        {
            ValorTotal = Cantidad * Valor;
        }
    }
}
