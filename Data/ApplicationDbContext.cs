using Microsoft.EntityFrameworkCore;
using System;
using MoerEsteticaBackend.Models;

namespace MoerEstetica.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tablas Normalizadas 3FN según DER MOER Estética
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Categoria_Servicio> Categorias_Servicios { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Estado_Turno> Estados_Turnos { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Horario_Inhabilitado> Horarios_Inhabilitados { get; set; }

        public DbSet<Categoria_Producto> Categorias_Productos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Estado_Pedido> Estados_Pedidos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Detalle_Pedido> Detalles_Pedidos { get; set; }

        public DbSet<Ingreso_Stock> Ingresos_Stock { get; set; }
        public DbSet<Detalle_Ingreso> Detalles_Ingresos { get; set; }

        public DbSet<Metodo_Pago> Metodos_Pago { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relaciones para Pagos (Nulleable para Pedido o Turno)
            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Pedido)
                .WithMany()
                .HasForeignKey(p => p.IdPedido)
                .IsRequired(false);

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Turno)
                .WithMany()
                .HasForeignKey(p => p.IdTurno)
                .IsRequired(false);

            // --- SEEDING DE DATOS (Ficticios para pruebas) ---

            // 1. Roles
            modelBuilder.Entity<Rol>().HasData(
                new Rol { IdRol = 1, Nombre = "Administrador" },
                new Rol { IdRol = 2, Nombre = "Cliente" }
            );

            // 2. Usuarios
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { IdUsuario = 1, Email = "admin@moer.com", Contrasenia = "123", IdRol = 1 },
                new Usuario { IdUsuario = 2, Email = "alex@correo.com", Contrasenia = "123", IdRol = 2 },
                new Usuario { IdUsuario = 3, Email = "maria@correo.com", Contrasenia = "123", IdRol = 2 }
            );

            // 3. Clientes
            modelBuilder.Entity<Cliente>().HasData(
                new Cliente { IdCliente = 1, Nombre = "Alex", Apellido = "Falkenstein", Telefono = "2954-111111", IdUsuario = 2 },
                new Cliente { IdCliente = 2, Nombre = "Maria", Apellido = "Gomez", Telefono = "2954-222222", IdUsuario = 3 }
            );

            // 4. Categorías de Servicios
            modelBuilder.Entity<Categoria_Servicio>().HasData(
                new Categoria_Servicio { IdCategoriaServicio = 1, Nombre = "Facial", Descripcion = "Tratamientos para el cuidado del rostro" },
                new Categoria_Servicio { IdCategoriaServicio = 2, Nombre = "Corporal", Descripcion = "Tratamientos estéticos corporales" }
            );

            // 5. Servicios
            modelBuilder.Entity<Servicio>().HasData(
                new Servicio { IdServicio = 1, Nombre = "Limpieza Facial Profunda", Descripcion = "Incluye extracción y mascarilla", Precio = 15000m, IdCategoriaServicio = 1 },
                new Servicio { IdServicio = 2, Nombre = "Masaje Descontracturante", Descripcion = "Sesión de 60 minutos", Precio = 12000m, IdCategoriaServicio = 2 },
                new Servicio { IdServicio = 3, Nombre = "Peeling Químico", Descripcion = "Renovación celular", Precio = 20000m, IdCategoriaServicio = 1 }
            );

            // 6. Estados de Turno
            modelBuilder.Entity<Estado_Turno>().HasData(
                new Estado_Turno { IdEstadoTurno = 1, Nombre = "Pendiente", Descripcion = "Turno agendado, a la espera de ser atendido" },
                new Estado_Turno { IdEstadoTurno = 2, Nombre = "Completado", Descripcion = "El servicio fue realizado" },
                new Estado_Turno { IdEstadoTurno = 3, Nombre = "Cancelado", Descripcion = "Turno cancelado por cliente o admin" }
            );

            // 7. Categorías de Productos
            modelBuilder.Entity<Categoria_Producto>().HasData(
                new Categoria_Producto { IdCategoriaProducto = 1, Nombre = "Cremas Faciales", Descripcion = "Cremas hidratantes y anti-age" },
                new Categoria_Producto { IdCategoriaProducto = 2, Nombre = "Cuidado Corporal", Descripcion = "Lociones y exfoliantes" }
            );

            // 8. Productos
            modelBuilder.Entity<Producto>().HasData(
                new Producto { IdProducto = 1, Nombre = "Crema Hidratante Día", Descripcion = "Con ácido hialurónico 50g", Precio = 8500m, StockActual = 15, Imagen = "crema_dia.jpg", Activo = true, IdCategoriaProducto = 1 },
                new Producto { IdProducto = 2, Nombre = "Serum Vitamina C", Descripcion = "Antioxidante 30ml", Precio = 11000m, StockActual = 10, Imagen = "serum_vitc.jpg", Activo = true, IdCategoriaProducto = 1 },
                new Producto { IdProducto = 3, Nombre = "Exfoliante Corporal", Descripcion = "Con microgránulos naturales 200g", Precio = 9000m, StockActual = 20, Imagen = "exfoliante.jpg", Activo = true, IdCategoriaProducto = 2 }
            );

            // 9. Estados de Pedido
            modelBuilder.Entity<Estado_Pedido>().HasData(
                new Estado_Pedido { IdEstadoPedido = 1, Nombre = "Pendiente", Descripcion = "Pedido generado, falta preparar y pagar" },
                new Estado_Pedido { IdEstadoPedido = 2, Nombre = "Listo para Retiro", Descripcion = "Separado y esperando al cliente" },
                new Estado_Pedido { IdEstadoPedido = 3, Nombre = "Entregado", Descripcion = "Pagado y entregado" },
                new Estado_Pedido { IdEstadoPedido = 4, Nombre = "Cancelado", Descripcion = "Pedido anulado" }
            );

            // 10. Métodos de Pago
            modelBuilder.Entity<Metodo_Pago>().HasData(
                new Metodo_Pago { IdMetodoPago = 1, Nombre = "Efectivo", Activo = true },
                new Metodo_Pago { IdMetodoPago = 2, Nombre = "Tarjeta de Débito", Activo = true },
                new Metodo_Pago { IdMetodoPago = 3, Nombre = "MercadoPago (Transferencia)", Activo = true }
            );

            // Semillas opcionales de prueba: 1 Turno y 1 Pedido para arrancar
            modelBuilder.Entity<Turno>().HasData(
                new Turno { IdTurno = 1, Fecha = DateTime.Today.AddDays(1), HoraInicio = new TimeSpan(10, 0, 0), IdCliente = 1, IdServicio = 1, IdEstadoTurno = 1 }
            );

            modelBuilder.Entity<Pedido>().HasData(
                new Pedido { IdPedido = 1, Fecha = DateTime.Today, IdCliente = 2, IdEstadoPedido = 1 }
            );

            modelBuilder.Entity<Detalle_Pedido>().HasData(
                new Detalle_Pedido { IdDetallePedido = 1, Cantidad = 2, PrecioUnitario = 8500m, IdPedido = 1, IdProducto = 1 }
            );
        }
    }
}