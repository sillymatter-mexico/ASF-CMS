namespace asf.cms.model
{
    public class PublicationLinkAccessResultVO
    {
        public virtual string AccessUrl { get; set; }

        public virtual uint TotalHits { get; set; }

        public virtual uint YearHits { get; set; }

        public virtual uint MonthHits { get; set; }

        public virtual uint DayHits { get; set; }
    }
}