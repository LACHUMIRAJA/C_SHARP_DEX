using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using Neo.SmartContract.Framework.Services.System;
using System;
using System.Numerics;


namespace Dex1WayConverted
{
    public class Contract1 : SmartContract
    {
        public static readonly byte[] Owner = "ANiSxR6mgYZxhTXpJqzjcnyFCQ16HVaC4W".ToScriptHash();
        public static readonly byte[] address0 = Neo.SmartContract.Framework.Helper.AsByteArray("AMiSxR6mgYZxhTXpJqzjcnyFBQF6HVaC4W");

        public static Object Main(string oper, params Object[] args)
        {
            if (oper == "Deploy")
            {

                byte[] approver = (byte[])args[0];
                byte[] _baseToken = (byte[])args[1];
                byte[] _etherToken = (byte[])args[2];

                return deploy(approver, _baseToken, _etherToken);
            }


            if (Runtime.CheckWitness(Owner))
            {


                if (oper == "updateFeeSchedule")
                {
                    uint baseTokenFee = (uint)args[0];
                    uint etherTokenFee = (uint)args[1];
                    uint normalTokenFee = (uint)args[2];

                    return updateFeeSchedule(baseTokenFee, etherTokenFee, normalTokenFee);
                }


                if (oper == "changeApprover")
                {


                    byte[] newApprover = (byte[])args[0];

                    return changeApprover(newApprover);

                }


                if (oper == "addAutherizedAddress")
                {


                    byte[] appIntegrator = (byte[])args[0];


                    return addAuthorizedAddress(appIntegrator);
                }


                if (oper == "removeAutherizedAddress")
                {

                    byte[] appIntegrator = (byte[])args[0];

                    return removeAuthorizedAddress(appIntegrator);
                }


                if (oper == "addOwner")
                {

                    byte[] newOwner = (byte[])args[0];

                    return addOwner(newOwner);

                }

                if (oper == "removeOwner")

                {

                    byte[] reowner = (byte[])args[0];

                    return removeOwner(reowner);
                }


                if (oper == "openVault")
                {
                    BigInteger startTime = (BigInteger)args[0];
                    BigInteger closureTime = (BigInteger)args[1];

                    return openVault(startTime, closureTime);


                }


                /*if (oper == "orderVault")
                {
                    byte[] check = Storage.Get(Storage.CurrentContext, "Vault is Open");
                    byte[] check1 = Storage.Get(Storage.CurrentContext, "Vault is Seal");

                    if (check1.Equals(0) && (check.Equals(0)))
                    {

                        approver = (byte[])args[0];

                        Storage.Put(Storage.CurrentContext, "Approver", approver);
                        Runtime.Log("Approver Setted Successfully");
                        return true;
                    }
                    return false;
                }*/


                if (oper == "sealVault")
                {
                    BigInteger now = Blockchain.GetHeader(Blockchain.GetHeight()).Timestamp;

                    return sealVault(now);


                }

                //----
                /*case "getnumoforders":
                    {
                        BigInteger nooh = Neo.SmartContract.Framework.Helper.AsBigInteger(Storage.Get(Storage.CurrentContext, "OrderHash"));
                        Runtime.Notify("No. Of OrderHashes " +nooh);
                        return true;

                    }*/

                //---


                if (oper == "extendVault")
                {
                    BigInteger closureTime = (BigInteger)args[0];
                    BigInteger now = Blockchain.GetHeader(Blockchain.GetHeight()).Timestamp;

                    return extendVault(closureTime, now);



                }


                if (oper == "storeVault")
                {


                    byte[] oHash = (byte[])args[0];
                    byte[] orderId = ((byte[])args[1]);
                    BigInteger now = Blockchain.GetHeader(Blockchain.GetHeight()).Timestamp;


                    return storeVault(oHash, orderId, now);


                }



                if (oper == "closeVault")
                {


                    BigInteger now = Blockchain.GetHeader(Blockchain.GetHeight()).Timestamp;

                    return closeVault(now);
                }

            }




            if (oper == "getFeeIndex")
            {
                byte[] _token = (byte[])args[0];

                return getFeeIndex(_token);
            }



            if (oper == "getSellerHash")//
            {
                byte[][] _sellerTokens = (byte[][])args[0];
                BigInteger[] _sellerValues = (BigInteger[])args[1];

                byte[][] _orderAddresses = new byte[5][];
                _orderAddresses = (byte[][])args[2];

                BigInteger[] _orderValues = new BigInteger[5];
                _orderValues = (BigInteger[])args[3];

                byte[] _orderID = (byte[])args[4];

                return getSellerHash(_sellerTokens, _sellerValues, _orderAddresses, _orderValues, _orderID);
            }



            if (oper == "getBuyerHash")
            {

                byte[][] _buyerTokens = (byte[][])args[0];

                BigInteger[] _buyerValues = (BigInteger[])args[1];
                byte[][] _orderAddresses = new byte[5][];
                _orderAddresses = (byte[][])args[2];

                BigInteger[] _orderValues = new BigInteger[5];
                _orderValues = (BigInteger[])args[3];

                byte[] _orderID = (byte[])args[4];
                return getBuyerHash(_buyerTokens, _buyerValues, _orderAddresses, _orderValues, _orderID);


            }




            if (oper == "getTwoWayOrderHash") //[["sellerT","sellerT1","SellerT2"],["buyerT","buyerT1","buyerT2"],[500,450,400],[458,444,380],["AA1","BB1","CC1","DD1","EE1"],[540,450,580,550,650],["OI01"]]
            {

                //

                byte[][] _sellerTokens = (byte[][])args[0];
                byte[][] _buyerTokens = (byte[][])args[1];
                BigInteger[] _sellerValues = (BigInteger[])args[2];
                BigInteger[] _buyerValues = (BigInteger[])args[3];
                byte[][] _orderAddresses = new byte[5][];
                _orderAddresses = (byte[][])args[4];



                BigInteger[] _orderValues = new BigInteger[5];
                _orderValues = (BigInteger[])args[5];

                byte[] _orderID = (byte[])args[6];

                return getTwoWayOrderHash(_sellerTokens, _buyerTokens, _sellerValues, _buyerValues, _orderAddresses, _orderValues, _orderID);
            }




            if (oper == "basicSigValidations") //
            {
                byte[][] _orderAddresses = new byte[5][];
                _orderAddresses = (byte[][])args[0];


                uint[] _v = new uint[2];
                _v = (uint[])args[1];

                byte[] _sr = (byte[])args[2];
                byte[] _ss = (byte[])args[3];
                byte[] _br = (byte[])args[4];
                byte[] _bs = (byte[])args[5];
                byte[] _sellerHash = (byte[])args[6];
                byte[] _buyerHash = (byte[])args[7];


                return basicSigValidations(_orderAddresses, _v, _sr, _ss, _br, _bs, _sellerHash, _buyerHash);


            }




            //One_Way_Full_Fil_Function_Pending


            if (oper == "transferForTokens") //[["sellerT","sellerT1"],["buyerT","buyerT1"],[500,450],[458,444],["AA1","BB1","CC1"],[540,450,580]]
            {
                byte[][] _sellerTokens = (byte[][])args[0];
                byte[][] _buyerTokens = (byte[][])args[1];
                BigInteger[] _sellerValues = (BigInteger[])args[2];
                BigInteger[] _buyerValues = (BigInteger[])args[3];

                byte[][] _orderAddresses = new byte[5][];
                _orderAddresses = (byte[][])args[4];


                BigInteger[] _orderValues = new BigInteger[5];
                _orderValues = (BigInteger[])args[5];

                return transferforTokens(_sellerTokens, _buyerTokens, _sellerValues, _buyerValues, _orderAddresses, _orderValues);

            }



            if (oper == "validateAuthorization")
            {
                byte[][] _sellerTokens = (byte[][])args[0];
                byte[][] _buyerTokens = (byte[][])args[1];
                BigInteger[] _sellerValues = (BigInteger[])args[2];
                BigInteger[] _buyerValues = (BigInteger[])args[3];

                byte[][] _orderAddresses = new byte[5][];
                _orderAddresses = (byte[][])args[4];



                BigInteger[] _orderValues = new BigInteger[5];
                _orderValues = (BigInteger[])args[5];

                return validateAuthorization(_sellerTokens, _buyerTokens, _sellerValues, _buyerValues, _orderAddresses, _orderValues);
            }




            if (oper == "calcTradeFee")
            {
                BigInteger values = (BigInteger)args[0];
                uint feeIndex = (uint)args[1];

                return calcTradeFee(values, feeIndex);

            }



            if (oper == "calcTradeFeeMulti")
            {
                uint[] values = (uint[])args[0];
                uint[] feeIndexes = (uint[])args[1];

                return calcTradeFeeMulti(values, feeIndexes);
            }




            if (oper == "ecverify")  //parameters :Address, msghash, v,r,s, signer
            {
                Runtime.Notify("Signature Started Verification");
                byte[] address001 = (byte[])args[0];
                byte _msgHash = (byte)args[1];
                uint v = (uint)args[2];
                byte r = (byte)args[3];
                byte s = (byte)args[4];
                byte[] _signer = (byte[])args[5];

                if (_signer == address001)
                {
                    return false;
                }


                if (v < 27)
                {
                    v += 27;
                }

                if (v != 27 && v != 28)
                {
                    return address001 == _signer;
                }

                if (v == 27)
                {
                    Runtime.Log("It Verified");
                    return Main("ecrecover", _msgHash, v, r, s) == _signer;
                }
                else if (v == 28)
                {
                    Runtime.Log("It Verified");
                    return Main("ecrecover", _msgHash, v, r, s) == _signer;
                }

                return address001 == _signer;
            }


            if (oper == "ecrecover")
            {
                byte _msgHash = (byte)args[0];
                uint v = (uint)args[1];
                byte r = (byte)args[2];
                byte s = (byte)args[3];
                String address1 = "ANiSxR6mgYZxhTXpJqzjcnyFCQ16HVaC4W";
                return address1;
            }





            if (oper == "orderLocated")
            {
                byte[] hash = (byte[])args[0];
                byte[] orderId = (byte[])args[1];

                Runtime.Log("Order  Location Checked");
                // Storage.Get(Storage.CurrentContext, orderHashes);
                return (Storage.Get(Storage.CurrentContext, hash.Concat(Neo.SmartContract.Framework.Helper.AsByteArray(" "))).Equals("orderhash"));



            }




            if (oper == "killExchange")
            {
                if (Runtime.CheckWitness(Storage.Get(Storage.CurrentContext, "approver")))
                {
                    Storage.Put(Storage.CurrentContext, "Active", 0);
                    if (Storage.Get(Storage.CurrentContext, "Active")[0] == 0)
                    {
                        Storage.Delete(Storage.CurrentContext, "Active");
                    }
                    return true;
                }
            }



            if (oper == "isOrderSigned")  //parameters :Address, msghash, v,r,s, signer
            {
                Runtime.Notify("Signature Verified");


                byte[] address = (byte[])args[0];
                byte _msgHash = (byte)args[1];
                uint v = (uint)args[2];
                byte r = (byte)args[3];
                byte s = (byte)args[4];
                byte[] _signer = (byte[])args[5];

                return Main("ecverify", address, _msgHash, v, r, s, _signer);

            }

            if (oper == "validExchangeFee")
            {
                byte[] _sellerFeeToken = (byte[])args[0];
                byte[] _buyerFeeToken = (byte[])args[1];
                BigInteger _sellerFeeValue = (BigInteger)args[2];
                BigInteger _buyrFeeValue = (BigInteger)args[3];

                if (_sellerFeeToken != null && _buyerFeeToken != null && _sellerFeeValue > 0 && _buyrFeeValue > 0)
                {
                    return true;
                }
                return false;
            }



            if (oper == "orderExists")
            {
                byte[] _hash = (byte[])args[0];
                byte[] _orderId = (byte[])args[1];

                if (Main("orderLocated", _hash, _orderId) != null)
                {
                    return true;
                }

                return false;

            }





            if (oper == "deposit")
            {
                byte[] acc = (byte[])args[0];
                BigInteger val = (BigInteger)args[1];

                return deposit(acc, val);
            }

            if (oper == "balanceOf")
            {
                byte[] acc = (byte[])args[0];

                return balanceOf(acc);
            }


            if (oper == "transferFrom")
            {
                byte[] from = (byte[])args[0];
                byte[] to = (byte[])args[1];
                BigInteger value = (BigInteger)args[2];

                return transferFrom(from, to, value);

            }


            if (oper == "approve")
            {
                byte[] add1 = (byte[])args[0];
                byte[] add2 = (byte[])args[1];
                BigInteger value = (BigInteger)args[2];

                return approval(add1, add2, value);
            }

            if (oper == "allowance")
            {
                byte[] add1 = (byte[])args[0];
                byte[] add2 = (byte[])args[1];

                return allowance(add1, add2);

            }




            /*if (oper == "oneWayFulfillPO")
             {
                  byte[] _sellerTokens = (byte[])args[0];
                  byte[] _buyerTokens = (byte[])args[1];
                  BigInteger[] _sellerValues = (BigInteger[])args[2];
                  BigInteger[] _buyerValues = (BigInteger[])args[3];
                  byte[][] _orderAddresses = new byte[5][];
                  _orderAddresses = (byte[][])args[4];
                  BigInteger[] _orderValues = new BigInteger[5];
                  _orderValues = (BigInteger[])args[5];
                  BigInteger[] _v = new BigInteger[2];
                  _v = (BigInteger[])args[6];
                  byte[] _br = (byte[])args[7];
                  byte[] _bs = (byte[])args[8];
                  byte[] _sr = (byte[])args[9];
                  byte[] _ss = (byte[])args[10];
                  byte[] _orderID = (byte[])args[11];
                  BigInteger now = Blockchain.GetHeader(Blockchain.GetHeight()).Timestamp;

                  byte[] check = Storage.Get(Storage.CurrentContext, "Active");
                  byte[] check1 = Storage.Get(Storage.CurrentContext, address0);

                  if (check.Equals(1) && _orderValues[2].CompareTo(now) == 1 && _orderValues[2].CompareTo(now) == 0)
                  {

                      if (!address0.Equals(_orderAddresses[1]) && (!address0.Equals(_orderAddresses[2])) && check1.Equals(0))
                      {
                          //event
                          return false;
                      }
                  }


                  byte[] SellerHash = (byte[]) Main("getSellerHash", _sellerTokens, _sellerValues, _orderAddresses, _orderValues, _orderID);

                  byte[] BuyerHash = (byte[]) Main("getBuyerHash", _buyerTokens, _buyerValues, _orderAddresses, _orderValues, _orderID);


                  if (Main("basicSigValidations", _orderAddresses, _v, _sr, _ss, _br, _bs, SellerHash, BuyerHash) != null)
                  {
                      //event
                      return false;
                  }

                  //event


                  byte[] twc = (ExecutionEngine.ExecutingScriptHash).Concat(SellerHash);
                  byte[] twc1 = BuyerHash.Concat(twc).Concat(_orderID);

                  byte[] TWH = Hash256(twc.Concat(twc1));

                  if ((Main("orderExists",  TWH , _orderID) != null))
                  {

                      if (Main("validExchangeFee", _orderAddresses[3], _orderAddresses[4], _orderAddresses[0], _orderAddresses[1]) != null)

                      {
                          byte[] fi = (byte[]) Main("getFeeIndex", _orderAddresses[3]);
                          byte[] fi1 = (byte[])Main("getFeeIndex", _orderAddresses[4]);
                          BigInteger FeeIndex = Neo.SmartContract.Framework.Helper.AsBigInteger(fi);
                          BigInteger FeeIndex1 = Neo.SmartContract.Framework.Helper.AsBigInteger(fi1);

                          byte[] ov = (byte[])(Main("calcTradeFee", _orderValues[0], FeeIndex));
                          byte[] ov1 = (byte[])(Main("calcTradeFee", _orderValues[1], FeeIndex1));

                          _orderValues[0] = Neo.SmartContract.Framework.Helper.AsBigInteger(ov) ;
                          _orderValues[1] = Neo.SmartContract.Framework.Helper.AsBigInteger(ov1);

                          if (_orderValues[0] > 0 && _orderValues[1] > 0 && _orderAddresses[0] != 0 && _orderAddresses[1] != 0 && _orderAddresses[2] != 0 && _orderAddresses[1] != _orderAddresses[2] && _sellerTokens.Length > 0 && _sellerValues.Length > 0 && _sellerTokens.Length == _sellerValues.Length && _buyerTokens.Length > 0 && _buyerValues.Length > 0 && _buyerTokens.Length == _buyerValues.Length)
                          {
                              if (Main("validAuthorization", _sellerTokens, _buyerTokens, _buyerValues, _orderAddresses, _orderValues) != null)
                              {
                                  return false;
                              }
                              //event

                              Main("transferforTokens", _sellerTokens, _buyerTokens, _sellerValues, _buyerValues, _orderAddresses, _orderValues);
                              Main("storeInVault",TWH, _orderID);
                              //event

                          }
                      }
                  }
                  ///------

             }*/


             return false;
        }






