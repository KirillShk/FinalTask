using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Linq.Expressions;
using System.Windows;

namespace FinalTask
{
    public class SelectionUtils
    {
        public static List<Room> GetOblects(ExternalCommandData commandData, string message = "Выберите элементы")
        {
            UIDocument arDoc = commandData.Application.ActiveUIDocument;
            Document doc = arDoc.Document;
            var roomList = new List<Room>();
            try
            {
                IList<Reference> selectedElenmentRef = arDoc.Selection.PickObjects(ObjectType.Element, "Выберите помещения");
                foreach (var selectedElement in selectedElenmentRef)
                {
                    Element element = doc.GetElement(selectedElement);
                    if (element is Room)
                    {
                        Room oRoom = (Room)element;
                        roomList.Add(oRoom);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return roomList;
        }
        public static List<Room> AllRooms(ExternalCommandData commandData, string message = "Выберите элементы")
        {
            Document arDoc = commandData.Application.ActiveUIDocument.Document;
            List<Room> rooms = new FilteredElementCollector(arDoc, arDoc.ActiveView.Id)
                .OfCategory(BuiltInCategory.OST_Rooms)
                .Cast<Room>()
                .ToList();
            return rooms;
        }

    }
}
