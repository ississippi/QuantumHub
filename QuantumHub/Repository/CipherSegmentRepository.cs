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
    public static class CipherSegmentRepository
    {
        //static string _connectionString = @"server=localhost;userid=root;password=Siberia$111;database=quantumencrypt";
        static string _connectionString = @"server=127.0.0.1;port=54794;database=quantumencrypt;user=azure;password=6#vWHD_$;";
        #region Public Methods

        public static CipherSerials GetSerialNumbers(int userId)
        {
            CipherSerials serials = null;
            try
            {
                using (var dbConn = new MySqlConnection(_connectionString))
                {
                    dbConn.Open();
                    using (MySqlCommand dbCmd = dbConn.CreateCommand())
                    {
                        dbCmd.CommandText = "QEH_CipherSerialNumberGet";
                        dbCmd.CommandType = CommandType.StoredProcedure;
                        dbCmd.Parameters.AddWithValue("userId", userId);

                        using (var rdr = dbCmd.ExecuteReader())
                        {
                            serials = new CipherSerials();
                            while (rdr.Read())
                            {
                                //var userId = DataUtil.NullToZero(rdr["userId"]);
                                //var activated = DataUtil.NullToEmpty(rdr["activated"]);
                                //var created = DataUtil.NullToDateTimeMinValue(rdr["createdatetime"]);
                                var serial = DataUtil.NullToEmpty(rdr["serialnumber"]);
                                if (!string.IsNullOrEmpty(serial))
                                {
                                    serials.SerialNumbers.Add(serial);
                                }
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
            return serials;
        }
        public static void SaveSerials(int userId, CipherSerials serials)
        {
            try
            {
                using (var dbConn = new MySqlConnection(_connectionString))
                {
                    dbConn.Open();
                    using (MySqlCommand dbCmd = dbConn.CreateCommand())
                    {
                        dbCmd.CommandText = "QEH_CipherSerialNumberSave";
                        dbCmd.CommandType = CommandType.StoredProcedure;
                        foreach (var serialNumber in serials.SerialNumbers)
                        {
                            dbCmd.Parameters.AddWithValue("userId", userId);
                            dbCmd.Parameters.AddWithValue("serialNumber", serialNumber);
                            dbCmd.Parameters.AddWithValue("activated", "N");
                            dbCmd.ExecuteScalar();
                        }
                    }
                    if (dbConn.State == ConnectionState.Open)
                        dbConn.Close();
                }
            }
            catch (Exception e)
            {
            }
        }

        public static void DeleteSerials(int userId)
        {
            try
            {
                using (var dbConn = new MySqlConnection(_connectionString))
                {
                    dbConn.Open();
                    using (MySqlCommand dbCmd = dbConn.CreateCommand())
                    {
                        dbCmd.CommandText = "QEH_CipherSerialNumberDelete";
                        dbCmd.CommandType = CommandType.StoredProcedure;
                        dbCmd.Parameters.AddWithValue("userId", userId);
                        dbCmd.ExecuteScalar();
                    }
                    if (dbConn.State == ConnectionState.Open)
                        dbConn.Close();
                }
            }
            catch (Exception e)
            {
            }
        }

        #endregion Public Methods

        #region Private Functions


        #endregion Private Functions
    }
}

