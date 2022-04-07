using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CSAsync.ImplementandoMultitareas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CreateTask();
        }

        void CreateTask()
        {
            Task tarea1;
            var code = new Action(ShowMessage);
            tarea1 = new Task(code);

            Task tarea2 = new Task(delegate
            {
                MessageBox.Show("Ejecutando una tarea en un método anónimo");
            });

            Task tarea3 = new Task(() => ShowMessage());
            Task tarea4 = new Task(() => MessageBox.Show("Ejecutando la tarea 4."));
            Task tarea5 = new Task(() =>
            {
                DateTime CurrentDate = DateTime.Today;
                DateTime StartDate = CurrentDate.AddDays(30);
                MessageBox.Show($"Tarea 5. Fecha calculada:{StartDate}");
            });

            Task tarea6 = new Task((message) =>
            MessageBox.Show(message.ToString()), "Expresión lambda con parámetros.");

            Task tarea7 = new Task(() => AddMessage("Ejecutando la tarea"));
            tarea7.Start();
            AddMessage("En el hilo principal");

            Task tarea8 = Task.Factory.StartNew(() =>
                AddMessage("Tarea iniciada con TaskFactory")
            );

            Task tarea9 = Task.Run(() => AddMessage("Tarea ejecutada con Task.Run"));
        }

        void ShowMessage()
        {
            MessageBox.Show("Ejecutando el método ShowMessage");
        }

        void AddMessage(string message)
        {
            int CurrentThreadID = Thread.CurrentThread.ManagedThreadId;

            this.Dispatcher.Invoke(() =>
            {
                Messages.Content +=
                    $"Mensaje: {message}, " +
                    $"Hilo actual: {CurrentThreadID}\n";
            });
        }
    }
}
