namespace Informa.Library.Salesforce.EBIWebServices
{
    public partial class IN_ProfilePreferencesQueryResponse : IEbiResponse
    {
        private EBI_Error[] errorsField;

        public EBI_Error[] errors
        {
            get
            {
                return this.errorsField;
            }
            set
            {
                this.errorsField = value;
            }
        }
    }
}
