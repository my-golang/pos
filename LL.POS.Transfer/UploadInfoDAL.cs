namespace LL.POS.Transfer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;
    using LL.POS.Common;
    using LL.Utility;
    /// <summary>
    /// 上传数据数据逻辑
    /// </summary>
    public class UploadInfoDAL
    {
        /// <summary>
        /// 获取需要上传消费流水号信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetUploadPayNo(String connectionString)
        {
            DataTable table = null;
            String sql = String.Empty;
            String errorMessage = String.Empty;
            try
            {
                sql = "SELECT distinct flow_no FROM t_app_payflow WHERE  com_flag<>1 order by flow_no ";//datetime(oper_date) < datetime('now','-5 second','localtime') and
                table = DbUtilitySQLite.Instance.GetDataTable(connectionString, sql, null, ref errorMessage);
                if (errorMessage.Length > 0)
                {
                    LoggerHelper.Log("MsmkLogger", "LL.POS.DAL--->PayInfoDAL----->GetUploadPayNo---->Error:" + errorMessage, LogEnum.ExceptionLog);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS.DAL--->PayInfoDAL----->GetUploadPayNo---->Error:" + ex.ToString(), LogEnum.ExceptionLog);
            }
            return table;
        }
        /// <summary>
        /// 获取需要上传消费支付流水信息
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="flowno"></param>
        /// <returns></returns>
        public static DataTable GetUploadPayInfoByFlowNo(String connectionString, String flowno)
        {
            DataTable table = null;
            String sql = String.Empty;
            SQLiteParameter[] parameters = null;
            String errorMessage = String.Empty;
            try
            {
                sql = "SELECT flow_id,flow_no,sale_amount,pay_way,pay_amount,coin_type,pay_name,coin_rate,convert_amt,card_no,memo,oper_date,oper_id,voucher_no,branch_no,pos_id,sale_way,com_flag,vip_no FROM t_app_payflow WHERE flow_no=@flow_no and com_flag<>1";
                parameters = new SQLiteParameter[1];
                parameters[0] = new SQLiteParameter("@flow_no", DbType.String);
                parameters[0].Value = flowno;
                table = DbUtilitySQLite.Instance.GetDataTable(connectionString, sql, parameters, ref errorMessage);
                if (errorMessage.Length > 0)
                {
                    LoggerHelper.Log("MsmkLogger", "LL.POS.DAL--->PayInfoDAL----->GetUploadPayInfoByFlowNo---->Error:" + errorMessage, LogEnum.ExceptionLog);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS.DAL--->PayInfoDAL----->GetUploadPayInfoByFlowNo---->Error:" + ex.ToString(), LogEnum.ExceptionLog);
            }
            return table;
        }
        /// <summary>
        /// 获取需要上传销售流水信息
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="flowno"></param>
        /// <returns></returns>
        public static DataTable GetUploadSaleInfoByFlowNo(String connectionString, String flowno)
        {
            DataTable table = null;
            String sql = String.Empty;
            SQLiteParameter[] parameters = null;
            String errorMessage = String.Empty;
            try
            {
                sql = "SELECT flow_id,flow_no,item_no,unit_price,sale_price,sale_qnty,sale_money,sale_way,discount_rate,oper_id,oper_date,item_subno,item_clsno,item_status,reasonid,branch_no,pos_id,price,plan_no,item_brand  FROM t_app_saleflow WHERE flow_no=@flow_no and com_flag<>1";
                parameters = new SQLiteParameter[1];
                parameters[0] = new SQLiteParameter("@flow_no", DbType.String);
                parameters[0].Value = flowno;
                table = DbUtilitySQLite.Instance.GetDataTable(connectionString, sql, parameters, ref errorMessage);
                if (errorMessage.Length > 0)
                {
                    LoggerHelper.Log("MsmkLogger", "LL.POS.DAL--->PayInfoDAL----->GetUploadSaleInfoByFlowNo---->Error:" + errorMessage, LogEnum.ExceptionLog);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS.DAL--->PayInfoDAL----->GetUploadSaleInfoByFlowNo---->Error:" + ex.ToString(), LogEnum.ExceptionLog);
            }
            return table;
        }
        /// <summary>
        /// 更新上传数据标志字段
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="flowno"></param>
        /// <returns></returns>
        public static bool UpdateFlowComFlag(String connectionString, String flowno)
        {
            bool isok = true;
            //SqlTransEntity
            List<SqlParaEntity> sqlParas = null;
            SQLiteParameter[] parameters = null;
            String errorMessage = String.Empty;
            try
            {
                sqlParas = new List<SqlParaEntity>();
                parameters = new SQLiteParameter[1];
                parameters[0] = new SQLiteParameter("@flow_no", flowno);
                sqlParas.Add(new SqlParaEntity()
                {
                    Sql = "update t_app_payflow set com_flag='1' where flow_no=@flow_no",
                    parameters = parameters
                });
                sqlParas.Add(new SqlParaEntity()
                {
                    Sql = "update t_app_saleflow set com_flag='1' where flow_no=@flow_no",
                    parameters = parameters
                });
                isok = DbUtilitySQLite.Instance.ExecuteSqlsByTrans(connectionString, sqlParas, ref errorMessage) > 0 ? true : false;
                if (errorMessage.Length > 0)
                {
                    LoggerHelper.Log("MsmkLogger", "LL.POS.DAL--->PayInfoDAL----->UpdateFlowComFlag---->Error:" + errorMessage, LogEnum.ExceptionLog);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS.DAL--->PayInfoDAL----->UpdateFlowComFlag---->Error:" + ex.ToString(), LogEnum.ExceptionLog);
            }
            return isok;
        }
        /// <summary>
        /// 获取未上传的会员消费信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetNonUploadMember(String connectionString)
        {
            DataTable table = null;
            string errorMessage = string.Empty;
            string sql = string.Empty;
            try
            {
                sql = "select flow_no,card_no,score,voucher_no,oper_date from t_app_viplist where over_flag<>1 ";
                table = DbUtilitySQLite.Instance.GetDataTable(connectionString, sql, null, ref errorMessage);
                if (errorMessage.Length > 0)
                {
                    LoggerHelper.Log("MsmkLogger", "LL.POS.Transger->UploadInfoDAL-->Error:" + errorMessage, LogEnum.ExceptionLog);
                }
            }
            catch (SQLiteException sqlex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS.Transger->UploadInfoDAL-->SQLiteException:" + sqlex.ToString(), LogEnum.ExceptionLog);
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS.Transger->UploadInfoDAL-->Exception:" + ex.ToString(), LogEnum.ExceptionLog);
            }
            return table;
        }
        /// <summary>
        /// 更新会员消费状态修改
        /// </summary>
        /// <param name="connectString">SQLite数据库连接字符串</param>
        /// <param name="flowno">消费流水</param>
        /// <param name="cardno">会员卡号</param>
        /// <returns>成功true，失败false</returns>
        public static bool UpdateUploadMember(String connectionString, String flowno, String cardno, String over_flag, String com_flag)
        {
            bool isok = false;
            string errorMessage = string.Empty;
            string sql = string.Empty;
            SQLiteParameter[] _parameters = null;
            try
            {
                sql = "update t_app_viplist set over_flag=@over_flag,com_flag=@com_flag where flow_no=@flow_no and card_no=@card_no";
                _parameters = new SQLiteParameter[4];
                _parameters[0] = new SQLiteParameter("@flow_no", flowno);
                _parameters[1] = new SQLiteParameter("@card_no", cardno);
                _parameters[2] = new SQLiteParameter("@com_flag", com_flag);
                _parameters[3] = new SQLiteParameter("@over_flag", over_flag);

                int result = DbUtilitySQLite.Instance.ExecuteSql(connectionString, sql, _parameters, ref errorMessage);
                if (result > 0)
                {
                    isok = true;
                }
                if (errorMessage.Length > 0)
                {
                    LoggerHelper.Log("MsmkLogger", "LL.POS.Transger->UpdateUploadMember-->Error:" + errorMessage, LogEnum.ExceptionLog);
                }
            }
            catch (SQLiteException sqlex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS.Transger->UpdateUploadMember-->SQLiteException:" + sqlex.ToString(), LogEnum.ExceptionLog);
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS.Transger->UpdateUploadMember-->Exception:" + ex.ToString(), LogEnum.ExceptionLog);
            }
            return isok;
        }
        /// <summary>
        /// 获取未上传的收银员对账记录
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static DataTable GetUploadAccount(String connectionString)
        {
            DataTable table = null;
            string errorMessage = string.Empty;
            string sql = string.Empty;
            try
            {
                sql = "select flow_id,branch_no,pos_id,oper_id,oper_date,start_time,end_time,sale_amt,hand_amt from t_rm_pos_account where com_flag<>1 ";
                table = DbUtilitySQLite.Instance.GetDataTable(connectionString, sql, null, ref errorMessage);
                if (errorMessage.Length > 0)
                {
                    LoggerHelper.Log("MsmkLogger", "LL.POS.Transger->GetUploadAccount-->Error:" + errorMessage, LogEnum.ExceptionLog);
                }
            }
            catch (SQLiteException sqlex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS.Transger->GetUploadAccount-->SQLiteException:" + sqlex.ToString(), LogEnum.ExceptionLog);
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS.Transger->GetUploadAccount-->Exception:" + ex.ToString(), LogEnum.ExceptionLog);
            }
            return table;
        }
        /// <summary>
        /// 更新对账记录上传标志
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="flow_id">记录ID</param>
        /// <returns>成功true失败false</returns>
        public static bool UpdateAccountFlag(String connectionString, String flow_id)
        {
            bool isok = false;
            string errorMessage = string.Empty;
            string sql = string.Empty;
            SQLiteParameter[] _parameters = null;
            try
            {
                sql = "update t_rm_pos_account set com_flag=1 where flow_id=@flow_id ";
                _parameters = new SQLiteParameter[1];
                _parameters[0] = new SQLiteParameter("@flow_id", flow_id);
                int result = DbUtilitySQLite.Instance.ExecuteSql(connectionString, sql, _parameters, ref errorMessage);
                if (result > 0)
                {
                    isok = true;
                }
                if (errorMessage.Length > 0)
                {
                    LoggerHelper.Log("MsmkLogger", "LL.POS.Transger->UpdateAccountFlag-->Error:" + errorMessage, LogEnum.ExceptionLog);
                }
            }
            catch (SQLiteException sqlex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS.Transger->UpdateAccountFlag-->SQLiteException:" + sqlex.ToString(), LogEnum.ExceptionLog);
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS.Transger->UpdateAccountFlag-->Exception:" + ex.ToString(), LogEnum.ExceptionLog);
            }
            return isok;
        }


        /// <summary>
        /// 获取未上传的收银员对账记录
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static DataTable GetUploadVip(String connectionString)
        {
            DataTable table = null;
            string errorMessage = string.Empty;
            string sql = string.Empty;
            try
            {
                sql = "select flow_no,card_no,score,sale_amt,card_score,card_amount,oper_date,voucher_no from t_app_viplist where com_flag<>1 and over_flag=1";
                table = DbUtilitySQLite.Instance.GetDataTable(connectionString, sql, null, ref errorMessage);
                if (errorMessage.Length > 0)
                {
                    LoggerHelper.Log("MsmkLogger", "LL.POS.Transger->GetUploadVip-->Error:" + errorMessage, LogEnum.ExceptionLog);
                }
            }
            catch (SQLiteException sqlex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS.Transger->GetUploadVip-->SQLiteException:" + sqlex.ToString(), LogEnum.ExceptionLog);
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS.Transger->GetUploadVip-->Exception:" + ex.ToString(), LogEnum.ExceptionLog);
            }
            return table;
        }
        /// <summary>
        /// 更新对账记录上传标志
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="flow_id">记录ID</param>
        /// <returns>成功true失败false</returns>
        public static bool UpdateVipFlag(String connectionString, String flow_no, String card_no)
        {
            bool isok = false;
            string errorMessage = string.Empty;
            string sql = string.Empty;
            SQLiteParameter[] _parameters = null;
            try
            {
                sql = "update t_app_viplist set com_flag=1 where flow_no=@flow_no and card_no=@card_no";
                _parameters = new SQLiteParameter[2];
                _parameters[0] = new SQLiteParameter("@flow_no", flow_no);
                _parameters[1] = new SQLiteParameter("@card_no", card_no);
                int result = DbUtilitySQLite.Instance.ExecuteSql(connectionString, sql, _parameters, ref errorMessage);
                if (result > 0)
                {
                    isok = true;
                }
                if (errorMessage.Length > 0)
                {
                    LoggerHelper.Log("MsmkLogger", "LL.POS.Transger->UpdateVipFlag-->Error:" + errorMessage, LogEnum.ExceptionLog);
                }
            }
            catch (SQLiteException sqlex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS.Transger->UpdateVipFlag-->SQLiteException:" + sqlex.ToString(), LogEnum.ExceptionLog);
            }
            catch (Exception ex)
            {
                LoggerHelper.Log("MsmkLogger", "LL.POS.Transger->UpdateVipFlag-->Exception:" + ex.ToString(), LogEnum.ExceptionLog);
            }
            return isok;
        }
    }
}
