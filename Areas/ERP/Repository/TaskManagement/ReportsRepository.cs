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
    public class ReportsRepository
    {
        private readonly string _conString;

        public ReportsRepository()
        {
            _conString = DbContext.ERP_Connection;
        }

        internal async Task<IEnumerable<TaskReport>> TaskReport(string decryptProjectId, string decryptModuleId, string decryptStartDate, string decryptEndDate, string decryptEmpId)
        {
            try
            {
                string sqlReport = string.Empty;
                if (string.IsNullOrEmpty(decryptProjectId) && string.IsNullOrEmpty(decryptModuleId) && string.IsNullOrEmpty(decryptStartDate.ToString()) && string.IsNullOrEmpty(decryptEndDate.ToString()) && string.IsNullOrEmpty(decryptEmpId))
                {
                    sqlReport = " select distinct TA.ACTIVITY_ID, TTL.COMPLETE_BY_ADMIN, TTL.TASKLIST_ID, TTL.TASK_DETAILS,TA.END_TIME,TA.START_TIME, (select PROJECT_NAME from ERP_TASK_PROJECT P where P.PROJECT_ID = TTL.PROJECT_ID ) as PROJECT_NAME, " +
                        "(select MODULE_NAME from ERP_TASK_MODULE M where M.MODULE_ID = TTL.MODULE_ID ) as MODULE_NAME, " +
                        "(select STATUS_NAME from ERP_TASK_STATUS_LIST TSL where TSL.TASK_STATUS_LIST_ID = TA.TASK_STATUS_LIST_ID ) as ACTIVITY_STATUS, " +
                        "TTL.IMAGE, TTL.LINK, TTA.TEMPACTIVITY_TYPE, TTA.TIME_EXTEND_NOTE,TTA.EXTEND_TIME,  TA.REMARKS " +
                        ", TRUNC (MOD ( (TA.END_TIME - TA.START_TIME) * 24, 24)) as DURATION_OF ,NVL(TO_CHAR(TA.EMP_ID), TAS.EMP_ID)EMP_ID  " +
                        "from ERP_TASK_TASK_LIST TTL left join ERP_TASK_TASK_ASSIGNEE TAS   on TTL.TASKLIST_ID = TAS.TASKLIST_ID " +
                        "left join ERP_TASK_ACTIVITY TA   on TAS.TASK_ASSIGNEE_ID = TA.TASK_ASSIGNEE_ID " +
                        "left join ERP_TASK_TEMPACTIVITY TTA   on TAS.TASK_ASSIGNEE_ID = TTA.TASK_ASSIGNEE_ID " +
                        "Where TTL.ISDELETE = 0 order by TA.ACTIVITY_ID desc ";
                    using var conn = new SqliteConnection(_conString);
                    var report = await conn.QueryAsync<TaskReport>(sqlReport, null);
                    return report;
                }
                // for Project
                else if ((!string.IsNullOrEmpty(decryptProjectId) || string.IsNullOrEmpty(decryptModuleId)) && string.IsNullOrEmpty(decryptStartDate.ToString()) && string.IsNullOrEmpty(decryptEndDate.ToString()) && string.IsNullOrEmpty(decryptEmpId))
                {
                    sqlReport = " select distinct TA.ACTIVITY_ID, TTL.COMPLETE_BY_ADMIN,  TTL.TASKLIST_ID, TTL.TASK_DETAILS,TA.END_TIME,TA.START_TIME,TTL.ESTIMATED_TIME, (select PROJECT_NAME from ERP_TASK_PROJECT P where P.PROJECT_ID = TTL.PROJECT_ID ) as PROJECT_NAME, " +
                        "(select MODULE_NAME from ERP_TASK_MODULE M where M.MODULE_ID = TTL.MODULE_ID ) as MODULE_NAME, " +
                        "(select STATUS_NAME from ERP_TASK_STATUS_LIST TSL where TSL.TASK_STATUS_LIST_ID = TA.TASK_STATUS_LIST_ID ) as ACTIVITY_STATUS, " +
                        "TTL.IMAGE, TTL.LINK, TTA.TEMPACTIVITY_TYPE, TTA.TIME_EXTEND_NOTE,TTA.EXTEND_TIME,  TA.REMARKS " +
                        ", TRUNC (MOD ( (TA.END_TIME - TA.START_TIME) * 24, 24)) as DURATION_OF,NVL(TO_CHAR(TA.EMP_ID),  TAS.EMP_ID)EMP_ID " +
                        "from ERP_TASK_TASK_LIST TTL left join ERP_TASK_TASK_ASSIGNEE TAS   on TTL.TASKLIST_ID = TAS.TASKLIST_ID " +
                        "left join ERP_TASK_ACTIVITY TA   on TAS.TASK_ASSIGNEE_ID = TA.TASK_ASSIGNEE_ID " +
                        "left join ERP_TASK_TEMPACTIVITY TTA   on TAS.TASK_ASSIGNEE_ID = TTA.TASK_ASSIGNEE_ID " +
                        "Where TTL.ISDELETE = 0 AND TTL.PROJECT_ID=:PROJECT_ID order by TA.ACTIVITY_ID desc ";
                    using var conn = new SqliteConnection(_conString);
                    var param = new { PROJECT_ID = decryptProjectId, MODULE_ID = decryptModuleId, START_TIME = decryptStartDate, END_TIME = decryptEndDate, EMP_ID = decryptEmpId };
                    var report = await conn.QueryAsync<TaskReport>(sqlReport, param);
                    return report;
                }
                // for Module
                else if (string.IsNullOrEmpty(decryptProjectId) && !string.IsNullOrEmpty(decryptModuleId) && string.IsNullOrEmpty(decryptStartDate.ToString()) && string.IsNullOrEmpty(decryptEndDate.ToString()) && string.IsNullOrEmpty(decryptEmpId))
                {
                    sqlReport = " select distinct TA.ACTIVITY_ID, TTL.COMPLETE_BY_ADMIN,  TTL.TASKLIST_ID, TTL.TASK_DETAILS,TA.END_TIME,TA.START_TIME,TTL.ESTIMATED_TIME, (select PROJECT_NAME from ERP_TASK_PROJECT P where P.PROJECT_ID = TTL.PROJECT_ID ) as PROJECT_NAME, " +
                        "(select MODULE_NAME from ERP_TASK_MODULE M where M.MODULE_ID = TTL.MODULE_ID ) as MODULE_NAME, " +
                        "(select STATUS_NAME from ERP_TASK_STATUS_LIST TSL where TSL.TASK_STATUS_LIST_ID = TA.TASK_STATUS_LIST_ID ) as ACTIVITY_STATUS, " +
                        "TTL.IMAGE, TTL.LINK, TTA.TEMPACTIVITY_TYPE, TTA.TIME_EXTEND_NOTE,TTA.EXTEND_TIME,  TA.REMARKS " +
                        ", TRUNC (MOD ( (TA.END_TIME - TA.START_TIME) * 24, 24)) as DURATION_OF,NVL(TA.EMP_ID, TAS.EMP_ID)EMP_ID " +
                        "from ERP_TASK_TASK_LIST TTL left join ERP_TASK_TASK_ASSIGNEE TAS   on TTL.TASKLIST_ID = TAS.TASKLIST_ID " +
                        "left join ERP_TASK_ACTIVITY TA   on TAS.TASK_ASSIGNEE_ID = TA.TASK_ASSIGNEE_ID " +
                        "left join ERP_TASK_TEMPACTIVITY TTA   on TAS.TASK_ASSIGNEE_ID = TTA.TASK_ASSIGNEE_ID " +
                        "Where TTL.ISDELETE = 0 AND TTL.MODULE_ID=:MODULE_ID order by TA.ACTIVITY_ID desc ";
                    using var conn = new SqliteConnection(_conString);
                    var param = new { PROJECT_ID = decryptProjectId, MODULE_ID = decryptModuleId, START_TIME = decryptStartDate, END_TIME = decryptEndDate, EMP_ID = decryptEmpId };
                    var report = await conn.QueryAsync<TaskReport>(sqlReport, param);
                    return report;
                }
                // for date range
                else if (string.IsNullOrEmpty(decryptProjectId) && string.IsNullOrEmpty(decryptModuleId) && !string.IsNullOrEmpty(decryptStartDate.ToString()) && !string.IsNullOrEmpty(decryptEndDate.ToString()) && string.IsNullOrEmpty(decryptEmpId))
                {
                    sqlReport = " select distinct TA.ACTIVITY_ID,  TTL.COMPLETE_BY_ADMIN, TTL.TASKLIST_ID, TTL.TASK_DETAILS,TA.END_TIME,TA.START_TIME,TTL.ESTIMATED_TIME, (select PROJECT_NAME from ERP_TASK_PROJECT P where P.PROJECT_ID = TTL.PROJECT_ID ) as PROJECT_NAME, " +
                        "(select MODULE_NAME from ERP_TASK_MODULE M where M.MODULE_ID = TTL.MODULE_ID ) as MODULE_NAME, " +
                        "(select STATUS_NAME from ERP_TASK_STATUS_LIST TSL where TSL.TASK_STATUS_LIST_ID = TA.TASK_STATUS_LIST_ID ) as ACTIVITY_STATUS, " +
                        " TTL.LINK, TTA.TEMPACTIVITY_TYPE, TTA.TIME_EXTEND_NOTE,TTA.EXTEND_TIME,  TA.REMARKS " +
                        ", TRUNC (MOD ( (TA.END_TIME - TA.START_TIME) * 24, 24)) as DURATION_OF,NVL(TA.EMP_ID, TAS.EMP_ID)EMP_ID  " +
                        "from ERP_TASK_TASK_LIST TTL left join ERP_TASK_TASK_ASSIGNEE TAS   on TTL.TASKLIST_ID = TAS.TASKLIST_ID " +
                        "left join ERP_TASK_ACTIVITY TA   on TAS.TASK_ASSIGNEE_ID = TA.TASK_ASSIGNEE_ID " +
                        "left join ERP_TASK_TEMPACTIVITY TTA   on TAS.TASK_ASSIGNEE_ID = TTA.TASK_ASSIGNEE_ID " +
                        "Where TTL.ISDELETE = 0 AND TA.START_TIME between " + decryptStartDate + " AND " + decryptEndDate + " order by TA.ACTIVITY_ID desc ";
                    using var conn = new SqliteConnection(_conString);
                    var param = new { PROJECT_ID = decryptProjectId, MODULE_ID = decryptModuleId, START_TIME = decryptStartDate, END_TIME = decryptEndDate, EMP_ID = decryptEmpId };
                    var report = await conn.QueryAsync<TaskReport>(sqlReport, param);
                    return report;
                }
                // for module or project with date range without emp
                ////else if ((!string.IsNullOrEmpty(decryptProjectId) || !string.IsNullOrEmpty(decryptModuleId)) && !string.IsNullOrEmpty(decryptStartDate.ToString()) && !string.IsNullOrEmpty(decryptEndDate.ToString()) && string.IsNullOrEmpty(decryptEmpId))
                ////{
                ////    if (string.IsNullOrEmpty(decryptModuleId))
                ////    {

                ////        sqlReport = " select distinct TA.ACTIVITY_ID,  TTL.TASKLIST_ID, TTL.TASK_DETAILS,TA.END_TIME,TA.START_TIME,TTL.ESTIMATED_TIME, (select PROJECT_NAME from ERP_TASK_PROJECT P where P.PROJECT_ID = TTL.PROJECT_ID ) as PROJECT_NAME, " +
                ////            "(select MODULE_NAME from ERP_TASK_MODULE M where M.MODULE_ID = TTL.MODULE_ID ) as MODULE_NAME, " +
                ////            "(select STATUS_NAME from ERP_TASK_STATUS_LIST TSL where TSL.TASK_STATUS_LIST_ID = TA.TASK_STATUS_LIST_ID ) as ACTIVITY_STATUS, " +
                ////            "TTL.IMAGE, TTL.LINK, TTA.TEMPACTIVITY_TYPE, TTA.TIME_EXTEND_NOTE,TTA.EXTEND_TIME,  TA.REMARKS " +
                ////            ", TRUNC (MOD ( (TA.END_TIME - TA.START_TIME) * 24, 24)) as DURATION_OF,NVL(TA.EMP_ID, TAS.EMP_ID)EMP_ID  " +
                ////            "from ERP_TASK_TASK_LIST TTL left join ERP_TASK_TASK_ASSIGNEE TAS   on TTL.TASKLIST_ID = TAS.TASKLIST_ID " +
                ////            "left join ERP_TASK_ACTIVITY TA   on TAS.TASK_ASSIGNEE_ID = TA.TASK_ASSIGNEE_ID " +
                ////            "left join ERP_TASK_TEMPACTIVITY TTA   on TAS.TASK_ASSIGNEE_ID = TTA.TASK_ASSIGNEE_ID " +
                ////            "Where TTL.ISDELETE = 0 AND TTL.PROJECT_ID=:PROJECT_ID  AND TA.START_TIME between " + decryptStartDate + " AND " + decryptEndDate + " order by TA.ACTIVITY_ID desc ";

                ////    }
                ////    else
                ////    {
                ////        sqlReport = " select distinct TA.ACTIVITY_ID,  TTL.TASKLIST_ID, TTL.TASK_DETAILS,TA.END_TIME,TA.START_TIME,TTL.ESTIMATED_TIME, (select PROJECT_NAME from ERP_TASK_PROJECT P where P.PROJECT_ID = TTL.PROJECT_ID ) as PROJECT_NAME, " +
                ////                              " (select MODULE_NAME from ERP_TASK_MODULE M where M.MODULE_ID = TTL.MODULE_ID ) as MODULE_NAME, " +
                ////                              " (select STATUS_NAME from ERP_TASK_STATUS_LIST TSL where TSL.TASK_STATUS_LIST_ID = TA.TASK_STATUS_LIST_ID ) as ACTIVITY_STATUS, " +
                ////                              " TTL.IMAGE, TTL.LINK, TTA.TEMPACTIVITY_TYPE, TTA.TIME_EXTEND_NOTE,TTA.EXTEND_TIME,  TA.REMARKS " +
                ////                              " , TRUNC (MOD ( (TA.END_TIME - TA.START_TIME) * 24, 24)) as DURATION_OF,NVL(TA.EMP_ID, TAS.EMP_ID)EMP_ID " +
                ////                              " from ERP_TASK_TASK_LIST TTL left join ERP_TASK_TASK_ASSIGNEE TAS   on TTL.TASKLIST_ID = TAS.TASKLIST_ID " +
                ////                              " left join ERP_TASK_ACTIVITY TA   on TAS.TASK_ASSIGNEE_ID = TA.TASK_ASSIGNEE_ID " +
                ////                              " left join ERP_TASK_TEMPACTIVITY TTA   on TAS.TASK_ASSIGNEE_ID = TTA.TASK_ASSIGNEE_ID " +
                ////                              " Where TTL.ISDELETE = 0 AND TTL.PROJECT_ID=:PROJECT_ID  AND TTL.MODULE_ID=:MODULE_ID   AND TA.START_TIME between  " + decryptStartDate + " AND " + decryptEndDate + " order by TA.ACTIVITY_ID desc ";

                ////    }
                ////    using var conn = new SqliteConnection(_conString);
                ////    var param = new { PROJECT_ID = decryptProjectId, MODULE_ID = decryptModuleId, START_TIME = decryptStartDate, END_TIME = decryptEndDate, EMP_ID = decryptEmpId };
                ////    var report = await conn.QueryAsync<TaskReport>(sqlReport, param);
                ////    return report;
                ////}

                //only for user
                else if (string.IsNullOrEmpty(decryptProjectId) && string.IsNullOrEmpty(decryptModuleId) && string.IsNullOrEmpty(decryptStartDate.ToString()) && string.IsNullOrEmpty(decryptEndDate.ToString()) && !string.IsNullOrEmpty(decryptEmpId))
                {
                    sqlReport = " select distinct TA.ACTIVITY_ID, TTL.COMPLETE_BY_ADMIN,  TTL.TASKLIST_ID, TTL.TASK_DETAILS,TA.END_TIME,TA.START_TIME,TTL.ESTIMATED_TIME, (select PROJECT_NAME from ERP_TASK_PROJECT P where P.PROJECT_ID = TTL.PROJECT_ID ) as PROJECT_NAME, " +
                        "(select MODULE_NAME from ERP_TASK_MODULE M where M.MODULE_ID = TTL.MODULE_ID ) as MODULE_NAME, " +
                        "(select STATUS_NAME from ERP_TASK_STATUS_LIST TSL where TSL.TASK_STATUS_LIST_ID = TA.TASK_STATUS_LIST_ID ) as ACTIVITY_STATUS, " +
                        "TTL.IMAGE, TTL.LINK, TTA.TEMPACTIVITY_TYPE, TTA.TIME_EXTEND_NOTE,TTA.EXTEND_TIME,  TA.REMARKS " +
                        ", TRUNC (MOD ( (TA.END_TIME - TA.START_TIME) * 24, 24)) as DURATION_OF,NVL(TA.EMP_ID, TAS.EMP_ID)EMP_ID  " +
                        "from ERP_TASK_TASK_LIST TTL left join ERP_TASK_TASK_ASSIGNEE TAS   on TTL.TASKLIST_ID = TAS.TASKLIST_ID " +
                        "left join ERP_TASK_ACTIVITY TA   on TAS.TASK_ASSIGNEE_ID = TA.TASK_ASSIGNEE_ID " +
                        "left join ERP_TASK_TEMPACTIVITY TTA   on TAS.TASK_ASSIGNEE_ID = TTA.TASK_ASSIGNEE_ID " +
                        "Where TTL.ISDELETE = 0 AND TAS.EMP_ID=:EMP_ID order by TA.ACTIVITY_ID desc ";
                    using var conn = new SqliteConnection(_conString);
                    var param = new { EMP_ID = decryptEmpId };
                    var report = await conn.QueryAsync<TaskReport>(sqlReport, param);
                    return report;
                }
                // project and user
                else if (!string.IsNullOrEmpty(decryptProjectId) && string.IsNullOrEmpty(decryptModuleId) && string.IsNullOrEmpty(decryptStartDate.ToString()) && string.IsNullOrEmpty(decryptEndDate.ToString()) && !string.IsNullOrEmpty(decryptEmpId))
                {
                    sqlReport = " select distinct TA.ACTIVITY_ID, TTL.COMPLETE_BY_ADMIN,  TTL.TASKLIST_ID, TTL.TASK_DETAILS,TA.END_TIME,TA.START_TIME,TTL.ESTIMATED_TIME, (select PROJECT_NAME from ERP_TASK_PROJECT P where P.PROJECT_ID = TTL.PROJECT_ID ) as PROJECT_NAME, " +
                        "(select MODULE_NAME from ERP_TASK_MODULE M where M.MODULE_ID = TTL.MODULE_ID ) as MODULE_NAME, " +
                        "(select STATUS_NAME from ERP_TASK_STATUS_LIST TSL where TSL.TASK_STATUS_LIST_ID = TA.TASK_STATUS_LIST_ID ) as ACTIVITY_STATUS, " +
                        "TTL.IMAGE, TTL.LINK, TTA.TEMPACTIVITY_TYPE, TTA.TIME_EXTEND_NOTE,TTA.EXTEND_TIME,  TA.REMARKS " +
                        ", TRUNC (MOD ( (TA.END_TIME - TA.START_TIME) * 24, 24)) as DURATION_OF,NVL(TA.EMP_ID, TAS.EMP_ID) EMP_ID " +
                        "from ERP_TASK_TASK_LIST TTL left join ERP_TASK_TASK_ASSIGNEE TAS   on TTL.TASKLIST_ID = TAS.TASKLIST_ID " +
                        "left join ERP_TASK_ACTIVITY TA   on TAS.TASK_ASSIGNEE_ID = TA.TASK_ASSIGNEE_ID " +
                        "left join ERP_TASK_TEMPACTIVITY TTA   on TAS.TASK_ASSIGNEE_ID = TTA.TASK_ASSIGNEE_ID " +
                        "Where TTL.ISDELETE = 0 AND TTL.PROJECT_ID=:PROJECT_ID  AND TAS.EMP_ID=:EMP_ID order by TA.ACTIVITY_ID desc ";
                    using var conn = new SqliteConnection(_conString);
                    var param = new { PROJECT_ID = decryptProjectId, EMP_ID = decryptEmpId };
                    var report = await conn.QueryAsync<TaskReport>(sqlReport, param);
                    return report;
                }
                //project or module of person in date
                else if ((!string.IsNullOrEmpty(decryptProjectId) || !string.IsNullOrEmpty(decryptModuleId)) && !string.IsNullOrEmpty(decryptStartDate.ToString()) && !string.IsNullOrEmpty(decryptEndDate.ToString()) && !string.IsNullOrEmpty(decryptEmpId))
                {
                    if (string.IsNullOrEmpty(decryptModuleId))
                    {
                        sqlReport = " select distinct TA.ACTIVITY_ID, TTL.COMPLETE_BY_ADMIN,  TTL.TASKLIST_ID, TTL.TASK_DETAILS,TA.END_TIME,TA.START_TIME,TTL.ESTIMATED_TIME, (select PROJECT_NAME from ERP_TASK_PROJECT P where P.PROJECT_ID = TTL.PROJECT_ID ) as PROJECT_NAME, " +
                            "(select MODULE_NAME from ERP_TASK_MODULE M where M.MODULE_ID = TTL.MODULE_ID ) as MODULE_NAME, " +
                            "(select STATUS_NAME from ERP_TASK_STATUS_LIST TSL where TSL.TASK_STATUS_LIST_ID = TA.TASK_STATUS_LIST_ID ) as ACTIVITY_STATUS, " +
                            "TTL.IMAGE, TTL.LINK, TTA.TEMPACTIVITY_TYPE, TTA.TIME_EXTEND_NOTE,TTA.EXTEND_TIME, TA.REMARKS " +
                            ", TRUNC (MOD ( (TA.END_TIME - TA.START_TIME) * 24, 24)) as DURATION_OF,NVL(TA.EMP_ID, TAS.EMP_ID)EMP_ID  " +
                            "from ERP_TASK_TASK_LIST TTL left join ERP_TASK_TASK_ASSIGNEE TAS   on TTL.TASKLIST_ID = TAS.TASKLIST_ID " +
                            "left join ERP_TASK_ACTIVITY TA   on TAS.TASK_ASSIGNEE_ID = TA.TASK_ASSIGNEE_ID " +
                            "left join ERP_TASK_TEMPACTIVITY TTA   on TAS.TASK_ASSIGNEE_ID = TTA.TASK_ASSIGNEE_ID " +
                            "Where TTL.ISDELETE = 0 AND TTL.PROJECT_ID=:PROJECT_ID AND TAS.EMP_ID=:EMP_ID AND " +
                            "TA.START_TIME between  " + decryptStartDate + " AND " + decryptEndDate + " order by TA.ACTIVITY_ID desc";
                    }
                    if (string.IsNullOrEmpty(decryptProjectId))
                    {
                        sqlReport = " select distinct TA.ACTIVITY_ID, TTL.COMPLETE_BY_ADMIN,  TTL.TASKLIST_ID, TTL.TASK_DETAILS,TA.END_TIME,TA.START_TIME,TTL.ESTIMATED_TIME, (select PROJECT_NAME from ERP_TASK_PROJECT P where P.PROJECT_ID = TTL.PROJECT_ID ) as PROJECT_NAME, " +
                            "(select MODULE_NAME from ERP_TASK_MODULE M where M.MODULE_ID = TTL.MODULE_ID ) as MODULE_NAME, " +
                            "(select STATUS_NAME from ERP_TASK_STATUS_LIST TSL where TSL.TASK_STATUS_LIST_ID = TA.TASK_STATUS_LIST_ID ) as ACTIVITY_STATUS, " +
                            "TTL.IMAGE, TTL.LINK, TTA.TEMPACTIVITY_TYPE, TTA.TIME_EXTEND_NOTE,TTA.EXTEND_TIME, TA.REMARKS " +
                            ", TRUNC (MOD ( (TA.END_TIME - TA.START_TIME) * 24, 24)) as DURATION_OF,NVL(TA.EMP_ID, TAS.EMP_ID)EMP_ID  " +
                            "from ERP_TASK_TASK_LIST TTL left join ERP_TASK_TASK_ASSIGNEE TAS   on TTL.TASKLIST_ID = TAS.TASKLIST_ID " +
                            "left join ERP_TASK_ACTIVITY TA   on TAS.TASK_ASSIGNEE_ID = TA.TASK_ASSIGNEE_ID " +
                            "left join ERP_TASK_TEMPACTIVITY TTA   on TAS.TASK_ASSIGNEE_ID = TTA.TASK_ASSIGNEE_ID " +
                            "Where TTL.ISDELETE = 0 AND  TTL.MODULE_ID=:MODULE_ID  AND TAS.EMP_ID=:EMP_ID AND " +
                            "TA.START_TIME between  " + decryptStartDate + " AND " + decryptEndDate + " order by TA.ACTIVITY_ID desc";
                    }
                    else if (!string.IsNullOrEmpty(decryptProjectId) && !string.IsNullOrEmpty(decryptModuleId))
                    {
                        sqlReport = " select distinct TA.ACTIVITY_ID, TTL.COMPLETE_BY_ADMIN,  TTL.TASKLIST_ID, TTL.TASK_DETAILS,TA.END_TIME,TA.START_TIME,TTL.ESTIMATED_TIME, (select PROJECT_NAME from ERP_TASK_PROJECT P where P.PROJECT_ID = TTL.PROJECT_ID ) as PROJECT_NAME, " +
                                    " (select MODULE_NAME from ERP_TASK_MODULE M where M.MODULE_ID = TTL.MODULE_ID ) as MODULE_NAME, " +
                                    " (select STATUS_NAME from ERP_TASK_STATUS_LIST TSL where TSL.TASK_STATUS_LIST_ID = TA.TASK_STATUS_LIST_ID ) as ACTIVITY_STATUS, " +
                                     " TTL.IMAGE, TTL.LINK, TTA.TEMPACTIVITY_TYPE, TTA.TIME_EXTEND_NOTE,TTA.EXTEND_TIME, TA.REMARKS " +
                                    " , TRUNC (MOD ( (TA.END_TIME - TA.START_TIME) * 24, 24)) as DURATION_OF,NVL(TA.EMP_ID, TAS.EMP_ID)EMP_ID  " +
                                    " from ERP_TASK_TASK_LIST TTL left join ERP_TASK_TASK_ASSIGNEE TAS   on TTL.TASKLIST_ID = TAS.TASKLIST_ID " +
                                    " left join ERP_TASK_ACTIVITY TA   on TAS.TASK_ASSIGNEE_ID = TA.TASK_ASSIGNEE_ID " +
                                    " left join ERP_TASK_TEMPACTIVITY TTA   on TAS.TASK_ASSIGNEE_ID = TTA.TASK_ASSIGNEE_ID " +
                                    " Where TTL.ISDELETE = 0 AND TTL.PROJECT_ID=:PROJECT_ID AND  TTL.MODULE_ID=:MODULE_ID  AND TAS.EMP_ID=:EMP_ID AND " +
                                    " TA.START_TIME between  " + decryptStartDate + " AND " + decryptEndDate + " order by TA.ACTIVITY_ID desc ";
                    }
                    var param = new { PROJECT_ID = decryptProjectId, MODULE_ID = decryptModuleId, START_TIME = decryptStartDate, END_TIME = decryptEndDate, EMP_ID = decryptEmpId };
                    using var conn = new SqliteConnection(_conString);
                    var report = await conn.QueryAsync<TaskReport>(sqlReport, param);
                    return report;
                }
                else if (string.IsNullOrEmpty(decryptProjectId) && string.IsNullOrEmpty(decryptModuleId) && !string.IsNullOrEmpty(decryptStartDate.ToString()) && !string.IsNullOrEmpty(decryptEndDate.ToString()) && !string.IsNullOrEmpty(decryptEmpId))
                {
                    sqlReport = " select distinct TA.ACTIVITY_ID, TTL.COMPLETE_BY_ADMIN,  TTL.TASKLIST_ID, TTL.TASK_DETAILS,TA.END_TIME,TA.START_TIME,TTL.ESTIMATED_TIME, (select PROJECT_NAME from ERP_TASK_PROJECT P where P.PROJECT_ID = TTL.PROJECT_ID ) as PROJECT_NAME, " +
                        "(select MODULE_NAME from ERP_TASK_MODULE M where M.MODULE_ID = TTL.MODULE_ID ) as MODULE_NAME, " +
                        "(select STATUS_NAME from ERP_TASK_STATUS_LIST TSL where TSL.TASK_STATUS_LIST_ID = TA.TASK_STATUS_LIST_ID ) as ACTIVITY_STATUS, " +
                        "TTL.IMAGE, TTL.LINK, TTA.TEMPACTIVITY_TYPE, TTA.TIME_EXTEND_NOTE,TTA.EXTEND_TIME, TA.REMARKS " +
                        ", TRUNC (MOD ( (TA.END_TIME - TA.START_TIME) * 24, 24)) as DURATION_OF,NVL(TA.EMP_ID, TAS.EMP_ID)EMP_ID  " +
                        "from ERP_TASK_TASK_LIST TTL left join ERP_TASK_TASK_ASSIGNEE TAS   on TTL.TASKLIST_ID = TAS.TASKLIST_ID " +
                        "left join ERP_TASK_ACTIVITY TA   on TAS.TASK_ASSIGNEE_ID = TA.TASK_ASSIGNEE_ID " +
                        "left join ERP_TASK_TEMPACTIVITY TTA   on TAS.TASK_ASSIGNEE_ID = TTA.TASK_ASSIGNEE_ID " +
                        "Where TTL.ISDELETE = 0  AND TAS.EMP_ID=:EMP_ID AND " +
                        "TA.START_TIME between  " + decryptStartDate + " AND " + decryptEndDate + " order by TA.ACTIVITY_ID desc";
                    var param = new { PROJECT_ID = decryptProjectId, MODULE_ID = decryptModuleId, START_TIME = decryptStartDate, END_TIME = decryptEndDate, EMP_ID = decryptEmpId };
                    using var conn = new SqliteConnection(_conString);
                    var report = await conn.QueryAsync<TaskReport>(sqlReport, param);
                    return report;
                }
                return null;

            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskReport>> ReportEmployeeActiveinactive(string decryptProjectId, string decryptModuleId, string decryptStartDate, string decryptEndDate, string decryptEmpId, List<string> decryptEmpList)
        {
            string itemEmployee = "";
            try
            {
                if (decryptEmpList.Count() > 0)
                {
                    foreach (var emp in decryptEmpList)
                    {
                        itemEmployee = itemEmployee + " '" + emp + "', ";
                    }
                    itemEmployee = itemEmployee.Substring(0, itemEmployee.Length - 2);
                }

                string sqlReport = string.Empty;
                if (!string.IsNullOrEmpty(decryptStartDate.ToString()) && !string.IsNullOrEmpty(decryptEndDate.ToString()) && decryptEmpList.Count() == 0)
                {
                    sqlReport = " select distinct  TM.EMP_ID,  TAS.ASSIGNEE_DATE, TAS.CREATE_BY, TAS.TASKLIST_ID,NVL(TTL.TASKLIST_ID,'Not-Assigned') ACTIVITY_STATUS,  " +
                        " (select L.TASK_DETAILS from erp_task_task_list L where L.tasklist_id= tas.tasklist_id) TASK_DETAILS , " +
                        "(select MODULE_NAME from ERP_TASK_MODULE M, ERP_TASK_TASK_LIST TTL where M.MODULE_ID = TTL.MODULE_ID and TTL.TASKLIST_ID = TAS.TASKLIST_ID) as MODULE_NAME, " +

                        "  (select PROJECT_NAME from ERP_TASK_PROJECT P, ERP_TASK_TASK_LIST TTL where P.PROJECT_ID = TTL.PROJECT_ID and TTL.TASKLIST_ID = TAS.TASKLIST_ID) as PROJECT_NAME  from ERP_TASK_TASK_ASSIGNEE TAS" +
                        " right join ERP_TASK_EMP_TEAM_MAPPING TM on TM.EMP_ID = TAS.EMP_ID left outer join ERP_TASK_TASK_LIST TTL on TTL.TASKLIST_ID = TAS.TASKLIST_ID " +
                        " and TAS.ISDELETE = 0   AND TAS.ASSIGNEE_DATE between " + decryptStartDate + " AND " + decryptEndDate + " order by TAS.ASSIGNEE_DATE desc ";
                    using var conn = new SqliteConnection(_conString);
                    var param = new { PROJECT_ID = decryptProjectId, MODULE_ID = decryptModuleId, START_TIME = decryptStartDate, END_TIME = decryptEndDate, EMP_ID = decryptEmpId };
                    var report = await conn.QueryAsync<TaskReport>(sqlReport, param);
                    return report;
                }

                else if (!string.IsNullOrEmpty(decryptStartDate.ToString()) && !string.IsNullOrEmpty(decryptEndDate.ToString()) && decryptEmpList.Count() > 0)
                {
                    sqlReport = "  select distinct  TM.EMP_ID,  TAS.ASSIGNEE_DATE, TAS.CREATE_BY, TAS.TASKLIST_ID,NVL(TTL.TASKLIST_ID,'Not-Assigned') ACTIVITY_STATUS," +
                        " (select L.TASK_DETAILS from erp_task_task_list L where L.tasklist_id= tas.tasklist_id) TASK_DETAILS , " +
                        "(select MODULE_NAME from ERP_TASK_MODULE M, ERP_TASK_TASK_LIST TTL where M.MODULE_ID = TTL.MODULE_ID and TTL.TASKLIST_ID = TAS.TASKLIST_ID) as MODULE_NAME, " +
                        "  (select PROJECT_NAME from ERP_TASK_PROJECT P, ERP_TASK_TASK_LIST TTL where P.PROJECT_ID = TTL.PROJECT_ID and TTL.TASKLIST_ID = TAS.TASKLIST_ID) as PROJECT_NAME  from ERP_TASK_TASK_ASSIGNEE TAS" +
                        " right join ERP_TASK_EMP_TEAM_MAPPING TM on TM.EMP_ID = TAS.EMP_ID left outer join ERP_TASK_TASK_LIST TTL on TTL.TASKLIST_ID = TAS.TASKLIST_ID " +
                        " and TAS.ISDELETE = 0   AND TAS.ASSIGNEE_DATE between " + decryptStartDate + " AND " + decryptEndDate + "  Where TAS.EMP_ID in (" + itemEmployee + ") order by TAS.ASSIGNEE_DATE desc ";
                    using var conn = new SqliteConnection(_conString);
                    var param = new { PROJECT_ID = decryptProjectId, MODULE_ID = decryptModuleId, START_TIME = decryptStartDate, END_TIME = decryptEndDate, EMP_ID = decryptEmpId };
                    var report = await conn.QueryAsync<TaskReport>(sqlReport, param);
                    return report;
                }

                return null;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }


        internal async Task<IEnumerable<TaskReport>> PersonnelActivitySummary(string decryptStartDate, string decryptEndDate, List<string> decryptEmpList)
        {
            string sqlReport = string.Empty;
            string sqlEstimateTime = string.Empty;
            string sqlReportForEmployeesActivity = string.Empty;
            string itemEmployee = "";

            try
            {
                var param = new { START_TIME = decryptStartDate, END_TIME = decryptEndDate };
                using var conn = new SqliteConnection(_conString);
                List<TaskReport> report = new List<TaskReport>();
                if (decryptEmpList.Count() > 0)
                {
                    foreach (var emp in decryptEmpList)
                    {

                        sqlReportForEmployeesActivity = @" select distinct project_name FROM ( select distinct TAS.TASK_ASSIGNEE_ID,TAS.TASKLIST_ID,(select PROJECT_NAME from ERP_TASK_PROJECT P, ERP_TASK_TASK_LIST TTL
                        where P.PROJECT_ID = TTL.PROJECT_ID and TTL.TASKLIST_ID = TAS.TASKLIST_ID) PROJECT_NAME from ERP_TASK_TASK_ASSIGNEE TAS
                        right join ERP_TASK_EMP_TEAM_MAPPING TM on TM.EMP_ID = TAS.EMP_ID left outer join ERP_TASK_TASK_LIST TTL on TTL.TASKLIST_ID = TAS.TASKLIST_ID 
                        and TAS.ISDELETE = 0  and TTL.ISDELETE = 0  AND TAS.ASSIGNEE_DATE between  " + decryptStartDate + " AND " + decryptEndDate + " Where TAS.EMP_ID in ('" + emp + "') ) ";


                        var personActivityProjectCount = await conn.QueryAsync<TaskReport>(sqlReportForEmployeesActivity, param);

                        string sql1 = @"  select count(TASK_DETAILS) TASK_DETAILS from ( select distinct TTL.TASK_DETAILS,TAS.TASK_ASSIGNEE_ID,TAS.TASKLIST_ID from ERP_TASK_TASK_ASSIGNEE TAS
                        right join ERP_TASK_EMP_TEAM_MAPPING TM on TM.EMP_ID = TAS.EMP_ID left outer join ERP_TASK_TASK_LIST TTL on TTL.TASKLIST_ID = TAS.TASKLIST_ID 
                        and TAS.ISDELETE = 0  and TTL.ISDELETE = 0  AND TAS.ASSIGNEE_DATE between  " + decryptStartDate + " AND " + decryptEndDate + " Where TAS.EMP_ID in ('" + emp + "') )";
                        var personActivityTaskCount = await conn.QueryAsync<TaskReport>(sql1, null);

                        string sql3 = @"  select  distinct TAS.EMP_ID from ERP_TASK_TASK_ASSIGNEE TAS
                        right join ERP_TASK_EMP_TEAM_MAPPING TM on TM.EMP_ID = TAS.EMP_ID left outer join ERP_TASK_TASK_LIST TTL on TTL.TASKLIST_ID = TAS.TASKLIST_ID 
                        and TAS.ISDELETE = 0  and TTL.ISDELETE = 0  AND TAS.ASSIGNEE_DATE between  " + decryptStartDate + " AND " + decryptEndDate + " Where TAS.EMP_ID in ('" + emp + "') ";
                        var personActivity = await conn.QueryAsync<TaskReport>(sql3, null);
                        if (personActivityProjectCount.Count() > 0)
                        {
                            personActivity.FirstOrDefault().PROJECT_NAME = Convert.ToString(personActivityProjectCount.Count());
                        }
                        if (personActivityTaskCount.Count() > 0)
                        {
                            var x = personActivityTaskCount.FirstOrDefault().TASK_DETAILS;
                            if(!string.IsNullOrEmpty(x) && x != "0")
                            personActivity.FirstOrDefault().TASK_DETAILS = x;
                        }

                        report.Add(personActivity.FirstOrDefault());
                    }

                    return report;
                }

                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskReport>> EstimatedTimeTaskListId(string emp, string decryptStartDate, string decryptEndDate)
        {
            try
            {
                string sqlReport = "";
                var param = new { START_TIME = decryptStartDate, END_TIME = decryptEndDate };
                using var conn = new SqliteConnection(_conString);
                sqlReport = "  select distinct  TTL.TASKLIST_ID,TTL.STATUS_LIST_ID,TAS.TASK_ASSIGNEE_ID from ERP_TASK_TASK_ASSIGNEE TAS " +
                 " right join ERP_TASK_EMP_TEAM_MAPPING TM on TM.EMP_ID = TAS.EMP_ID left outer join ERP_TASK_TASK_LIST TTL on TTL.TASKLIST_ID = TAS.TASKLIST_ID " +
                 " and TAS.ISDELETE = 0  and TTL.ISDELETE = 0  AND TAS.ASSIGNEE_DATE between " + decryptStartDate + " AND " + decryptEndDate + "  Where TAS.EMP_ID in ('" + emp + "')   ";


                var personAllEstimateTime = await conn.QueryAsync<TaskReport>(sqlReport, param);
                return personAllEstimateTime;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<TaskList> DoneTask(string donetaskListId)
        {
            try
            {
                string sqlEstimateTime = "";
                using var conn = new SqliteConnection(_conString);
                sqlEstimateTime = " select ESTIMATED_TIME FROM ERP_TASK_TASK_LIST WHERE TASKLIST_ID= '" + donetaskListId + "' ";
                var estimateData = await conn.QuerySingleAsync<TaskList>(sqlEstimateTime, null);

                return estimateData;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskTempActivitry>> TaskTempActivities(string donetaskListId)
        {
            try
            {
                using var conn = new SqliteConnection(_conString);
                string sql1 = " select TEM.* from ERP_TASK_TASK_ASSIGNEE TAS,ERP_TASK_TEMPACTIVITY TEM where TAS.TASKLIST_ID = '" + donetaskListId + "' AND TAS.TASK_ASSIGNEE_ID=TEM.TASK_ASSIGNEE_ID  ";
                var sql1Data = await conn.QueryAsync<TaskTempActivitry>(sql1, null);
                return sql1Data;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<TaskReport>> PersonnelActivitySummaryDetails( string decryptStartDate, string decryptEndDate, string empId)
        {
            try
            {
                string strSql = string.Empty;
                using var conn = new SqliteConnection(_conString);

                if ( !string.IsNullOrEmpty(decryptStartDate.ToString()) &&! string.IsNullOrEmpty(decryptEndDate.ToString()) && !string.IsNullOrEmpty(empId))
                {
                    strSql = "  SELECT DISTINCT tas.TASKLIST_ID,tas.TASK_ASSIGNEE_ID,TTL.ESTIMATED_TIME ,TTL.TASK_DATE,EMP_ID,task_details,TTL.STATUS_LIST_ID FROM ERP_TASK_TASK_ASSIGNEE tas,erp_task_task_list ttl  where EMP_ID = :EMP_ID" +
                        " and ttl.ISDELETE = 0 and ttl.TASKLIST_ID = tas.TASKLIST_ID  AND ttl.TASK_DATE between " + decryptStartDate + " AND " + decryptEndDate + "  and TAS.TASKLIST_ID is not null order by TASK_ASSIGNEE_ID desc ";
                
                    var paramTask = new { EMP_ID = empId };
                    var taskAssignList = await conn.QueryAsync<TaskReport>(strSql, paramTask);

                  return taskAssignList.ToList();
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }


    }
}
