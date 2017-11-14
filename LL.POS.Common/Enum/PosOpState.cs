
namespace LL.POS.Common
{
    public enum PosOpState
    {
        /// <summary>
        /// 抹零钱
        /// </summary>
        CHG = 2,
        CMD = 99,
        /// <summary>
        /// 查看交易
        /// </summary>
        LOOK = 3,
        /// <summary>
        /// 结算状态
        /// </summary>
        PAY = 1,
        /// <summary>
        /// 销售状态
        /// </summary>
        PLU = 0
    }
}
