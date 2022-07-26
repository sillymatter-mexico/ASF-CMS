using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using System.Collections.Specialized;
using MySql.Data.MySqlClient;
namespace asf.cms.membership
{
    public class MySQLSessionProvider// : SessionStateStoreProviderBase
    {
 /*       private int sessionTimeout = 10;
        private DateTime lastCleanDate;

        private string connectionString;
        private string eventSource = "SessionProvider";
        private string eventLog = "Application";
        private string exceptionMessage =
          "An exception occurred. Please contact your administrator.";
        private string pApplicationName;


        private bool pWriteExceptionsToEventLog = false;

        public bool WriteExceptionsToEventLog
        {
            get { return pWriteExceptionsToEventLog; }
            set { pWriteExceptionsToEventLog = value; }
        }

        public string ApplicationName
        {
            get { return pApplicationName; }
        }


        public override void Initialize(string name, NameValueCollection config)
        {

            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "SessionProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Sample ODBC Session State Store provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);



            //
            // Get <sessionState> configuration element.
            //
            //            string stt = AppSettingsProxy.GetAppSetting("sessionTimeout");
            //          if (!string.IsNullOrEmpty(stt))
            //            int.TryParse(stt, out sessionTimeout);

            if (sessionTimeout == 0)
                sessionTimeout = 10;

            pWriteExceptionsToEventLog = false;

            if (config["writeExceptionsToEventLog"] != null)
            {
                if (config["writeExceptionsToEventLog"].ToUpper() == "TRUE")
                    pWriteExceptionsToEventLog = true;
            }


        }

        /// <summary>
        /// Cleans old sessions
        /// </summary>
        /// <param name="conn"></param>
        private void CleanOldSessions(MySqlConnection conn)
        {
            if (conn == null || conn.State != ConnectionState.Open)
                return;

            MySqlCommand cleanDeleteCmd = null;
            TimeSpan twoweeks = new TimeSpan(15, 0, 0, 0);

            // OdbcCommand to clear an existing expired session if it exists.
            cleanDeleteCmd = new MySqlCommand("DELETE FROM sessions " +
                "WHERE Expires < ?Expires", conn);
            cleanDeleteCmd.Parameters.Add
              ("?Expires", MySqlDbType.DateTime).Value = DateTime.Now.Subtract(twoweeks);

            cleanDeleteCmd.ExecuteNonQuery();

            lastCleanDate = DateTime.Now;

        }


        //
        // SessionStateStoreProviderBase members
        //

        public override void Dispose()
        {
        }


        //
        // SessionStateProviderBase.SetItemExpireCallback
        //

        public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
        {
            return false;
        }


        //
        // SessionStateProviderBase.SetAndReleaseItemExclusive
        //

        public override void SetAndReleaseItemExclusive(HttpContext context,
          string id,
          SessionStateStoreData item,
          object lockId,
          bool newItem)
        {
            // Serialize the SessionStateItemCollection as a string.
            string sessItems = Serialize((SessionStateItemCollection)item.Items);
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand cmd = null;
            MySqlCommand deleteCmd = null;

            if (newItem)
            {
                // OdbcCommand to clear an existing expired session if it exists.
                deleteCmd = new MySqlCommand("DELETE FROM sessions " +
                    "WHERE SessionId = ?SessionId AND ApplicationName = ?ApplicationName AND Expires < ?Expires", conn);
                deleteCmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
                deleteCmd.Parameters.Add
                  ("?ApplicationName", MySqlDbType.VarChar, 255).Value = ApplicationName;
                deleteCmd.Parameters.Add
                  ("?Expires", MySqlDbType.Datetime).Value = DateTime.Now;

                CreateNewSession(conn, ref cmd, id, ApplicationName, item.Timeout, sessItems);
            }
            else
            {
                // OdbcCommand to update the existing session item.
                cmd = new MySqlCommand(
                  "UPDATE sessions SET Expires = ?Expires, SessionItems = ?SessionItems, Locked = ?Locked " +
                  " WHERE SessionId = ?SessionId AND ApplicationName = ?ApplicationName AND LockId = ?LockId", conn);
                cmd.Parameters.Add("?Expires", MySqlDbType.Datetime).Value =
                  DateTime.Now.AddMinutes((Double)item.Timeout);
                cmd.Parameters.Add("?SessionItems",
                  MySqlDbType.VarChar, sessItems.Length).Value = sessItems;
                cmd.Parameters.Add("?Locked", MySqlDbType.Bit).Value = false;
                cmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
                cmd.Parameters.Add("?ApplicationName", MySqlDbType.VarChar,
                  255).Value = ApplicationName;
                cmd.Parameters.Add("?LockId", MySqlDbType.Int32).Value = lockId;
            }

            try
            {
                conn.Open();

                TimeSpan tendays = new TimeSpan(10, 0, 0, 0);
                if (lastCleanDate != null && (lastCleanDate.Add(tendays) < DateTime.Now))
                    CleanOldSessions(conn);

                if (deleteCmd != null)
                    deleteCmd.ExecuteNonQuery();

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "SetAndReleaseItemExclusive");
                    throw new ProviderException(exceptionMessage);
                }
                else
                    throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void CreateNewSession(MySqlConnection conn, ref MySqlCommand cmd, string id, string ApplicationName, Double timeout, string sessItems)
        {
            // OdbcCommand to insert the new session item.
            cmd = new MySqlCommand("INSERT INTO sessions " +
              " (SessionId, ApplicationName, Created, Expires, " +
              "  LockDate, LockId, Timeout, Locked, SessionItems, Flags) " +
              " Values(?SessionId, ?ApplicationName, ?Created, ?Expires, ?LockDate, ?LockId , ?Timeout, ?Locked, ?SessionItems, ?Flags)", conn);
            cmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
            cmd.Parameters.Add
              ("?ApplicationName", MySqlDbType.VarChar, 255).Value = ApplicationName;
            cmd.Parameters.Add
              ("?Created", MySqlDbType.Datetime).Value = DateTime.Now;
            cmd.Parameters.Add
              ("?Expires", MySqlDbType.Datetime).Value = DateTime.Now.AddMinutes(timeout);
            cmd.Parameters.Add
              ("?LockDate", MySqlDbType.Datetime).Value = DateTime.Now;
            cmd.Parameters.Add("?LockId", MySqlDbType.Int32).Value = 0;
            cmd.Parameters.Add
              ("?Timeout", MySqlDbType.Int32).Value = timeout;
            cmd.Parameters.Add("?Locked", MySqlDbType.Bit).Value = false;
            cmd.Parameters.Add
              ("?SessionItems", MySqlDbType.VarChar, sessItems.Length).Value = sessItems;
            cmd.Parameters.Add("?Flags", MySqlDbType.Int32).Value = 0;
        }


        //
        // SessionStateProviderBase.GetItem
        //

        public override SessionStateStoreData GetItem(HttpContext context,
          string id,
          out bool locked,
          out TimeSpan lockAge,
          out object lockId,
          out SessionStateActions actionFlags)
        {
            return GetSessionStoreItem(false, context, id, out locked,
              out lockAge, out lockId, out actionFlags);
        }


        //
        // SessionStateProviderBase.GetItemExclusive
        //

        public override SessionStateStoreData GetItemExclusive(HttpContext context,
          string id,
          out bool locked,
          out TimeSpan lockAge,
          out object lockId,
          out SessionStateActions actionFlags)
        {
            return GetSessionStoreItem(true, context, id, out locked,
              out lockAge, out lockId, out actionFlags);
        }


        //
        // GetSessionStoreItem is called by both the GetItem and 
        // GetItemExclusive methods. GetSessionStoreItem retrieves the 
        // session data from the data source. If the lockRecord parameter
        // is true (in the case of GetItemExclusive), then GetSessionStoreItem
        // locks the record and sets a new LockId and LockDate.
        //

        private SessionStateStoreData GetSessionStoreItem(bool lockRecord,
          HttpContext context,
          string id,
          out bool locked,
          out TimeSpan lockAge,
          out object lockId,
          out SessionStateActions actionFlags)
        {
            // Initial values for return value and out parameters.
            SessionStateStoreData item = null;
            lockAge = TimeSpan.Zero;
            lockId = null;
            locked = false;
            actionFlags = 0;

            // Mysql database connection.
            MySqlConnection conn = new MySqlConnection(connectionString);
            // MySqlCommand for database commands.
            MySqlCommand cmd = null;
            // DataReader to read database record.
            MySqlDataReader reader = null;
            // DateTime to check if current session item is expired.
            DateTime expires;
            // String to hold serialized SessionStateItemCollection.
            string serializedItems = "";
            // True if a record is found in the database.
            bool foundRecord = false;
            // True if the returned session item is expired and needs to be deleted.
            bool deleteData = false;
            // Timeout value from the data store.
            int timeout = 0;

            try
            {
                conn.Open();

                // lockRecord is true when called from GetItemExclusive and
                // false when called from GetItem.
                // Obtain a lock if possible. Ignore the record if it is expired.
                if (lockRecord)
                {
                    cmd = new MySqlCommand(
                      "UPDATE sessions SET" +
                      " Locked = ?Locked, LockDate = ?LockDate " +
                      " WHERE SessionId = ?SessionId AND ApplicationName = ?ApplicationName AND Locked = ?Locked2 AND Expires > ?Expires", conn);
                    cmd.Parameters.Add("?Locked", MySqlDbType.Bit).Value = true;
                    cmd.Parameters.Add("?LockDate", MySqlDbType.Datetime).Value
                      = DateTime.Now;
                    cmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
                    cmd.Parameters.Add("?ApplicationName", MySqlDbType.VarChar,
                      255).Value = ApplicationName;
                    cmd.Parameters.Add("?Locked2", MySqlDbType.Int32).Value = false;
                    cmd.Parameters.Add
                      ("?Expires", MySqlDbType.Datetime).Value = DateTime.Now;

                    if (cmd.ExecuteNonQuery() == 0)
                        // No record was updated because the record was locked or not found.
                        locked = true;
                    else
                        // The record was updated.

                        locked = false;
                }

                // Retrieve the current session item information.
                cmd = new MySqlCommand(
                  "SELECT Expires, SessionItems, LockId, LockDate, Flags, Timeout " +
                  "  FROM sessions " +
                  "  WHERE SessionId = ?SessionId AND ApplicationName = ?ApplicationName", conn);
                cmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
                cmd.Parameters.Add("?ApplicationName", MySqlDbType.VarChar,
                  255).Value = ApplicationName;

                // Retrieve session item data from the data source.
                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                while (reader.Read())
                {
                    expires = reader.GetDateTime(0);

                    if (expires < DateTime.Now)
                    {
                        // The record was expired. Mark it as not locked.
                        locked = false;
                        // The session was expired. Mark the data for deletion.
                        deleteData = true;
                    }
                    else
                        foundRecord = true;

                    serializedItems = reader.GetString(1);
                    lockId = reader.GetInt32(2);
                    lockAge = DateTime.Now.Subtract(reader.GetDateTime(3));
                    actionFlags = (SessionStateActions)reader.GetInt32(4);
                    timeout = reader.GetInt32(5);
                }
                reader.Close();


                // If the returned session item is expired, 
                // delete the record from the data source.
                if (deleteData)
                {
                    cmd = new MySqlCommand("DELETE FROM sessions " +
                      "WHERE SessionId = ?SessionId AND ApplicationName = ?ApplicationName", conn);
                    cmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
                    cmd.Parameters.Add("?ApplicationName", MySqlDbType.VarChar,
                      255).Value = ApplicationName;

                    cmd.ExecuteNonQuery();
                }

                // The record was not found. Ensure that locked is false.
                if (!foundRecord)
                    locked = false;

                // If the record was found and you obtained a lock, then set 
                // the lockId, clear the actionFlags,
                // and create the SessionStateStoreItem to return.
                if (foundRecord && !locked)
                {
                    lockId = (int)lockId + 1;

                    cmd = new MySqlCommand("UPDATE sessions SET" +
                      " LockId = ?LockId, Flags = 0 " +
                      " WHERE SessionId = ?SessionId AND ApplicationName = ?ApplicationName", conn);
                    cmd.Parameters.Add("?LockId", MySqlDbType.Int32).Value = lockId;
                    cmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
                    cmd.Parameters.Add("?ApplicationName", MySqlDbType.VarChar, 255).Value = ApplicationName;

                    cmd.ExecuteNonQuery();

                    // If the actionFlags parameter is not InitializeItem, 
                    // deserialize the stored SessionStateItemCollection.
                    if (actionFlags == SessionStateActions.InitializeItem)
                        item = CreateNewStoreData(context, sessionTimeout);
                    else
                        item = Deserialize(context, serializedItems, timeout);
                }
            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetSessionStoreItem");
                    throw new ProviderException(exceptionMessage);
                }
                else
                    throw e;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return item;
        }




        //
        // Serialize is called by the SetAndReleaseItemExclusive method to 
        // convert the SessionStateItemCollection into a Base64 string to    
        // be stored in an Access Memo field.
        //

        private string Serialize(SessionStateItemCollection items)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);

            if (items != null)
                items.Serialize(writer);

            writer.Close();

            return Convert.ToBase64String(ms.ToArray());
        }

        //
        // DeSerialize is called by the GetSessionStoreItem method to 
        // convert the Base64 string stored in the Access Memo field to a 
        // SessionStateItemCollection.
        //

        private SessionStateStoreData Deserialize(HttpContext context,
          string serializedItems, int timeout)
        {
            MemoryStream ms =
              new MemoryStream(Convert.FromBase64String(serializedItems));

            SessionStateItemCollection sessionItems =
              new SessionStateItemCollection();

            if (ms.Length > 0)
            {
                BinaryReader reader = new BinaryReader(ms);
                sessionItems = SessionStateItemCollection.Deserialize(reader);
            }

            return new SessionStateStoreData(sessionItems,
              SessionStateUtility.GetSessionStaticObjects(context),
              timeout);
        }

        //
        // SessionStateProviderBase.ReleaseItemExclusive
        //

        public override void ReleaseItemExclusive(HttpContext context,
          string id,
          object lockId)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand cmd =
              new MySqlCommand("UPDATE sessions SET Locked = 0, Expires = ?Expires " +
              "WHERE SessionId = ?SessionId AND ApplicationName = ?ApplicationName AND LockId = ?LockId", conn);
            cmd.Parameters.Add("?Expires", MySqlDbType.Datetime).Value =
              DateTime.Now.AddMinutes(sessionTimeout);
            cmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
            cmd.Parameters.Add("?ApplicationName", MySqlDbType.VarChar,
              255).Value = ApplicationName;
            cmd.Parameters.Add("?LockId", MySqlDbType.Int32).Value = lockId;

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ReleaseItemExclusive");
                    throw new ProviderException(exceptionMessage);
                }
                else
                    throw e;
            }
            finally
            {
                conn.Close();
            }
        }


        //
        // SessionStateProviderBase.RemoveItem
        //

        public override void RemoveItem(HttpContext context,
          string id,
          object lockId,
          SessionStateStoreData item)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand("DELETE FROM sessions " +
              "WHERE SessionId = ?SessionId AND ApplicationName = ?ApplicationName AND LockId = ?LockId", conn);
            cmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
            cmd.Parameters.Add("?ApplicationName", MySqlDbType.VarChar,
              255).Value = ApplicationName;
            cmd.Parameters.Add("?LockId", MySqlDbType.Int32).Value = lockId;

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "RemoveItem");
                    throw new ProviderException(exceptionMessage);
                }
                else
                    throw e;
            }
            finally
            {
                conn.Close();
            }
        }



        //
        // SessionStateProviderBase.CreateUninitializedItem
        //

        public override void CreateUninitializedItem(HttpContext context,
          string id,
          int timeout)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand("INSERT INTO sessions " +
              " (SessionId, ApplicationName, Created, Expires, " +
              "  LockDate, LockId, Timeout, Locked, SessionItems, Flags) " +
              " Values(?SessionId, ?ApplicationName, ?Created, ?Expires, ?LockDate, ?LockId , ?Timeout, ?Locked, ?SessionItems, ?Flags)", conn);
            cmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
            cmd.Parameters.Add("?ApplicationName", MySqlDbType.VarChar,
              255).Value = ApplicationName;
            cmd.Parameters.Add("?Created", MySqlDbType.Datetime).Value
              = DateTime.Now;
            cmd.Parameters.Add("?Expires", MySqlDbType.Datetime).Value
              = DateTime.Now.AddMinutes((Double)timeout);
            cmd.Parameters.Add("?LockDate", MySqlDbType.Datetime).Value
              = DateTime.Now;
            cmd.Parameters.Add("?LockId", MySqlDbType.Int32).Value = 0;
            cmd.Parameters.Add("?Timeout", MySqlDbType.Int32).Value = timeout;
            cmd.Parameters.Add("?Locked", MySqlDbType.Bit).Value = false;
            cmd.Parameters.Add("?SessionItems", MySqlDbType.VarChar, 0).Value = "";
            cmd.Parameters.Add("?Flags", MySqlDbType.Int32).Value = 1;

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();
            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "CreateUninitializedItem");
                    throw new ProviderException(exceptionMessage);
                }
                else
                    throw e;
            }
            finally
            {
                conn.Close();
            }
        }


        //
        // SessionStateProviderBase.CreateNewStoreData
        //

        public override SessionStateStoreData CreateNewStoreData(
          HttpContext context,
          int timeout)
        {
            return new SessionStateStoreData(new SessionStateItemCollection(),
              SessionStateUtility.GetSessionStaticObjects(context),
              timeout);
        }

        //
        // SessionStateProviderBase.ResetItemTimeout
        //

        public override void ResetItemTimeout(HttpContext context,
                                              string id)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand cmd =
              new MySqlCommand("UPDATE sessions SET Expires = ?Expires " +
              "WHERE SessionId = ?SessionId AND ApplicationName = ?ApplicationName", conn);
            cmd.Parameters.Add("?Expires", MySqlDbType.Datetime).Value
              = DateTime.Now.AddMinutes(sessionTimeout);
            cmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
            cmd.Parameters.Add("?ApplicationName", MySqlDbType.VarChar,
              255).Value = ApplicationName;

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ResetItemTimeout");
                    throw new ProviderException(exceptionMessage);
                }
                else
                    throw e;
            }
            finally
            {
                conn.Close();
            }
        }


        //
        // SessionStateProviderBase.InitializeRequest
        //

        public override void InitializeRequest(HttpContext context)
        {
        }


        //
        // SessionStateProviderBase.EndRequest
        //

        public override void EndRequest(HttpContext context)
        {
        }

        */
    }
}
