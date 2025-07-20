using Common.DataTransferObjects.Filter.CollectionPaging;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataAccess.Extensions
{
    public static class DbContextTensions
    {
        public static async Task<PagedList<T>> ExecutePagedStoredProcedureAsync<T>(
           this DbContext context,
           string storedProcedure,
           SqlParameter[] parameters,
           Func<DbDataReader, T> map,
           int pageNumber,
           int pageSize)
        {
            using var connection = context.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = storedProcedure;
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            using var reader = await command.ExecuteReaderAsync();

            var results = new List<T>();
            int totalCount = 0;

            while (await reader.ReadAsync())
            {
                results.Add(map(reader));
            }

            if (await reader.NextResultAsync() && await reader.ReadAsync())
            {
                totalCount = reader.GetInt32(0);
            }
            else
            {
                totalCount = results.Count;
            }

            return new PagedList<T>(results, new PagingMetadata(totalCount, pageNumber, pageSize));
        }

        public static async Task<T> ExecuteSingleResultStoredProcedureAsync<T>(
            this DbContext context,
            string storedProcedure,
            SqlParameter[] parameters,
            Func<DbDataReader, T> map)
        {
            var connection = context.Database.GetDbConnection();
            var shouldClose = connection.State != System.Data.ConnectionState.Open;

            if (shouldClose)
                await connection.OpenAsync();

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = storedProcedure;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddRange(parameters);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return map(reader);
                }

                return default!;
            }
            finally
            {
                if (shouldClose)
                    await connection.CloseAsync();
            }
        }


        //public static async Task<T> ExecuteEditResultStoredProcedure<T>(
        //    this DbContext context,
        //    string storedProcedure,
        //    SqlParameter[] parameters,
        //    Func<DbDataReader, T> map)
        //{
        //    using var connection = context.Database.GetDbConnection();
        //    await connection.OpenAsync();

        //    using var command = connection.CreateCommand();
        //    command.CommandText = storedProcedure;
        //    command.CommandType = System.Data.CommandType.StoredProcedure;
        //    command.Parameters.AddRange(parameters);

        //    using var reader = await command.ExecuteReaderAsync();

        //    if (await reader.ReadAsync())
        //    {
        //        return map(reader);
        //    }

        //    return default!;
        //}

    }
}
