using Common.DataTransferObjects._Base;

namespace Common.DataTransferObjects.Notification
{
    public class SampleMessage : SaveDataTransferObject
    {
        public string Recipient { get; set; }
        public string Message { get; set; }
    }
}
