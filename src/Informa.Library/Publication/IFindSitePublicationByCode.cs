namespace Informa.Library.Publication
{
	public interface IFindSitePublicationByCode
	{
		ISitePublication Find(string publicationCode);
	}
}