﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using Remuse.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Remuse.Activities
{
    [Activity(Label = "BookPage")]
    public class UserBooks : ListActivity
    {
        List<Book> usersbooks; //get this from BooksService or activity...
        LogOutBroadcastReceiver _logOutBroadcastReceiver = new LogOutBroadcastReceiver();
        List<string> mLeftItems = new List<string>();
        Author authorFromBase;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            UserInfo.Books = await GetUserBooksByUserIdAsync(UserInfo.User.Id);
            usersbooks = UserInfo.Books;
            List<string> titles = new List<string>();

            foreach (Book book in usersbooks)
            {
                titles.Add(book.Title);
            }

            ListAdapter = new ArrayAdapter<string>(this, Resource.Layout.list_item, titles);
            ListView.TextFilterEnabled = true;

            ListView.ItemClick += async delegate (object sender, AdapterView.ItemClickEventArgs args)
            {
                var server = new AuthorClient(new System.Net.Http.HttpClient());
                authorFromBase = await server.GetAsync(usersbooks[args.Position].AuthorId);
                usersbooks[args.Position].Author = authorFromBase;

                Intent intent = new Intent(this, typeof(OneBookInfo));
                intent.PutExtra("book", JsonConvert.SerializeObject(usersbooks[args.Position]));

                StartActivity(intent);
            };

            var intentFilter = new IntentFilter();
            intentFilter.AddAction("com.mypackagename.ActionLogOut");
            RegisterReceiver(_logOutBroadcastReceiver, intentFilter);
        }

        /// <summary>
        ///  Event,that works when user clicks on the item of menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MLeftDrawer_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Type type = typeof(UserPage);

            int position = e.Position;
            switch (position)
            {
                case 0:
                    Intent intent = new Intent(this, type);
                    StartActivity(intent);
                    break;
                case 1:
                    Toast.MakeText(this, mLeftItems[e.Position], ToastLength.Long).Show();
                    break;
                case 2:
                    Toast.MakeText(this, mLeftItems[e.Position], ToastLength.Long).Show();
                    break;
                case 3:
                    UserInfo.User = null;
                    UserInfo.Token = null;
                    UserInfo.BookId = new List<string>();
                    var broadcastIntent = new Intent();
                    broadcastIntent.SetAction("com.mypackagename.ActionLogOut");
                    SendBroadcast(broadcastIntent);
                    Intent intent1 = new Intent(this, typeof(StartGeneral));
                    StartActivity(intent1);
                    break;
            }
        }

        /// <summary>
        /// Destroys activiy
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            UnregisterReceiver(_logOutBroadcastReceiver);
        }

        /// <summary>
        /// To get user from base
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<Book>> GetUserBooksByUserIdAsync(int id)
        {
            HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync(HttpUri.ProfileApiUri + "api/profile/" + id);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                List<Book> books = JsonConvert.DeserializeObject<List<Book>>(responseBody.ToString());
                return books;
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}