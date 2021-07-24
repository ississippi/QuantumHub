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
    public static class NotificationsRepository
    {
        //static string _connectionString = @"server=localhost;userid=root;password=Siberia$111;database=quantumencrypt";
        static string _connectionString = @"Database = quantumencrypt;Data Source = 127.0.0.1; User Id = azure; Password=6#vWHD_$";

        #region Public Methods

        public static CipherSendList GetNotifications(int recipientUserId)
        {
            CipherSendList notifications = null;
            try
            {
                using (var dbConn = new MySqlConnection(_connectionString))
                {
                    dbConn.Open();
                    using (MySqlCommand dbCmd = dbConn.CreateCommand())
                    {
                        dbCmd.CommandText = "QEH_CipherSendRequestsGet";
                        dbCmd.CommandType = CommandType.StoredProcedure;
                        dbCmd.Parameters.AddWithValue("recipientUserId", recipientUserId);

                        using (var rdr = dbCmd.ExecuteReader())
                        {
                            notifications = new CipherSendList();
                            while (rdr.Read())
                            {
                                CipherSend n = new CipherSend();
                                n.CipherSendId = DataUtil.NullToZero(rdr["idcipher_send"]);
                                n.SenderUserId = DataUtil.NullToZero(rdr["idsender"]);
                                n.RecipientUserId = DataUtil.NullToZero(rdr["idrecipient"]);
                                n.CipherId = DataUtil.NullToZero(rdr["idcipher"]);
                                n.StartingPoint = DataUtil.NullToZero(rdr["startpoint"]);
                                n.AcceptDenyStatus = DataUtil.NullToEmpty(rdr["acceptdenystatus"]);
                                n.CreateDate = DataUtil.NullToDateTimeMinValue(rdr["createdate"]);

                                notifications.SendRequests.Add(n);
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
            return notifications;
        }

        #endregion Public Methods

        #region Private Functions


        #endregion Private Functions
    }
}
