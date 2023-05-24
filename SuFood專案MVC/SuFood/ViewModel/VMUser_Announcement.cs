using SuFood.Models;

namespace SuFood.ViewModel
{
    public class VMUser_Announcement
    {
        public int AnnouncementId { get; set; }
        public string AnnouncementContent { get; set; }
        public DateTime AnnouncementStartDate { get; set; }
        public DateTime AnnouncementEndDate { get; set; }
        public byte[] AnnouncementImage { get; set; }

    }
}
