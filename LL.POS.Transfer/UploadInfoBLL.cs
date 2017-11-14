namespace LL.POS.Transfer
{
    using System;
    using System.Data;
    /// <summary>
    /// 上传数据业务逻辑
    /// </summary>
    public class UploadInfoBLL
    {
        #region 单例模式
        private static object lockobj = new object();
        private static UploadInfoBLL _instance = null;
        private UploadInfoBLL()
        { }
        public static UploadInfoBLL Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockobj)
                    {
                        if (_instance == null)
                        {
                            _instance = new UploadInfoBLL();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        #region  公共方法
        /// <summary>
        /// 获取需要上传消费流水号信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetUploadPayNo(String connectinString)
        {
            return UploadInfoDAL.GetUploadPayNo(connectinString);
        }
        /// <summary>
        /// 获取需要上传消费支付流水信息
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="flowno"></param>
        /// <returns></returns>
        public DataTable GetUploadPayInfoByFlowNo(String connectionString, String flowno)
        {
            return UploadInfoDAL.GetUploadPayInfoByFlowNo(connectionString, flowno);
        }
        /// <summary>
        /// 获取需要上传销售流水信息
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="flowno"></param>
        /// <returns></returns>
        public DataTable GetUploadSaleInfoByFlowNo(String connectionString, String flowno)
        {
            return UploadInfoDAL.GetUploadSaleInfoByFlowNo(connectionString, flowno);
        }

        /// <summary>
        /// 更新上传数据标志字段
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="flowno"></param>
        /// <returns></returns>
        public bool UpdateFlowComFlag(String connectionString, String flowno)
        {
            return UploadInfoDAL.UpdateFlowComFlag(connectionString, flowno);
        }
        /// <summary>
        /// 获取离线会员消费信息
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public DataTable GetNoneUploadMember(String connectionString)
        {
            return UploadInfoDAL.GetNonUploadMember(connectionString);
        }
        /// <summary>
        /// 更新离线会员消费记录处理状态
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="flowno">消费流水号</param>
        /// <param name="cardno">会员卡号</param>
        /// <returns>成功true失败false</returns>
        public bool UpdateUploadMember(String connectionString, String flowno, String cardno, String over_flag, String com_flag)
        {
            return UploadInfoDAL.UpdateUploadMember(connectionString, flowno, cardno, over_flag, com_flag);
        }
        /// <summary>
        /// 获取未上传的收银员对账记录
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public DataTable GetUploadAccount(String connectionString)
        {
            return UploadInfoDAL.GetUploadAccount(connectionString);
        }
        /// <summary>
        /// 更新对账记录上传标志
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="flow_id">记录ID</param>
        /// <returns>成功true失败false</returns>
        public bool UpdateAccountFlag(String connectionString, String flow_id)
        {
            return UploadInfoDAL.UpdateAccountFlag(connectionString, flow_id);
        }

        /// <summary>
        /// 获取未上传的收银员对账记录
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public  DataTable GetUploadVip(String connectionString)
        {
            return UploadInfoDAL.GetUploadVip(connectionString);
        }
        /// <summary>
        /// 更新对账记录上传标志
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="flow_id">记录ID</param>
        /// <returns>成功true失败false</returns>
        public  bool UpdateVipFlag(String connectionString, String flow_no, String card_no)
        {
            return UploadInfoDAL.UpdateVipFlag(connectionString, flow_no, card_no);
        }
        #endregion
    }
}
