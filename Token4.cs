using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using Neo.SmartContract.Framework.Services.System;
using System;
using System.Numerics;

namespace RKToken
{
    public class Contract1 : SmartContract
    {
        public static string Name() => "SBTToken";
        public static string Symbol() => "SBT";
        public static readonly byte[] Owner = "APM9y5gA3dquRMUDN5Q2drJfjWXvpMYZP9".ToScriptHash();
        public static byte Decimals() => 0;

        private const ulong total_amount = 100000000;

        public static event Action<byte[], byte[], BigInteger> Transferred;

        public static Object Main(string operation, object[] args)
        {
            if (operation == "deploy")
            {
                Runtime.Notify(args[0], (byte[])args[0]);
                return Deploy((byte[])args[0]);
            }
            if (operation == "totalSupply")
            {
                Runtime.Notify("calling totalSupply function");
                return TotalSupply();
            }
            if (operation == "name")
            {
                Runtime.Notify(Name());
                return Name();
            }
            if (operation == "symbol")
            {
                Runtime.Notify(Symbol());
                return Symbol();
            }
            if (operation == "transfer")
            {
                if (args.Length != 3)
                {
                    Runtime.Notify("invalid arguments...");
                    return false;
                }
                byte[] from = (byte[])args[0];
                byte[] to = (byte[])args[1];
                BigInteger value = (BigInteger)args[2];
                Runtime.Notify(from, to, value);
                return Transfer(from, to, value);
            }
            if (operation == "balanceOf")
            {
                if (args.Length != 1) return 0;
                byte[] account = (byte[])args[0];
                Runtime.Notify(account);
                return BalanceOf(account);
            }
            if (operation == "decimals")
            {
                Runtime.Notify(Decimals());
                return Decimals();
            }
            if (operation == "getReceiver")
            {
                Runtime.Notify("calling receiver", GetReceiver());
                return GetReceiver();
            }
            Runtime.Notify("not matching");
            return false;
        }
        public static bool Deploy(byte[] originator)
        {
            Storage.Put(Storage.CurrentContext, originator, total_amount);
            Storage.Put(Storage.CurrentContext, "totalSupply", total_amount);
            Runtime.Notify("deployed successfully", total_amount);
            Transferred(null, originator, total_amount);
            return true;
        }
        public static BigInteger TotalSupply()
        {
            byte[] result = Storage.Get(Storage.CurrentContext, "totalSupply");
            Runtime.Notify("total supply", result);
            Runtime.Notify(new BigInteger(result));
            return new BigInteger(result);
        }
        public static bool Transfer(byte[] from, byte[] to, BigInteger value)
        {
            Runtime.Notify(from, to, value);
            Runtime.Notify(from);
            Runtime.Notify(to);
            Runtime.Notify(value);
            if (value <= 0)
            {
                Runtime.Notify(value);
                Runtime.Notify("value is less then or equal to zero...");
                return false;
            }
            if (from == to)
            {
                Runtime.Notify("from==to");
                return true;
            }
            Runtime.Notify(value);
            byte[] from_value = Storage.Get(Storage.CurrentContext, from);
            Runtime.Notify(from_value);
            Runtime.Notify(new BigInteger(from_value));
            Runtime.Notify(value);
            if (new BigInteger(from_value) < value) return false;
            if (new BigInteger(from_value) == value)
                Storage.Delete(Storage.CurrentContext, from);
            else
            {
                Runtime.Notify("in else");
                Runtime.Notify("from value", from_value);
                BigInteger newFromValue = new BigInteger(from_value) - value;
                Runtime.Notify("fv af inc", newFromValue);
                byte[] to_value = Storage.Get(Storage.CurrentContext, to);
                Runtime.Notify("to value", to_value);
                BigInteger newToValue = new BigInteger(to_value) + value;
                Runtime.Notify("tv af inc", newToValue);
                Storage.Put(Storage.CurrentContext, from, newFromValue);
                Storage.Put(Storage.CurrentContext, to, newToValue);
                Runtime.Notify("transferred successfully");
                Transferred(from, to, value);
                return true;
            }
            Runtime.Notify("not transferred...");
            return false;
        }
        public static BigInteger BalanceOf(byte[] _account)
        {
            Runtime.Notify("dsdf");
            byte[] result = Storage.Get(Storage.CurrentContext, _account);
            Runtime.Notify("DF");
            BigInteger amount = new BigInteger(result);
            Runtime.Notify(result, amount, result.AsBigInteger(), amount + 1);
            return amount;
        }
        private static byte[] GetReceiver()
        {
            Runtime.Notify(ExecutionEngine.ExecutingScriptHash);
            return ExecutionEngine.ExecutingScriptHash;
        }
    }
}
