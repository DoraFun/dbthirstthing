using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace dbthirstthing.Controllers
{
    public class OAuthController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                var url = Request.Url.Query;

                if (!string.IsNullOrEmpty(url))
                {
                    string queryString = url.ToString();
                    char[] delimiterChars = { '=' };
                    string[] words = queryString.Split(delimiterChars);
                    string code = words.Length > 1 ? words[1] : null;

                    if (!string.IsNullOrEmpty(code))
                    {
                        //get the access token
                        string client_id = "502661025869-mil445cj0bffmbhb4sf875vephto26v7.apps.googleusercontent.com";
                        string client_secret = "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";
                        string redirect_uri = "https://localhost:44368/signin-google";
                        string access_token;

                        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");
                        webRequest.Method = "POST";

                        string Parameters = "code=" + code + "&client_id=" + client_id + "&client_secret=" + client_secret + "&redirect_uri=" + redirect_uri + "&grant_type=authorization_code";
                        byte[] byteArray = Encoding.UTF8.GetBytes(Parameters);

                        webRequest.ContentType = "application/x-www-form-urlencoded";
                        webRequest.ContentLength = byteArray.Length;
                        Stream postStream = webRequest.GetRequestStream();

                        // Add the post data to the web request
                        postStream.Write(byteArray, 0, byteArray.Length);
                        postStream.Close();

                        WebResponse response = webRequest.GetResponse();
                        postStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(postStream);

                        string responseFromServer = reader.ReadToEnd();
                        GoogleAccessToken serStatus = JsonConvert.DeserializeObject<GoogleAccessToken>(responseFromServer);

                        if (serStatus != null)
                        {
                            access_token = serStatus.access_token;

                            if (!string.IsNullOrEmpty(access_token))
                            {
                                //call get user information function with access token as parameter

                                // Get user information
                                using (HttpClient client = new HttpClient())
                                {
                                    client.BaseAddress = new Uri("https://www.googleapis.com");

                                    string urlProfile = "/oauth2/v1/userinfo?access_token=" + access_token;
                                    HttpResponseMessage output = client.GetAsync(urlProfile).Result;

                                    if (output.IsSuccessStatusCode)
                                    {
                                        string outputData = output.Content.ReadAsStringAsync().Result;
                                        GoogleUserOutputData userData = JsonConvert.DeserializeObject<GoogleUserOutputData>(outputData);

                                        return View(userData);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                //Redirect the user to an error page
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOff()
        {

            //Logout from application
            FormsAuthentication.SignOut();
            Redirect(Url.Action("Index", "Home"));
            return RedirectToAction("Index", "Home");
        }

        

        
    }
}