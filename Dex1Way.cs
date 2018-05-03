using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using Neo.SmartContract.Framework.Services.System;
using System;
using System.Numerics;


namespace Dex1WayConverted
{
    public class Contract1 : SmartContract
    {
        public static readonly byte[] Owner = "APM9y5gA3dquRMUDN5Q2drJfjWXvpMYZP9".ToScriptHash();

        public delegate object NEP5Contract(string method, object[] args);

        public static Object Main(string oper, params Object[] args)
        {
            /**
                * @dev defines inital exchange parameters and connections required
                * @param approver is the approver details
                * @param _baseToken is the native token or Wand token
                * @param _etherToken is tokenizes the ether or tokenized ether
                */
            if (oper == "Deploy")
            {
                byte[] approver = (byte[])args[0];
                byte[] _baseToken = (byte[])args[1];
                byte[] _etherToken = (byte[])args[2];
                Runtime.Notify("deployed successfully");
                return deploy(approver, _baseToken, _etherToken);
            }


            if (Runtime.CheckWitness(Owner))
            {
                /**
                * @dev function updates the fees charged by the exchange. 
                * @param baseTokenFee is for the trades who pays fees in Native Tokens
                * @param etherTokenFee is for the trades who pays fees in Ether Tokens
                * @param normalTokenFee is for the trades who pays fees in Normal Tokens
                */
                if (oper == "updateFeeSchedule")
                {
                    uint baseTokenFee = (uint)args[0];
                    uint etherTokenFee = (uint)args[1];
                    uint normalTokenFee = (uint)args[2];
                    return updateFeeSchedule(baseTokenFee, etherTokenFee, normalTokenFee);
                }


                /**
                * @dev function for changing approver
                * @param newApprover new approver details 
                */
                if (oper == "changeApprover")
                {
                    byte[] newApprover = (byte[])args[0];
                    return changeApprover(newApprover);
                }

                /**
                * @dev Authorizes an address.
                * @param appIntegrator Address to authorize.
                */
                if (oper == "addAutherizedAddress")
                {
                    byte[] appIntegrator = (byte[])args[0];
                    return addAuthorizedAddress(appIntegrator);
                }

                /**
                * @dev Removes authorizion of an address.
                * @param appIntegrator Address to remove authorization from.
                */
                if (oper == "removeAutherizedAddress")
                {
                    byte[] appIntegrator = (byte[])args[0];
                    return removeAuthorizedAddress(appIntegrator);
                }



                /**
                 * @dev function adds a new owner to the ownership
                 * @param newOwner is the owner to be added
                 */
                if (oper == "addOwner")
                {
                    byte[] newOwner = (byte[])args[0];
                    return addOwner(newOwner);
                }


                /**
                * @dev function removes an exissting owner
                * @param reowner to be removed
                */
                if (oper == "removeOwner")
                {
                    byte[] reowner = (byte[])args[0];
                    return removeOwner(reowner);
                }



                /**
                 * @dev function opens the vault at specific time
                 * @param startTime specifies when to open the vault
                 * @param closureTime specifies when to close the vault
                 */
                if (oper == "openVault")
                {
                    BigInteger startTime = (BigInteger)args[0];
                    BigInteger closureTime = (BigInteger)args[1];
                    return openVault(startTime, closureTime);
                }

                /**
                * @dev function Close Vault does close the vault in future timestamp. and it can be reopen by owner again
                * But SealVault does it Immediately and permanently. Once sealed cant be open again. 
                */
                if (oper == "sealVault")
                {
                    BigInteger now = Blockchain.GetHeader(Blockchain.GetHeight()).Timestamp;
                    return sealVault(now);
                }


                /**
                * @dev function Extends the vault till the given time. Eventhough flag says its started,
                * its a logical start only not a real start. The real start happens at begin time. 
                * Extensions then possible when its really started
                * @param closureTime specifies when to close the vault 
                */
                if (oper == "extendVault")
                {
                    BigInteger closureTime = (BigInteger)args[0];
                    BigInteger now = Blockchain.GetHeader(Blockchain.GetHeight()).Timestamp;
                    return extendVault(closureTime, now);
                }

                /**
                 * @dev function Storens the given hash in the Vault
                 * @param oHash brings the hash to be stored 
                 * @param orderId brings the Id to be stored 
                 */
                if (oper == "storeVault")
                {
                    byte[] oHash = (byte[])args[0];
                    byte[] orderId = ((byte[])args[1]);

                    return storeVault(oHash, orderId);
                }

                /**
                 * @dev function closes the vault  
                 */
                if (oper == "closeVault")
                {
                    BigInteger now = Blockchain.GetHeader(Blockchain.GetHeight()).Timestamp;
                    return closeVault(now);
                }

            }
            //---runtime checkwitness ended ---


            /**
            * @dev function to get the indexes for Fee token types 
            * @param _token to be used to pay the fee
            * @return index of the fee array from Fee Calculator
            */
            if (oper == "getFeeIndex")
            {
                byte[] _token = (byte[])args[0];
                return getFeeIndex(_token);
            }


            /**
            * @dev function to generate seller order Hashes
            * @param _sellerTokens selling portfolio tokens
            * @param _sellerValues selling porfolio token amounts
            * @param _orderAddresses Contains maker, seller, buyer, seller fee token, and buyer fee token addresses
            * @param _orderValues contains values for seller fee, buyer fee, expiration time stamp, and salt used for Hash generation 
            * @param _orderID from Database
            * @return seller Order hash
            */
            if (oper == "getSellerHash")
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

            /**
            * @dev function to generate buyer order Hashes
            * @param _buyerTokens buying tokens
            * @param _buyerValues token amounts for buying portfolio
            * @param _orderAddresses Contains maker, seller, buyer, seller fee token, and buyer fee token addresses
            * @param _orderValues contains values for seller fee, buyer fee, expiration time stamp, and salt used for Hash generation 
            * @param _orderID from Database
            * @return buyer Order hash
            */
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



            /**
             * @dev function to generate the Hash for an Order, this will be stored in Orders Vault
             * @param _sellerTokens selleing portfolio tokens
             * @param _buyerTokens buying tokens
             * @param _sellerValues selling porfolio token amounts
             * @param _buyerValues token amounts for buying portfolio
             * @param _orderAddresses Contains maker, seller, buyer, seller fee token, and buyer fee token addresses
             * @param _orderValues contains values for seller fee, buyer fee, expiration time stamp, and salt used for Hash generation 
             * @param _orderID from Database
             * @return Order Hash generated by keccak256 hashing algorithm
             */
            if (oper == "getTwoWayOrderHash") //[["sellerT","sellerT1","SellerT2"],["buyerT","buyerT1","buyerT2"],[500,450,400],[458,444,380],["AA1","BB1","CC1","DD1","EE1"],[540,450,580,550,650],["OI01"]]
            {
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


            /**
            * @dev function for validating the buyer and seller signatures
            * @param _orderAddresses Contains maker, seller, buyer, seller fee token, and buyer fee token addresses
            * @return seller Address if seller signtaures are valid and if buyer signature are valid returns the buyer Address else return false 
            */
            if (oper == "basicSigValidations")
            {
                byte[][] _orderAddresses = new byte[5][];
                _orderAddresses = (byte[][])args[0];
                /*uint[] _v = new uint[2];
                _v = (uint[])args[1];
                byte[] _sr = (byte[])args[2];
                byte[] _ss = (byte[])args[3];
                byte[] _br = (byte[])args[4];
                byte[] _bs = (byte[])args[5];
                byte[] _sellerHash = (byte[])args[6];
                byte[] _buyerHash = (byte[])args[7];*/
                return basicSigValidations(_orderAddresses);//, _v, _sr, _ss, _br, _bs, _sellerHash, _buyerHash);
            }



            /**
            * @dev function for transfering the portfolio orders tokens. 
            * @param _sellerTokens selling portfolio tokens
            * @param _buyerTokens buying tokens
            * @param _sellerValues selling porfolio token amounts
            * @param _buyerValues token amounts for buying portfolio
            * @param _orderAddresses Contains maker, seller, buyer, seller fee token, and buyer fee token addresses
            * @param _orderValues contains values for seller fee, buyer fee, expiration time stamp  
            */
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


            /**
            * @dev function for validation token authorizations for Exchange contract
            * @param _sellerTokens selling portfolio tokens
            * @param _buyerTokens buying tokens
            * @param _sellerValues selling porfolio token amounts
            * @param _buyerValues token amounts for buying portfolio
            * @param _orderAddresses Contains maker, seller, buyer, seller fee token, and buyer fee token addresses
            * @param _orderValues contains values for seller fee, buyer fee, expiration time stamp 
            * @return True if all are authorized else false
            */
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


            /**
            * @dev function to calculate transaction fees for given value and token
            * @param value is the given trade overall value
            * @param feeIndex indicates token pay options
            * @return calculated trade fee
            */
            if (oper == "calcTradeFee")
            {
                BigInteger value = (BigInteger)args[0];
                uint feeIndex = (uint)args[1];
                return calcTradeFee(value, feeIndex);
            }


            /**
             * @dev function to calculate transaction fees for given list of values and tokens
             * @param values is the list of given trade overall values
             * @param feeIndexes indicates list token pay options for each value 
             * @return list of calculated trade fees each value
             */
            if (oper == "calcTradeFeeMulti")
            {
                uint[] values = (uint[])args[0];
                uint[] feeIndexes = (uint[])args[1];
                return calcTradeFeeMulti(values, feeIndexes);
            }



            /**
             * @dev function for order hash lookup
             * @param hash to be searched
             * @return true, if order hash already exists
             */
            if (oper == "orderLocated")
            {
                byte[] hash = (byte[])args[0];
                byte[] orderId = (byte[])args[1];
                Runtime.Notify("Order  Location Checked");
                return (Storage.Get(Storage.CurrentContext, hash.Concat(Neo.SmartContract.Framework.Helper.AsByteArray(" "))).Equals("orderhash"));
            }



            /**
            * @dev function kills the contract, so that no further fulfilments happens 
            */
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


            /**
             * @dev function verifies the singers signature and its autheticity
             * @param _msgHash signed hash of the order
             * @param v component of the signature
             * @param r component of the signature
             * @param s component of the signature
             * @param _signer address of the person who signed the order
             * @return true/false indicating valid/invalid signature
             */
            if (oper == "isOrderSigned")  //parameters :Address, msghash, v,r,s, signer
            {
                Runtime.Notify("Signature Verified");
                byte _msgHash = (byte)args[0];
                uint v = (uint)args[1];
                byte r = (byte)args[2];
                byte s = (byte)args[3];
                byte[] _signer = (byte[])args[4];

                //return Main("ecverify", address, _msgHash, v, r, s, _signer);
                if (Runtime.CheckWitness(_signer))
                {
                    return true;
                }
                return false;
            }


            /**
            * @dev function for validating input parameters for Fee Calculation
            * @param _sellerFeeToken token Address for seller fee payments
            * @param _buyerFeeToken token Address for buyer fee payments
            * @return True if vaidation successful, else false 
            */
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


            /**
            * @dev function checks whether the order already exists or not. It checks in current vaualt as well as in history vaults 
            * @param _hash of the Order
            * @return True if already exists else false
            */
            if (oper == "orderExists")
            {
                byte[] _hash = (byte[])args[0];
                byte[] _orderId = (byte[])args[1];
                orderExists(_hash, _orderId);
                return false;
            }


            /**
            * @dev function for order fulfilment with signatures from boththe parties  
            * @param _sellerTokens selling portfolio tokens
            * @param _buyerTokens buying tokens
            * @param _sellerValues selling porfolio token amounts
            * @param _buyerValues token amounts for buying portfolio
            * @param _orderAddresses Contains maker, seller, buyer, seller fee token, and buyer fee token addresses
            * @param _orderValues contains values for seller fee, buyer fee, expiration time stamp
            * @param _v is an array of v values for buyer ad seller signatures
            * @param _sr is r part of the seller signature
            * @param _ss is s part of the seller signature
            * @param _br is r part of the buyer signature
            * @param _bs is s part of the buyer signature 
            * @param _orderID from Database
            */
            if (oper == "oneWayFulfillPO")
            {
                byte[][] _sellerTokens = (byte[][])args[0];
                byte[][] _buyerTokens = (byte[][])args[1];
                BigInteger[] _sellerValues = (BigInteger[])args[2];
                BigInteger[] _buyerValues = (BigInteger[])args[3];
                byte[][] _orderAddresses = new byte[5][];
                _orderAddresses = (byte[][])args[4];
                BigInteger[] _orderValues = new BigInteger[5];
                _orderValues = (BigInteger[])args[5];
                uint[] _v = new uint[2];
                _v = (uint[])args[6];
                byte[] _br = (byte[])args[7];
                byte[] _bs = (byte[])args[8];
                byte[] _sr = (byte[])args[9];
                byte[] _ss = (byte[])args[10];
                byte[] _orderID = (byte[])args[11];

                return oneWayFulfillPO(_sellerTokens, _buyerTokens, _sellerValues, _buyerValues, _orderAddresses, _orderValues, _v, _br, _bs, _sr, _ss, _orderID);
            }

            return false;
        }




        //Function_Definition_Starts

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
                Runtime.Notify("deployed success");
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
                Runtime.Notify("updated fee schedules");
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
                Runtime.Notify("Approver Changed");
                return true;
            }
            return false;
        }


