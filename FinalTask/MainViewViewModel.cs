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

namespace FinalTask
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public DelegateCommand SaveCommand { get; }
        public string Prefix { get; set; }
        public string StartNumber { get; set; } 
        public string Suffix { get; set; }
        public bool allRooms { get; set; }
        public bool fewRooms { get; set; }
        public DelegateCommand Choose { get; }

        public List<Room> rooms { get; set; } = new List<Room>();

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            SaveCommand = new DelegateCommand(OnSelectCommand);
            Prefix = "";
            StartNumber ="";
            Suffix = "";
            allRooms = true;
            fewRooms = false;
            Choose = new DelegateCommand(ChooseRooms);
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
        private void OnSelectCommand()
        {
            Document arDoc = _commandData.Application.ActiveUIDocument.Document;

            if (allRooms is true)
            {
                rooms = SelectionUtils.AllRooms(_commandData);
            }

            if (rooms.Count == 0)
            {
                TaskDialog.Show("Count", "создайте помещения");
            }
            string level = arDoc.ActiveView.Name;
            int i = Convert.ToInt32(StartNumber);
            if (i <= 0) { TaskDialog.Show("Ошибка", "Порядковый номер д.б. положительный"); }
            int j;
            int.TryParse(string.Join("", level.Where(c => char.IsDigit(c))), out j);

            Transaction ts = new Transaction(arDoc);
            ts.Start("Change Number Rooms");
            foreach (Room room in rooms)
            {
                Parameter number = (Parameter)room.get_Parameter(BuiltInParameter.ROOM_NUMBER);
                string numberStr = $"{Prefix}-{j}.{i} [{Suffix}]";
                number.Set(numberStr);
                i++;
            }
            ts.Commit();

            RaiseCloseRequest();

        }
    }
}