        public static bool deploy(byte[] approver, byte[] _baseToken, byte[] _etherToken)
        {
            Storage.Put(Storage.CurrentContext, "Approver", approver);
            Storage.Put(Storage.CurrentContext, Owner.Concat(Neo.SmartContract.Framework.Helper.AsByteArray("owner")), "owner");

            if (_baseToken != null && _etherToken != null)
            {
                byte[] wallet = Storage.Get(Storage.CurrentContext, "Approver");
                Storage.Put(Storage.CurrentContext, "Wallet", wallet);
                Storage.Put(Storage.CurrentContext, "Active", 1);
                Storage.Put(Storage.CurrentContext, "BaseToken", _baseToken);
                Storage.Put(Storage.CurrentContext, "EtherToken", _etherToken);

                return true;
            }
            return true;
        }


        public static bool updateFeeSchedule(uint baseTokenFee, uint etherTokenFee, uint normalTokenFee)
        {
            if (baseTokenFee >= 0 && baseTokenFee <= 1 && etherTokenFee >= 0 && etherTokenFee <= 1 && normalTokenFee >= 0)
            {
                Storage.Put(Storage.CurrentContext, "0", baseTokenFee);
                Storage.Put(Storage.CurrentContext, "1", etherTokenFee);
                Storage.Put(Storage.CurrentContext, "2", normalTokenFee);

            }

            return true;
        }




