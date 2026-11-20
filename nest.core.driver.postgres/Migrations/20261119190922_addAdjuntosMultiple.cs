using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace nest.core.driver.postgres.Migrations;

/// <inheritdoc />
public partial class _20261119190922_addAdjuntosMultiple : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "registro_asistencia_adjuntos",
            schema: "rrhh",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false),
                EmpresaId = table.Column<int>(type: "integer", nullable: false),
                RegistroAsistenciaId = table.Column<long>(type: "bigint", nullable: false),
                AdjuntoId = table.Column<long>(type: "bigint", nullable: false),
                Tipo = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_registro_asistencia_adjuntos", x => x.Id);
                table.ForeignKey(
                    name: "FK_registro_asistencia_adjuntos_adjunto_AdjuntoId",
                    column: x => x.AdjuntoId,
                    principalSchema: "dbo",
                    principalTable: "adjunto",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_registro_asistencia_adjuntos_registro_asistencia_RegistroAs~",
                    column: x => x.RegistroAsistenciaId,
                    principalSchema: "rrhh",
                    principalTable: "registro_asistencia",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "registro_asistencia_adjuntos_audit",
            schema: "rrhh",
            columns: table => new
            {
                AuditId = table.Column<long>(type: "bigint", nullable: false),
                AdjuntoId = table.Column<long>(type: "bigint", nullable: false),
                AuditAccion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                AuditApp = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                AuditAppVersion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                AuditAssemblyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                AuditFecha = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                AuditHost = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                AuditIpRemoteOrigin = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                AuditIsHttps = table.Column<bool>(type: "boolean", nullable: false),
                AuditMethod = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                AuditOrigin = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                AuditProtocol = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                AuditReferer = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                AuditRequestId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                AuditUserAgent = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                AuditUsuario = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                EmpresaId = table.Column<int>(type: "integer", nullable: false),
                Id = table.Column<long>(type: "bigint", nullable: false),
                RegistroAsistenciaId = table.Column<long>(type: "bigint", nullable: false),
                Tipo = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_registro_asistencia_adjuntos_audit", x => x.AuditId);
            });

        migrationBuilder.UpdateData(
            schema: "security",
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "1",
            column: "ConcurrencyStamp",
            value: "25c5752f-e0bd-427b-936e-22d1194e2895");

        migrationBuilder.UpdateData(
            schema: "security",
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "2",
            column: "ConcurrencyStamp",
            value: "dd7518aa-a4b2-4efd-a7c6-39804523f4d6");

        migrationBuilder.InsertData(
            schema: "dbo",
            table: "adjunto_tipo",
            columns: new[] { "Id", "Activo", "Nombre", "NombreCorto" },
            values: new object[] { 7, true, "Firma digital", "FIR" });

        migrationBuilder.CreateIndex(
            name: "IX_registro_asistencia_adjuntos_AdjuntoId",
            schema: "rrhh",
            table: "registro_asistencia_adjuntos",
            column: "AdjuntoId");

        migrationBuilder.CreateIndex(
            name: "IX_registro_asistencia_adjuntos_EmpresaId",
            schema: "rrhh",
            table: "registro_asistencia_adjuntos",
            column: "EmpresaId");

        migrationBuilder.CreateIndex(
            name: "IX_registro_asistencia_adjuntos_RegistroAsistenciaId",
            schema: "rrhh",
            table: "registro_asistencia_adjuntos",
            column: "RegistroAsistenciaId");

        migrationBuilder.CreateIndex(
            name: "IX_registro_asistencia_adjuntos_audit_Id",
            schema: "rrhh",
            table: "registro_asistencia_adjuntos_audit",
            column: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "registro_asistencia_adjuntos",
            schema: "rrhh");

        migrationBuilder.DropTable(
            name: "registro_asistencia_adjuntos_audit",
            schema: "rrhh");

        migrationBuilder.DeleteData(
            schema: "dbo",
            table: "adjunto_tipo",
            keyColumn: "Id",
            keyValue: 7);

        migrationBuilder.UpdateData(
            schema: "security",
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "1",
            column: "ConcurrencyStamp",
            value: null);

        migrationBuilder.UpdateData(
            schema: "security",
            table: "AspNetRoles",
            keyColumn: "Id",
            keyValue: "2",
            column: "ConcurrencyStamp",
            value: null);
    }
}
