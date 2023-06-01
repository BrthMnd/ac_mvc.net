using System;
using System.Collections.Generic;
using System.Drawing;

namespace Tiendav3.Models
{
    public partial class Producto
    {
        public Producto()
        {
            Facturas = new HashSet<Factura>();
        }

        public int Codigo { get; set; }
        public string Nombre { get; set; } = null!;
        public double Precio { get; set; }
        public int Cantidad { get; set; }

        public virtual ICollection<Factura> Facturas { get; set; }

        public void CalcularValorTotal()
        {
            double ValorTotal = Cantidad * Precio;
        }
    }
}