        public static bool changeApprover(byte[] newApprover)
        {
            byte[] check = Storage.Get(Storage.CurrentContext, "Vault is Open");
            byte[] check1 = Storage.Get(Storage.CurrentContext, "Vault is Seal");

            if (check1.Equals(0) && (check.Equals(0)))
            {

                Storage.Put(Storage.CurrentContext, "Approver", newApprover);
                Runtime.Log("Approver Changed");
                return true;


            }
            return false;

        }

        public static bool addAuthorizedAddress(byte[] appIntegrator)
        {

            Storage.Put(Storage.CurrentContext, appIntegrator, 1);
            Runtime.Log("Authorized Address Added");

            return true;
        }


        public static bool removeAuthorizedAddress(byte[] appIntegrator)
        {

            Storage.Put(Storage.CurrentContext, appIntegrator, 0);
            if (Storage.Get(Storage.CurrentContext, appIntegrator)[0] == 0)
            {
                Storage.Delete(Storage.CurrentContext, appIntegrator);

            }

            Runtime.Log("Authorized Address Removed");


            return true;
        }


        public static bool addOwner(byte[] newOwner)
        {
            Runtime.Notify(newOwner + " is Added");
            Storage.Put(Storage.CurrentContext, newOwner, 1);
            Storage.Put(Storage.CurrentContext, newOwner.Concat(Neo.SmartContract.Framework.Helper.AsByteArray("owner")), "owner");


            return true;
        }

