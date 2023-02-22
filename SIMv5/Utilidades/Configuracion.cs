using Oracle.ManagedDataAccess.Client;
using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.SqlServer;

namespace SIM
{
    public sealed class EntityFrameworkConfiguration : DbConfiguration
    {
        public EntityFrameworkConfiguration()
        {
            this.AddInterceptor(new EfCommandInterceptor());
        }
    }

    public sealed class EfCommandInterceptor
    : DbCommandInterceptor
    {
        /// <summary>
        /// Called when Reader is executing.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="interceptionContext"></param>
        /// <inheritdoc />
        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            // Ajuste de consultas para evitar problemas con unicode, búsquedas fulltext full text y que siempre utilice los mismos separadores de decimales
            if (command is OracleCommand)
            {
                if (command.CommandText.Contains("N''"))
                {
                    command.CommandText = command.CommandText.Replace("N''", "''");
                }

                if (command.CommandText.Contains("TO_NCLOB"))
                {
                    command.CommandText = command.CommandText.Replace("TO_NCLOB", "TO_CHAR");
                }

                if (command.CommandText.Contains("number(3,0)"))
                {
                    command.CommandText = command.CommandText.Replace("number(3,0)", "number(10,0)");
                }

                if (command.CommandText.Contains("\"GENERAL\".\"BUSQUEDA\""))
                {
                    command.CommandText = command.CommandText.Replace("LOWER", "CATSEARCH").Replace("\") LIKE '%", "\", '*").Replace("%')", "*', '') > 0)");
                }

                (new OracleCommand("ALTER SESSION SET NLS_NUMERIC_CHARACTERS = '.,'", (OracleConnection)command.Connection)).ExecuteNonQuery();
            }

            try
            {
                base.ReaderExecuting(command, interceptionContext);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}