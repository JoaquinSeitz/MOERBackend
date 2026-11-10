using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace tp5_Trani_Joaco_Alex.Migrations
{
    /// <inheritdoc />
    public partial class InitialMoerEstetica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias_Productos",
                columns: table => new
                {
                    IdCategoriaProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias_Productos", x => x.IdCategoriaProducto);
                });

            migrationBuilder.CreateTable(
                name: "Categorias_Servicios",
                columns: table => new
                {
                    IdCategoriaServicio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias_Servicios", x => x.IdCategoriaServicio);
                });

            migrationBuilder.CreateTable(
                name: "Estados_Pedidos",
                columns: table => new
                {
                    IdEstadoPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados_Pedidos", x => x.IdEstadoPedido);
                });

            migrationBuilder.CreateTable(
                name: "Estados_Turnos",
                columns: table => new
                {
                    IdEstadoTurno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados_Turnos", x => x.IdEstadoTurno);
                });

            migrationBuilder.CreateTable(
                name: "Ingresos_Stock",
                columns: table => new
                {
                    IdIngreso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingresos_Stock", x => x.IdIngreso);
                });

            migrationBuilder.CreateTable(
                name: "Metodos_Pago",
                columns: table => new
                {
                    IdMetodoPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metodos_Pago", x => x.IdMetodoPago);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    IdProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockActual = table.Column<int>(type: "int", nullable: false),
                    Imagen = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    IdCategoriaProducto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.IdProducto);
                    table.ForeignKey(
                        name: "FK_Productos_Categorias_Productos_IdCategoriaProducto",
                        column: x => x.IdCategoriaProducto,
                        principalTable: "Categorias_Productos",
                        principalColumn: "IdCategoriaProducto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Servicios",
                columns: table => new
                {
                    IdServicio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdCategoriaServicio = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicios", x => x.IdServicio);
                    table.ForeignKey(
                        name: "FK_Servicios_Categorias_Servicios_IdCategoriaServicio",
                        column: x => x.IdCategoriaServicio,
                        principalTable: "Categorias_Servicios",
                        principalColumn: "IdCategoriaServicio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Contrasenia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdRol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Detalles_Ingresos",
                columns: table => new
                {
                    IdDetalleIngreso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioCompraUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdIngreso = table.Column<int>(type: "int", nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detalles_Ingresos", x => x.IdDetalleIngreso);
                    table.ForeignKey(
                        name: "FK_Detalles_Ingresos_Ingresos_Stock_IdIngreso",
                        column: x => x.IdIngreso,
                        principalTable: "Ingresos_Stock",
                        principalColumn: "IdIngreso",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Detalles_Ingresos_Productos_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.IdCliente);
                    table.ForeignKey(
                        name: "FK_Clientes_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Horarios_Inhabilitados",
                columns: table => new
                {
                    IdInhabilitacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "time", nullable: false),
                    HoraFin = table.Column<TimeSpan>(type: "time", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horarios_Inhabilitados", x => x.IdInhabilitacion);
                    table.ForeignKey(
                        name: "FK_Horarios_Inhabilitados_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    IdPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdEstadoPedido = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.IdPedido);
                    table.ForeignKey(
                        name: "FK_Pedidos_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pedidos_Estados_Pedidos_IdEstadoPedido",
                        column: x => x.IdEstadoPedido,
                        principalTable: "Estados_Pedidos",
                        principalColumn: "IdEstadoPedido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Turnos",
                columns: table => new
                {
                    IdTurno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "time", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdServicio = table.Column<int>(type: "int", nullable: false),
                    IdEstadoTurno = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turnos", x => x.IdTurno);
                    table.ForeignKey(
                        name: "FK_Turnos_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Turnos_Estados_Turnos_IdEstadoTurno",
                        column: x => x.IdEstadoTurno,
                        principalTable: "Estados_Turnos",
                        principalColumn: "IdEstadoTurno",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Turnos_Servicios_IdServicio",
                        column: x => x.IdServicio,
                        principalTable: "Servicios",
                        principalColumn: "IdServicio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Detalles_Pedidos",
                columns: table => new
                {
                    IdDetallePedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdPedido = table.Column<int>(type: "int", nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detalles_Pedidos", x => x.IdDetallePedido);
                    table.ForeignKey(
                        name: "FK_Detalles_Pedidos_Pedidos_IdPedido",
                        column: x => x.IdPedido,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Detalles_Pedidos_Productos_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    IdPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MontoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdMetodoPago = table.Column<int>(type: "int", nullable: false),
                    IdPedido = table.Column<int>(type: "int", nullable: true),
                    IdTurno = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.IdPago);
                    table.ForeignKey(
                        name: "FK_Pagos_Metodos_Pago_IdMetodoPago",
                        column: x => x.IdMetodoPago,
                        principalTable: "Metodos_Pago",
                        principalColumn: "IdMetodoPago",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pagos_Pedidos_IdPedido",
                        column: x => x.IdPedido,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido");
                    table.ForeignKey(
                        name: "FK_Pagos_Turnos_IdTurno",
                        column: x => x.IdTurno,
                        principalTable: "Turnos",
                        principalColumn: "IdTurno");
                });

            migrationBuilder.InsertData(
                table: "Categorias_Productos",
                columns: new[] { "IdCategoriaProducto", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Cremas hidratantes y anti-age", "Cremas Faciales" },
                    { 2, "Lociones y exfoliantes", "Cuidado Corporal" }
                });

            migrationBuilder.InsertData(
                table: "Categorias_Servicios",
                columns: new[] { "IdCategoriaServicio", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Tratamientos para el cuidado del rostro", "Facial" },
                    { 2, "Tratamientos estéticos corporales", "Corporal" }
                });

            migrationBuilder.InsertData(
                table: "Estados_Pedidos",
                columns: new[] { "IdEstadoPedido", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Pedido generado, falta preparar y pagar", "Pendiente" },
                    { 2, "Separado y esperando al cliente", "Listo para Retiro" },
                    { 3, "Pagado y entregado", "Entregado" },
                    { 4, "Pedido anulado", "Cancelado" }
                });

            migrationBuilder.InsertData(
                table: "Estados_Turnos",
                columns: new[] { "IdEstadoTurno", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Turno agendado, a la espera de ser atendido", "Pendiente" },
                    { 2, "El servicio fue realizado", "Completado" },
                    { 3, "Turno cancelado por cliente o admin", "Cancelado" }
                });

            migrationBuilder.InsertData(
                table: "Metodos_Pago",
                columns: new[] { "IdMetodoPago", "Activo", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Efectivo" },
                    { 2, true, "Tarjeta de Débito" },
                    { 3, true, "MercadoPago (Transferencia)" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "IdRol", "Nombre" },
                values: new object[,]
                {
                    { 1, "Administrador" },
                    { 2, "Cliente" }
                });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "IdProducto", "Activo", "Descripcion", "IdCategoriaProducto", "Imagen", "Nombre", "Precio", "StockActual" },
                values: new object[,]
                {
                    { 1, true, "Con ácido hialurónico 50g", 1, "crema_dia.jpg", "Crema Hidratante Día", 8500m, 15 },
                    { 2, true, "Antioxidante 30ml", 1, "serum_vitc.jpg", "Serum Vitamina C", 11000m, 10 },
                    { 3, true, "Con microgránulos naturales 200g", 2, "exfoliante.jpg", "Exfoliante Corporal", 9000m, 20 }
                });

            migrationBuilder.InsertData(
                table: "Servicios",
                columns: new[] { "IdServicio", "Descripcion", "IdCategoriaServicio", "Nombre", "Precio" },
                values: new object[,]
                {
                    { 1, "Incluye extracción y mascarilla", 1, "Limpieza Facial Profunda", 15000m },
                    { 2, "Sesión de 60 minutos", 2, "Masaje Descontracturante", 12000m },
                    { 3, "Renovación celular", 1, "Peeling Químico", 20000m }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "IdUsuario", "Contrasenia", "Email", "IdRol" },
                values: new object[,]
                {
                    { 1, "123", "admin@moer.com", 1 },
                    { 2, "123", "alex@correo.com", 2 },
                    { 3, "123", "maria@correo.com", 2 }
                });

            migrationBuilder.InsertData(
                table: "Clientes",
                columns: new[] { "IdCliente", "Apellido", "IdUsuario", "Nombre", "Telefono" },
                values: new object[,]
                {
                    { 1, "Falkenstein", 2, "Alex", "2954-111111" },
                    { 2, "Gomez", 3, "Maria", "2954-222222" }
                });

            migrationBuilder.InsertData(
                table: "Pedidos",
                columns: new[] { "IdPedido", "Fecha", "IdCliente", "IdEstadoPedido" },
                values: new object[] { 1, new DateTime(2026, 11, 9, 0, 0, 0, 0, DateTimeKind.Local), 2, 1 });

            migrationBuilder.InsertData(
                table: "Turnos",
                columns: new[] { "IdTurno", "Fecha", "HoraInicio", "IdCliente", "IdEstadoTurno", "IdServicio" },
                values: new object[] { 1, new DateTime(2026, 11, 10, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0), 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "Detalles_Pedidos",
                columns: new[] { "IdDetallePedido", "Cantidad", "IdPedido", "IdProducto", "PrecioUnitario" },
                values: new object[] { 1, 2, 1, 1, 8500m });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_IdUsuario",
                table: "Clientes",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Detalles_Ingresos_IdIngreso",
                table: "Detalles_Ingresos",
                column: "IdIngreso");

            migrationBuilder.CreateIndex(
                name: "IX_Detalles_Ingresos_IdProducto",
                table: "Detalles_Ingresos",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Detalles_Pedidos_IdPedido",
                table: "Detalles_Pedidos",
                column: "IdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_Detalles_Pedidos_IdProducto",
                table: "Detalles_Pedidos",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_Inhabilitados_IdUsuario",
                table: "Horarios_Inhabilitados",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdMetodoPago",
                table: "Pagos",
                column: "IdMetodoPago");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdPedido",
                table: "Pagos",
                column: "IdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdTurno",
                table: "Pagos",
                column: "IdTurno");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdCliente",
                table: "Pedidos",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdEstadoPedido",
                table: "Pedidos",
                column: "IdEstadoPedido");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_IdCategoriaProducto",
                table: "Productos",
                column: "IdCategoriaProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Servicios_IdCategoriaServicio",
                table: "Servicios",
                column: "IdCategoriaServicio");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_IdCliente",
                table: "Turnos",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_IdEstadoTurno",
                table: "Turnos",
                column: "IdEstadoTurno");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_IdServicio",
                table: "Turnos",
                column: "IdServicio");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdRol",
                table: "Usuarios",
                column: "IdRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Detalles_Ingresos");

            migrationBuilder.DropTable(
                name: "Detalles_Pedidos");

            migrationBuilder.DropTable(
                name: "Horarios_Inhabilitados");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Ingresos_Stock");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Metodos_Pago");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Turnos");

            migrationBuilder.DropTable(
                name: "Categorias_Productos");

            migrationBuilder.DropTable(
                name: "Estados_Pedidos");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Estados_Turnos");

            migrationBuilder.DropTable(
                name: "Servicios");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Categorias_Servicios");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
