namespace GuestInfoApi.Models
{
    public class GuestInfo
    {
        public string? GuestName { get; set; }
        public string? MobileNumber { get; set; }
    }

    public class GuestUpdateRequest
    {
        public string? CurrentMobileNumber { get; set; }
        public string? NewGuestName { get; set; }
        public string? NewMobileNumber { get; set; }
    }

}


