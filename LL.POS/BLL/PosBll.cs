namespace LL.POS
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;
    using System.Text;
    using LL.POS.Common;
    using LL.POS.Model;
    using LL.Utility;
    /// <summary>
    /// 业务逻辑类
    /// </summary>
    public class PosBll : BllBase
    {
        /// <summary>
        /// 生成对帐单据
        /// </summary>
        /// <param name="storeNo"></param>
        /// <param name="posId"></param>
        /// <param name="cashierId"></param>
        /// <param name="dtFrom"></param>
        /// <param name="dtTo"></param>
        /// <param name="intPrintLen"></param>
        /// <returns></returns>
        public List<string> AccountReportFromServer(string storeNo, string posId, string cashierId, DateTime dtFrom, DateTime dtTo, int intPrintLen)
        {
            List<string> list3;
            List<string> list = new List<string>();
            try
            {
                string str = "";
                string str2 = string.Empty;
                string str3 = string.Empty;
                int num = 0;
                int result = 0;
                int num3 = 0;
                int num4 = 0;
                decimal num5 = 0M;
                decimal num6 = 0M;
                decimal num7 = 0M;
                string branchNo = Gattr.BranchNo;//storeNo.Substring(0, 4);
                DataSet set = new DataSet();
                set = base._dal.GetPayCash(branchNo, cashierId, dtFrom, dtTo);
                if (set != null)
                {
                    DataTable table = set.Tables["t_rm_cashier_info"];
                    if (((table == null) || (table.Rows.Count == 0)) || (Convert.ToInt32(table.Rows[0][0]) == 0))
                    {
                        list.Add(str.PadLeft((intPrintLen < 0) ? 0 : intPrintLen, '-'));
                        list.Add(Gfunc.PrintStrAlign("收银对帐 ", intPrintLen, TextAlign.Center));
                        str = "";
                        list.Add(str.PadLeft((intPrintLen < 0) ? 0 : intPrintLen, '-'));
                        list.Add("收银员(" + cashierId + ")今天无收付款记录. ");
                        return list;
                    }
                    List<t_operator> cashierInfo = base._dal.GetOperatorInfo(Gattr.OperId, Gattr.BranchNo);
                    list.Add("");
                    list.Add(Gfunc.PrintStrAlign("   收银对帐单   ", intPrintLen, TextAlign.Center));
                    list.Add(Gfunc.PrintStrAlign("================", intPrintLen, TextAlign.Center));
                    str = "";
                    list.Add(str.PadLeft((intPrintLen < 0) ? 0 : intPrintLen, '-'));
                    list.Add("  机构: " + base._dal.GetBranchName(branchNo));
                    //list.Add("  仓库: " + base._dal.GetBranchName(storeNo));
                    list.Add("  收银机号: " + posId);
                    list.Add("  收 银 员: [" + cashierId.Trim() + "] " + cashierInfo[0].oper_name.Trim());
                    list.Add("  对账日期: " + DateTime.Today.ToString("yyyy-MM-dd"));
                    list.Add("  首笔: " + Convert.ToDateTime(table.Rows[0][1]).ToString("s"));
                    list.Add("  末笔: " + Convert.ToDateTime(table.Rows[0][2]).ToString("s"));
                    list.Add("  笔数: " + table.Rows[0][0].ToString());
                    str = "";
                    list.Add(str.PadLeft((intPrintLen < 0) ? 0 : intPrintLen, '-'));
                    DataTable table2 = set.Tables["t_rm_scash_info"];
                    if ((table2 != null) && (table2.Rows.Count > 0))
                    {
                        result = 0;
                        num5 = 0M;
                        int.TryParse(table2.Rows[0]["count"].ToString(), out result);
                        decimal.TryParse(table2.Rows[0]["amount"].ToString(), out num5);
                        list.Add("赠送");
                        list.Add("--笔数: " + Gfunc.PrintStrAlign(result.ToString(), 6, TextAlign.Left) + "--金额: " + num5.ToString(Gattr.PosSaleAmtPoint));
                    }
                    DataTable table3 = set.Tables["t_rm_jycash_info"];
                    decimal num8 = 0M;
                    if ((table3 != null) && (table3.Rows.Count > 0))
                    {
                        List<t_dict_payment_info> paymentInfo = base._dal.GetPaymentInfo(" pay_flag = '0' or pay_flag = '1' or pay_flag='6' ");
                        foreach (t_dict_payment_info _info in paymentInfo)
                        {
                            for (int i = 1; i <= 3; i++)
                            {
                                switch (i)
                                {
                                    case 1:
                                        str2 = "A";
                                        str3 = "销售";
                                        num = 1;
                                        break;

                                    case 2:
                                        str2 = "B";
                                        str3 = "退货";
                                        num = -1;
                                        break;

                                    case 3:
                                        str2 = "D";
                                        str3 = "找零";
                                        num = -1;
                                        break;
                                }
                                DataRow[] rowArray = table3.Select(string.Format(" pay_way='{0}' and sale_way='{1}' ", _info.pay_way, str2));
                                if ((rowArray != null) && (rowArray.Length > 0))
                                {
                                    int num10;
                                    if ((rowArray.Length > 0) && _info.pay_way.ToUpper().Equals("SAV"))
                                    {
                                        num10 = 0;
                                        while (num10 <= (rowArray.Length - 1))
                                        {
                                            result = Convert.ToInt32(rowArray[num10]["count"]);
                                            num5 = Convert.ToDecimal(rowArray[num10]["amount"]);
                                            if ((result > 0) || (num5 > 0M))
                                            {
                                                if (rowArray[num10]["memo"].ToString() == "1")
                                                {
                                                    list.Add(_info.pay_name + "-项目" + str3);
                                                }
                                                else
                                                {
                                                    list.Add(_info.pay_name + "-" + str3);
                                                }
                                                list.Add("--笔数: " + Gfunc.PrintStrAlign(result.ToString(), 6, TextAlign.Left) + "--金额: " + num5.ToString(Gattr.PosSaleAmtPoint));
                                                if (num == 1)
                                                {
                                                    num6 += num5 * _info.rate;
                                                    num3 += result;
                                                }
                                                else if (num == -1)
                                                {
                                                    num7 += num5 * _info.rate;
                                                    num4 += result;
                                                }
                                            }
                                            num10++;
                                        }
                                    }
                                    else
                                    {
                                        for (num10 = 0; num10 <= (rowArray.Length - 1); num10++)
                                        {
                                            result = Convert.ToInt32(rowArray[num10]["count"]);
                                            num5 = Convert.ToDecimal(rowArray[num10]["amount"]);
                                            if ((result > 0) || (num5 > 0M))
                                            {
                                                if ((rowArray[num10]["memo"].ToString() == "2") || _info.pay_way.ToUpper().Equals("ORD"))
                                                {
                                                    if (str2 == "B")
                                                    {
                                                        list.Add(string.Format("退订货订金(人民币)", new object[0]));
                                                    }
                                                    else
                                                    {
                                                        list.Add(_info.pay_name + "-储值卡充值");
                                                    }
                                                }
                                                else if (rowArray[num10]["memo"].ToString() == "3")
                                                {
                                                    list.Add(_info.pay_name + "-储值卡项目充值");
                                                }
                                                else
                                                {
                                                    list.Add(_info.pay_name + "-" + str3);
                                                }
                                                list.Add("--笔数: " + Gfunc.PrintStrAlign(result.ToString(), 6, TextAlign.Left) + "--金额: " + num5.ToString(Gattr.PosSaleAmtPoint));
                                                switch (num)
                                                {
                                                    case 1:
                                                        num6 += num5 * _info.rate;
                                                        num3 += result;
                                                        break;

                                                    case -1:
                                                        num7 += num5 * _info.rate;
                                                        num4 += result;
                                                        break;
                                                }
                                                if (_info.pay_way == "RMB")
                                                {
                                                    num8 += num5 * num;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if ((num3 > 0) || (num4 > 0))
                    {
                        list.Add("");
                        list.Add("收支合计  笔数: " + ((num3 + num4)).ToString() + " ");
                        if (num4 > 0)
                        {
                            list.Add(Gfunc.PrintStrAlign("--支出笔数: " + num4.ToString(), 0x10, TextAlign.Left) + "支出金额: " + num7.ToString(Gattr.PosSaleAmtPoint));
                        }
                        if (num3 > 0)
                        {
                            list.Add(Gfunc.PrintStrAlign("--收入笔数: " + num3.ToString(), 0x10, TextAlign.Left) + "收入金额: " + num6.ToString(Gattr.PosSaleAmtPoint));
                        }
                        list.Add("--合计金额: " + ((num6 - num7)).ToString(Gattr.PosSaleAmtPoint));
                        list.Add("");
                        list.Add("人民币现金额: " + num8.ToString(Gattr.PosSaleAmtPoint));
                        str = "";
                        list.Add(str.PadLeft((intPrintLen < 0) ? 0 : intPrintLen, '-'));
                    }
                }
                list.Add(Gfunc.PrintStrAlign("==完==", intPrintLen, TextAlign.Center));
                list3 = list;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return list3;
        }
        /// <summary>
        /// 商品对帐
        /// </summary>
        /// <param name="storeNo"></param>
        /// <param name="posId"></param>
        /// <param name="cashierId"></param>
        /// <param name="cashierName"></param>
        /// <param name="dtFrom"></param>
        /// <param name="dtTo"></param>
        /// <param name="intPrintLen"></param>
        /// <returns></returns>
        public List<string> ItemAccountStatement(string storeNo, string posId, string cashierId, string cashierName, DateTime dtFrom, DateTime dtTo, int intPrintLen)
        {
            int QtyLen = 6; //数量长度
            int PrcLen = 6; //单价长度
            int ItemLen = 15; //商品号
            int CntLen = 6;//

            List<string> list2;
            List<string> list = new List<string>();
            try
            {
                string str = "";
                DataTable table = new DataTable();
                table = base._dal.ItemAccountStatement(storeNo, cashierId, dtFrom, dtTo);
                if ((table == null) || (table.Rows.Count == 0))
                {
                    list.Add(str.PadLeft((intPrintLen < 0) ? 0 : intPrintLen, '-'));
                    list.Add(Gfunc.PrintStrAlign("商品对帐 ", intPrintLen, TextAlign.Center));
                    str = "";
                    list.Add(str.PadLeft((intPrintLen < 0) ? 0 : intPrintLen, '-'));
                    list.Add("收银员(" + cashierId + ")今天无收付款记录. ");
                    return list;
                }
                List<t_operator> cashierInfo = base._dal.GetOperatorInfo(cashierId, Gattr.BranchNo);
                list.Add("");
                list.Add(Gfunc.PrintStrAlign("   商品对帐单   ", intPrintLen, TextAlign.Center));
                list.Add(Gfunc.PrintStrAlign("================", intPrintLen, TextAlign.Center));
                str = "";
                list.Add(str.PadLeft((intPrintLen < 0) ? 0 : intPrintLen, '-'));
                list.Add("  机构: " + base._dal.GetBranchName(storeNo));
                //list.Add("  仓库: " + base._dal.GetBranchName(storeNo));
                list.Add("  收银机号: " + posId);
                list.Add("  收 银 员:[" + cashierId + "]" + cashierInfo[0].oper_name.Trim());
                list.Add("  对账日期: " + DateTime.Today.ToString("yyyy-MM-dd"));
                str = "";
                list.Add(str.PadLeft((intPrintLen < 0) ? 0 : intPrintLen, '-'));
                string item = (Gfunc.PrintStrAlign("品名", ItemLen, TextAlign.Left) + Gfunc.PrintStrAlign("售价", PrcLen, TextAlign.Right)) + Gfunc.PrintStrAlign("数量", QtyLen, TextAlign.Center) + Gfunc.PrintStrAlign("金额", CntLen, TextAlign.Right);
                list.Add(item);
                decimal num = 0M;
                decimal num2 = 0M;
                foreach (DataRow row in table.Rows)
                {
                    decimal num6;
                    string s = row["item_name"].ToString().Trim();
                    if (ItemLen < Encoding.Default.GetByteCount(s))
                    {
                        s = SIString.SubChar(s, ItemLen);
                    }
                    string str4 = Gfunc.PrintStrAlign(s, ItemLen, TextAlign.Left);
                    decimal num3 = SIString.TryDec(row["sale_price"]);
                    string str5 = Gfunc.PrintStrAlign(num3.ToString(Gattr.PosSaleAmtPoint), PrcLen, TextAlign.Right);
                    decimal num4 = SIString.TryDec(row["sale_qnty"]);
                    num += (row["sale_way"].ToString() == "B") ? (num4 * -1M) : num4;
                    string str6 = Gfunc.PrintStrAlign((row["sale_way"].ToString() == "B") ? (num6 = num4 * -1M).ToString("N0") : num4.ToString("N0"), QtyLen, TextAlign.Center);
                    decimal num5 = num3 * num4;
                    num2 += (row["sale_way"].ToString() == "B") ? (num5 * -1M) : num5;
                    list.Add(str4 + str5 + str6 + Gfunc.PrintStrAlign((row["sale_way"].ToString() == "B") ? (num6 = num5 * -1M).ToString(Gattr.PosSaleAmtPoint) : num5.ToString(Gattr.PosSaleAmtPoint), CntLen, TextAlign.Right));
                }
                list.Add(str.PadLeft((Gattr.PrtLen < 0) ? 0 : Gattr.PrtLen, '-'));
                string str8 = (Gfunc.PrintStrAlign("合计：", ItemLen, TextAlign.Left) + Gfunc.PrintStrAlign("", PrcLen, TextAlign.Right)) + Gfunc.PrintStrAlign(num.ToString("N0"), QtyLen, TextAlign.Center) + Gfunc.PrintStrAlign(num2.ToString(Gattr.PosSaleAmtPoint), CntLen, TextAlign.Right);
                list.Add(str8);
                list.Add(Gfunc.PrintStrAlign("==完==", Gattr.PrtLen, TextAlign.Center));
                list2 = list;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return list2;
        }
        /// <summary>
        /// 商品大类
        /// </summary>
        /// <param name="clsNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="isNextPages"></param>
        /// <returns></returns>
        public DataTable GetBigClsPage(string clsNo, int pageSize, bool isNextPages)
        {
            return base._dal.GetBigClsPage(clsNo, pageSize, isNextPages);
        }
        /// <summary>
        /// 商品小类
        /// </summary>
        /// <param name="clsNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="isNextPages"></param>
        /// <returns></returns>
        public DataTable GetClsPage(string clsBigNo, string clsNo, int pageSize, bool isNextPage)
        {
            return base._dal.GetClsPage(clsBigNo, clsNo, pageSize, isNextPage);
        }
        /// <summary>
        /// 商品列表分页
        /// </summary>
        /// <param name="clsNo"></param>
        /// <param name="itemNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="isNextPage"></param>
        /// <returns></returns>
        public DataTable GetItemPage(string clsNo, string itemNo, int pageSize, bool isNextPage)
        {
            return base._dal.GetItemPage(clsNo, itemNo, pageSize, isNextPage);
        }

        /// <summary>
        /// 获得商品信息
        /// </summary>
        /// <param name="itemNo"></param>
        /// <returns></returns>
        public t_cur_saleflow GetItemInfo(string itemNo)
        {
            return base._dal.GetItemInfo(itemNo);
        }
        /// <summary>
        /// 获得商品信息，封装t_item_info实体
        /// </summary>
        /// <param name="itemNo"></param>
        /// <returns></returns>
        public List<t_item_info> GetItemInfoNew(string itemNo)
        {
            return base._dal.GetItemInfoNew(itemNo);
        }
        /// <summary>
        /// 保存所有购买商品
        /// </summary>
        /// <param name="listSaleflow"></param>
        public void SaveTempSaleRow(List<t_cur_saleflow> listSaleflow)
        {
            base._dal.SaveTempSaleRow(listSaleflow);
        }
        /// <summary>
        /// 根据小类查询商品数量
        /// </summary>
        /// <param name="clsNo"></param>
        /// <param name="itemNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="isNextPage"></param>
        /// <returns></returns>
        public int GetClsItemCount(string clsNo)
        {
            return base._dal.GetClsItemCount(clsNo);
        }
        /// <summary>
        /// 获取商品价格
        /// </summary>
        /// <param name="itemNo"></param>
        /// <returns></returns>
        public decimal GetItemPrice(string itemNo)
        {
            return base._dal.GetItemPrice(itemNo);
        }
        /// <summary>
        /// 获取缺货商品
        /// </summary>
        /// <param name="itemNo"></param>
        /// <returns></returns>
        public decimal GetItemDm(string itemNo)
        {
            return base._dal.GetItemPrice(itemNo);
        }
        /// <summary>
        /// GetFuncKeys 快捷键
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public Dictionary<string, t_attr_function> GetFuncKeys(bool isAll)
        {
            return base._dal.GetFuncKeys(isAll);
        }
        /// <summary>
        /// 初始化销售商品数据
        /// </summary>
        /// <returns></returns>
        public List<t_cur_saleflow> GetTempSaleRow()
        {
            return base._dal.GetTempSaleRow();
        }
        /// <summary>
        /// 插入挂单数据
        /// </summary>
        /// <param name="listSaleFlow"></param>
        /// <param name="penOrd"></param>
        public void InsertPendingOrder(string flowNo, List<t_cur_saleflow> listSaleFlow, t_pending_order penOrd)
        {

            for (int i = 0; i < listSaleFlow.Count; i++)
            {
                listSaleFlow[i].flow_no = flowNo;
            }

            base._dal.InsertPendingOrder(listSaleFlow, penOrd);
        }
        /// <summary>
        /// 获取挂单主表数据
        /// </summary>
        /// <returns></returns>
        public List<t_pending_order> GetPenOrdRows()
        {
            return base._dal.GetPenOrdRows();
        }
        /// <summary>
        /// 获取挂单销售细项
        /// </summary>
        /// <param name="flowNo"></param>
        /// <returns></returns>
        public List<t_cur_saleflow> GetPenOrdSaleFlow(string flowNo)
        {
            return base._dal.GetPenOrdSaleFlow(flowNo);
        }


        public bool CheckPendingOrder(int flowNo)
        {
            string str = flowNo.ToString("0000");
            return (base._dal.CheckPendingOrder(str) >= 1);
        }
        /// <summary>
        /// 删除挂单数据
        /// </summary>
        /// <param name="flowNo"></param>
        /// <returns></returns>
        public bool DelPenOrd(string flowNo)
        {
            return (base._dal.DelPenOrd(flowNo) > 0);
        }
        /// <summary>
        /// 支付流水表
        /// </summary>
        /// <returns></returns>
        public List<t_cur_payflow> GetTempPayRow()
        {
            return base._dal.GetTempPayRow();
        }
        /// <summary>
        /// 挂起状态的支付流水表
        /// </summary>
        /// <returns></returns>
        public List<t_cur_payflow> GetPendingPayRow(string flowNo)
        {
            return base._dal.GetPendingPayRow(flowNo);
        }
        /// <summary>
        /// 保存支付流水
        /// </summary>
        /// <param name="listPayflow"></param>
        public void SaveTempPayRow(List<t_cur_payflow> listPayflow)
        {
            base._dal.SaveTempPayRow(listPayflow);
        }
        /// <summary>
        /// 保存挂单支付流水
        /// </summary>
        /// <param name="listPayflow"></param>
        public void SavePendingPayRow(List<t_cur_payflow> listPayflow, string FlowNo)
        {
            base._dal.SavePendingPayRow(listPayflow, FlowNo);
        }

        /// <summary>
        /// 删除挂单支付数据
        /// </summary>
        /// <param name="listPayflow"></param>
        public void DelPendingPayRow(string flowNo)
        {
            base._dal.DelPendingPayRow(flowNo);
        }
        /// <summary>
        /// 结算保存数据
        /// </summary>
        /// <param name="flowNo"></param>
        /// <param name="listSaleflow"></param>
        /// <param name="listPayflow"></param>
        public void SavePosBill(string flowNo, List<t_cur_saleflow> listSaleflow, List<t_cur_payflow> listPayflow)
        {
            DateTime operDataTime = this.GetOperDataTime();
            foreach (t_cur_saleflow _saleflow in listSaleflow)
            {
                _saleflow.flow_no = flowNo;
                _saleflow.sale_money = Math.Abs(_saleflow.sale_money);
                _saleflow.sale_qnty = Math.Abs(_saleflow.sale_qnty);
                _saleflow.discount_rate = ExtendUtility.Instance.ParseToDecimal((_saleflow.unit_price / _saleflow.sale_price).ToString(Gattr.PosSaleAmtPoint));
                _saleflow.oper_date = ExtendUtility.Instance.ParseToDateTime(operDataTime).ToString("s");
            }

            foreach (t_cur_payflow _payflow in listPayflow)
            {
                _payflow.flow_no = flowNo;
                _payflow.pay_amount = Math.Abs(_payflow.pay_amount);
                _payflow.sale_amount = Math.Abs(_payflow.sale_amount);
                _payflow.convert_amt = Math.Abs(_payflow.convert_amt);
                _payflow.oper_date = ExtendUtility.Instance.ParseToDateTime(operDataTime).ToString("s");
            }

            IDbTransaction objTrans = null;
            bool flag = base._dal.BeginSQLTrans(ref objTrans);
            try
            {
                base._dal.SavePosBill(listSaleflow, listPayflow, objTrans);
            }
            catch (SQLiteException exception)
            {
                if (flag)
                {
                    objTrans.Rollback();
                    throw exception;
                }
            }
            if (flag)
            {
                objTrans.Commit();
            }
        }
        /// <summary>
        /// 删除临时数据
        /// </summary>
        public void DeleteTempData()
        {
            base._dal.DeleteTempData();
        }
        /// <summary>
        /// 获取操作人信息
        /// </summary>
        /// <param name="operName"></param>
        /// <param name="operPwd"></param>
        /// <returns></returns>
        public t_operator GetOperatorInfo(string operName, string operPwd, string branckId)
        {
            return base._dal.GetOperatorInfo(operName, operPwd, branckId);
        }
        /// <summary>
        ///  更新登录时间
        /// </summary>
        /// <param name="operId"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int SetOperatorLoginTime(string operId, DateTime dt)
        {
            return (base._dal.SetOperLoginTime(operId, dt) > 0) ? 1 : 0;
        }
        /// <summary>
        /// 根据操作人ID和收银ID返回操作人信息
        /// </summary>
        /// <param name="operId"></param>
        /// <returns></returns>
        public List<t_operator> GetOperatorInfo(string operId, string branchId)
        {
            return base._dal.GetOperatorInfo(operId, branchId);
        }

        /// <summary>
        /// 根据WHERE条件查询付款信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<t_dict_payment_info> GetPaymentInfo(string where)
        {
            return base._dal.GetPaymentInfo(where);
        }
        /// <summary>
        /// 获取VIP信息
        /// </summary>
        /// <returns></returns>
        public List<t_vip_info> GetVipInfo(string cardNo, SearchType searchType)
        {
            return base._dal.GetVipInfo(cardNo, searchType);
        }
        /// <summary>
        ///  更新VIP消费信息
        /// </summary>
        /// <param name="vipInfo"></param>
        /// <param name="flowNo"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public bool InsertAccList(t_vip_info vipInfo, string flowNo)
        {
            return base._dal.InsertAccList(vipInfo, flowNo);
        }

        /// <summary>
        /// 获取退货原因信息
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public List<t_base_code> GetReasonInfos(string flag)
        {
            return base._dal.GetAllReasonInfo(flag);
        }
        /// <summary>
        /// 获取销售信息
        /// </summary>
        /// <param name="flowNo">流水单号</param>
        /// <returns>销售信息</returns>
        public List<t_cur_saleflow> GetSaleFlowInfoByFlowNo(string flowNo)
        {
            return base._dal.GetSaleFlowInfo(flowNo);
        }
        /// <summary>
        /// 获取退货信息
        /// </summary>
        /// <param name="flowNo">流水单号</param>
        /// <returns>销售信息</returns>
        public List<t_cur_saleflow> GetVoucherInfoByFlowNo(string flowNo)
        {
            return base._dal.GetVoucherInfo(flowNo);
        }
        /// <summary>
        /// 获取支付流水信息
        /// </summary>
        /// <param name="flowNo">流水单号</param>
        /// <returns>支付流水信息</returns>
        public List<t_cur_payflow> GetPayFlowInfoByFlowNo(string flowNo)
        {
            return base._dal.GetPayFlowInfo(flowNo);
        }
        /// <summary>
        /// 获取所有支付信息
        /// </summary>
        /// <param name="flowNo"></param>
        /// <returns></returns>
        public DataTable GetBranchInfo(string where)
        {
            return base._dal.GetBranchInfo(where);
        }
        /// <summary>
        /// 查询支付流水单号是否是退货单号
        /// </summary>
        /// <param name="flowNo"></param>
        /// <returns>大于0是退货凭证号否则是支付订单号</returns>
        public Int32 GetVoucherNoCount(String flowNo)
        {
            return base._dal.GetVoucherNoCount(flowNo);
        }
        #region 商品查询
        /// <summary>
        /// 获取最新的支付记录
        /// </summary>
        /// <param name="brancheNo"></param>
        /// <param name="posNo"></param>
        /// <returns></returns>
        public DataTable GetMaxPayFlowInfo(String brancheNo, String posNo, String flowno, String operate)
        {
            return base._dal.GetMaxPayFlowInfo(brancheNo, posNo, flowno, operate);
        }
        /// <summary>
        /// 根据订单流水号查找订单信息
        /// </summary>
        /// <param name="flowNo">订单流水号</param>
        /// <returns></returns>
        public DataTable GetSaleFlowInfos(String flowNo)
        {
            return base._dal.GetSaleFlowInfoByFlowNo(flowNo);
        }
        /// <summary>
        /// 获取挂账记录   未回款以及部分回款
        /// </summary>
        public DataTable GetPayFlowInfoDataTableByE()
        {
            return base._dal.GetPayFlowInfoDataTableByE();
        }
        /// <summary>
        /// 根据流水号查询支付流水信息
        /// </summary>
        /// <param name="flowNo"></param>
        /// <returns></returns>
        public List<t_cur_payflow> GetPayFlowInfoListByFlowNo(String flowNo)
        {
            return base._dal.GetPayFlowInfoListByFlowNo(flowNo);
        }
        /// <summary>
        /// 根据流水号返回支付流水数据集信息
        /// </summary>
        /// <param name="flowNo">流水表</param>
        /// <returns></returns>
        public DataTable GetPayFlowInfoDataTableByFlowNo(String flowNo)
        {
            return base._dal.GetPayFlowInfoDataTableByFlowNo(flowNo);
        }
        /// <summary>
        /// 根据支付流水的ID删除支付流水信息
        /// </summary>
        /// <returns>是否成功</returns>
        public bool DelPayFlowById(string id)
        {
            return base._dal.DelPayFlowById(id);
        }
        /// <summary>
        /// 更新挂账的回款状态  sale_way     E:未回款  EB：部分回款   EQ：全部回款
        /// </summary>
        /// <returns>是否更新成功</returns>
        public bool UpdatePayFlowStatus(t_pos_payflow _payFlow)
        {
            return base._dal.UpdatePayFlowStatus(_payFlow);
        }
        /// <summary>
        /// 根据支付流水的ID来更新支付流水
        /// </summary>
        /// <returns>是否更新成功</returns>
        public bool UpdatePayFlowByFid(t_pos_payflow _payFlow)
        {
            return base._dal.UpdatePayFlowByFid(_payFlow);
        }
        /// <summary>
        /// 根据流水号查询销售流水列表信息
        /// </summary>
        /// <param name="flowNo">流水号</param>
        /// <returns></returns>
        public List<t_cur_saleflow> GetSaleFlowInfoListByFlowNo(String flowNo)
        {
            return base._dal.GetSaleFlowInfoListByFlowNo(flowNo);
        }
        /// <summary>
        /// 根据流水号查询销售流水数据集信息
        /// </summary>
        /// <param name="flowNo">流水号</param>
        /// <returns></returns>
        public DataTable GetSaleFlowInfoDataTableByFlowNo(String flowNo)
        {
            return base._dal.GetSaleFlowInfoDataTableByFlowNo(flowNo);
        }
        #endregion

        #region 基础数据更新
        /// <summary>
        /// 获取增量更新标识
        /// </summary>
        public t_handle SelectHandleData()
        {
            return this._dal.SelectHandleDate();
        }
        /// <summary>
        /// 更新增量更新的标识
        /// </summary>
        /// <param name="t_h"></param>
        public bool UpdateHandleData(t_handle t_h)
        {
            return this._dal.UpdateHandleDate(t_h);
        }
        /// <summary>
        /// 将商品数据更新到数据库
        /// </summary>
        /// <param name="table">商品数据</param>
        /// <returns></returns>
        public bool InsertItemData(DataTable table,decimal del,ref decimal rid)
        {
            return this._dal.InsertItemData(table,del,ref rid);
        }
        /// <summary>
        /// 将条形码数据更新到数据库
        /// </summary>
        /// <param name="table">条形码数据</param>
        /// <returns></returns>
        public bool InsertBarcodeData(DataTable table, decimal del, ref decimal rid)
        {
            return this._dal.InsertBarCodeData(table,del,ref rid);
        }
        /// <summary>
        /// 将商品分类数据更新到数据库
        /// </summary>
        /// <param name="table">条形码数据</param>
        /// <returns></returns>
        public bool InsertItemClsData(DataTable table, decimal del, ref decimal rid)
        {
            return this._dal.InsertClsData(table,del,ref rid);
        }
        /// <summary>
        /// 将收银员数据更新到数据库
        /// </summary>
        /// <param name="table">收银员数据</param>
        /// <returns></returns>
        public bool InsertCashierData(DataTable table, decimal del, ref decimal rid)
        {
            return this._dal.InsertCashierData(table,del,ref rid);
        }
        /// <summary>
        /// 基本信息类型表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool InsertBaseTypeData(DataTable table,decimal del,ref decimal rid)
        {
            return this._dal.InsertBaseTypeData(table,del,ref rid);
        }
        /// <summary>
        /// 基本信息类型表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool InsertBaseData(DataTable table,decimal del,ref decimal rid)
        {
            return this._dal.InsertBaseData(table,del,ref rid);
        }
        /// <summary>
        /// 支付方式
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool InsertPaymentInfo(DataTable table)
        {
            return this._dal.InsertPaymentData(table);
        }
        /// <summary>
        /// 快捷键
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool InsertFunction(DataTable table)
        {
            return this._dal.InsertFunction(table);
        }
        /// <summary>
        /// 促销规则信息
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool InsertPlanRule(DataTable table)
        {
            return this._dal.InsertPlanRule(table);
        }
        /// <summary>
        /// 促销信息
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool InsertPlanMaster(DataTable table,decimal del, ref decimal rid)
        {
            return this._dal.InsertPlanMaster(table,del,ref rid);
        }

        /// <summary>
        /// 促销详细信息
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool InsertPlanDetail(DataTable table, decimal del, ref decimal rid)
        {
            return this._dal.InsertPlanDetail(table,del,ref rid);
        }
        /// <summary>
        /// 插入POS机设置 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool InsertPosSysSet(List<t_sys_pos_set> sets)
        {
            return this._dal.InsertPosSysSet(sets);
        }
        /// <summary>
        /// 插入门店信息
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool InsertBrancheList(DataTable table)
        {
            return this._dal.InsertBranchList(table);
        }
        /// <summary>
        /// 获取流水号
        /// </summary>
        /// <param name="flowNo"></param>
        /// <returns></returns>
        public int GetIntFlow(string flowNo)
        {
            return this._dal.GetIntFlow(flowNo);
        }
        /// <summary>
        /// 插入非交易记录
        /// </summary>
        /// <param name="_payflow"></param>
        /// <returns></returns>
        public bool InNonTrading(t_cur_payflow _payflow)
        {
            return this._dal.InNonTrading(_payflow);
        }
        /// <summary>
        /// 更新门店的商品价格
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool UpdateItemPrice(DataTable table,ref decimal rid)
        {
            return _dal.UpdateItemPrice(table,ref rid);
        }
        /// <summary>
        /// 更新门店的商品库存
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool UpdateItemStock(DataTable table,ref decimal rid)
        {
            return _dal.UpdateItemStock(table,ref rid);
        }
        #endregion
        /// <summary>
        /// 挂单流水号
        /// </summary>
        /// <returns></returns>
        public int GetMaxPendingOrderFlowNo()
        {
            return Gfunc.TypeToInt(base._dal.GetMaxPendingOrderFlow(), 0);
        }
        /// <summary>
        /// 最后对账时间
        /// </summary>
        /// <returns></returns>
        public DateTime? GetLastAccountTime()
        {
            return base._dal.GetLastAccountTime();
        }
        /// <summary>
        /// 对帐总额
        /// </summary>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public decimal GetSumAccountAmt(DateTime dtStart, DateTime dtEnd)
        {
            return base._dal.GetSumAccountAmt(dtStart, dtEnd);
        }
        /// <summary>
        /// 获得查询时间
        /// </summary>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="hasData"></param>
        public void GetQueryPeriod(out DateTime dtStart, out DateTime dtEnd, out bool hasData)
        {
            DateTime? lastAccountTime = this.GetLastAccountTime();
            base._dal.GetQueryPeriod(lastAccountTime, out dtStart, out dtEnd, out hasData);
        }
        /// <summary>
        /// 插入对帐信息
        /// </summary>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="sumAmt"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool InsertAccountRecord(DateTime dtStart, DateTime dtEnd, decimal sumAmt,decimal sumMoney, out string errMsg)
        {
            return base._dal.InsertAccountRecord(dtStart, dtEnd, sumAmt,sumMoney,out errMsg);
        }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetOperDataTime()
        {
            DateTime now = DateTime.Now;
            return now;
        }



        #region POS机设置
        /// <summary>
        /// 获取POS基本设置
        /// </summary>
        /// <returns></returns>
        public List<t_sys_pos_set> GetPosSets()
        {
            return _dal.GetPosSets();
        }
        #endregion

        #region 促销模块
        /// <summary>
        /// 获取当前可以使用的促销信息:时间规则校验
        /// </summary>
        /// <param name="plu_flag">PLU标志</param>
        /// <param name="rule_no">规则编号</param>
        /// <param name="plan_no">促销方案编码</param>
        /// <param name="rrule_no">需要排除的规则编号</param>
        /// <returns>促销信息</returns>
        public DataTable GetPlanMaster(string plu_flag, string rule_no, string plan_no, string rrule_no, string vip_type)
        {
            return this._dal.GetPlanMaster(plu_flag, rule_no, plan_no, rrule_no, vip_type);
        }
        /// <summary>
        /// 获取可用的促销详细方案信息
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public DataTable GetPlanDetail(DataTable table)
        {
            return this._dal.GetPlanDetail(table);
        }
        /// <summary>
        ///根据促销编号获取促销相信信息
        /// </summary>
        /// <param name="plan_no">促销编号</param>
        /// <returns>促销详细信息</returns>
        public DataTable GetPlanDetailByPlanNo(string plan_no)
        {
            return this._dal.GetPlanDetailByPlanNo(plan_no);
        }
        /// <summary>
        /// 根据促销规则编号和促销范围查询促销规则具体信息
        /// </summary>
        /// <param name="rule_no">促销编号</param>
        /// <param name="range_flag">促销范围</param>
        /// <returns></returns>
        public PlanRule GetPlanRuleInfo(string rule_no, string range_flag)
        {
            return this._dal.GetPlanRuleInfo(rule_no, range_flag);
        }
        /// <summary>
        /// 根据分类编号获取分类名称
        /// </summary>
        /// <param name="cls_no">分类名称</param>
        /// <returns>分类名称</returns>
        public string GetClsNameByClsNo(string cls_no)
        {
            return this._dal.GetClsNameByClsNo(cls_no);
        }
        /// <summary>
        /// 根据品牌编号获取品牌名称
        /// </summary>
        /// <param name="brand">品牌编号</param>
        /// <returns>品牌名称</returns>
        public string GetBrandName(string brand)
        {
            return this._dal.GetBrandName(brand);
        }
        /// <summary>
        /// 根据商品编号获取商品名称
        /// </summary>
        /// <param name="item_no">商品编号</param>
        /// <returns>商品名称</returns>
        public string GetItemName(string item_no)
        {
            return this._dal.GetItemName(item_no);
        }
        #endregion

        /// <summary>
        /// 保存非交易支付流水
        /// </summary>
        /// <param name="_pay">支付流水</param>
        /// <returns>成功TRUE失败false</returns>
        public bool SaveNonTrading(t_pos_payflow _pay)
        {
            return this._dal.SaveNonTrading(_pay);
        }
        /// <summary>
        /// 获取门店名称
        /// </summary>
        /// <param name="branch_no">门店编号</param>
        /// <returns></returns>
        public string GetBranchNameByNo(string branch_no)
        {
            return this._dal.GetBranchNameByNo(branch_no);
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool InsertItemComb(DataTable table,decimal del,ref decimal rid)
        {
            return this._dal.InsertItemComb(table,del,ref rid);
        }
        /// <summary>
        /// 查询商品信息
        /// </summary>
        /// <param name="item_no"></param>
        /// <returns></returns>
        public List<t_cur_saleflow> GetItemsForPos(string item_no)
        {
            return this._dal.GetItemsForPos(item_no);
        }
        /// <summary>
        /// 操作人密码
        /// </summary>
        /// <param name="operId">操作人ID</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public int UpdateOperPwd(string operId, string pwd)
        {
            return this._dal.UpdateOperPwd(operId, pwd);
        }
        /// <summary>
        /// 获取为上传数据的条数
        /// </summary>
        /// <returns></returns>
        public int GetNonUpdateFlow()
        {
            return this._dal.GetNonUpdateFlow();
        }
        /// <summary>
        /// 获取会员卡信息
        /// </summary>
        /// <param name="memo"></param>
        /// <returns></returns>
        public string GetVipCardByNo(string memo)
        {
            return this._dal.GetVipCardByNo(memo);
        }
        /// <summary>
        /// 新增会员本地积分记录
        /// </summary>
        /// <param name="vipinfo">会员支付记录</param>
        /// <returns></returns>
        public bool AddVipflow(t_app_viplist vipinfo)
        {
            return this._dal.AddVipflow(vipinfo);
        }
        /// <summary>
        /// 删除临时销售数据:退货商品
        /// </summary>
        /// <returns></returns>
        public bool DeleteTempNonSaleData()
        {
            return this._dal.DeleteTempNonSaleData();
        }
        #region 基础信息添加供应商
        /// <summary>
        /// 插入供应商信息
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool InsertSupinfo(DataTable table)
        {
            return this._dal.InsertSupinfo(table);
        }
        #endregion
        #region 公共查询控件查询
        /// <summary>
        /// 公共查询控件
        /// </summary>
        /// <param name="type">查询类别</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>二维数据集</returns>
        public DataTable GetCommonSelData(string type, string keyword)
        {
            return this._dal.GetCommonSelData(type, keyword);
        }
        #endregion


        /// <summary>
        /// 获取当前门店商品库存
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public DataTable GetItemStock()
        {
            return this._dal.GetItemStock();
        }
        /// <summary>
        /// 新增会员本地积分记录
        /// </summary>
        /// <param name="vipinfo">会员支付记录</param>
        /// <returns></returns>
        public t_app_viplist GetVipflow(string flow_no)
        {
            return this._dal.GetVipflow(flow_no);
        }
    }
}

