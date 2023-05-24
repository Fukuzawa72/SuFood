using SuFood.Models;

namespace SuFood.ViewModel
{
    public class VMAnnouncement
    {
        public int AnnouncementId { get; set; }
        public string AnnouncementContent { get; set; }
        public string AnnouncementStatus { get; set; }
        public DateTime AnnouncementStartDate { get; set; }
        public DateTime AnnouncementEndDate { get; set; }
        public byte[] AnnouncementImage { get; set; }
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
