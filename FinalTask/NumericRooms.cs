using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalTask
{
    public class NumericRooms
    {
        public static void CreateNumeric(Document arDoc, List<Room> rooms, int? StartNumber, int j, string k, string Prefix)
        {
            Transaction ts = new Transaction(arDoc);
            ts.Start("Change Number Rooms");
            foreach (Room room in rooms)
            {
                Parameter number = (Parameter)room.get_Parameter(BuiltInParameter.ROOM_NUMBER);
                string numberStr = $"{Prefix}-{j}.{StartNumber} {k}";
                number.Set(numberStr);
                StartNumber++;
            }
            ts.Commit();
        }
    }
}
