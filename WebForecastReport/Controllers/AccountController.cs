﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Models;
using WebForecastReport.Service;

namespace WebForecastReport.Controllers
{
    public class AccountController : Controller
    {
        private IUser Users;
        string user = "";
        string dep = "";
        byte[] image = new byte[0];
        public AccountController()
        {
            Users = new UserService();
        }
        public IActionResult Index()
        {
            return View(new LoginModel());
        }
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.user == null)
                {
                    ModelState.AddModelError("Password", "Invalid login attempt.");
                    return View("Index");
                }
                else
                {
                    bool check = ActiveDirectoryAuthenticate(model.user, model.password);
                    if (check)
                    {
                        //update user in sale_user table
                        Users.InsertUser();

                        bool b = true;
                        //SqlCommand cmd = new SqlCommand("SELECT * FROM Authen_TripExpenseWeb WHERE Names='" + user + "'", ConnectSQL.OpenConnect());
                        //SqlDataReader dr = cmd.ExecuteReader();
                        //if (dr.HasRows)
                        //{
                        //    b = true;
                        //}
                        //dr.Close();
                        //ConnectSQL.CloseConnect();
                        if (b)
                        {
                            HttpContext.Session.SetString("userId", user);
                            HttpContext.Session.SetString("Department", dep);
                            HttpContext.Session.Set("Image", image);
                            HttpContext.Session.SetString("Login_MES", "1234");
                            return RedirectToAction("Index", "Main");
                        }
                        else
                        {
                            ModelState.AddModelError("Password", "Not Authorization!!!");
                            return View("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Invalid login attempt.");
                        return View("Index");
                    }
                }
            }
            else
            {
                return View("Login");
            }
        }
        public bool ActiveDirectoryAuthenticate(string username, string password)
        {
            bool userOk = false;
            try
            {
                using (DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://192.168.15.1", username, password))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(directoryEntry))
                    {
                        searcher.Filter = "(samaccountname=" + username + ")";
                        searcher.PropertiesToLoad.Add("displayname");
                        searcher.PropertiesToLoad.Add("thumbnailPhoto");
                        searcher.PropertiesToLoad.Add("department");

                        SearchResult adsSearchResult = searcher.FindOne();

                        if (adsSearchResult != null)
                        {

                            var prop = adsSearchResult.Properties["thumbnailPhoto"];
                            if (adsSearchResult.Properties["displayname"].Count == 1)
                            {
                                user = (string)adsSearchResult.Properties["displayname"][0];
                                dep = (string)adsSearchResult.Properties["department"][0];
                                var img = adsSearchResult.Properties["thumbnailPhoto"].Count;
                                if (img > 0)
                                {
                                    image = adsSearchResult.Properties["thumbnailPhoto"][0] as byte[];
                                }
                            }
                            userOk = true;
                        }

                        return userOk;
                    }
                }
            }
            catch
            {
                return userOk;
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Account");
        }
    }
}
