﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    apellido = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false)
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
                name: "alquileres");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "vehiculos");
        }
    }
}
