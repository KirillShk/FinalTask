using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI.Selection;
using System.Windows;

namespace FinalTask
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public DelegateCommand SaveCommand { get; }
        public string Prefix { get; set; }
        public int? StartNumber { get; set; }
        //{
        //    if (value < 0)
        //        Console.WriteLine("Порядковый номер не может быть отрицательным");
        //    else
        //        StartNumber = value; } }
        public string Suffix { get; set; }
        public bool allRooms { get; set; }
        public bool fewRooms { get; set; }
        public DelegateCommand Choose { get; }

        private RevitTask revitTask;

        public List<Room> rooms { get; set; } = new List<Room>();

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            SaveCommand = new DelegateCommand(OnSelectCommand);
            Prefix = null;
            StartNumber = null;
            Suffix = null;
            allRooms = true;
            fewRooms = false;
            Choose = new DelegateCommand(ChooseRooms);
            revitTask = new RevitTask();
        }

        private void ChooseRooms()
        {
            RaiseHideRequest();
            rooms = SelectionUtils.GetOblects(_commandData);
            RaiseShowRequest();
        }
        #region События
        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler HideRequest;
        private void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler ShowRequest;
        private void RaiseShowRequest()
        {
            ShowRequest?.Invoke(this, EventArgs.Empty);
        }
        #endregion 
        private async void OnSelectCommand()
        {
            Document arDoc = _commandData.Application.ActiveUIDocument.Document;
            if (allRooms is true)
                {
                    rooms = SelectionUtils.AllRooms(_commandData);
                }
            if (StartNumber < 0)
            {
                TaskDialog.Show("Ошибка", "Порядковый номер не может быть отрицательным");
            }
            else if (rooms.Count == 0)
            {
                TaskDialog.Show("Rooms", "На плане отсутствуют помещения. Создайте помещения");
            }
            else 
            {
                string level = arDoc.ActiveView.Name;
                int j;
                int.TryParse(string.Join("", level.Where(c => char.IsDigit(c))), out j);
                string k = null;
                if (Suffix != null)
                {
                    k = $"[{Suffix}]";
                }

                if (fewRooms is true)
                {
                    await revitTask.Run(app =>
                    {
                        NumericRooms.CreateNumeric(arDoc, rooms, StartNumber, j, k, Prefix);
                    });
                }
                else
                {
                    NumericRooms.CreateNumeric(arDoc, rooms, StartNumber, j, k, Prefix);
                }
            }
            RaiseCloseRequest();

        }
    }
}
