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
    public class StatusListRepository
    {
        private readonly string _conString;
        public StatusListRepository()
        {
            _conString = DbContext.ERP_Connection;
        }

        internal async Task<IEnumerable<TaskStatusList>> GetStatusList()
        {
            try
            {
                string strSql = " select SC.STATUS_CATEGORY_ID, SC.CATEGORY_NAME, SL.STATUS_NAME, SL.TASK_STATUS_LIST_ID, SL.PROGRESS_STATUS " +
                                " from ERP_TASK_STATUS_CATEGORY SC left join ERP_TASK_STATUS_LIST SL on " +
                                " SC.STATUS_CATEGORY_ID = SL.STATUS_CATEGORY_ID where SL.isdelete ='0' ";
                //string strSql = " SELECT * from ERP_TASK_PROJECT  where PROJECT_ID=:PROJECT_ID ";

                // var parameter = new { PROJECT_ID = project_id };

                using var conn = new SqliteConnection(_conString);
                var sTATUS = await conn.QueryAsync<TaskStatusList>(strSql, null);

                return sTATUS;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskStatusList>> GetStatusById(string statusId)
        {
            try
            {
                string strSql = " SELECT SC.STATUS_CATEGORY_ID, SC.CATEGORY_NAME, SL.STATUS_NAME, SL.TASK_STATUS_LIST_ID " +
                                "from ERP_TASK_STATUS_LIST SL, ERP_TASK_STATUS_CATEGORY SC where SL.STATUS_CATEGORY_ID = SC.STATUS_CATEGORY_ID AND " +
                                "SL.TASK_STATUS_LIST_ID= :TASK_STATUS_LIST_ID ";

                var parameter = new { TASK_STATUS_LIST_ID = statusId };

                using var conn = new SqliteConnection(_conString);
                var sTATUS = await conn.QueryAsync<TaskStatusList>(strSql, parameter);

                return sTATUS.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskStatusList>> GetCategoryWiseStatusList(string CategoryId)
        {
            try
            {
                string strSql = " SELECT SC.STATUS_CATEGORY_ID, SC.CATEGORY_NAME, SL.STATUS_NAME, SL.TASK_STATUS_LIST_ID " +
                                "from ERP_TASK_STATUS_LIST SL, ERP_TASK_STATUS_CATEGORY SC where SL.STATUS_CATEGORY_ID = SC.STATUS_CATEGORY_ID AND " +
                                "SC.STATUS_CATEGORY_ID= :STATUS_CATEGORY_ID ";

                var parameter = new { STATUS_CATEGORY_ID = CategoryId };

                using var conn = new SqliteConnection(_conString);
                var sTATUS = await conn.QueryAsync<TaskStatusList>(strSql, parameter);

                return sTATUS.ToList();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> CreateStatus(string statusName, string statusCategoryId, string progressStatus, string createdBy, DateTime createDate)
        {
            try
            {
                string strSql = " INSERT INTO ERP_TASK_STATUS_LIST  (STATUS_NAME,PROGRESS_STATUS,STATUS_CATEGORY_ID,CREATE_BY,CREATE_DATE,ISDELETE) " +
                                "  VALUES (:STATUS_NAME,:PROGRESS_STATUS,:STATUS_CATEGORY_ID, :CREATE_BY,:CREATE_DATE,'0' ) ";

                var parameter = new { 
                                    STATUS_NAME = statusName, 
                                    PROGRESS_STATUS = progressStatus, 
                                    STATUS_CATEGORY_ID = statusCategoryId, 
                                    CREATE_BY= createdBy, 
                                    CREATE_DATE= createDate
                                    };

                using var conn = new SqliteConnection(_conString);
                var sTATUS = await conn.QueryAsync(strSql, parameter);

                return statusName + "is created" ;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<TaskStatusList> UpdteStatusById(string statusId, string statusName, string progressStatus, string statusCategoryId,string updatedBy, DateTime updateDate)
        {
            try
            {
                string strSql = " UPDATE ERP_TASK_STATUS_LIST SET  STATUS_NAME=:STATUS_NAME, PROGRESS_STATUS=:PROGRESS_STATUS, STATUS_CATEGORY_ID =: STATUS_CATEGORY_ID, UPDATE_BY =: UPDATE_BY, UPDATE_DATE =: UPDATE_DATE where TASK_STATUS_LIST_ID= :TASK_STATUS_LIST_ID";

                var parameter = new 
                { 
                    STATUS_NAME = statusName, 
                    PROGRESS_STATUS = progressStatus,
                    TASK_STATUS_LIST_ID= statusId,
                    STATUS_CATEGORY_ID = statusCategoryId,
                    UPDATE_BY = updatedBy,
                    UPDATE_DATE = updateDate
                };

                using var conn = new SqliteConnection(_conString);
                var sTATUS = await conn.QueryAsync(strSql, parameter);

                return sTATUS.FirstOrDefault();
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> DeleteStatusById(string statusId)
        {
            try
            {
                string strSql = " UPDATE ERP_TASK_STATUS_LIST  SET ISDELETE='1' where TASK_STATUS_LIST_ID= :TASK_STATUS_LIST_ID";

                var parameter = new
                {
                    TASK_STATUS_LIST_ID = statusId
                };

                using var conn = new SqliteConnection(_conString);
                var sTATUS = await conn.QueryAsync(strSql, parameter);

                return statusId + "task status is deleted";
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

    }
}
