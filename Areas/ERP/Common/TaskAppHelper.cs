namespace BizWebAPI.Areas.ERP.Common
{
    public class TaskAppHelper
    {
        public static string GetFieldValueById<T>(T Model, string FldName)
        {
            if (Model.Equals(null))
            {
                return string.Empty;
            }

            var type = Model.GetType();
            var field = type.GetProperty(FldName);
            var emp = field.GetValue(Model);
            if (emp == null)
            {
                return string.Empty;
            }

            return emp.ToString();
        }
    }
}
