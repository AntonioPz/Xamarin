using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Reto5.Services;
using Android;
using Android.Content;
using Android.Runtime;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Reto5
{
    [Activity(Label = "Reto 5 - Xamarin Championship", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private TextView tvReg;
        private TextView tvReg2;
        //Mobile Service Client reference
        private MobileServiceClient client;

        //Mobile Service sync table used to access data
        private IMobileServiceSyncTable<TorneoItem> torneoItemTable;

        const string applicationURL = @"http://xamarinchampions.azurewebsites.net";
        const string localDbFilename = "localstore.db";
        const string emailParticipante = "antonio_dddd@hotmail.es";
        const string pais = "mx"; // Consulta el código de país en http://www.worldstandards.eu/other/tlds/
        Button btnRegistro;

        protected async override void OnCreate(Bundle bundle)
        {
            

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            // Create the Mobile Service Client instance, using the provided
            // Mobile Service URL
            client = new MobileServiceClient(applicationURL);
            await InitLocalStoreAsync();

            // Get the Mobile Service sync table instance to use
            torneoItemTable = client.GetSyncTable<TorneoItem>();

            // Obtener una referencia al botón Siguiente
            btnRegistro = FindViewById<Button>(Resource.Id.BtnRegistro);
            tvReg = FindViewById<TextView>(Resource.Id.textViewResults);
            tvReg2 = FindViewById<TextView>(Resource.Id.textView1);
            // Registrar el manejador de evento click del botón Siguiente
            //btnRegistro.Click += BtnSiguienteClick;
            OnRefreshItemsSelected();        

        }

        private async void BtnSiguienteClick(object sender, EventArgs e)
        {
            string AndroidId = Android.Provider.Settings.Secure.GetString(ContentResolver,Android.Provider.Settings.Secure.AndroidId);

            await torneoItemTable.InsertAsync(new TorneoItem
            {
                Email = emailParticipante,
                DeviceId = AndroidId,
                Reto = "Reto5@" + pais
            });

        }
        private async Task InitLocalStoreAsync()
        {
            string path = Path.Combine(System.Environment.GetFolderPath(
                System.Environment.SpecialFolder.Personal),localDbFilename);
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
            var store = new MobileServiceSQLiteStore(path);
            store.DefineTable<TorneoItem>();
            await client.SyncContext.InitializeAsync(store);
        }
        private async Task SyncAsync()
        {
            try
            {
                //await client.SyncContext.PushAsync();
                await torneoItemTable.PullAsync("allTorneoItems",torneoItemTable.CreateQuery().Where(
                    item=> item.Email==emailParticipante));
            }
            catch (Exception e)
            {
                string error = e.Message;
            }
        }
        private async Task RefreshItemsFromTableAsync()
        {tvReg2.Text = "ajhdvsjkvasbhdsjkbs";
            try
            {
                // Get the items that weren't marked as completed and add them in the adapter
                var resultado = await torneoItemTable.ToListAsync();
                //string textToDisplay = FindViewById(Resource.Id.textViewResults).Text + "\n\n";
                string textToDisplay = tvReg.Text + "\n\n";
                for (int i = 0; i < resultado.Count; i++)
                {
                    textToDisplay = textToDisplay + resultado[i].Email + " " + resultado[i].Reto + "\n";
                }
                tvReg.Text = textToDisplay;
                
                //FindViewById(Resource.Id.textViewResults).Text = textToDisplay;
            }
            catch (Exception e)
            {
                string error = e.Message;
            }
        }
        private async void OnRefreshItemsSelected()
        {
            await SyncAsync();
            await RefreshItemsFromTableAsync();

        }
    }
} 