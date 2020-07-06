using System.ComponentModel;

namespace Org.VeChain.Thor.Devkit.Abi
{
    public enum AbiStateMutability
    {
        [Description("pure")]
        Pure,
        [Description("view")]
        View,
        [Description("constant")]
        Constant,
        [Description("payable")]
        Payable,
        [Description("nonpayable")]
        Nonpayable
    }
}