        public static bool removeOwner(byte[] reowner)
        {
            Runtime.Notify(reowner + " is Removed");
            Storage.Put(Storage.CurrentContext, reowner, 0);
            if (Storage.Get(Storage.CurrentContext, reowner)[0] == 0)
            {
                Storage.Delete(Storage.CurrentContext, reowner);
                Storage.Delete(Storage.CurrentContext, reowner.Concat(Neo.SmartContract.Framework.Helper.AsByteArray("owner")));
                return true;
            }

            return true;
        }


        public static byte[] openVault(BigInteger startTime, BigInteger closureTime)
        {


            byte[] check = Storage.Get(Storage.CurrentContext, "Vault is Open");
            byte[] check1 = Storage.Get(Storage.CurrentContext, "Vault is Seal");


            BigInteger now = Blockchain.GetHeader(Blockchain.GetHeight()).Timestamp;
            Runtime.Notify(now);
            if (check1.Equals(0) && (check.Equals(0) && startTime <= now && closureTime >= now && closureTime >= startTime))
            {
                Storage.Put(Storage.CurrentContext, "starttime", startTime);
                Storage.Put(Storage.CurrentContext, "closuretime", closureTime);

                Storage.Put(Storage.CurrentContext, "Vault is Open", 1);
                Runtime.Log("Vault is Opened");

            }
            else
            {
                Storage.Put(Storage.CurrentContext, "Vault is Open", 0);
                if (Storage.Get(Storage.CurrentContext, "Vault is Open")[0] == 0)
                {
                    Storage.Delete(Storage.CurrentContext, "Vault is Open");
                }

                Runtime.Log("Vault is Closed");
            }
            return Storage.Get(Storage.CurrentContext, "Vault is Open");



        }

