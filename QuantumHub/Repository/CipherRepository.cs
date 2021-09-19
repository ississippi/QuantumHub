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
        //static string _connectionString = @"server=localhost;userid=root;password=Siberia$111;database=quantumencrypt";
        static string _connectionString = @"server=127.0.0.1;port=54794;database=quantumencrypt;user=azure;password=6#vWHD_$;";
        #region Public Methods

        public static Cipher GetCipher(int userId = 0, int cipherId = 0)
        {
            Cipher cipher = null;
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
                            cipher = new Cipher();
                            cipher.CipherId = DataUtil.NullToZero(rdr["idcipher"]);
                            cipher.UserId = DataUtil.NullToZero(rdr["iduser"]);
                            cipher.SerialNumber = DataUtil.NullToEmpty(rdr["serialnumber"]);
                            cipher.StartingPoint = DataUtil.NullToZero(rdr["startpoint"]);
                            cipher.CipherString = DataUtil.NullToEmpty(rdr["cipherstring"]);
                            cipher.CreatedDateTime = DataUtil.NullToDateTimeMinValue(rdr["createdatetime"]);
                            cipher.MaxEncryptionLength = DataUtil.NullToZero(rdr["maxencryptionlength"]);
                        }
                    }
                }
                if (dbConn.State == ConnectionState.Open)
                    dbConn.Close();
            }
            return cipher;
        }
        public static CipherList GetCipherListByUser(int userId)
        {
            CipherList ciphers = null;
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
                            Cipher cipher = new Cipher();
                            cipher.CipherId = DataUtil.NullToZero(rdr["idcipher"]);
                            cipher.UserId = DataUtil.NullToZero(rdr["iduser"]);
                            cipher.SerialNumber = DataUtil.NullToEmpty(rdr["serialnumber"]);
                            cipher.StartingPoint = DataUtil.NullToZero(rdr["startpoint"]);
                            cipher.CipherString = DataUtil.NullToEmpty(rdr["cipherstring"]);
                            cipher.CreatedDateTime = DataUtil.NullToDateTimeMinValue(rdr["createdatetime"]);
                            cipher.MaxEncryptionLength = DataUtil.NullToZero(rdr["maxencryptionlength"]);

                            ciphers.Ciphers.Add(cipher);
                        }
                    }
                }
                if (dbConn.State == ConnectionState.Open)
                    dbConn.Close();
            }

            return ciphers;
        }
        public static int SaveCipher(Cipher cipher)
        {
            var newCipherId = 0;
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
                    dbCmd.Parameters.AddWithValue("MaxEncryptionLength", cipher.MaxEncryptionLength);
                    var newCipherIdObj = dbCmd.ExecuteScalar();
                    newCipherId = Convert.ToInt32(newCipherIdObj);
                }
                if (dbConn.State == ConnectionState.Open)
                    dbConn.Close();
            }
            return newCipherId;

        }

        public static int SendCipher(CipherSend s)
        {
            var newCipherSendId = 0;
            var maxEncryptLength = 0;
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
                                cipher = new Cipher();
                                //cipher.CipherId = DataUtil.NullToZero(rdr["idcipher"]);
                                cipher.CipherId = 3; //TODO: Remove this hard-code
                                cipher.UserId = DataUtil.NullToZero(rdr["iduser"]);
                                cipher.SerialNumber = DataUtil.NullToEmpty(rdr["serialnumber"]);
                                cipher.StartingPoint = DataUtil.NullToZero(rdr["startpoint"]);
                                cipher.CipherString = DataUtil.NullToEmpty(rdr["cipherstring"]);
                                cipher.CreatedDateTime = DataUtil.NullToDateTimeMinValue(rdr["createdatetime"]);
                                cipher.MaxEncryptionLength = DataUtil.NullToZero(rdr["maxencryptionlength"]);
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
                        dbCmd.Parameters.AddWithValue("status", a.AcceptDeny.ToLower());
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

        public static string getConnectionString()
        {
#if DEBUG
            return @"server=localhost;userid=root;password=Siberia$111;database=quantumencrypt";
#else
            string connectionString = Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb");

            string[] options = connectionString.Split(";");
            string database = options[0].Split("=")[1]; ;
            string serverport = options[1].Split("=")[1];
            string server = serverport.Split(":")[0];
            string port = serverport.Split(":")[1];
            string user = options[2].Split("=")[1];
            string password = options[3].Split("=")[1]; ;

            connectionString = $"server={server};port={port};database={database};user={user};password={password};";

            return connectionString;
#endif
        }

        #endregion Private Functions
    }
}
