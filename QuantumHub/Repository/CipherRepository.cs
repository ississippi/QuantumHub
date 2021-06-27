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

        public static Cipher GetCipher(int userId, string serialNo)
        {

            return null;
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
            try
            {
                using (var dbConn = new MySqlConnection(_connectionString))
                {
                    dbConn.Open();
                    using (MySqlCommand dbCmd = dbConn.CreateCommand())
                    {
                        dbCmd.CommandText = "QEH_ciphersave";
                        dbCmd.CommandType = CommandType.StoredProcedure;
                        dbCmd.Parameters.AddWithValue("UserId", cipher.UserId);
                        dbCmd.Parameters.AddWithValue("SerialNumber", cipher.SerialNumber);
                        dbCmd.Parameters.AddWithValue("StartPoint", cipher.StartingPoint);
                        dbCmd.Parameters.AddWithValue("CipherString", cipher.CipherString);
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
