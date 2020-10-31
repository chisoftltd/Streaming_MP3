using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Dyanamitechetan.Vusikview;
using Android.Media;
using Android.Widget;
using static Android.Views.View;
using Android.Views;
using Java.Lang;

namespace Streaming_MP3
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnClickListener
    {
        public ImageButton btn_play_pause;
        public SeekBar seekBar;
        public TextView txt_timer;

        public VusikView musicView;

        public MediaPlayer mediaPlayer;
        public long mediaFileLength, realTime;
        public Handler handler = new Handler();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            musicView = FindViewById<VusikView>(Resource.Id.musicView);

            btn_play_pause = FindViewById<Button>(Resource.Id.btn_play_pause);
            btn_play_pause.SetOnClickListener(this);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnClick(View v)
        {
            if (v.Id == Resource.Id.btn_play_pause)
            {
                new MP3Play(this).Execute("http://mic.duytan.edu.vn:86/ncs.mp3"); // Direct link mp3
                musicView.Start();
            } 
        }
    }

    internal class MP3Play:AsyncTask<string, string, string>
    {
        private MainActivity mainActivity;
        private ProgressDialog mDialog;

        public MP3Play(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            mDialog = new ProgressDialog(mainActivity.BaseContext);
            mDialog.Window.SetType(WindowManagerTypes.SystemAlert);
            mDialog.SetMessage("Please wait...");
            mDialog.Show();
        }
        protected override string RunInBackground(params string[] @params)
        {
            try
            {
                mainActivity.mediaPlayer.SetDataSource(@params[0]);
                mainActivity.mediaPlayer.Prepare();
            }
            catch (System.Exception ex)
            {

            }
            return "";
        }
        protected override void OnPostExecute(string result)
        {
            mainActivity.mediaFileLength = mainActivity.realTime = mainActivity.mediaPlayer.Duration;
            if (!mainActivity.mediaPlayer.IsPlaying)
            {
                mainActivity.mediaPlayer.Start();
                mainActivity.btn_play_pause.SetImageResource(Resource.Drawable.baseline_pause_circle_outline_black_18dp);
            }
            else
            {
                mainActivity.mediaPlayer.Pause();
                mainActivity.btn_play_pause.SetImageResource(Resource.Drawable.baseline_play_circle_outline_black_18dp);
            }
        }
    }
}