        public static byte[] sealVault(BigInteger now)
        {
            byte[] check1 = Storage.Get(Storage.CurrentContext, "Vault is Seal");

            if (check1.Equals(0))
            {
                Storage.Put(Storage.CurrentContext, "closuretime", now);

                Storage.Put(Storage.CurrentContext, "Vault is Seal", 1);

                Storage.Put(Storage.CurrentContext, "Vault is Open", 0);
                if (Storage.Get(Storage.CurrentContext, "Vault is Open")[0] == 0)
                {
                    Storage.Delete(Storage.CurrentContext, "Vault is Open");
                }

                Runtime.Log("Vault is closed and Sealed Successfully");

                return Storage.Get(Storage.CurrentContext, "Vault is Seal");

            }
            Runtime.Log("Vault is not Sealed");
            return Storage.Get(Storage.CurrentContext, "Vault is Seal");



        }

        public static bool extendVault(BigInteger closureTime, BigInteger now)
        {


            BigInteger beginTime = Neo.SmartContract.Framework.Helper.AsBigInteger(Storage.Get(Storage.CurrentContext, "starttime"));



            byte[] check = Storage.Get(Storage.CurrentContext, "Vault is Open");
            byte[] check1 = Storage.Get(Storage.CurrentContext, "Vault is Seal");

            Runtime.Notify("Checking...");
            if (check1.Equals(0) && check.Equals(1) && beginTime <= now && closureTime >= now && closureTime >= beginTime)
            {
                Storage.Put(Storage.CurrentContext, "closuretime", closureTime);
                Storage.Put(Storage.CurrentContext, "Vault is Open", 1);
                Runtime.Log("Vault Time is extended");
                return true;
            }

            Runtime.Log("Vault Time is not extended");
            return false;
        }



