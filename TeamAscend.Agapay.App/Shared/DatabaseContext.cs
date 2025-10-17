using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamAscend.Agapay.App.Model;
using TeamAscend.Agapay.App.Models;

namespace TeamAscend.Agapay.App.Shared
{
    /// <summary>
    /// Centralized Class for handing local SQLite Database things for the App
    /// Loaded in MauiProgram as Singleton (one instance only within the App)
    /// </summary>
    public class DatabaseContext
    {
        SQLiteConnection database;
        public static DatabaseContext Instance { set; get; }
        public DatabaseContext()
        {
            //init from constructor
            DatabaseContext.Instance = this;
        }

        /// <summary>
        /// Initialize Database Availability
        /// </summary>
        /// <returns></returns>
        public void Init()
        {
            if (database is not null)
                return;

            database = new SQLiteConnection(Constants.DatabasePath, Constants.Flags);
            database.CreateTable<AppUser>();
            database.CreateTable<AppBlogPost>();
            database.CreateTable<AppMapLocation>();
            database.CreateTable<AppPhonebook>();
            database.CreateTable<AppGoPlan>();  // Add this
            database.CreateTable<AppGoBag>();   // Add this
        }

        // AppUser CRUD
        public List<AppUser> Users
        {
            get { 
                Init();
                return database.Table<AppUser>().ToList();
            }
        }

        public int SaveUser(AppUser incoming)
        {
            Init();
            return database.Insert(incoming);//insert new
        }

        public int UpdateUser(AppUser incoming)
        {
            Init();
            return database.Update(incoming);//update existing
        }

        public int DeleteUser(AppUser incoming)
        {
            Init();
            return database.Delete(incoming);
        }

        // AppBlogPost CRUD
        public List<AppBlogPost> BlogPosts
        {
            get
            {
                Init();
                return database.Table<AppBlogPost>().ToList();
            }
        }

        public int SaveBlogPost(AppBlogPost incoming)
        {
            Init();
            return database.Insert(incoming);//insert new
        }

        public int UpdateBlogPost(AppBlogPost incoming)
        {
            Init();
            return database.Update(incoming);//update existing
        }

        // AppMapLocation CRUD
        public List<AppMapLocation> MapLocations
        {
            get
            {
                Init();
                return database.Table<AppMapLocation>().ToList();
            }
        }

        public int SaveMapLocation(AppMapLocation incoming)
        {
            Init();
            return database.Insert(incoming);//insert new
        }

        public int UpdateMapLocation(AppMapLocation incoming)
        {
            Init();
            return database.Update(incoming);//update existing
        }

        // AppPhonebook CRUD
        public  List<AppPhonebook> PhonebookEntries
        {            
            get
            {
                Init();
                return database.Table<AppPhonebook>().ToList();
            }
        }

        public int SavePhonebookEntry(AppPhonebook incoming)
        {
            Init();
            return database.Insert(incoming);//insert new
        }

        public int UpdatePhonebookEntry(AppPhonebook incoming)
        {
            Init();
            return database.Update(incoming);//update existing
        }

        public int DeletePhonebookEntry(AppPhonebook incoming)
        {
            Init();
            return database.Delete(incoming);
        }

        // Add AppGoPlan CRUD operations
        public List<AppGoPlan> AppGoPlans
        {
            get
            {
                Init();
                return database.Table<AppGoPlan>().ToList();
            }
        }

        public int SaveAppGoPlan(AppGoPlan incoming)
        {
            Init();
            return database.Insert(incoming);
        }

        public int UpdateAppGoPlan(AppGoPlan incoming)
        {
            Init();
            return database.Update(incoming);
        }

        public int DeleteAppGoPlan(AppGoPlan incoming)
        {
            Init();
            return database.Delete(incoming);
        }

        // Add AppGoBag CRUD operations
        public List<AppGoBag> AppGoBags
        {
            get
            {
                Init();
                return database.Table<AppGoBag>().ToList();
            }
        }

        public int SaveAppGoBag(AppGoBag incoming)
        {
            Init();
            return database.Insert(incoming);
        }

        public int UpdateAppGoBag(AppGoBag incoming)
        {
            Init();
            return database.Update(incoming);
        }

        public int DeleteAppGoBag(AppGoBag incoming)
        {
            Init();
            return database.Delete(incoming);
        }
    }
}
