using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

using QuantumHub.Common;
using QuantumHub.Models;

namespace QuantumHub.Repository
{
    public static class CipherRepository
    {
        static string _connectionString = @"server=localhost;userid=root;password=Siberia$111;database=quantumencrypt";

        #region Public Methods

        public static Cipher GetCipher(int userId = 0, int cipherId = 0)
        {
            Cipher cipher = null;
            try
            {
                using (var dbConn = new MySqlConnection(_connectionString))
                {
                    dbConn.Open();
                    using (MySqlCommand dbCmd = dbConn.CreateCommand())
                    {
                        dbCmd.CommandText = "QEH_cipherget";
                        dbCmd.CommandType = CommandType.StoredProcedure;
                        dbCmd.Parameters.AddWithValue("userId", userId);
                        dbCmd.Parameters.AddWithValue("cipherId", cipherId);

                        using (var rdr = dbCmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                cipher = new();
                                cipher.CipherId = DataUtil.NullToZero(rdr["idcipher"]);
                                cipher.UserId = DataUtil.NullToZero(rdr["iduser"]);
                                cipher.SerialNumber = DataUtil.NullToEmpty(rdr["serialnumber"]);
                                cipher.StartingPoint = DataUtil.NullToZero(rdr["startpoint"]);
                                cipher.CipherString = DataUtil.NullToEmpty(rdr["cipherstring"]);
                                cipher.CreatedDateTime = DataUtil.NullToDateTimeMinValue(rdr["createdatetime"]);
                            }
                        }
                    }
                    if (dbConn.State == ConnectionState.Open)
                        dbConn.Close();
                }
            }
            catch (Exception e)
            {
            }
            return cipher;
        }
        public static CipherList GetCipherListByUser(int userId)
        {
            CipherList ciphers = null;
            try
            {
                using (var dbConn = new MySqlConnection(_connectionString))
                {
                    dbConn.Open();
                    using (MySqlCommand dbCmd = dbConn.CreateCommand())
                    {
                        dbCmd.CommandText = "QEH_ciphersget";
                        dbCmd.CommandType = CommandType.StoredProcedure;
                        dbCmd.Parameters.AddWithValue("userId", userId);

                        using (var rdr = dbCmd.ExecuteReader())
                        {
                            ciphers = new CipherList();
                            while (rdr.Read())
                            {
                                Cipher cipher = new();
                                //cipher.CipherId = DataUtil.NullToZero(rdr["idcipher"]);
                                cipher.UserId = DataUtil.NullToZero(rdr["iduser"]);
                                cipher.SerialNumber = DataUtil.NullToEmpty(rdr["serialnumber"]);
                                cipher.StartingPoint = DataUtil.NullToZero(rdr["startpoint"]);
                                cipher.CipherString = DataUtil.NullToEmpty(rdr["cipherstring"]);
                                cipher.CreatedDateTime = DataUtil.NullToDateTimeMinValue(rdr["createdatetime"]);

                                ciphers.Ciphers.Add(cipher);
                            }
                        }
                    }
                    if (dbConn.State == ConnectionState.Open)
                        dbConn.Close();
                }
            }
            catch (Exception e)
            {
            }
            return ciphers;
        }
        public static int SaveCipher(Cipher cipher)
        {
            var newCipherId = 0;
            try
            {
                using (var dbConn = new MySqlConnection(_connectionString))
                {
                    int cipherId = 0;
                    dbConn.Open();
                    using (MySqlCommand dbCmd = dbConn.CreateCommand())
                    {
                        dbCmd.CommandText = "QEH_ciphersave";
                        dbCmd.CommandType = CommandType.StoredProcedure;
                        dbCmd.Parameters.AddWithValue("UserId", cipher.UserId);
                        dbCmd.Parameters.AddWithValue("SerialNumber", cipher.SerialNumber);
                        dbCmd.Parameters.AddWithValue("StartPoint", cipher.StartingPoint);
                        dbCmd.Parameters.AddWithValue("CipherString", cipher.CipherString);
                        var newCipherIdObj = dbCmd.ExecuteScalar();
                        newCipherId = Convert.ToInt32(newCipherIdObj);
                    }
                    if (dbConn.State == ConnectionState.Open)
                        dbConn.Close();
                }
            }
            catch (Exception e)
            {
            }
            return newCipherId;

        }

