
namespace LL.POS
{


    using System;
    using System.Collections.Generic;
    using System.Data;
    //using LL.POS.Msmk;
    public class ServerBll
    {
        private string _serverUrl = string.Empty;

        /// <summary>
        /// 测试连接是否成功
        /// </summary>
        /// <returns></returns>
        public bool TestConnection()
        {
            try
            {
                //new PosServices { Url = GetWebRequestAddress(Gattr.HttpAddress, Gattr.WebPosServicePath), Timeout = 0x2710 }.TestConnection();
                return true;
            }
            catch (Exception exception)
            {
                Gattr.ErrMsg = "连接出错，错误信息为:\r\n\r\n" + exception.Message;
                return false;
            }
        }

        //public PosServices GetPosServices()
        //{
        //    PosServices pos = null;
        //    if (TestConnection())
        //    {
        //        pos = new PosServices();
        //        pos.Url = GetWebRequestAddress(Gattr.HttpAddress, Gattr.WebPosServicePath);
        //        return pos;

        //    }
        //    else
        //    {
        //        return null;
        //    }

        //}
        /// <summary>
        /// 要货商品列表
        /// </summary>
        /// <param name="sheetNO"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        //public t_pm_sheet_master GetPMSheetDetail(string sheetNO, ref DataTable dt)
        //{
        //    if (GetPosServices() != null)
        //    {
        //        return this.GetPosServices().GetPMSheetDetail(sheetNO, ref dt);
        //    }
        //    return null;
        //}
        /// <summary>
        /// 更新付款列表
        /// </summary>
        /// <param name="sheetNO"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        //public bool InPayFlowList(List<t_rm_payflow> payflowList)
        //{
        //    if (GetPosServices() != null)
        //    {
        //        return this.GetPosServices().InPayFlowList(payflowList.ToArray());
        //    }
        //    return false;
        //}
        /// <summary>
        /// 返回URL的访问地址
        /// </summary>
        /// <param name="uri">服务的地址或者IP</param>
        /// <param name="path">服务地址</param>
        /// <returns>address字符串</returns>
        private String GetWebRequestAddress(String uri, String path)
        {
            String address = String.Empty;
            if (!SIString.IsNullOrEmptyOrDBNull(uri) && !SIString.IsNullOrEmptyOrDBNull(path))
            {
                if (uri.EndsWith(@"/"))
                {
                    address = string.Format(@"{0}{1}", uri, path);
                }
                else
                {
                    address = string.Format(@"{0}/{1}", uri, path);
                }
            }
            return address;
        }
        //public string SetPwd()
        //{
        //    PosServices service = new PosServices();
        //    try
        //    {
        //        service.Url = Gattr.WebPosServicePath;
        //        // return service.vippasswordchange("15020364165", "8f92b9ac87ccc718dea786477e46e1c4");
        //    }
        //    catch (Exception exception)
        //    {
        //        throw exception;
        //    }
        //    finally
        //    {
        //        service.Dispose();
        //    }
        //    return "";
        //}

        //public string GetHellWorld()
        //{
        //    //if (this.TestConnection())
        //    //{
        //    //PosServices service = new PosServices();
        //    //try
        //    //{
        //    //    service.Url = this.GetWebUrl(Gattr.WebPosServicePath);

        //    //    return "";// service.GetData("sss");
        //    //}
        //    //catch (Exception exception)
        //    //{
        //    //    throw exception;
        //    //}
        //    //finally
        //    //{
        //    //    service.Dispose();
        //    //}
        //    //}
        //    //return "";
        //}

        /// <summary>
        /// URL
        /// </summary>
        /// <param name="webPath"></param>
        /// <returns></returns>
        public string GetWebUrl(string webPath)
        {
            if (SIString.TryStr(this._serverUrl).Trim() == string.Empty)
            {
                throw new Exception("服务器地址不能为空！");
            }
            return SIString.TryStr(this._serverUrl + webPath).Trim();
        }
        /// <summary>
        /// 服务器URL
        /// </summary>
        public string ServerUrl
        {
            get { return _serverUrl; }
            set { _serverUrl = value; }
        }
    }
}
