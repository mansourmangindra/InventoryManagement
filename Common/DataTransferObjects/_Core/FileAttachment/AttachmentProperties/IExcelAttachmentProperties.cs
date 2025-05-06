namespace Common.DataTransferObjects._Core.FileAttachment.AttachmentProperties
{
    public interface IExcelAttachmentProperties
    {
        public string HeaderCellColor { get; set; }
        public string HeaderTextColor { get; set; }

        public int StartColumn { get; set; }

        public bool ChangeCellColor { get; set; }
        public string CellColor { get; set; }
        public List<string> ChangeCellColorOnColumns { get; set; }

        public bool ChangeTextColor { get; set; }
        public string TextColor { get; set; }
        public List<string> ChangeTextColorOnColumns { get; set; }
    }
}