        public static bool storeVault(byte[] oHash, byte[] orderId, BigInteger now)
        {
            BigInteger beginTime = Neo.SmartContract.Framework.Helper.AsBigInteger(Storage.Get(Storage.CurrentContext, "starttime"));
            BigInteger endTime = Neo.SmartContract.Framework.Helper.AsBigInteger(Storage.Get(Storage.CurrentContext, "closuretime"));

            byte[] check = Storage.Get(Storage.CurrentContext, "Vault is Open");
            byte[] check1 = Storage.Get(Storage.CurrentContext, "Vault is Seal");

            //Storage.Put(Storage.CurrentContext, "OrderHash", Neo.SmartContract.Framework.Helper.AsBigInteger(Storage.Get(Storage.CurrentContext, "OrderHash"))+ 1); ;

            Runtime.Log("Checking....");
            Runtime.Notify(beginTime, endTime);
            if (check1.Equals(0) && check.Equals(1) && beginTime <= now && endTime >= now && endTime >= beginTime)
            {
                Storage.Put(Storage.CurrentContext, oHash.Concat(Neo.SmartContract.Framework.Helper.AsByteArray(" ")), "orderhashes");
                Storage.Put(Storage.CurrentContext, oHash, 1);
                Storage.Put(Storage.CurrentContext, orderId, 1);
                Runtime.Log("Well Stored");
            }
            else
            {
                Runtime.Log("Vault Not Stored");
            }
            return true;

        }

        public static bool closeVault(BigInteger now)
        {

            byte[] check = Storage.Get(Storage.CurrentContext, "Vault is Open");
            byte[] check1 = Storage.Get(Storage.CurrentContext, "Vault is Seal");

            if (check1.Equals(0) && check.Equals(1))
            {
                Storage.Put(Storage.CurrentContext, "closuretime", now);
                Storage.Put(Storage.CurrentContext, "Vault is Open", 0);

                if (Storage.Get(Storage.CurrentContext, "Vault is Open")[0] == 0)
                {
                    Storage.Delete(Storage.CurrentContext, "Vault is Open");
                }

                Runtime.Log("Vault is Closed Successfully");
                return true;
            }


            Runtime.Log("Vault is does not Closed");


            return true;
        }

        public static Object getFeeIndex(byte[] _token)
        {

            byte[] baseToken = Storage.Get(Storage.CurrentContext, "BaseToken");
            byte[] etherToken = Storage.Get(Storage.CurrentContext, "EtherToken");
            if (_token.Length >= 0)
            {
                if (_token == baseToken)
                    return "0";
                else if (_token == etherToken)
                    return "1";
                return "2";
            }

            return "false";



        }


        public static Object getSellerHash(byte[][] _sellerTokens, BigInteger[] _sellerValues, byte[][] _orderAddresses, BigInteger[] _orderValues, byte[] _orderID)
        {

            byte[] sellerHash = null;
            int slength = _sellerTokens.Length;
            Runtime.Notify(slength);
            byte[] sellerv;// = new byte[length];
            byte[] sorderv = Neo.SmartContract.Framework.Helper.AsByteArray(_orderValues[3]);
            byte[] sorderv1 = Neo.SmartContract.Framework.Helper.AsByteArray(_orderValues[0]);

            if (slength != 0)
                Runtime.Log("SellerHash ");
            for (int j = 0; j < slength; j++)
            {
                sellerv = Neo.SmartContract.Framework.Helper.AsByteArray(_sellerValues[j]);
                Runtime.Notify("Seller Hash : ", j);
                byte[] sc = _sellerTokens[j].Concat(sellerv);

                byte[] sc1 = sorderv.Concat(sorderv1);
                byte[] sc2 = _orderAddresses[3].Concat(_orderAddresses[0]);
                byte[] soa = sc.Concat(sc1).Concat(_orderID);
                byte[] soa1 = sc2.Concat(_orderAddresses[1]);


                sellerHash = Sha256(soa.Concat(soa1));

                Storage.Put(Storage.CurrentContext, sellerHash, "orderHash");
                Runtime.Notify(sellerHash);


            }
            return sellerHash;

        }


