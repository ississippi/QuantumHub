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

        #region Public Methods

        public static Cipher GetCipher(int userId, string serialNo)
        {

            return null;
        }
        public static CipherList GetCipherListByUser(int userId)
        {
            CipherList ciphers = null;
            var connectionString = @"server=localhost:3306;userid=root;password=;database=QuantumEncrypt";
            try
            {
                using (var dbConn = new MySqlConnection(connectionString))
                {
                    dbConn.Open();
                    using (MySqlCommand dbCmd = dbConn.CreateCommand())
                    {
                        dbCmd.CommandText = "TTH_ProductGet";
                        dbCmd.CommandType = CommandType.StoredProcedure;

                        using (var rdr = dbCmd.ExecuteReader())
                        {
                            ciphers = new CipherList();
                            while (rdr.Read())
                            {
                                Cipher cipher = new();
                                cipher.UserId = DataUtil.NullToZero(rdr["UserId"]);
                                cipher.SerialNumber = DataUtil.NullToEmpty(rdr["SerialNumber"]);
                                cipher.StartingPoint = DataUtil.NullToZero(rdr["StartPoint"]);
                                cipher.CipherString = DataUtil.NullToEmpty(rdr["CipherString"]);

                                ciphers.Cipher.Add(cipher);
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
            var cipherId = -1;

            return cipherId;
        }

        #endregion Public Methods

        #region Private Functions


        #endregion Private Functions
    }
}
