namespace Informa.Library.Services.NlmExport.Validation.Config
{
    public class NlmValidationConfiguration
    {
        public NlmValidationConfiguration(string dtdPath)
        {
            DtdPath = dtdPath;
        }

        public string DtdPath { get; private set; }
    }
}