        public static Object getBuyerHash(byte[][] _buyerTokens, BigInteger[] _buyerValues, byte[][] _orderAddresses, BigInteger[] _orderValues, byte[] _orderID)
        {
            byte[] buyerHash = null;
            int blength = _buyerTokens.Length;
            byte[] buyerv;
            byte[] borderv = Neo.SmartContract.Framework.Helper.AsByteArray(_orderValues[4]);
            byte[] borderv1 = Neo.SmartContract.Framework.Helper.AsByteArray(_orderValues[0]);
            if (blength != 0)
                Runtime.Log("BuyerHash ");
            for (int k = 0; k < blength; k++)
            {
                buyerv = Neo.SmartContract.Framework.Helper.AsByteArray(_buyerValues[k]);
                Runtime.Notify("Buyer Hash : ", k);
                byte[] bc = _buyerTokens[k].Concat(buyerv);
                byte[] bc1 = borderv.Concat(borderv1);
                byte[] bc2 = _orderAddresses[4].Concat(_orderAddresses[0]);

                byte[] boa = bc.Concat(bc1).Concat(_orderID);
                byte[] boa1 = bc2.Concat(_orderAddresses[2]);
                buyerHash = Sha256(boa.Concat(boa1));
                Storage.Put(Storage.CurrentContext, buyerHash, "orderHash");
                Runtime.Notify(buyerHash);

            }
            return buyerHash;



        }




        public static Object getTwoWayOrderHash(byte[][] _sellerTokens, byte[][] _buyerTokens, BigInteger[] _sellerValues, BigInteger[] _buyerValues, byte[][] _orderAddresses, BigInteger[] _orderValues, byte[] _orderID)
        {


            byte[] SellerHash = (byte[])getSellerHash(_sellerTokens, _sellerValues, _orderAddresses, _orderValues, _orderID);

            byte[] BuyerHash = (byte[])getBuyerHash(_buyerTokens, _buyerValues, _orderAddresses, _orderValues, _orderID);



            byte[] twc = (ExecutionEngine.ExecutingScriptHash).Concat(SellerHash);
            byte[] twc1 = BuyerHash.Concat(twc).Concat(_orderID);


            return Hash256(twc1);

        }


        public static Object basicSigValidations(byte[][] _orderAddresses, uint[] _v, byte[] _sr, byte[] _ss, byte[] _br, byte[] _bs, byte[] _sellerHash, byte[] _buyerHash)
        {
            //if (!(Main("ecverify", _sellerHash, _v[0], _sr, _ss, _orderAddresses[1])).Equals(address0))
            if (Runtime.CheckWitness(_orderAddresses[1]))
            {

                return _orderAddresses[1];
            }

            //if (!(Main("ecverify", _sellerHash, _v[1], _sr, _ss, _orderAddresses[2])).Equals(address0))
            if (Runtime.CheckWitness(_orderAddresses[2]))
            {
                return _orderAddresses[2];
            }
            return 0x0;
        }


        public static Object transferforTokens(byte[][] _sellerTokens, byte[][] _buyerTokens, BigInteger[] _sellerValues, BigInteger[] _buyerValues, byte[][] _orderAddresses, BigInteger[] _orderValues)
        {
            int len = _sellerTokens.Length;
            Runtime.Notify(_orderAddresses[1], _sellerTokens.Length);
            for (uint i = 0; i < len; i++)
            {
                Runtime.Log("In");
                Runtime.Notify(_orderAddresses[1], _orderAddresses[2], _sellerValues[0]);
                transferFrom(_orderAddresses[1], _orderAddresses[2], _sellerValues[i]);
            }

            Runtime.Log("SellerValues Are Transferred Successfully");

            int len1 = _buyerTokens.Length;

            for (uint i = 0; i < len1; i++)
            {
                transferFrom(_orderAddresses[2], _orderAddresses[1], _buyerValues[i]);
            }

            byte[] wallet = Storage.Get(Storage.CurrentContext, "Wallet");

            transferFrom(_orderAddresses[1], wallet, _orderValues[0]);
            transferFrom(_orderAddresses[2], wallet, _orderValues[1]);


            return true;


        }


