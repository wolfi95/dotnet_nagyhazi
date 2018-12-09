using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AspNetCore.Http.Extensions;
using Flatbuilder.DTO;
using FreshMvvm;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Fb.MC.Views
{
    class NewOrderPageModel : FreshBasePageModel
    {
        public Costumer User{ get; set; }
        static readonly Uri baseAddress = new Uri("http://10.0.2.2:51502/");

        private double tip = 10;
        public double Tip
        {
            get
            {
                return tip;
            }
            set
            {
                tip = value;
                RaisePropertyChanged("Tip");
                ReCountPrice();
            }
        }

        public ICommand CreateOrderCommand { get; }
        private ObservableCollection<int>kpicker;
        public ObservableCollection<int> Kpicker
        {
            get
            {
                return kpicker;
            }
            set
            {
                kpicker = value;
                RaisePropertyChanged("Kpicker");
            }
        }
        private int selectedK;
        public int SelectedK
        {
            get
            {
                return selectedK;
            }
            set
            {
                for (int i = 0; i < selectedK; i++)
                {
                    Room room = rooms.Find(r => r.Type.ToString().Equals("Flatbuilder.DTO.Kitchen"));
                    rooms.Remove(room);
                    freeRooms.Add(room);
                }
                selectedK = value;
                for (int i = 0; i < selectedK; i++)
                {
                    Room room = freeRooms.Find(r => r.Type.ToString().Equals("Flatbuilder.DTO.Kitchen"));
                    rooms.Add(room);
                    freeRooms.Remove(room);
                }
                RaisePropertyChanged("SelectedK");
                ReCountPrice();
            }
        }

        private ObservableCollection<int> spicker;
        public ObservableCollection<int> Spicker
        {
            get
            {
                return spicker;
            }
            set
            {
                spicker = value;
                RaisePropertyChanged("Spicker");
            }
        }
        private int selectedS;
        public int SelectedS
        {
            get
            {
                return selectedS;
            }
            set
            {
                for (int i = 0; i < selectedS; i++)
                {
                    Room room = rooms.Find(r => r.Type.ToString().Equals("Flatbuilder.DTO.Shower"));
                    rooms.Remove(room);
                    freeRooms.Add(room);
                }
                selectedS = value;
                for (int i = 0; i < selectedS; i++)
                {
                    Room room = freeRooms.Find(r => r.Type.ToString().Equals("Flatbuilder.DTO.Shower"));
                    rooms.Add(room);
                    freeRooms.Remove(room);
                }
                RaisePropertyChanged("SelectedS");
                ReCountPrice();
            }
        }

        private ObservableCollection<int> bpicker;
        public ObservableCollection<int> Bpicker
        {
            get
            {
                return bpicker;
            }
            set
            {
                bpicker = value;
                RaisePropertyChanged("Bpicker");
            }
        }
        private int selectedB;
        public int SelectedB
        {
            get
            {
                return selectedB;
            }
            set
            {
                for (int i = 0; i < selectedB; i++)
                {
                    Room room = rooms.Find(r => r.Type.ToString().Equals("Flatbuilder.DTO.Bedroom"));
                    rooms.Remove(room);
                    freeRooms.Add(room);
                }
                selectedB = value;
                for (int i = 0; i < selectedB; i++)
                {
                    Room room = freeRooms.Find(r => r.Type.ToString().Equals("Flatbuilder.DTO.Bedroom"));
                    rooms.Add(room);
                    freeRooms.Remove(room);
                }
                RaisePropertyChanged("SelectedB");
                ReCountPrice();
            }
        }
        private DateTime minDateStart = DateTime.Now;
        public DateTime MinDateStart
        {
            get
            {
                return minDateStart;
            }
            set
            {
                minDateStart = value;
                RaisePropertyChanged("MinDateStart");
            }
        }
        private DateTime maxDateStart = DateTime.Now.AddDays(5);
        public DateTime MaxDateStart
        {
            get
            {
                return maxDateStart;
            }
            set
            {
                maxDateStart = value;
                RaisePropertyChanged("MaxDateStart");
            }
        }
        private DateTime startDate = DateTime.Now;
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                if (EndDate < DateTime.Now)
                {
                    startDate = value;
                }
                else
                {
                    if (EndDate < value)
                        startDate = EndDate.AddDays(-1);
                    else
                        startDate = value;
                }
                MinDateEnd = startDate.AddDays(1);
                PropertyChangedOwn();
                RaisePropertyChanged("StartDate");
            }
        }
        private DateTime minDateEnd;
        public DateTime MinDateEnd
        {
            get
            {
                return minDateEnd;
            }
            set
            {
                minDateEnd = value;
                RaisePropertyChanged("MinDateEnd");
            }
            
        }
        private DateTime endDate = DateTime.Now.AddDays(5);
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                if (StartDate > value)
                    endDate = StartDate.AddDays(1);
                else
                    endDate = value;
                MaxDateStart = endDate.AddDays(-1);
                PropertyChangedOwn();
                RaisePropertyChanged("EndDate");
            }
        }
        private double price;
        public double Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
                RaisePropertyChanged("Price");
            }
        }
        private List<Room> freeRooms;
        private List<Room> rooms = new List<Room>();

        public NewOrderPageModel()
        {
            CreateOrderCommand = new Command(
                execute: async () =>
                {
                    if (rooms==null || rooms.Count == 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("No Rooms selected", "Please select at least one room!", "Ok");
                        return;
                    }
                    var order = new Order()
                    {
                        Costumer = User,
                        StartDate = this.StartDate,
                        EndDate = this.EndDate,
                        Rooms = rooms,
                        Price = this.Price
                    };

                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = baseAddress;

                        var response = await client.PostAsJsonAsync<Order>("api/Order/create",order);
                        if (response.StatusCode == System.Net.HttpStatusCode.Created)
                        {
                            var navpage = new FreshNavigationContainer(FreshPageModelResolver.ResolvePageModel<MainPageModel>(User));
                            Application.Current.MainPage = navpage;
                        }
                    }
                }
                );
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            User = (Costumer)initData;
        }

        private async Task<List<Room>> GetRooms(DateTime start, DateTime end)
        {
            var sd = start.ToString("MM-dd-yyyy");
            var ed = end.ToString("MM-dd-yyyy");

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = baseAddress;

                var response = await client.GetAsync("api/Order/" + sd + "/" + ed);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Room>>(json);
            }
        }

        protected async void PropertyChangedOwn()
        {
            SelectedK = 0;
            SelectedS = 0;
            SelectedB = 0;

            freeRooms = await GetRooms(StartDate, EndDate);
            if(Kpicker==null)
                Kpicker = new ObservableCollection<int>();
            if (Spicker == null)
                Spicker = new ObservableCollection<int>();
            if (Bpicker == null)
                Bpicker = new ObservableCollection<int>();
            Kpicker.Clear();
            Kpicker.Add(0);
            Spicker.Clear();
            Spicker.Add(0);
            Bpicker.Clear();
            Bpicker.Add(0);
            if (freeRooms != null || freeRooms.Count != 0)
            {
                int k = 0;
                int s = 0;
                int b = 0;

                foreach (Room r in freeRooms)
                {
                    if (r.Type.ToString().Equals("Flatbuilder.DTO.Kitchen"))
                        Kpicker.Add(++k);
                    if (r.Type.ToString().Equals("Flatbuilder.DTO.Shower"))
                        Spicker.Add(++s);
                    if (r.Type.ToString().Equals("Flatbuilder.DTO.Bedroom"))
                        Bpicker.Add(++b);
                }
                RaisePropertyChanged("Spicker");
                RaisePropertyChanged("Kpicker");
                RaisePropertyChanged("Bpicker");
            }
            ReCountPrice();
        }
        public void ReCountPrice()
        {
            if (rooms == null || rooms.Count == 0)
            {
                Price = 0;
                return;
            }
            else
            {
                double pr = 0;
                double days = (EndDate - StartDate).TotalDays;

                foreach (Room r in rooms)
                {
                    pr += r.Price * days;

                }
                pr = (Tip/100*pr)+pr;
                Price = pr;
            }
        }
    }
}
