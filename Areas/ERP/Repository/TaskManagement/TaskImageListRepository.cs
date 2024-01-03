using BizWebAPI.Areas.ERP.Models.TaskManagement;
using BizWebAPI.Common;
using Dapper;
using Microsoft.Data.Sqlite;
//using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Repository.TaskManagement
{
    public class TaskImageListRepository
    {
        private readonly string _conString;
        public TaskImageListRepository()
        {
            _conString = DbContext.ERP_Connection;
        }

        internal async Task<IEnumerable<TaskImage>> GetTaskImageById(string task_id)
        {
            try
            {
                string strSql = @" SELECT ETP.* from ERP_TASK_IMAGE ETP WHERE  ETP.REFERENCE_ID =:REFERENCE_ID  and ETP.isdelete = '0' ";
                var parameter = new { REFERENCE_ID = task_id };
                using var conn = new SqliteConnection(_conString);
                var tasks = await conn.QueryAsync<TaskImage>(strSql, parameter);

                return tasks;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> DeleteTaskImageById(string imageId)
        {
            try
            {
                string strSql = @" update ERP_TASK_IMAGE ETP set ISDELETE = '1'  WHERE  ETP.IMAGE_ID =:IMAGE_ID   ";
                var parameter = new { IMAGE_ID = imageId };
                using var conn = new SqliteConnection(_conString);
                var tasks = await conn.QueryAsync<TaskImage>(strSql, parameter);
                return " Delete success ";
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }
    }
}
