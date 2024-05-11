namespace T3_RevillaFernandezHectorRolando.Models
{
    public class Compra
    {
        public int id { get; set; }
        public string nombreProducto { get; set; }
        public DateTime fechaCompra { get; set; }
        public int cantidad { get; set; }
        public decimal precioUnitario { get; set; }
    }
}