        public static int SendCipher(CipherSend s)
        {
            var newCipherSendId = 0;
            try
            {

                using (var dbConn = new MySqlConnection(_connectionString))
                {
                    dbConn.Open();
                    using (MySqlCommand dbCmd = dbConn.CreateCommand())
                    {
                        dbCmd.CommandText = "QEH_ciphersend";
                        dbCmd.CommandType = CommandType.StoredProcedure;
                        dbCmd.Parameters.AddWithValue("UserId", s.SenderUserId);
                        dbCmd.Parameters.AddWithValue("RecipientUserId", s.RecipientUserId);
                        dbCmd.Parameters.AddWithValue("CipherId", s.CipherId);
                        dbCmd.Parameters.AddWithValue("StartPoint", s.StartingPoint);
                        var newCipherSendIdObj = dbCmd.ExecuteScalar();
                        newCipherSendId = Convert.ToInt32(newCipherSendIdObj);
                    }
                    if (dbConn.State == ConnectionState.Open)
                        dbConn.Close();
                }
            }
            catch (Exception e)
            {
            }

            return newCipherSendId;

        }
        public static Cipher GetCipherFromSend(int cipherSendId)
        {
            Cipher cipher = null;
            try
            {
                using (var dbConn = new MySqlConnection(_connectionString))
                {
                    dbConn.Open();
                    using (MySqlCommand dbCmd = dbConn.CreateCommand())
                    {
                        dbCmd.CommandText = "QEH_sendcipherIdget";
                        dbCmd.CommandType = CommandType.StoredProcedure;
                        dbCmd.Parameters.AddWithValue("sendId", cipherSendId);

                        using (var rdr = dbCmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                cipher = new();
                                //cipher.CipherId = DataUtil.NullToZero(rdr["idcipher"]);
                                cipher.CipherId = 3; //TODO: Remove this hard-code
                                cipher.UserId = DataUtil.NullToZero(rdr["iduser"]);
                                cipher.SerialNumber = DataUtil.NullToEmpty(rdr["serialnumber"]);
                                cipher.StartingPoint = DataUtil.NullToZero(rdr["startpoint"]);
                                cipher.CipherString = DataUtil.NullToEmpty(rdr["cipherstring"]);
                                cipher.CreatedDateTime = DataUtil.NullToDateTimeMinValue(rdr["createdatetime"]);
                            }
                        }
                    }
                    if (dbConn.State == ConnectionState.Open)
                        dbConn.Close();
                }
            }
            catch (Exception e)
            {
            }
            return cipher;
        }
        //TODO: Needs to return cipher id
        public static int SaveSendCipherStatus(CipherAcceptDeny a)
        {
            try
            {
                using (var dbConn = new MySqlConnection(_connectionString))
                {
                    dbConn.Open();
                    using (MySqlCommand dbCmd = dbConn.CreateCommand())
                    {
                        dbCmd.CommandText = "QEH_AcceptDenyStatusSave";
                        dbCmd.CommandType = CommandType.StoredProcedure;
                        dbCmd.Parameters.AddWithValue("idCipher", a.CipherSendRequestId);
                        dbCmd.Parameters.AddWithValue("status", a.AcceptDeny);
                        //SqlParameter newOrderID = dbCmd.Parameters.Add("NewOrderID", SqlDbType.Int);
                        //newOrderID.Direction = ParameterDirection.Output;
                        dbCmd.ExecuteNonQuery();
                        //orderID = Helpers.NullToZero((int)dbCmd.Parameters["NewOrderID"].Value);
                    }
                    if (dbConn.State == ConnectionState.Open)
                        dbConn.Close();
                }
            }
            catch (Exception e)
            {
            }
            return -1;
        }

        #endregion Public Methods

        #region Private Functions


        #endregion Private Functions
    }
}