        public static bool addAuthorizedAddress(byte[] appIntegrator)
        {
            Storage.Put(Storage.CurrentContext, appIntegrator, 1);
            Runtime.Notify("Authorized Address Added");
            return true;
        }


        public static bool removeAuthorizedAddress(byte[] appIntegrator)
        {
            Storage.Put(Storage.CurrentContext, appIntegrator, 0);
            if (Storage.Get(Storage.CurrentContext, appIntegrator)[0] == 0)
            {
                Storage.Delete(Storage.CurrentContext, appIntegrator);
            }
            Runtime.Notify("Authorized Address Removed");
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
                Runtime.Notify("remove successfull");
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
                Runtime.Notify("Vault is Opened");
            }
            else
            {
                Storage.Put(Storage.CurrentContext, "Vault is Open", 0);
                if (Storage.Get(Storage.CurrentContext, "Vault is Open")[0] == 0)
                {
                    Storage.Delete(Storage.CurrentContext, "Vault is Open");
                    Runtime.Notify("vault is open");
                }

                Runtime.Notify("Vault is Closed");
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
                    Runtime.Notify("vault is sealed");
                }
                Runtime.Notify("Vault is closed and Sealed Successfully");
                return Storage.Get(Storage.CurrentContext, "Vault is Seal");
            }
            Runtime.Notify("Vault is not Sealed");
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
                Runtime.Notify("Vault Time is extended");
                return true;
            }

            Runtime.Notify("Vault Time is not extended");
            return false;
        }



        public static bool storeVault(byte[] oHash, byte[] orderId)
        {
            BigInteger now = Blockchain.GetHeader(Blockchain.GetHeight()).Timestamp;
            BigInteger beginTime = Neo.SmartContract.Framework.Helper.AsBigInteger(Storage.Get(Storage.CurrentContext, "starttime"));
            BigInteger endTime = Neo.SmartContract.Framework.Helper.AsBigInteger(Storage.Get(Storage.CurrentContext, "closuretime"));
            byte[] check = Storage.Get(Storage.CurrentContext, "Vault is Open");
            byte[] check1 = Storage.Get(Storage.CurrentContext, "Vault is Seal");
            Runtime.Notify("Processing....");
            Runtime.Notify(beginTime, endTime);
            if (check1.Equals(0) && check.Equals(1) && beginTime <= now && endTime >= now && endTime >= beginTime)
            {
                Storage.Put(Storage.CurrentContext, oHash.Concat(Neo.SmartContract.Framework.Helper.AsByteArray(" ")), "orderhashes");
                Storage.Put(Storage.CurrentContext, oHash, 1);
                Storage.Put(Storage.CurrentContext, orderId, 1);
                Runtime.Notify("Well Stored");
            }
            else
            {
                Runtime.Notify("Vault Not Stored");
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

                Runtime.Notify("Vault is Closed Successfully");
                return true;
            }
            Runtime.Notify("Vault is does not Closed");
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
            return false;
        }



        public static byte[] getSellerHash(byte[][] _sellerTokens, BigInteger[] _sellerValues, byte[][] _orderAddresses, BigInteger[] _orderValues, byte[] _orderID)
        {
            byte[] sellerHash = null;
            int slength = _sellerTokens.Length;
            Runtime.Notify(slength);
            byte[] sellerv;
            byte[] sorderv = Neo.SmartContract.Framework.Helper.AsByteArray(_orderValues[3]);
            byte[] sorderv1 = Neo.SmartContract.Framework.Helper.AsByteArray(_orderValues[0]);
            if (slength != 0)
                Runtime.Notify("SellerHash Processing.. ");
            for (int j = 0; j < slength; j++)
            {
                sellerv = Neo.SmartContract.Framework.Helper.AsByteArray(_sellerValues[j]);
                Runtime.Notify("Seller Hash Count : ", j);
                byte[] sc = _sellerTokens[j].Concat(sellerv);
                byte[] sc1 = sorderv.Concat(sorderv1);
                byte[] sc2 = _orderAddresses[3].Concat(_orderAddresses[0]);
                byte[] soa = sc.Concat(sc1).Concat(_orderID);
                byte[] soa1 = sc2.Concat(_orderAddresses[1]);
                sellerHash = Sha256(soa.Concat(soa1));
                Storage.Put(Storage.CurrentContext, sellerHash, "orderHash");
                Runtime.Notify(sellerHash);
                Runtime.Notify("seller hash calculated");
            }
            return sellerHash;
        }


        public static byte[] getBuyerHash(byte[][] _buyerTokens, BigInteger[] _buyerValues, byte[][] _orderAddresses, BigInteger[] _orderValues, byte[] _orderID)
        {
            byte[] buyerHash = null;
            int blength = _buyerTokens.Length;
            byte[] buyerv;
            byte[] borderv = Neo.SmartContract.Framework.Helper.AsByteArray(_orderValues[4]);
            byte[] borderv1 = Neo.SmartContract.Framework.Helper.AsByteArray(_orderValues[0]);
            if (blength != 0)
                Runtime.Notify("BuyerHash Processing... ");
            for (int k = 0; k < blength; k++)
            {
                buyerv = Neo.SmartContract.Framework.Helper.AsByteArray(_buyerValues[k]);
                Runtime.Notify("Buyer Hash Count : ", k);
                byte[] bc = _buyerTokens[k].Concat(buyerv);
                byte[] bc1 = borderv.Concat(borderv1);
                byte[] bc2 = _orderAddresses[4].Concat(_orderAddresses[0]);
                byte[] boa = bc.Concat(bc1).Concat(_orderID);
                byte[] boa1 = bc2.Concat(_orderAddresses[2]);
                buyerHash = Sha256(boa.Concat(boa1));
                Storage.Put(Storage.CurrentContext, buyerHash, "orderHash");
                Runtime.Notify("buyerhash calculated");
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
            Runtime.Notify("twway calculaeeeeeeeeed");
            return Hash256(twc1);
        }


        public static Object basicSigValidations(byte[][] _orderAddresses)//, uint[] _v, byte[] _sr, byte[] _ss, byte[] _br, byte[] _bs, byte[] _sellerHash, byte[] _buyerHash)
        {
            //if (!(Main("ecverify", _sellerHash, _v[0], _sr, _ss, _orderAddresses[1])).Equals(address0))
            if (Runtime.CheckWitness(_orderAddresses[1]))
            {
                Runtime.Notify("verified true");
                return _orderAddresses[1];
            }

            //if (!(Main("ecverify", _sellerHash, _v[1], _sr, _ss, _orderAddresses[2])).Equals(address0))
            if (Runtime.CheckWitness(_orderAddresses[2]))
            {
                Runtime.Notify("verified as false");
                return _orderAddresses[2];
            }
            return false;
        }



        public static Object transferforTokens(byte[][] _sellerTokens, byte[][] _buyerTokens, BigInteger[] _sellerValues, BigInteger[] _buyerValues, byte[][] _orderAddresses, BigInteger[] _orderValues)
        {
            int len = _sellerTokens.Length;
            Runtime.Notify(_orderAddresses[1], _sellerTokens.Length);
            Object[] arg;
            byte[] token;
            for (uint i = 0; i < len; i++)
            {
                Runtime.Notify("Seller Transfer Process Count");
                Runtime.Notify(i);
                arg = new Object[] { _orderAddresses[1], _orderAddresses[2], _sellerValues[i] };
                token = (byte[])_sellerTokens[i];
                var sellerContract = (NEP5Contract)token.ToDelegate();
                sellerContract("transfer", arg);
                //transferFrom(_orderAddresses[1], _orderAddresses[2], _sellerValues[i]);
            }

            Runtime.Notify("Seller Values Are Transferred Successfully");
            int len1 = _buyerTokens.Length;

            for (uint i = 0; i < len1; i++)
            {
                Runtime.Notify("Buyer Transfer Process Count");
                Runtime.Notify(i);
                token = (byte[])_buyerTokens[i];
                arg = new Object[] { _orderAddresses[2], _orderAddresses[1], _buyerValues[i] };
                var buyerContract = (NEP5Contract)token.ToDelegate();
                buyerContract("transfer", arg);
                //transferFrom(_orderAddresses[2], _orderAddresses[1], _buyerValues[i]);
            }

            Runtime.Notify("Wallet Transfer Process ");
            byte[] wallet = Storage.Get(Storage.CurrentContext, "Wallet");
            token = (byte[])_orderAddresses[3];
            arg = new Object[] { _orderAddresses[1], wallet, _orderValues[0] };
            var Contract = (NEP5Contract)token.ToDelegate();
            Contract("transfer", arg);
            //transferFrom(_orderAddresses[1], wallet, _orderValues[0]);

            token = (byte[])_orderAddresses[4];
            arg = new object[] { _orderAddresses[2], wallet, _orderValues[1] };
            Contract = (NEP5Contract)token.ToDelegate();
            Contract("transfer", arg);
            // transferFrom(_orderAddresses[2], wallet, _orderValues[1]);

            return true;
        }



        public static Object validateAuthorization(byte[][] _sellerTokens, byte[][] _buyerTokens, BigInteger[] _sellerValues, BigInteger[] _buyerValues, byte[][] _orderAddresses, BigInteger[] _orderValues)
        {
            var arg = new Object[] { _orderAddresses[2], (byte[])ExecutionEngine.ExecutingScriptHash };
            byte[] token = (byte[])_orderAddresses[4];


            var Contract = (NEP5Contract)token.ToDelegate();
            var result = (BigInteger)Contract("allowance", arg);
            if (result <= _orderValues[1]) return false;
            arg = new Object[] { _orderAddresses[1], (byte[])ExecutionEngine.ExecutingScriptHash };
            token = (byte[])_orderAddresses[3];
            Contract = (NEP5Contract)token.ToDelegate();
            result = (BigInteger)Contract("allowance", arg);

            if (result <= _orderValues[0])
                return false;

            for (uint i = 0; i < _buyerTokens.Length; i++)
            {
                arg = new Object[] { _orderAddresses[2], (byte[])ExecutionEngine.ExecutingScriptHash };
                token = (byte[])_buyerTokens[i];
                var buyertoken = (NEP5Contract)token.ToDelegate();
                result = (BigInteger)buyertoken("allowance", arg);
                if (result <= _buyerValues[i])
                    return false;
            }

            for (uint i = 0; i < _sellerTokens.Length; i++)
            {
                arg = new Object[] { _orderAddresses[1], (byte[])ExecutionEngine.ExecutingScriptHash };
                token = (byte[])_sellerTokens[i];
                var sellertoken = (NEP5Contract)token.ToDelegate();
                result = (BigInteger)sellertoken("allowance", arg);

                if (result <= _sellerValues[i])
                    return false;
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


        public static bool orderExists(byte[] _hash, byte[] _orderId)
        {
            if (Main("orderLocated", _hash, _orderId) != null)
            {
                return true;
            }
            return false;

        }


        public static bool oneWayFulfillPO(byte[][] _sellerTokens, byte[][] _buyerTokens, BigInteger[] _sellerValues, BigInteger[] _buyerValues, byte[][] _orderAddresses,
                BigInteger[] _orderValues, uint[] _v, byte[] _br, byte[] _bs, byte[] _sr, byte[] _ss, byte[] _orderID)
        {
            BigInteger now = Blockchain.GetHeader(Blockchain.GetHeight()).Timestamp;
            byte[] address0 = "AMjhfjhe34735ndajtw6ed".AsByteArray();
            byte[] check = Storage.Get(Storage.CurrentContext, "Active");
            byte[] check1 = Storage.Get(Storage.CurrentContext, address0);

            if (check.Equals(1) && _orderValues[2] >= now)
            {
                if (!address0.Equals(_orderAddresses[1]) && (!address0.Equals(_orderAddresses[2])) && check1.Equals(0))
                {
                    return false;
                }
            }

            byte[] SellerHash = getSellerHash(_sellerTokens, _sellerValues, _orderAddresses, _orderValues, _orderID);
            byte[] BuyerHash = getBuyerHash(_buyerTokens, _buyerValues, _orderAddresses, _orderValues, _orderID);


            if (basicSigValidations(_orderAddresses) != null)//, _v, _sr, _ss, _br, _bs, SellerHash, BuyerHash) != null)
            {
                return false;
            }

            byte[] twc = (ExecutionEngine.ExecutingScriptHash).Concat(SellerHash);
            byte[] twc1 = BuyerHash.Concat(twc).Concat(_orderID);
            byte[] TWH = Hash256(twc.Concat(twc1));

            if ((orderExists(TWH, _orderID) == true))
            {
                if (Main("validExchangeFee", _orderAddresses[3], _orderAddresses[4], _orderAddresses[0], _orderAddresses[1]) != null)

                {
                    byte[] fi = (byte[])getFeeIndex(_orderAddresses[3]);
                    byte[] fi1 = (byte[])getFeeIndex(_orderAddresses[4]);
                    BigInteger FeeIndex = Neo.SmartContract.Framework.Helper.AsBigInteger(fi);
                    BigInteger FeeIndex1 = Neo.SmartContract.Framework.Helper.AsBigInteger(fi1);
                    byte[] ov = (byte[])(calcTradeFee(_orderValues[0], (uint)FeeIndex));
                    byte[] ov1 = (byte[])(calcTradeFee(_orderValues[1], (uint)FeeIndex1));
                    _orderValues[0] = Neo.SmartContract.Framework.Helper.AsBigInteger(ov);
                    _orderValues[1] = Neo.SmartContract.Framework.Helper.AsBigInteger(ov1);

                    if (_orderValues[0] > 0 && _orderValues[1] > 0 && _orderAddresses[0] != null && _orderAddresses[1] != null && _orderAddresses[2] != null && _orderAddresses[1] != _orderAddresses[2] && _sellerTokens.Length > 0 && _sellerValues.Length > 0 && _sellerTokens.Length == _sellerValues.Length && _buyerTokens.Length > 0 && _buyerValues.Length > 0 && _buyerTokens.Length == _buyerValues.Length)
                    {
                        if (validateAuthorization(_sellerTokens, _buyerTokens, _sellerValues, _buyerValues, _orderAddresses, _orderValues) != null)
                        {
                            return false;
                        }
                        transferforTokens(_sellerTokens, _buyerTokens, _sellerValues, _buyerValues, _orderAddresses, _orderValues);
                        storeVault(TWH, _orderID);
                        return true;
                    }
                }
            }
            return true;
        }



    }
}
//sh: 0xa43e68d474edb7c6e1d108cd9430800e655283af
