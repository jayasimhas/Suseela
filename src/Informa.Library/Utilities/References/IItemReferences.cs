using System;

namespace Informa.Library.Utilities.References
{
	public interface IItemReferences
	{
		Guid HomePage { get; }
		
		// Templates
		Guid FolderTemplate { get; }
    }
}
