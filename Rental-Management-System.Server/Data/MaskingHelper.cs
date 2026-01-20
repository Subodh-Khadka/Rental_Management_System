namespace Rental_Management_System.Server.Data
{
    public class MaskingHelper
    {
        public static string MaskId(string id)
        {
            if (string.IsNullOrEmpty(id)) return string.Empty;

            // only the last 4 characters are visible
            var visible = id.Length > 4 ? id[^4..] : id;
            return $"xxxx-xxxx-xxxx-{visible}";
        }
    }
}
