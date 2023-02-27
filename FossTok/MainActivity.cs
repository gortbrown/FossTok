using System.IO;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.OS;
using Android.Widget;
using Android;
using Android.Webkit;
using static Java.Util.Concurrent.Flow;
using Newtonsoft.Json;
using Android.Content.Res;

namespace FossTok
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private ListView followingList;
        //private string[] following = {"Kurtis Conner,@kurtisconner", "Networkchuck,@networkchuck", "Duolingo,@duolingo", "Trailer Park Boys,@trailerparkboysofficial", "Dallas Ponzo,@dallas_ponzo"};
        private List<string> following = new List<string>();
        private List<string> followingNames = new List<string>();
        private List<string> followingUnames = new List<string>();
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            
            SetSubscriptions();
        }

        public void SetSubscriptions()
        {
            followingNames.Add("Discover");
            followingUnames.Add("discover");
            followingNames.Add("Trending");
            followingUnames.Add("trending");
            AssetManager assets = this.Assets;
            using (StreamReader file = new StreamReader(assets.Open("following.txt")))
            {
                while (!file.EndOfStream)
                {
                    following.Add(file.ReadLine());
                }
            }
            foreach (string user in following)
            {
                string[] userInfo = user.Split(",");
                followingNames.Add(userInfo[0]);
                followingUnames.Add(userInfo[1]);
            }
            followingList = FindViewById<ListView>(Resource.Id.following);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, followingNames);
            followingList.Adapter = adapter;
            followingList.ItemClick += followingItemClick;
            Button search = FindViewById<Button>(Resource.Id.searchButton);
            search.Click += searchUserClick;
        }

        private void followingItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            SetContentView(Resource.Layout.browser);
            WebView page = FindViewById<WebView>(Resource.Id.pageView);
            page.LoadUrl("https://tik.hostux.net/" + followingUnames[e.Position]);
            Button back = FindViewById<Button>(Resource.Id.backButton);
            back.Click += backButtonClick;
        }

        private void backButtonClick(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.activity_main);
            following.Clear();
            followingNames.Clear();
            followingUnames.Clear();
            SetSubscriptions();
        }

        private void addFollowClick(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.add);
            Button addFollow = FindViewById<Button>(Resource.Id.addToList);
            addFollow.Click += addClick;
        }

        private void addClick(object sender, EventArgs e)
        {
            AssetManager assets = this.Assets;
            EditText addName = FindViewById<EditText>(Resource.Id.addName);
            EditText addUname = FindViewById<EditText>(Resource.Id.addUname);
            using (StreamWriter add = new StreamWriter(assets.Open("following.txt")))
            {
                add.WriteLine(addName.Text + "," + addUname.Text);
            }
        }

        private void searchUserClick(object sender, EventArgs e)
        {
            EditText usernameBox = FindViewById<EditText>(Resource.Id.searchBox);
            SetContentView(Resource.Layout.browser);
            WebView page = FindViewById<WebView>(Resource.Id.pageView);
            page.LoadUrl("https://tik.hostux.net/" + usernameBox.Text);
            Button back = FindViewById<Button>(Resource.Id.backButton);
            back.Click += backButtonClick;
        }
    }
}