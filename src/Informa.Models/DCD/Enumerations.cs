
namespace Informa.Models.DCD
{
    public enum ImportResult
    {
        InProgress,
        Success,
        PartialSuccess,
        Failure
    }

    public enum RecordImportResult
    {
        Success,
        Skipped,
        Failure
    }

    public enum RecordImportOperation
    {
        Update,
        Insert,
        Delete
    }
}
