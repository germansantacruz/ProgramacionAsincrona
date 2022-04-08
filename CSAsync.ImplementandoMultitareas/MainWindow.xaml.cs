using System;
using System.Diagnostics;
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
            //CreateTask();
            RunTaskGroup();
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

            Task tarea10 = Task.Run(() =>
            {
                WriteToOutput("Iniciando tarea 10...");
                // Simular un proceso que dura 10 segundos
                Thread.Sleep(10000);
                WriteToOutput("Fin de la tarea 10.");
            });
            WriteToOutput("Esperando a la tarea 10.");
            tarea10.Wait();
            WriteToOutput("La tarea finalizó su ejecución");
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

        void WriteToOutput(string message)
        {
            Debug.WriteLine(
            $"Mensaje: {message}, " +
            $"Hilo actual: {Thread.CurrentThread.ManagedThreadId}");
        }

        void RunTask(byte taskNumber)
        {
            WriteToOutput($"Iniciando tarea {taskNumber}.");
            // Simular un proceso que dura 10 segundos
            Thread.Sleep(10000); // El hilo es suspendido por 10000 milisegundos
            WriteToOutput($"Finalizando tarea {taskNumber}.");
        }

        void RunTaskGroup()
        {
            Task[] TaskGroup = new Task[]
            {
                Task.Run(() => RunTask(1)),
                Task.Run(() => RunTask(2)),
                Task.Run(() => RunTask(3)),
                Task.Run(() => RunTask(4)),
                Task.Run(() => RunTask(5))
            };

            WriteToOutput("Esperando a que finalicen todas las tareas...");
            Task.WaitAll(TaskGroup);
            WriteToOutput("Todas las tareas han finalizado.");
        }
    }
}