        public static Object validateAuthorization(byte[][] _sellerTokens, byte[][] _buyerTokens, BigInteger[] _sellerValues, BigInteger[] _buyerValues, byte[][] _orderAddresses, BigInteger[] _orderValues)
        {
            object a1 = (allowance(_orderAddresses[2], address0));
            BigInteger allow = (BigInteger)a1;

            if (allow <= _orderValues[1])
            {
                return false;

            }

            object a2 = (allowance( _orderAddresses[2], address0));
            BigInteger allow1 = (BigInteger)a2;



            if (allow1 <= _orderValues[1])
            {
                return false;

            }

            object a3 = (allowance( _orderAddresses[2], address0));
            BigInteger allow2 = (BigInteger)a3;
            for (uint i = 0; i <= _buyerTokens.Length; i++)
            {
                if (allow2 <= _buyerValues[i])
                {
                    return false;

                }
            }
            return true;


        }


        public static Object calcTradeFee(BigInteger values, uint feeIndex)
        {
            byte[] check;

            if (feeIndex >= 0 && feeIndex <= 2 && values > 0)
            {
                if (feeIndex == 0)
                    check = Storage.Get(Storage.CurrentContext, "0");
                else if (feeIndex == 1)
                    check = Storage.Get(Storage.CurrentContext, "1");
                else
                    check = Storage.Get(Storage.CurrentContext, "2");


                BigInteger I = new BigInteger(check);
                BigInteger totalFees = (I * values);///(1 NEO);

                if (totalFees > 0)
                {

                    return totalFees;
                }


            }

            return true;

        }



        public static Object calcTradeFeeMulti(uint[] values, uint[] feeIndexes)
        {
            if (values.Length > 0 && feeIndexes.Length > 0 && values.Length == feeIndexes.Length)
            {
                BigInteger[] totalFees = new BigInteger[values.Length];
                byte[] check;

                for (uint i = 0; i < values.Length; i++)
                {
                    if (feeIndexes[i] >= 0 && feeIndexes[i] <= 2 && values[i] > 0)
                    {
                        if (feeIndexes[i] == 0)
                            check = Storage.Get(Storage.CurrentContext, "0");
                        else if (feeIndexes[i] == 1)
                            check = Storage.Get(Storage.CurrentContext, "1");
                        else
                            check = Storage.Get(Storage.CurrentContext, "2");


                        BigInteger I = new BigInteger(check);

                        totalFees[i] = (values[i] * I);
                    }
                }

                return totalFees;

            }
            return false;

        }




        public static Object deposit(byte[] acc, BigInteger val)
        {
            Runtime.Log("Deposited Successfully Balance -");
            BigInteger balance = Neo.SmartContract.Framework.Helper.AsBigInteger(Storage.Get(Storage.CurrentContext, acc));
            BigInteger total = balance + val;
            Storage.Put(Storage.CurrentContext, acc, total);
            Runtime.Notify(total);
            return true;

        }

        public static Object balanceOf(byte[] acc)
        {

            BigInteger balance = Neo.SmartContract.Framework.Helper.AsBigInteger(Storage.Get(Storage.CurrentContext, acc));

            Runtime.Log("Balance - ");
            Runtime.Notify(balance + 0);

            return Storage.Get(Storage.CurrentContext, acc);


        }


        public static Object transferFrom(byte[] from, byte[] to, BigInteger value)
        {
            BigInteger from_value = Neo.SmartContract.Framework.Helper.AsBigInteger(Storage.Get(Storage.CurrentContext, from));

            if (value <= 0 || from_value < value)
            {

                Runtime.Log("Insufficient Balance... If You Want to Deposit please use Deposit Function..");
                return false;
            }

            if (from == to) return true;


            //BigInteger from_value = 500000;// if you need to test choose this



            Runtime.Log("Transfer Process ");



            if (from_value > value)
            {
                BigInteger bal = from_value - value;
                Runtime.Notify(from);
                Runtime.Log("Balance: ");
                Runtime.Notify(bal);
                Storage.Put(Storage.CurrentContext, from, bal);
                BigInteger to_value = Neo.SmartContract.Framework.Helper.AsBigInteger(Storage.Get(Storage.CurrentContext, to));

                BigInteger bal1 = to_value + value;
                Storage.Put(Storage.CurrentContext, to, bal1);
                Runtime.Notify(to);
                Runtime.Log("Balance: ");
                Runtime.Notify(bal1);
                Runtime.Log("-------------------");
                return true;
            }
            return false;


        }

        public static Object approval(byte[] from, byte[] to, BigInteger value)
        {
            Storage.Put(Storage.CurrentContext, from.Concat(to), value);
            return true;

        }



            public static Object allowance(byte[] add1, byte[] add2)
        {
            return Storage.Get(Storage.CurrentContext, add1.Concat(add2));
        }
    }
}
//0x27cff39b7d6d9105251b879945fc3e36914574fb







