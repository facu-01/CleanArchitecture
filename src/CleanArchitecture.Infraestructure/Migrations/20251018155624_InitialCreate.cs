using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CleanArchitecture.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles_permissions",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    permission_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles_permissions", x => new { x.role_id, x.permission_id });
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    apellido = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vehiculos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    modelo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    direccion_pais = table.Column<string>(type: "text", nullable: false),
                    direccion_departamento = table.Column<string>(type: "text", nullable: false),
                    direccion_provincia = table.Column<string>(type: "text", nullable: false),
                    direccion_ciudad = table.Column<string>(type: "text", nullable: false),
                    direccion_calle = table.Column<string>(type: "text", nullable: false),
                    vin = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    precio_monto = table.Column<decimal>(type: "numeric", nullable: false),
                    precio_tipo_moneda = table.Column<string>(type: "text", nullable: false),
                    mantenimiento_monto = table.Column<decimal>(type: "numeric", nullable: false),
                    mantenimiento_tipo_moneda = table.Column<string>(type: "text", nullable: false),
                    accesorios = table.Column<int[]>(type: "integer[]", nullable: false),
                    fecha_ultimo_alquiler = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehiculos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role_permission_configuration",
                columns: table => new
                {
                    permissions_id = table.Column<int>(type: "integer", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_permission_configuration", x => new { x.permissions_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_role_permission_configuration_permissions_permissions_id",
                        column: x => x.permissions_id,
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_permission_configuration_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "alquileres",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    vehiculo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    periodo_inicio = table.Column<DateOnly>(type: "date", nullable: false),
                    periodo_fin = table.Column<DateOnly>(type: "date", nullable: false),
                    precio_por_periodo_monto = table.Column<decimal>(type: "numeric", nullable: false),
                    precio_por_periodo_tipo_moneda = table.Column<string>(type: "text", nullable: false),
                    precio_mantenimiento_monto = table.Column<decimal>(type: "numeric", nullable: false),
                    precio_mantenimiento_tipo_moneda = table.Column<string>(type: "text", nullable: false),
                    precio_accesorios_monto = table.Column<decimal>(type: "numeric", nullable: false),
                    precio_accesorios_tipo_moneda = table.Column<string>(type: "text", nullable: false),
                    precio_total_monto = table.Column<decimal>(type: "numeric", nullable: false),
                    precio_total_tipo_moneda = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_confirmacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_denegacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_completado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_cancelacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_alquileres", x => x.id);
                    table.ForeignKey(
                        name: "fk_alquileres_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_alquileres_vehiculos_vehiculo_id",
                        column: x => x.vehiculo_id,
                        principalTable: "vehiculos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    vehiculo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    alquiler_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    comentario = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reviews", x => x.id);
                    table.ForeignKey(
                        name: "fk_reviews_alquileres_alquiler_id",
                        column: x => x.alquiler_id,
                        principalTable: "alquileres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_reviews_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_reviews_vehiculos_vehiculo_id",
                        column: x => x.vehiculo_id,
                        principalTable: "vehiculos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "id", "nombre" },
                values: new object[,]
                {
                    { 1, "ReadUser" },
                    { 2, "WriteUser" },
                    { 3, "UpdateUser" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Administrator" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "roles_permissions",
                columns: new[] { "permission_id", "role_id" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 1, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "ix_alquileres_user_id",
                table: "alquileres",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_alquileres_vehiculo_id",
                table: "alquileres",
                column: "vehiculo_id");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_alquiler_id",
                table: "reviews",
                column: "alquiler_id");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_user_id",
                table: "reviews",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_vehiculo_id",
                table: "reviews",
                column: "vehiculo_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_permission_configuration_role_id",
                table: "role_permission_configuration",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_role_id",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "role_permission_configuration");

            migrationBuilder.DropTable(
                name: "roles_permissions");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "alquileres");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "vehiculos");
        }
    }
}
