namespace DDDEastAnglia.Domain
{
    public sealed class SessionizeInfo
    {
        private readonly string id;
        private readonly string name;

        public SessionizeInfo(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(id);
        }

        public string ConferenceId { get { return id; } }

        public string SubmissionUrl
        {
            get { return string.Format("https://sessionize.com/{0}/", name); }
        }
    }
}
