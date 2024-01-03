using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
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
    public class TaskUsersRepository
    {
        private readonly string _conString;
        public TaskUsersRepository()
        {
            _conString = DbContext.ERP_Connection;
        }

        internal async Task<IEnumerable<CmSystemUsers>> GetUsersList(string userId)
        {
            try
            {
                string strSql = " select * from erp_cm_system_users where cmp_branch_id in ( select CMP_BRANCH_ID from ERP_CM_SYSTEM_USERS  where sys_usr_id = :sys_usr_id ) ";
                var param = new { sys_usr_id = userId };
                using var conn = new SqliteConnection(_conString);
                var employee = await conn.QueryAsync<CmSystemUsers>(strSql, param);

                return employee;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }  
        
        internal async Task<IEnumerable<CmSystemUsers>> GetCompanyBranch(string userId)
        {
            try
            {
                string strSql = " select * from ERP_CM_CMP_BRANCH where CMP_COMPANY_ID = ( select distinct COMPANY_ID from ERP_CM_SYSTEM_USERS  where sys_usr_id = :sys_usr_id ) ";
                var param = new { sys_usr_id = userId };
                using var conn = new SqliteConnection(_conString);
                var branchDetails = await conn.QueryAsync<CmSystemUsers>(strSql, param);

                return branchDetails;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }
        
        internal async Task<IEnumerable<CmSystemUsers>> GetCompany(string userId)
        {
            try
            {
                string strSql = " select * from ERP_CM_COMPANY where COMPANY_ID = ( select distinct COMPANY_ID from ERP_CM_SYSTEM_USERS  where sys_usr_id = :sys_usr_id ) ";
                var param = new { sys_usr_id = userId };
                using var conn = new SqliteConnection(_conString);
                var branchDetails = await conn.QueryAsync<CmSystemUsers>(strSql, param);

                return branchDetails;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }
        
        internal async Task<IEnumerable<CmSystemUsers>> GetUsersById(string userId)
        {
            try
            {
                string strSql = "  select * from ERP_CM_SYSTEM_USERS  where sys_usr_id = :sys_usr_id  ";
                var param = new { sys_usr_id = userId };
                using var conn = new SqliteConnection(_conString);
                var userDetails = await conn.QueryAsync<CmSystemUsers>(strSql, param);

                return userDetails;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<CmSystemUsers>> GetUsersGroupById(string userGroupId)
        {
            try
            {
                string strSql = "  select * from ERP_CM_SYSTEM_USER_GROUP  where SYS_USR_GRP_ID = :SYS_USR_GRP_ID  ";
                var param = new { SYS_USR_GRP_ID = userGroupId };
                using var conn = new SqliteConnection(_conString);
                var userDetails = await conn.QueryAsync<CmSystemUsers>(strSql, param);

                return userDetails;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> CreateUsers(string empId, string comId, string loginName, string userPass, string usrActive, string email, string groupId, string branchId, DateTime passwordExpiredate, string userType)
        {
           try
            {
                using var conn = new SqliteConnection(_conString);

                string sqlchkUserId = "select sys_usr_id from erp_cm_system_users where SYS_USR_LOGIN_NAME=:SYS_USR_LOGIN_NAME   ";

                var paramChkUser = new { SYS_USR_GRP_ID = groupId, SYS_USR_LOGIN_NAME = loginName};
                var companychkUserId = await conn.QueryAsync<CmSystemUsers>(sqlchkUserId, paramChkUser);
                if (companychkUserId.Count() == 0 )
                {
                    string strSql = " insert into erp_cm_system_users (COMPANY_ID, SYS_USR_LOGIN_NAME,SYS_USR_DNAME, SYS_USR_PASS, USER_ACTIVE, SYS_USR_EMAIL, SYS_USR_GRP_ID, "
                                  + "CMP_BRANCH_ID, PASSWORD_EXPIRED_DATE, USER_TYPE) values (:COMPANY_ID, :SYS_USR_LOGIN_NAME,:SYS_USR_DNAME, :SYS_USR_PASS,: USER_ACTIVE,: SYS_USR_EMAIL,: SYS_USR_GRP_ID, "
                                  + ":CMP_BRANCH_ID,: PASSWORD_EXPIRED_DATE,: USER_TYPE) ";
                    var param = new { COMPANY_ID = comId, SYS_USR_LOGIN_NAME = loginName, SYS_USR_PASS = userPass, SYS_USR_DNAME = loginName, USER_ACTIVE = usrActive, SYS_USR_EMAIL = email, SYS_USR_GRP_ID = groupId, CMP_BRANCH_ID = branchId, PASSWORD_EXPIRED_DATE = passwordExpiredate, USER_TYPE = userType };
                    var employee = await conn.QueryAsync<CmSystemUsers>(strSql, param);

                    string sqlgetUserId = "select sys_usr_id from erp_cm_system_users where SYS_USR_LOGIN_NAME=:SYS_USR_LOGIN_NAME   AND ERP_GET_CM_USER_PASS( SYS_USR_ID,'admi0086A2C80001') = :GET_PIN  ";

                    var paramUserRoleMap = new { SYS_USR_GRP_ID = groupId, SYS_USR_LOGIN_NAME = loginName, GET_PIN = userPass };
                    var companyUserId = await conn.QueryAsync<CmSystemUsers>(sqlgetUserId, paramUserRoleMap);

                    string updateEmployUserIdquery = "UPDATE ERP_PR_EMPLOYEE_LIST SET SYS_USR_ID =:SYS_USR_ID  WHERE EMP_ID =: EMP_ID ";
                    var paramupdateEmployUserId = new { EMP_ID = empId, SYS_USR_ID = companyUserId.FirstOrDefault().SYS_USR_ID };
                    var updateEmployUserId = await conn.QueryAsync<CmSystemUsers>(updateEmployUserIdquery, paramupdateEmployUserId);
                   
                    string querChkusrMenu = " select * from ERP_SYSTEM_USERS_ROLE_MAPPING where SYS_USER_ID =:SYS_USER_ID ";
                    var paramChkusrmenu = new { SYS_USER_ID = companyUserId.FirstOrDefault().SYS_USR_ID };
                    var ChkusrmenuMapId = await conn.QueryAsync<CmSystemUsers>(querChkusrMenu, paramChkusrmenu);

                    if (ChkusrmenuMapId.Count() < 1)
                    {
                        string sqlInsertUserRole = " insert into ERP_SYSTEM_USERS_ROLE_MAPPING (SYS_USER_ROLE_ID,SYS_USER_ID,IS_ACTIVE) values (:SYS_USER_ROLE_ID, :SYS_USER_ID,'Y') ";
                        var insertParamUserRoleMap = new { SYS_USER_ROLE_ID = groupId, SYS_USER_ID = companyUserId.FirstOrDefault().SYS_USR_ID };

                        var userMapId = await conn.QueryAsync<CmSystemUsers>(sqlInsertUserRole, insertParamUserRoleMap);
                    }
                    return "User successfully created ";
                }
                else
                {
                    return "User already exist ";
                }
               
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> UpdateUsers(string empId, string userId, string comId, string loginName, string userPass, string usrActive, string email, string groupId, string branchId, string passwordExpiredate, string userType)
        {
            //branchId = "200511239";
            try
            {
                string strSql = " update  erp_cm_system_users set   SYS_USR_LOGIN_NAME=:SYS_USR_LOGIN_NAME,SYS_USR_DNAME=:SYS_USR_DNAME,  SYS_USR_EMAIL=: SYS_USR_EMAIL, SYS_USR_GRP_ID=:SYS_USR_GRP_ID, "
               + "CMP_BRANCH_ID=:CMP_BRANCH_ID, PASSWORD_EXPIRED_DATE=:PASSWORD_EXPIRED_DATE, USER_TYPE=:USER_TYPE, SYS_USR_PASS= :SYS_USR_PASS,USER_ACTIVE=: USER_ACTIVE where SYS_USR_ID=:SYS_USR_ID ";
                var param = new { SYS_USR_ID = userId, COMPANY_ID = comId, SYS_USR_LOGIN_NAME = loginName, SYS_USR_DNAME= loginName, SYS_USR_PASS = userPass, USER_ACTIVE = usrActive, SYS_USR_EMAIL = email, SYS_USR_GRP_ID = groupId, CMP_BRANCH_ID = branchId, PASSWORD_EXPIRED_DATE = passwordExpiredate, USER_TYPE = userType };
                using var conn = new SqliteConnection(_conString);
                var employee = await conn.QueryAsync<CmSystemUsers>(strSql, param);
               
                string updateEmployUserIdquery = "UPDATE ERP_PR_EMPLOYEE_LIST SET SYS_USR_ID =:SYS_USR_ID  WHERE EMP_ID =: EMP_ID ";
                var paramupdateEmployUserId = new { EMP_ID = empId, SYS_USR_ID = userId };
                var updateEmployUserId = await conn.QueryAsync<CmSystemUsers>(updateEmployUserIdquery, paramupdateEmployUserId);
                string querChkusrMenu = " select * from ERP_SYSTEM_USERS_ROLE_MAPPING where SYS_USER_ID =:SYS_USER_ID ";
                var paramChkusrmenu = new { SYS_USER_ID = userId };
                var ChkusrmenuMapId = await conn.QueryAsync<CmSystemUsers>(querChkusrMenu, paramChkusrmenu);
                
                if (ChkusrmenuMapId.Count() < 1) 
                {
                    string sqlInsertUserRole = " insert into ERP_SYSTEM_USERS_ROLE_MAPPING (SYS_USER_ROLE_ID,SYS_USER_ID,IS_ACTIVE) values (:SYS_USER_ROLE_ID, :SYS_USER_ID,'Y') ";
                    var insertParamUserRoleMap = new { SYS_USER_ROLE_ID = groupId, SYS_USER_ID = userId };

                    var userMapId = await conn.QueryAsync<CmSystemUsers>(sqlInsertUserRole, insertParamUserRoleMap);
                }
               
                return "User successfully updated ";
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<CmSystemUsers>> GetUserGroup(string userId, string userName)
        {
            string strSql = "";
            string strSqlCompany = "";
            try
            {
                strSqlCompany = " select SOFT_PKG_ID from ERP_CM_COMPANY COM,ERP_CM_SYSTEM_USERS SU where SU.COMPANY_ID= COM.COMPANY_ID and sys_usr_id=:sys_usr_id  ";
                var paramPackageId = new { sys_usr_id = userId };
                using var conn = new SqliteConnection(_conString);
                var softwarePackage = await conn.QueryAsync<CmSystemUsers>(strSqlCompany, paramPackageId);
                var softwarePkgId = softwarePackage.FirstOrDefault().SOFT_PKG_ID;
                string strSqlUsrGroupBranch = " select cmp_branch_id from ERP_CM_SYSTEM_USERS where sys_usr_id=:sys_usr_id ";
                var paramBranch = new { sys_usr_id = userId };
                var branch = await conn.QueryAsync<CmSystemUsers>(strSqlUsrGroupBranch, paramBranch);
                string branchId = branch.FirstOrDefault().CMP_BRANCH_ID;
                strSql = "SELECT SYS_USR_GRP_ID, SYS_USR_GRP_TITLE,SYS_USR_GRP_TYPE,CMP_BRANCH_ID,SYS_USR_GRP_PARENT FROM ERP_CM_SYSTEM_USER_GROUP UG WHERE SYS_USR_GRP_TITLE IN "
                       + "( SELECT UG.SYS_USR_GRP_TITLE FROM ERP_CM_SYSTEM_USER_GROUP UG WHERE  UG.CMP_BRANCH_ID= :CMP_BRANCH_ID  "
                       + "UNION  SELECT SYS_USR_GRP_TITLE FROM ERP_CM_SYSTEM_USER_GROUP UG, ERP_CM_SOFTWARE_PACKAGE SP "
                       + "WHERE UG.SYS_USR_GRP_ID=SP.SYS_USR_GRP_ID AND SP.SOFT_PKG_ID=:SOFT_PKG_ID  ) ORDER BY CODE ";
                var paramUserGroup = new { SOFT_PKG_ID = softwarePkgId, CMP_BRANCH_ID = branchId };
                var userGroupDetails = await conn.QueryAsync<CmSystemUsers>(strSql, paramUserGroup);

                return userGroupDetails;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> CreateUserGroup(string groupId, string groupTitle, string groupType, string groupBranchId)
        {
            string sqlCreateGroup = "";
            try
            {
                sqlCreateGroup = " insert into erp_cm_system_user_group (SYS_USR_GRP_PARENT, SYS_USR_GRP_TITLE,SYS_USR_GRP_TYPE,CMP_BRANCH_ID)" +
                    " values (:SYS_USR_GRP_PARENT,:SYS_USR_GRP_TITLE, :SYS_USR_GRP_TYPE, :CMP_BRANCH_ID ) ";
                var paramCreateGroup = new { SYS_USR_GRP_PARENT = groupId, SYS_USR_GRP_TITLE = groupTitle, SYS_USR_GRP_TYPE = groupType, CMP_BRANCH_ID = groupBranchId };
                using var conn = new SqliteConnection(_conString);
                var createGroup = await conn.QueryAsync<CmSystemUsers>(sqlCreateGroup, paramCreateGroup);

                return " Group Role Successfully Created ";
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> UpdateUserGroup(string groupId, string groupParent, string groupTitle, string groupType, string groupBranchId)
        {
            string sqlUpdateGroup = "";
            try
            {
                sqlUpdateGroup = " update erp_cm_system_user_group set SYS_USR_GRP_PARENT=:SYS_USR_GRP_PARENT," +
                    " SYS_USR_GRP_TITLE=:SYS_USR_GRP_TITLE,SYS_USR_GRP_TYPE=:SYS_USR_GRP_TYPE where SYS_USR_GRP_ID=:SYS_USR_GRP_ID ";
                var paramCreateGroup = new { SYS_USR_GRP_ID = groupId, SYS_USR_GRP_PARENT = groupParent, SYS_USR_GRP_TITLE = groupTitle, SYS_USR_GRP_TYPE = groupType, CMP_BRANCH_ID = groupBranchId };
                using var conn = new SqliteConnection(_conString);
                var updateGroup = await conn.QueryAsync<CmSystemUsers>(sqlUpdateGroup, paramCreateGroup);

                return " User Group Successfully Updated ";
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<CmSystemUsers>> DecrypPassword(string sessionId, string logInName, string usrId)
        {
            try
            {
                string strSql;
                strSql = "SELECT ERP_GET_CM_USER_PASS_PREVIOUS(SU.SYS_USR_ID,:V_SESSION_ID) PREV_PASS,ERP_GET_CM_USER_PASS(SU.SYS_USR_ID,:V_SESSION_ID)EXIS_PASS FROM ERP_CM_SYSTEM_USERS SU," +
                    " ERP_CM_CMP_BRANCH CB, ERP_CM_COMPANY CO WHERE SU.CMP_BRANCH_ID = CB.CMP_BRANCH_ID AND CB.CMP_COMPANY_ID = CO.COMPANY_ID AND SU.SYS_USR_LOGIN_NAME =:SYS_USR_LOGIN_NAME AND  SYS_USR_ID=:SYS_USR_ID  ";
                var param = new { V_SESSION_ID = sessionId, SYS_USR_LOGIN_NAME = logInName, SYS_USR_ID = usrId };
                using var conn = new SqliteConnection(_conString);
                var passWord = await conn.QueryAsync<CmSystemUsers>(strSql, param);

                return passWord;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> ChangePassword(string strUserID, string newPass, string oldPass, string newLoginName)
        {
            try
            {
                string strSql;
                strSql = " update ERP_CM_SYSTEM_USERS set SYS_USR_PASS= :SYS_USR_PASS, SYS_USR_PASS_PREVIOUS=:SYS_USR_PASS_PREVIOUS, SYS_USR_LOGIN_NAME=:SYS_USR_LOGIN_NAME WHERE SYS_USR_ID = :SYS_USR_ID  ";

                var param = new { SYS_USR_PASS = newPass, SYS_USR_PASS_PREVIOUS = oldPass, SYS_USR_LOGIN_NAME = newLoginName, SYS_USR_ID = strUserID };
                using var conn = new SqliteConnection(_conString);
                var passWord = await conn.QueryAsync<CmSystemUsers>(strSql, param);

                return " Login Name and Password Changed successfully ";
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<CmSystemAccessPolicy>> GetCompanyAccessPolicyWiseMenuData(string userId)
        {
            try
            {
                string strSql = "";
        //        strSql = @" ((SELECT distinct  SM.SYS_MENU_ID, SM.SYS_MENU_TITLE , SM.SYS_MENU_FILE, SM.PATH_NAME, SM.SYS_MENU_PARENT,SM.SYS_MENU_TYPE,SM.MENU_ICON,SYS_MENU_SERIAL
        //                   FROM ERP_USR_ROLE_MENU_MAPPING  URMM
        //                  left join ERP_SYSTEM_USERS_ROLE_MAPPING URM ON URM.SYS_USER_ROLE_MAP_ID = URMM.SYS_USER_ROLE_MAP_ID
        //                   LEFT JOIN  ERP_CM_SYSTEM_MENU SM  ON SM.SYS_MENU_ID = URMM.SYS_MENU_ID
        //                  where URM.SYS_USER_ID = :SYS_USR_ID AND URMM.IS_ACTIVE='Y' ) 
        //                 union
        //                   select  SYS_MENU_ID, SYS_MENU_TITLE , SYS_MENU_FILE, PATH_NAME, SYS_MENU_PARENT,SYS_MENU_TYPE,MENU_ICON,SYS_MENU_SERIAL  from ERP_CM_SYSTEM_MENU where   SYS_MENU_ID in
        //                  ( select  SYS_MENU_PARENT   from ERP_CM_SYSTEM_MENU where  SYS_MENU_ID in   (select SYS_MENU_PARENT from (select  * FROM ERP_USR_ROLE_MENU_MAPPING  URMM
        //               left join ERP_SYSTEM_USERS_ROLE_MAPPING URM ON URM.SYS_USER_ROLE_MAP_ID = URMM.SYS_USER_ROLE_MAP_ID   LEFT JOIN  ERP_CM_SYSTEM_MENU SM  ON SM.SYS_MENU_ID = URMM.SYS_MENU_ID 
        //             where URM.sys_user_id = :SYS_USR_ID AND URMM.IS_ACTIVE='Y'  )  ) ) 
        //                  union
        //     select  SYS_MENU_ID, SYS_MENU_TITLE , SYS_MENU_FILE, PATH_NAME, SYS_MENU_PARENT,SYS_MENU_TYPE,MENU_ICON ,SYS_MENU_SERIAL from ERP_CM_SYSTEM_MENU where   SYS_MENU_ID in
        //     (     (select SYS_MENU_PARENT from (select  * FROM ERP_USR_ROLE_MENU_MAPPING  URMM    left join ERP_SYSTEM_USERS_ROLE_MAPPING URM ON URM.SYS_USER_ROLE_MAP_ID = URMM.SYS_USER_ROLE_MAP_ID
        //        LEFT JOIN  ERP_CM_SYSTEM_MENU SM  ON SM.SYS_MENU_ID = URMM.SYS_MENU_ID   where URM.sys_user_id = :SYS_USR_ID AND URMM.IS_ACTIVE='Y'  )  ) ) )
        //order by SYS_MENU_SERIAL asc ";
                      strSql = @" select * from (  select * from ( SELECT distinct SM.SYS_MENU_ID, SM.SYS_MENU_TITLE , SM.SYS_MENU_FILE, SM.PATH_NAME, SM.SYS_MENU_PARENT,SM.SYS_MENU_TYPE,SM.MENU_ICON,SYS_MENU_SERIAL
                                          FROM ERP_USR_ROLE_MENU_MAPPING URMM
                          left join ERP_SYSTEM_USERS_ROLE_MAPPING URM ON URM.SYS_USER_ROLE_MAP_ID = URMM.SYS_USER_ROLE_MAP_ID
                           LEFT JOIN  ERP_CM_SYSTEM_MENU SM  ON SM.SYS_MENU_ID = URMM.SYS_MENU_ID
                          where URM.SYS_USER_ID = '"+userId+"' AND URMM.IS_ACTIVE = 'Y' ) where SYS_MENU_ID NOTNULL  " + @"
		                 UNION
		     			  select  SYS_MENU_ID, SYS_MENU_TITLE , SYS_MENU_FILE, PATH_NAME, SYS_MENU_PARENT,SYS_MENU_TYPE,MENU_ICON,SYS_MENU_SERIAL from ERP_CM_SYSTEM_MENU where   SYS_MENU_ID in
                          (select  SYS_MENU_PARENT from ERP_CM_SYSTEM_MENU where  SYS_MENU_ID in   (select SYS_MENU_PARENT from(select* FROM ERP_USR_ROLE_MENU_MAPPING URMM

                          left join ERP_SYSTEM_USERS_ROLE_MAPPING URM ON URM.SYS_USER_ROLE_MAP_ID = URMM.SYS_USER_ROLE_MAP_ID   LEFT JOIN  ERP_CM_SYSTEM_MENU SM  ON SM.SYS_MENU_ID = URMM.SYS_MENU_ID 

                             where URM.sys_user_id = '"+userId+"' AND URMM.IS_ACTIVE = 'Y'  )  ) )  union   "+ @"
                                      
                          select  SYS_MENU_ID, SYS_MENU_TITLE , SYS_MENU_FILE, PATH_NAME, SYS_MENU_PARENT,SYS_MENU_TYPE,MENU_ICON ,SYS_MENU_SERIAL from ERP_CM_SYSTEM_MENU where   SYS_MENU_ID in
                         ( select DISTINCT SYS_MENU_PARENT from(select* FROM ERP_USR_ROLE_MENU_MAPPING URMM    left join ERP_SYSTEM_USERS_ROLE_MAPPING URM ON URM.SYS_USER_ROLE_MAP_ID = URMM.SYS_USER_ROLE_MAP_ID

                         LEFT JOIN  ERP_CM_SYSTEM_MENU SM  ON SM.SYS_MENU_ID = URMM.SYS_MENU_ID   where URM.sys_user_id = '"+userId+"' AND URMM.IS_ACTIVE = 'Y'  )   )  order by SYS_MENU_PARENT asc ) ";
                var param = new { SYS_USR_ID = userId };
                using var conn = new SqliteConnection(_conString);
                var menuData = await conn.QueryAsync<CmSystemAccessPolicy>(strSql, null);
                return menuData;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<CmSystemAccessPolicy>> GetUserGroupWiseAccessPolicyMenu(string userId)
        {
            string strSqlGetRoleWiseMenu = "";
            try
            {

                strSqlGetRoleWiseMenu = @"  select distinct tt.SYS_USR_GRP_ID,tt.SYSTEM_ROLE_MENU_MAP_ID, tt.SYS_MENU_SERIAL ,PATH_NAME, SYS_MENU_TITLE,tt.SYS_MENU_ID,SYS_USR_GRP_TITLE,CSU.COMPANY_ID, tt.MENU_ICON from (select rmm.SYSTEM_ROLE_MENU_MAP_ID, CSM.SYS_MENU_TITLE, rmm.SYS_MENU_ID, CSM.PATH_NAME, SUG.SYS_USR_GRP_ID, SYS_USR_GRP_TITLE, CSM.MENU_ICON,CSM.SYS_MENU_SERIAL
                         from ERP_SYSTEM_ROLE_MENU_MAPPING RMM left join ERP_CM_SYSTEM_MENU CSM on rmm.SYS_MENU_ID = CSM.SYS_MENU_ID left join ERP_CM_SYSTEM_USER_GROUP SUG on SUG.SYS_USR_GRP_ID = RMM.SYS_USER_ROLE_ID and RMM.IS_ACTIVE = 'Y') tt
                          join ERP_CM_SYSTEM_USERS CSU  on CSU.SYS_USR_GRP_ID = tt.SYS_USR_GRP_ID where CSU.COMPANY_ID =  (select company_id from erp_cm_system_users where sys_usr_id =:sys_usr_id) order by tt.SYS_USR_GRP_ID, tt.SYS_MENU_SERIAL asc ";
                var paramGetRoleWiseMenu = new { sys_usr_id = userId };
                using var conn = new SqliteConnection(_conString);
                var menuData = await conn.QueryAsync<CmSystemAccessPolicy>(strSqlGetRoleWiseMenu, paramGetRoleWiseMenu);
                return menuData;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> InsertSystemMenu(string userId, string parentMenuId, string menuTitle, string pathName, string menuType,string menuIcon, string menuSerial)
        {
            string strSqlCompany = "";
            string inserMenuSql = "";
            try
            {
                strSqlCompany = " select COM.SOFT_PKG_ID,CB.CMP_BRANCH_ID from ERP_CM_COMPANY COM,ERP_CM_SYSTEM_USERS SU,ERP_CM_CMP_BRANCH CB where SU.COMPANY_ID= COM.COMPANY_ID AND CB.CMP_COMPANY_ID=COM.PARENT_ID AND CB.CMP_BRANCH_TYPE_ID='A' and SU.sys_usr_id=:sys_usr_id  ";
                var paramPackageId = new { sys_usr_id = userId };
                using var conn = new SqliteConnection(_conString);
                var softwarePackage = await conn.QueryAsync<CmSystemUsers>(strSqlCompany, paramPackageId);
                var softwarePkgId = softwarePackage.FirstOrDefault().SOFT_PKG_ID;

                inserMenuSql = " INSERT INTO ERP_CM_SYSTEM_MENU (SYS_MENU_TITLE,SYS_MENU_TYPE, PATH_NAME,CMP_BRANCH_ID,SYS_MENU_PARENT,SOFT_PKG_ID,MENU_ICON,SYS_MENU_SERIAL ) VALUES (:SYS_MENU_TITLE,:SYS_MENU_TYPE,:PATH_NAME,:CMP_BRANCH_ID,:SYS_MENU_PARENT,:SOFT_PKG_ID, :MENU_ICON,:SYS_MENU_SERIAL  ) ";
                var paramMenu = new { SYS_MENU_PARENT = parentMenuId, SYS_MENU_TITLE = menuTitle, PATH_NAME = pathName, SOFT_PKG_ID = softwarePkgId, SYS_MENU_TYPE = menuType, SYS_MENU_SERIAL = menuSerial, CMP_BRANCH_ID = softwarePackage.FirstOrDefault().CMP_BRANCH_ID , MENU_ICON=menuIcon };
                var insertmenuData = await conn.QueryAsync<CmSystemAccessPolicy>(inserMenuSql, paramMenu);

                return "record saved ";
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> CreateRoleWiseAccessPolicyMenu(string usrId, string roleId, string menuId, List<string> decryptMenuList)
        {
            string strSql = "";
            string menuData = "";
            string insertSqlUserMenu = "";
            int insertFlag = 0;
            string strUpdateMenuidInactive = "";
            try
            {
                var param = new { SYS_USER_ROLE_ID = roleId, SYS_MENU_ID = menuId, sys_user_id = usrId };
                using var conn = new SqliteConnection(_conString);
                if (string.IsNullOrEmpty(usrId))
                {
                    string chkSqlGRoupMenu = " SELECT * FROM ERP_SYSTEM_ROLE_MENU_MAPPING WHERE SYS_USER_ROLE_ID=:SYS_USER_ROLE_ID AND SYS_MENU_ID=:SYS_MENU_ID and IS_ACTIVE = 'Y'    ";
                    var chkMenuDataExist = await conn.QueryAsync<CmSystemAccessPolicy>(chkSqlGRoupMenu, param);

                    if (chkMenuDataExist.Count() == 0)
                    {
                        string chkSqlGRoupMenuInactive = " SELECT * FROM ERP_SYSTEM_ROLE_MENU_MAPPING WHERE SYS_USER_ROLE_ID=:SYS_USER_ROLE_ID AND SYS_MENU_ID=:SYS_MENU_ID and IS_ACTIVE = 'N'    ";
                        var chkMenuInactive = await conn.QueryAsync<CmSystemAccessPolicy>(chkSqlGRoupMenuInactive, param);
                        if (chkMenuInactive.Count() == 0)
                        {
                            strSql = "INSERT INTO ERP_SYSTEM_ROLE_MENU_MAPPING (SYS_USER_ROLE_ID,SYS_MENU_ID,IS_ACTIVE) VALUES (:SYS_USER_ROLE_ID,:SYS_MENU_ID,'Y' ) ";
                            var menuInsert = await conn.QueryAsync<CmSystemAccessPolicy>(strSql, param);
                        }
                        else
                        {
                            string chkSqlUpdateMenuToActive = " UPDATE ERP_SYSTEM_ROLE_MENU_MAPPING set IS_ACTIVE = 'Y'  WHERE SYS_USER_ROLE_ID=:SYS_USER_ROLE_ID AND SYS_MENU_ID=:SYS_MENU_ID  ";
                            var UpdateMenuActive = await conn.QueryAsync<CmSystemAccessPolicy>(chkSqlUpdateMenuToActive, param);
                        }

                        insertFlag = 1;
                    }
                    else
                    {
                        menuData = "record exist";
                    }
                    string getSqlGRoupMenu = " SELECT SYS_MENU_ID FROM ERP_SYSTEM_ROLE_MENU_MAPPING WHERE SYS_USER_ROLE_ID=:SYS_USER_ROLE_ID and IS_ACTIVE = 'Y'   ";
                    var geGrouptMenuDataExist = await conn.QueryAsync<CmSystemAccessPolicy>(getSqlGRoupMenu, param);
                    if (geGrouptMenuDataExist.Count() > decryptMenuList.Count())
                    {
                        foreach (var mapmenuItem in geGrouptMenuDataExist)
                        {
                            var match = decryptMenuList.FirstOrDefault(x => x == mapmenuItem.SYS_MENU_ID);
                            if (match == null)
                            {
                                strUpdateMenuidInactive = "update ERP_SYSTEM_ROLE_MENU_MAPPING set IS_ACTIVE='N' WHERE SYS_USER_ROLE_ID=:SYS_USER_ROLE_ID AND SYS_MENU_ID=:SYS_MENU_ID   ";
                                var paramUpdateMenuInactive = new { SYS_USER_ROLE_ID = roleId, SYS_MENU_ID = mapmenuItem.SYS_MENU_ID };
                                var updateDeselectedId = await conn.QueryAsync<CmSystemAccessPolicy>(strUpdateMenuidInactive, paramUpdateMenuInactive);
                                insertFlag = 1;
                            }
                        }
                    }
                }
                else
                {
                    string chkSqlGroupMenu = "select * from ERP_USR_ROLE_MENU_MAPPING where SYS_USER_ROLE_MAP_ID = ( select distinct URM.SYS_USER_ROLE_MAP_ID from erp_system_users_role_mapping URM left join ERP_SYSTEM_ROLE_MENU_MAPPING SRMM on URM.SYS_USER_ROLE_ID = SRMM.SYS_USER_ROLE_ID where URM.sys_user_id = :sys_user_id ) and SYS_MENU_ID=:SYS_MENU_ID  ";
                    var chkMenuDataExist = await conn.QueryAsync<CmSystemAccessPolicy>(chkSqlGroupMenu, param);

                    if (chkMenuDataExist.Count() == 0)
                    {
                        string sqlRolMapId = "select SYS_USER_ROLE_MAP_ID from erp_system_users_role_mapping URM left join ERP_SYSTEM_ROLE_MENU_MAPPING SRMM on URM.SYS_USER_ROLE_ID = SRMM.SYS_USER_ROLE_ID where URM.sys_user_id = :sys_user_id  ";
                        var data = await conn.QueryAsync<CmSystemAccessPolicy>(sqlRolMapId, param);
                        insertSqlUserMenu = "insert into ERP_USR_ROLE_MENU_MAPPING (SYS_USER_ROLE_ID, SYS_USER_ROLE_MAP_ID, SYS_MENU_ID) values(:SYS_USER_ROLE_ID,:SYS_USER_ROLE_MAP_ID, :SYS_MENU_ID)";
                        var paramUsermenu = new { SYS_USER_ROLE_ID = roleId, SYS_MENU_ID = menuId, SYS_USER_ROLE_MAP_ID = data.FirstOrDefault().SYS_USER_ROLE_MAP_ID };
                        var menuInsert = await conn.QueryAsync<CmSystemAccessPolicy>(insertSqlUserMenu, paramUsermenu);

                        insertFlag = 1;
                    }
                    else
                    {
                        menuData = "record exist";
                    }

                }
                if (insertFlag == 1)
                {
                    menuData = "record saved";
                }

                return menuData;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);

            }
        }

        internal async Task<IEnumerable<CmSystemAccessPolicy>> GetUserWiseAccessPolicy()
        {
            string strSql = "";
            try
            {
                strSql = @" SELECT  SU.SYS_USR_ID, SM.SYS_MENU_ID, SM.SYS_MENU_TITLE , SM.PATH_NAME, SU.SYS_USR_LOGIN_NAME
                           FROM ERP_USR_ROLE_MENU_MAPPING  URMM
                          left join ERP_SYSTEM_USERS_ROLE_MAPPING URM ON URM.SYS_USER_ROLE_MAP_ID = URMM.SYS_USER_ROLE_MAP_ID
                           LEFT JOIN  ERP_CM_SYSTEM_MENU SM  ON SM.SYS_MENU_ID = URMM.SYS_MENU_ID
                           left join ERP_CM_SYSTEM_USERS SU ON SU.SYS_USR_ID = URM.sys_user_id
                           where URM.SYS_USER_ROLE_ID = SU.SYS_USR_GRP_ID
                          order by SU.SYS_USR_ID, SM.SYS_MENU_ID asc   ";
                using var conn = new SqliteConnection(_conString);
                var menuData = await conn.QueryAsync<CmSystemAccessPolicy>(strSql, null);
                return menuData;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<CmSystemAccessPolicy>> GetMenuByGroup(string menuId,string userId)
        {
            string sQl = "";
            string strSqlCompany = "";

            try
            {
                strSqlCompany = " select COM.SOFT_PKG_ID,CB.CMP_BRANCH_ID from ERP_CM_COMPANY COM,ERP_CM_SYSTEM_USERS SU,ERP_CM_CMP_BRANCH CB where SU.COMPANY_ID= COM.COMPANY_ID AND CB.CMP_COMPANY_ID=COM.PARENT_ID AND CB.CMP_BRANCH_TYPE_ID='A' and SU.sys_usr_id=:sys_usr_id  ";
                var paramPackageId = new { sys_usr_id = userId };
                using var conn = new SqliteConnection(_conString);
                var softwarePackage = await conn.QueryAsync<CmSystemUsers>(strSqlCompany, paramPackageId);
                var softwarePkgId = softwarePackage.FirstOrDefault().SOFT_PKG_ID;
                if (!string.IsNullOrEmpty(menuId))
                {
                    sQl = " SELECT SM.* FROM ERP_CM_SYSTEM_MENU SM  " +
                    " WHERE SM.SYS_MENU_ID = :MENU_ID  order by SYS_MENU_ID ";
                }
                else
                {
                    sQl = " SELECT SM.* FROM ERP_CM_SYSTEM_MENU SM  where SOFT_PKG_ID = :SOFT_PKG_ID " +
                        " order by SYS_MENU_ID  desc   ";
                }
                var param = new { MENU_ID = menuId, SOFT_PKG_ID= softwarePkgId };
                var menuData = await conn.QueryAsync<CmSystemAccessPolicy>(sQl, param);
                return menuData;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }

        }

        internal async Task<string> UpdateSystemMenuById(string userId, string decrypMenuId,string decryptMenuFile,string decryptMenuTitle, string menuIcon,string menuSerial)
        {
            string strSqlCompany = "";
            string sqlMenuById = "";
            try
            {
                strSqlCompany = " select COM.SOFT_PKG_ID,CB.CMP_BRANCH_ID from ERP_CM_COMPANY COM,ERP_CM_SYSTEM_USERS SU,ERP_CM_CMP_BRANCH CB where SU.COMPANY_ID= COM.COMPANY_ID AND CB.CMP_COMPANY_ID=COM.PARENT_ID AND CB.CMP_BRANCH_TYPE_ID='A' and SU.sys_usr_id=:sys_usr_id  ";
                var paramPackageId = new { sys_usr_id = userId };
                using var conn = new SqliteConnection(_conString);
                var softwarePackage = await conn.QueryAsync<CmSystemUsers>(strSqlCompany, paramPackageId);
                var softwarePkgId = softwarePackage.FirstOrDefault().SOFT_PKG_ID;

                var param = new { SYS_MENU_ID = decrypMenuId, SYS_MENU_TITLE= decryptMenuTitle, PATH_NAME= decryptMenuFile, MENU_ICON= menuIcon, SYS_MENU_SERIAL= menuSerial, SOFT_PKG_ID = softwarePkgId };

                sqlMenuById = " UPDATE ERP_CM_SYSTEM_MENU   SET SYS_MENU_TITLE=:SYS_MENU_TITLE,PATH_NAME=:PATH_NAME,MENU_ICON=:MENU_ICON  WHERE SYS_MENU_ID =:SYS_MENU_ID  ";
                
                var updateMenuData = await conn.QueryAsync<CmSystemAccessPolicy>(sqlMenuById, param);

                
                return " Successfully Updated ";
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<IEnumerable<RoleWiseMenu>> RoleWiseMenu(string userId)
        {
            string sQl = "";
            try
            {
                using var conn = new SqliteConnection(_conString);

                string strSqlCompany = " select COM.SOFT_PKG_ID,CB.CMP_BRANCH_ID from ERP_CM_COMPANY COM,ERP_CM_SYSTEM_USERS SU,ERP_CM_CMP_BRANCH CB where SU.COMPANY_ID= COM.COMPANY_ID AND CB.CMP_COMPANY_ID=COM.PARENT_ID AND CB.CMP_BRANCH_TYPE_ID='A' and SU.sys_usr_id=:sys_usr_id  ";
                var paramPackageId = new { sys_usr_id = userId };
                var softwarePackage = await conn.QueryAsync<CmSystemUsers>(strSqlCompany, paramPackageId);
                var softwarePkgId = softwarePackage.FirstOrDefault().SOFT_PKG_ID;
                //sQl = "SELECT DISTINCT CSM.SYS_MENU_ID, CSM.SYS_MENU_TITLE, CSM.SYS_MENU_PARENT, CSM.SYS_MENU_TYPE, DECODE(URMM.IS_ACTIVE,'Y','Y','N') IS_ACTIVE " +
                //    "FROM ERP_SYSTEM_ROLE_MENU_MAPPING SRMM, ERP_USR_ROLE_MENU_MAPPING URMM, ERP_SYSTEM_USERS_ROLE_MAPPING SURM, ERP_CM_SYSTEM_USERS CSU, " +
                //    "ERP_CM_SYSTEM_MENU CSM WHERE URMM.SYS_MENU_ID(+) = SRMM.SYS_MENU_ID AND URMM.SYS_USER_ROLE_ID = SURM.SYS_USER_ROLE_ID(+)  " +
                //    "AND SURM.SYS_USER_ID = CSU.SYS_USR_ID(+) AND SRMM.SYS_MENU_ID = CSM.SYS_MENU_ID AND CSU.SYS_USR_ID(+) = '"+ userId + "' ";
                sQl = @"SELECT distinct SM.SYS_MENU_ID, SM.SYS_MENU_TITLE, SM.SYS_MENU_PARENT, SM.SYS_MENU_TYPE,
CASE
    WHEN URMM.IS_ACTIVE = 'Y' THEN 'Y'
    ELSE 'N'
END AS IS_ACTIVE
FROM ERP_USR_ROLE_MENU_MAPPING URMM
LEFT JOIN ERP_SYSTEM_USERS_ROLE_MAPPING URM ON URM.SYS_USER_ROLE_MAP_ID = URMM.SYS_USER_ROLE_MAP_ID
LEFT JOIN ERP_CM_SYSTEM_MENU SM ON SM.SYS_MENU_ID = URMM.SYS_MENU_ID
WHERE URM.SYS_USER_ID = :sys_usr_id 

UNION

SELECT SYS_MENU_ID, SYS_MENU_TITLE, SYS_MENU_PARENT, SYS_MENU_TYPE, 'Y' AS IS_ACTIVE
FROM ERP_CM_SYSTEM_MENU
WHERE SYS_MENU_ID IN (
    SELECT SYS_MENU_PARENT
    FROM (
        SELECT *
        FROM ERP_USR_ROLE_MENU_MAPPING URMM
        LEFT JOIN ERP_SYSTEM_USERS_ROLE_MAPPING URM ON URM.SYS_USER_ROLE_MAP_ID = URMM.SYS_USER_ROLE_MAP_ID
        LEFT JOIN ERP_CM_SYSTEM_MENU SM ON SM.SYS_MENU_ID = URMM.SYS_MENU_ID
    )
)

UNION

SELECT distinct SM.SYS_MENU_ID, SM.SYS_MENU_TITLE, SM.SYS_MENU_PARENT, SM.SYS_MENU_TYPE, 'N' AS IS_ACTIVE
FROM ERP_CM_SYSTEM_MENU SM
WHERE SM.SOFT_PKG_ID = '10081411060000000'
AND SM.SYS_MENU_ID NOT IN (
    SELECT distinct SM.SYS_MENU_ID
    FROM ERP_USR_ROLE_MENU_MAPPING URMM
    LEFT JOIN ERP_SYSTEM_USERS_ROLE_MAPPING URM ON URM.SYS_USER_ROLE_MAP_ID = URMM.SYS_USER_ROLE_MAP_ID
    LEFT JOIN ERP_CM_SYSTEM_MENU SM ON SM.SYS_MENU_ID = URMM.SYS_MENU_ID
    WHERE URM.SYS_USER_ID =:sys_usr_id AND URMM.IS_ACTIVE = 'Y'
)
AND SM.SYS_MENU_ID NOT IN (
    SELECT SYS_MENU_ID
    FROM ERP_CM_SYSTEM_MENU
    WHERE SYS_MENU_ID IN (
        SELECT SYS_MENU_PARENT
        FROM (
            SELECT *
            FROM ERP_USR_ROLE_MENU_MAPPING URMM
            LEFT JOIN ERP_SYSTEM_USERS_ROLE_MAPPING URM ON URM.SYS_USER_ROLE_MAP_ID = URMM.SYS_USER_ROLE_MAP_ID
            LEFT JOIN ERP_CM_SYSTEM_MENU SM ON SM.SYS_MENU_ID = URMM.SYS_MENU_ID
        )
    )
)
";

                var param = new { sys_usr_id = userId };
                var menuData = await conn.QueryAsync<RoleWiseMenu>(sQl, param);
                return menuData;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }

        internal async Task<string> PostUserWiseMenu(string decrypUserId, List<string> decryptMenuList)
        {
            string sQl = "";
            try
            {
                sQl = " select * from ERP_SYSTEM_USERS_ROLE_MAPPING where SYS_USER_ID = :SYS_USER_ID ";
                var param = new { SYS_USER_ID = decrypUserId };
                using var conn = new SqliteConnection(_conString);
                var userRoleMapId = await conn.QuerySingleAsync<SystemUserRoleMapping>(sQl, param);
                if(userRoleMapId != null)
                {
                    var existingData = "select * from ERP_USR_ROLE_MENU_MAPPING Where SYS_USER_ROLE_MAP_ID=:SYS_USER_ROLE_MAP_ID ";
                    var existingDataParm = new { SYS_USER_ROLE_MAP_ID = userRoleMapId.SYS_USER_ROLE_MAP_ID };
                    var existing = await conn.QueryAsync<SystemUsersRoleMenuMapping>(existingData, existingDataParm);

                    if(existing.Count() > 0)
                    {
                        var userMenu = "update ERP_USR_ROLE_MENU_MAPPING set IS_ACTIVE='N' Where SYS_USER_ROLE_MAP_ID=:SYS_USER_ROLE_MAP_ID ";
                        var userMenuParm = new { SYS_USER_ROLE_MAP_ID = userRoleMapId.SYS_USER_ROLE_MAP_ID };
                        await conn.QueryAsync<SystemUsersRoleMenuMapping>(userMenu, userMenuParm);
                    }                    

                    foreach (var menu in decryptMenuList)
                    {
                        var existingMenuSql = " select * from ERP_USR_ROLE_MENU_MAPPING where SYS_MENU_ID=:SYS_MENU_ID AND SYS_USER_ROLE_ID=:SYS_USER_ROLE_ID AND SYS_USER_ROLE_MAP_ID=:SYS_USER_ROLE_MAP_ID ";
                        var existingMenuParam = new { SYS_MENU_ID = menu,
                                                    SYS_USER_ROLE_ID = userRoleMapId.SYS_USER_ROLE_ID,
                                                    SYS_USER_ROLE_MAP_ID = userRoleMapId.SYS_USER_ROLE_MAP_ID
                                                    };
                        var existingMenu = await conn.QueryAsync<SystemUserRoleMapping>(existingMenuSql, existingMenuParam);

                        if (existingMenu.Count() > 0)
                        {
                            var menuSql = " update ERP_USR_ROLE_MENU_MAPPING set IS_ACTIVE='Y' where SYS_MENU_ID=:SYS_MENU_ID AND SYS_USER_ROLE_ID=:SYS_USER_ROLE_ID AND" +
                                        " SYS_USER_ROLE_MAP_ID=:SYS_USER_ROLE_MAP_ID  ";
                            var menuParam = new {
                                                SYS_MENU_ID = menu,
                                                SYS_USER_ROLE_ID = userRoleMapId.SYS_USER_ROLE_ID,
                                                SYS_USER_ROLE_MAP_ID = userRoleMapId.SYS_USER_ROLE_MAP_ID
                                                };
                            await conn.QueryAsync<SystemUsersRoleMenuMapping>(menuSql, menuParam);
                        }
                        else
                        {
                            var menuSql = " INSERT INTO ERP_USR_ROLE_MENU_MAPPING (SYS_USER_ROLE_ID, SYS_USER_ROLE_MAP_ID, SYS_MENU_ID, IS_ACTIVE) " +
                                        "values (:SYS_USER_ROLE_ID,  :SYS_USER_ROLE_MAP_ID, :SYS_MENU_ID, :IS_ACTIVE) ";
                            var menuParam = new
                            {
                                SYS_USER_ROLE_ID = userRoleMapId.SYS_USER_ROLE_ID,
                                SYS_USER_ROLE_MAP_ID = userRoleMapId.SYS_USER_ROLE_MAP_ID,
                                SYS_MENU_ID = menu,
                                IS_ACTIVE = "Y"
                            };
                            await conn.QueryAsync<SystemUsersRoleMenuMapping>(menuSql, menuParam);
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string strResult = ex.Message.ToString();
                throw new Exception(strResult);
            }
        }
    }
}
