using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamAscend.Agapay.App.Shared
{
    /// <summary>
    /// Centralized Class for global constants (unchangable values)
    /// </summary>
    public static class Constants
    {
        //public const string AgapayWebAPI_URL = "https://localhost:44352/";
        public const string AgapayWebAPI_URL = "https://q4m7d5v7-44352.asse.devtunnels.ms/";


        /// <summary>
        /// Name for the SQLite DB created in the device
        /// </summary>
        public const string DatabaseFilename = "applocal.db";


        /// <summary>
        /// Flags or Settings on what the SQLite DB can do
        /// </summary>
        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;


        /// <summary>
        /// Property that returns the Database Location Path
        /// </summary>
        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }
}
